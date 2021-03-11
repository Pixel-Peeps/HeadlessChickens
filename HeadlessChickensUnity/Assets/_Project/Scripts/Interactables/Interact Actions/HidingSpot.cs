using System;
using Photon.Pun;
using UnityEngine;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using Cinemachine;
using JetBrains.Annotations;
using PixelPeeps.HeadlessChickens.GameState;
using PixelPeeps.HeadlessChickens.Network;
using PixelPeeps.HeadlessChickens.UI;
using System.Collections;

// ReSharper disable UnusedMember.Global

public class HidingSpot : MonoBehaviourPunCallbacks, IInteractable
{
    //public ChickenBehaviour chickenInSpot;
    public bool canAccessHiding = true;

    public CinemachineVirtualCamera hidingCam;
    [UsedImplicitly] private MainCam mainCam;

    public GameObject hidingMesh;

    public float searchProgress = 0;
    private Coroutine searchRoutine;
    private AudioSource audioSource;

    public AudioClip hidingSpotSoundEffect;
    private void Awake()
    {
        hidingCam = GetComponentInChildren<CinemachineVirtualCamera>(true);
        
        if (Camera.main != null)
        {
            mainCam = Camera.main.GetComponent<MainCam>();
            audioSource = Camera.main.GetComponent<AudioSource>();
        }
    }

    public void Interact(CharacterBase character, bool willLoop)
    {
        photonView.SetControllerInternal(character.photonView.Owner.ActorNumber);
        if (photonView.IsMine)
        {
            if (canAccessHiding && character.GetComponent<Interactor>().characterType == Interactor.eCharacterType.Chick)
            {
                Debug.Log("Interact() method called, inUse false");
                //character.photonView.GetComponent<CharacterBase>().currentHidingSpot = transform;
                character.HidingInteraction(canAccessHiding, transform);
                
                photonView.RPC("RPC_ToggleAccess", RpcTarget.AllViaServer);
                
                // mainCam.CullHidingSpots();
                //EnableHidingCam();
                Debug.Log("<color=cyan> Hiding spot Deactivated </color>");
            }
            else if (character.isFox)
            {
                searchRoutine = StartCoroutine(SearchLoop(character));
            }
            else
            {
                // character.currentHidingSpot.position = null;
                
                character.HidingInteraction(canAccessHiding, transform);
                // mainCam.SeeEverything();
            }

        }
    }

    [PunRPC]
    public void RPC_ToggleAccess()
    {
        canAccessHiding = !canAccessHiding;
    }


    IEnumerator SearchLoop(CharacterBase character)
    {
        // interactable.photonView.RPC("RPC_ToggleInteractAllowed", RpcTarget.AllBufferedViaServer);

        character._controller._anim.SetBool("SearchBool", true);

        for (var tempProgress = searchProgress; tempProgress < 1; tempProgress += 0.01f)
        {
            if (character._controller.interactCanceled)
            {
                character._controller._anim.SetBool("SearchBool", false);
                character._controller.interactCanceled = false;
                searchProgress = 0;
                yield break;
            }

            searchProgress = tempProgress;
            // Debug.Log("leverProgress is " + leverProgress);
            yield return new WaitForSeconds(0.01f);
        }
        searchProgress = 0;
        character._controller._anim.SetBool("SearchBool", false);

        character.HidingInteraction(canAccessHiding, transform);

        searchRoutine = null;
    }

    public void EnableHidingCam()
    {
        if (photonView.IsMine)
        {
            
                audioSource.loop = true;
                audioSource.clip = hidingSpotSoundEffect;
                audioSource.volume = 0.4f;
                audioSource.Play();
            
            hidingMesh.SetActive(false);
            hidingCam.gameObject.SetActive(true);
        }
    }

    public void DisableHidingCam()
    {
        if (photonView.IsMine)
        {
            audioSource.Stop();
            hidingCam.gameObject.SetActive(false);
            hidingMesh.SetActive(true);
            Debug.Log("<color=yellow> Hiding spot REactivated </color>");
        }
    }


    public void InteractionFocus(bool focussed, CharacterBase character)
    {        
        photonView.SetControllerInternal(character.photonView.Owner.ActorNumber);
        if (!photonView.IsMine) return;
        
        if (focussed)
        {
            switch ( NewGameManager.Instance.myType )
            {
                case PlayerType.Fox:
                    HUDManager.Instance.UpdateInteractionText("SEARCH");
                    break;
                
                case PlayerType.Chick:
                    HUDManager.Instance.UpdateInteractionText("HIDE");
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }  
        }

        if (!focussed)
        {
            HUDManager.Instance.UpdateInteractionText();
        }

    }
}
