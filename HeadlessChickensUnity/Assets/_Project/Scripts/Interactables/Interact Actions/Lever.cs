using Photon.Pun;
using UnityEngine;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using PixelPeeps.HeadlessChickens.Network;
using PixelPeeps.HeadlessChickens.UI;
// ReSharper disable UnusedMember.Global

public class Lever : MonoBehaviourPunCallbacks, IInteractable
{
    private Interactable interactable;
    public LeverManager leverManager;
    public GameObject blueprintBits;
    public GameObject regularBits;
    public bool isFake = true;
    public bool isActive;
    public bool isShowingBlueprints;
    public Animator animator;


    private void Awake()
    {
        isFake = true;
    }

    private void Start()
    {
        interactable = GetComponent<Interactable>();
        leverManager = GameObject.FindWithTag("LeverManager").GetComponent<LeverManager>();

        if (!isFake)
        {
            photonView.RPC("RPC_SetUp", RpcTarget.AllViaServer);
        }

        if (isFake)
        {
            photonView.RPC("RPC_SetUp", RpcTarget.AllViaServer, true);
        }
    }

    [PunRPC]
    public void RPC_SetUp()
    {
        Debug.Log("Setting up lever. fake? : "+isFake);
        if (isFake)
        {
            //if fake then
            blueprintBits.gameObject.SetActive(false);
            regularBits.gameObject.SetActive(false);
            
        }else if (!isFake)
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
        Debug.Log("Setting up lever. fake? : "+isFake);
        if (isFake)
        {
            blueprintBits.gameObject.SetActive(false);
            regularBits.gameObject.SetActive(false);
        }else if (!isFake)
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
        Debug.Log("Setting up lever. fake? : "+isFake);
        
        if (isFake)
        {
            regularBits.gameObject.SetActive(true);
            blueprintBits.gameObject.SetActive(false);
            Debug.Log("fake lever set up should be complete");
            
        }else if (!isFake)
        {
            Debug.Log("fake lever set up did not work");
        }
    }

    public void ShowBlueprints()
    {
        if(!isShowingBlueprints) blueprintBits.gameObject.SetActive(true);
        else if (isShowingBlueprints) blueprintBits.gameObject.SetActive(false);
        isShowingBlueprints = !isShowingBlueprints;
    }

    public void Interact(CharacterBase characterBase, bool willLoop)
    {
       // PhotonView gameManagerPhotonView = NewGameManager.Instance.GetComponent<PhotonView>();
       if (!isFake && !characterBase.isFox)
       {
           leverManager.photonView.RPC("RPC_IncrementLeverCount", RpcTarget.AllBufferedViaServer);
           interactable.photonView.RPC("RPC_ToggleInteractAllowed", RpcTarget.AllBufferedViaServer);
           photonView.RPC("PlayLeverAnimation", RpcTarget.AllBufferedViaServer);
           HUDManager.Instance.UpdateInteractionText();
           
       } else if (isFake && !characterBase.isFox)
       {
           Debug.Log("wee woo wee woo FAKE LEVER");
           regularBits.gameObject.SetActive(false);
       }
       else if (isFake && isShowingBlueprints && characterBase.isFox)
       {
           Debug.Log(" fake lever is READY AND GOGOGO");
           //regularBits.gameObject.SetActive(true);
           //blueprintBits.gameObject.SetActive(false);
           characterBase.hasTrap = false;
           characterBase.isBlueprintActive = false;
           characterBase.hasLever = false;
           photonView.RPC("RPC_SetUpFake", RpcTarget.AllViaServer);

       } 
       //interactable.interactAllowed = false;
    }

    public void InteractionFocus(bool focussed, CharacterBase character)
    {        
        photonView.SetControllerInternal(character.photonView.Owner.ActorNumber);
        if (!photonView.IsMine) return;
        
        if ( !regularBits.activeInHierarchy )
        {
            HUDManager.Instance.UpdateInteractionText();
            return;
        }
        
        if (focussed)
        {
            HUDManager.Instance.UpdateInteractionText("ACTIVATE");
        }

        if (!focussed)
        {
            HUDManager.Instance.UpdateInteractionText();
        }
    }

    [PunRPC]
    public void PlayLeverAnimation()
    {
        animator.Play("LeverOn");
    }

    // public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    // {
    //     
    // }
}
