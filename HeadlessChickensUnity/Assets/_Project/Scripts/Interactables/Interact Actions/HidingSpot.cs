﻿using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using Cinemachine;

public class HidingSpot : MonoBehaviourPunCallbacks, IInteractable
{
    //public ChickenBehaviour chickenInSpot;
    public bool canAccessHiding = true;

    public CinemachineVirtualCamera hidingCam;
    MainCam mainCam;

    public GameObject hidingMesh;

    private void Awake()
    {
        hidingCam = GetComponentInChildren<CinemachineVirtualCamera>(true);
        mainCam = Camera.main.GetComponent<MainCam>();
    }

    public void Interact(CharacterBase character)
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
                EnableHidingCam();
                Debug.Log("<color=cyan> Hiding spot DEactivated </color>");
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


    public void EnableHidingCam()
    {
        if (photonView.IsMine)
        {
            hidingMesh.SetActive(false);
            hidingCam.gameObject.SetActive(true);
        }
    }

    public void DisableHidingCam()
    {
        if (photonView.IsMine)
        {
            hidingCam.gameObject.SetActive(false);
            hidingMesh.SetActive(true);
            Debug.Log("<color=yellow> Hiding spot REactivated </color>");
        }
    }


    public void InteractionFocus(bool focussed)
    {

    }
}
