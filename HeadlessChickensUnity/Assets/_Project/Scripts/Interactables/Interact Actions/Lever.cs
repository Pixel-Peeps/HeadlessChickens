using Photon.Pun;
using UnityEngine;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using PixelPeeps.HeadlessChickens.Network;
using PixelPeeps.HeadlessChickens.UI;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;
using System;

// ReSharper disable UnusedMember.Global

public class Lever : MonoBehaviourPunCallbacks, IInteractable
{
    private Interactable interactable;
    private InputControls _controls;
    public LeverManager leverManager;
    public GameObject blueprintBits;
    public GameObject regularBits;
    public Animator animator;
    public bool isFake = true;
    public bool isActive;
    public bool isShowingBlueprints;
    public bool interactCanceled;
    public bool hasBeenPulled = false;
    public float leverProgress = 0;

    public float timeIncrement = 0.1f;
    public float progressIncrement = 0.025f;
    public float timeNeededToTrigger;

    [Header("Coroutines")]
    public IEnumerator progressRoutine;
    public IEnumerator resetRoutine;

    public ProgressBar progressBar;
    
    [Header("Sounds")] 
    public AudioSource audioSource;
    public AudioClip falseLeverAlarm;
    public AudioClip leverActivated;


    private void Awake()
    {
        isFake = true;
    }

    private void Start()
    {
        if (Camera.main != null)
        {
            audioSource = Camera.main.GetComponent<AudioSource>();
        }
        
        progressBar = transform.parent.GetComponentInChildren<ProgressBar>();

        interactable = GetComponent<Interactable>();
        leverManager = GameObject.FindWithTag("LeverManager").GetComponent<LeverManager>();

        // TODO: BEKAH HELP
        if (!isFake)
        {
            photonView.RPC("RPC_SetUp", RpcTarget.AllViaServer);
        }

        if (isFake)
        {
            photonView.RPC("RPC_SetUp", RpcTarget.AllViaServer, true);
        }

        timeNeededToTrigger = timeIncrement / progressIncrement;
    }

    [PunRPC]
    public void RPC_SetUp()
    {
        Debug.Log("Setting up lever. fake? : " + isFake);
        
        if (isFake)
        {
            //if fake then
            blueprintBits.gameObject.SetActive(false);
            regularBits.gameObject.SetActive(false);
        }
        
        else if (!isFake)
        {
            regularBits.gameObject.SetActive(true);
            blueprintBits.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    public void RPC_SetUp(bool changeIfFake)
    {
        Debug.Log("hello this is the second rpc");
        isFake = changeIfFake;
        Debug.Log("Setting up lever. fake? : " + isFake);
        
        if (isFake)
        {
            blueprintBits.gameObject.SetActive(false);
            regularBits.gameObject.SetActive(false);
        }
        
        else if (!isFake)
        {
            regularBits.gameObject.SetActive(true);
            blueprintBits.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    public void RPC_SetUpFake()
    {
        isFake = true;
        Debug.Log("hello this is the fake lever rpc");
        Debug.Log("Setting up lever. fake? : " + isFake);

        if (isFake)
        {
            regularBits.gameObject.SetActive(true);
            blueprintBits.gameObject.SetActive(false);
            Debug.Log("fake lever set up should be complete");

        }
        else if (!isFake)
        {
            Debug.Log("fake lever set up did not work");
        }
    }

    public void ShowBlueprints()
    {
        if (!isShowingBlueprints) blueprintBits.gameObject.SetActive(true);
        else if (isShowingBlueprints) blueprintBits.gameObject.SetActive(false);
        isShowingBlueprints = !isShowingBlueprints;
    }

    public void Interact(CharacterBase characterBase, bool willLoop)
    {
        // PhotonView gameManagerPhotonView = NewGameManager.Instance.GetComponent<PhotonView>();
        if (!characterBase.isFox)
        {
            // Chick pulling normal lever
            progressRoutine = ProgressLoop(characterBase);
            StartCoroutine(progressRoutine);
            progressBar.BeginFadeIn();
        }
        
        else if (isFake && isShowingBlueprints && characterBase.isFox)
        {
            // Fox placing fake lever
            PlaceFakeLever(characterBase);
        }

        //interactable.interactAllowed = false;
    }

    IEnumerator ProgressLoop(CharacterBase characterBase)
    {
        // interactable.photonView.RPC("RPC_ToggleInteractAllowed", RpcTarget.AllBufferedViaServer);

        characterBase._controller._anim.SetBool("LeverBool", true);

        for (var tempProgress = leverProgress; tempProgress < 1; tempProgress += progressIncrement)
        {
            if (characterBase._controller.interactCanceled)
            {
                characterBase._controller.interactCanceled = false;
                HaltProgression(characterBase);

                yield break;
            }

            leverProgress = tempProgress;

            if (leverProgress > 0)
            {
                progressBar.progress = Mathf.FloorToInt(leverProgress * 100);
                progressBar.GetCurrentFill();
            }

            yield return new WaitForSeconds(timeIncrement);
        }

        characterBase._controller._anim.SetBool("LeverBool", false);
        leverProgress = 1;
        hasBeenPulled = true;

        progressBar.progress = 100;
        progressBar.GetCurrentFill();
        

        if (isFake)
        {
            TriggerFakeLever();
        }
        else
        {
            LeverActivated();
        }

        progressRoutine = null;
    }

    private void HaltProgression(CharacterBase characterBase)
    {
        Debug.Log("<color=cyan>HaltProgression called</color>");

        resetRoutine = ResetLoop();
        StartCoroutine(resetRoutine);

        characterBase._controller._anim.SetBool("LeverBool", false);
    }

    private IEnumerator ResetLoop()
    {
        // interactable.photonView.RPC("RPC_ToggleInteractAllowed", RpcTarget.AllBufferedViaServer);

        for (var tempProgress = leverProgress; tempProgress > 0; tempProgress -= 0.025f)
        {
            leverProgress = tempProgress;
            // Debug.Log("leverProgress is " + leverProgress);
            yield return new WaitForSeconds(0.1f);
            progressBar.progress = Mathf.FloorToInt(leverProgress * 100);
            progressBar.GetCurrentFill();
        }

        leverProgress = 0;
        progressBar.progress = 0;
        progressBar.BeginFadeOut();

        resetRoutine = null;
    }

    private void LeverActivated()
    {
        StopCoroutine(progressRoutine);
        progressBar.BeginFadeOut();
        
        leverManager.photonView.RPC("RPC_IncrementLeverCount", RpcTarget.AllBufferedViaServer);
        interactable.photonView.RPC("RPC_ToggleInteractAllowed", RpcTarget.AllBufferedViaServer);
        photonView.RPC("PlayLeverAnimation", RpcTarget.AllBufferedViaServer);
        
        HUDManager.Instance.UpdateInteractionText();
    }

    private void TriggerFakeLever()
    {
        StopCoroutine(progressRoutine);
        progressBar.BeginFadeOut();
        
        audioSource.PlayOneShot(falseLeverAlarm);
        Debug.Log("wee woo wee woo FAKE LEVER");
        regularBits.gameObject.SetActive(false);
    }

    private void PlaceFakeLever(CharacterBase characterBase)
    {
        Debug.Log(" fake lever is READY AND GOGOGO");
        //regularBits.gameObject.SetActive(true);
        //blueprintBits.gameObject.SetActive(false);
        characterBase.hasTrap = false;
        characterBase.isBlueprintActive = false;
        characterBase.hasLever = false;
        photonView.RPC("RPC_SetUpFake", RpcTarget.AllViaServer);
    }

    public void InteractionFocus(bool focussed, CharacterBase character)
    {
        photonView.SetControllerInternal(character.photonView.Owner.ActorNumber);
        if (!photonView.IsMine) return;

        if (!regularBits.activeInHierarchy)
        {
            HUDManager.Instance.UpdateInteractionText();
            return;
        }

        if (focussed && NewGameManager.Instance.myType == PlayerType.Chick)
        {
            HUDManager.Instance.UpdateInteractionText("ACTIVATE");
        }

        if (!focussed)
        {
            HUDManager.Instance.UpdateInteractionText();
            
            if (leverProgress > 0 && !hasBeenPulled)
            {
                StopCoroutine(progressRoutine);
                HaltProgression(character);
            }
        }
    }

    [PunRPC]
    public void PlayLeverAnimation()
    {
        animator.Play("LeverOn");
        photonView.RPC("PlayLeverSound", RpcTarget.Others);
    }

    [PunRPC]
    public void PlayLeverSound()
    {
        Debug.Log("lever sound to others RPC");
        
        if (audioSource != null)
        {
            audioSource.PlayOneShot(leverActivated);
        }
    }
    
    // public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    // {
    //     
    // }
}
