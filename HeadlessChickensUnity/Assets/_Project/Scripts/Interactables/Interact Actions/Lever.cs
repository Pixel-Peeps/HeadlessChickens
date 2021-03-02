using Photon.Pun;
using UnityEngine;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using PixelPeeps.HeadlessChickens.Network;

public class Lever : MonoBehaviourPunCallbacks, IInteractable
{
    Interactable interactable;
    public LeverManager leverManager;
    public GameObject blueprintBits;
    public GameObject regularBits;
    public bool isFake;

    public Animator animator;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
        leverManager = GameObject.FindWithTag("LeverManager").GetComponent<LeverManager>();
        
        photonView.RPC("RPC_SetUp", RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void RPC_SetUp()
    {
        Debug.Log("Setting up lever. fake? : "+isFake);
        if (isFake)
        {
            blueprintBits.gameObject.SetActive(true);
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
            blueprintBits.gameObject.SetActive(true);
            regularBits.gameObject.SetActive(false);
        }else if (!isFake)
        {
            regularBits.gameObject.SetActive(true);
            blueprintBits.gameObject.SetActive(false);
        }
    }

    public void Interact(CharacterBase characterBase)
    {
       // PhotonView gameManagerPhotonView = NewGameManager.Instance.GetComponent<PhotonView>();
       if (!isFake)
       {
           leverManager.photonView.RPC("RPC_IncrementLeverCount", RpcTarget.AllBufferedViaServer);
           interactable.photonView.RPC("RPC_ToggleInteractAllowed", RpcTarget.AllBufferedViaServer);
           photonView.RPC("PlayLeverAnimation", RpcTarget.AllBufferedViaServer);
       } else if (isFake)
       {
           Debug.Log("Wee woo wee woo fake lever");
       }
       //interactable.interactAllowed = false;
    }

    public void InteractionFocus(bool focussed)
    {
        if (focussed && interactable.interactAllowed)
        {
            Debug.Log("InteractionFocus() true");
        }
        else
        {
            Debug.Log("InteractionFocus() false");
        }
        
    }

    [PunRPC]
    public void PlayLeverAnimation()
    {
        animator.Play("LeverOn");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
