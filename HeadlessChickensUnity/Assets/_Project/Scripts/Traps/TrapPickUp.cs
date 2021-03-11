using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using PixelPeeps.HeadlessChickens.Network;
using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;


public class TrapPickUp : MonoBehaviourPunCallbacks
{
    private Material origMat;
    public Material invalidMat;
    public GameObject eggPart;
    private bool matChanged;
    private MeshRenderer mR;
    private bool isOrigColour;
    private AudioSource _audioSource;
    public AudioClip pickUpSoundEffect;
    
    [SerializeField] private List<GameObject> chickenTraps;

    [Header("Trap BLUEPRINTS")] public GameObject tubGluePrefab;
    public GameObject rottenEggPrefab;
    public GameObject eggShellPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = Camera.main.GetComponent<AudioSource>();
        chickenTraps.Add(rottenEggPrefab);
        chickenTraps.Add(eggShellPrefab);
        
        mR = eggPart.GetComponent<MeshRenderer>();
        origMat = mR.material;
        isOrigColour = true;
    }

    public IEnumerator BetterLuckNextTime()
    {
       // mR.material = invalidMat;
       
        photonView.RPC("RPC_ChangeColour", RpcTarget.AllBufferedViaServer);
        yield return new WaitForSeconds(0.7f);
       // mR.material = origMat;
        photonView.RPC("RPC_ChangeColour", RpcTarget.AllBufferedViaServer);
        matChanged = false;

    }

    [PunRPC]
    public void RPC_ChangeColour()
    {
        if (isOrigColour)
        {
            this.mR.material = invalidMat;
            isOrigColour = false;
        }
        else
        {  
            this.mR.material = origMat;
            isOrigColour = true;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        photonView.SetControllerInternal(other.gameObject.GetComponent<PhotonView>().Owner.ActorNumber);

        if (other.gameObject.GetComponent<CharacterBase>().hasTrap || other.gameObject.GetComponent<CharacterBase>().hasBeenCaught)
        {
            Debug.Log("has trap already! or is caught!");
            if (!matChanged)
            {
                matChanged = true;
                StartCoroutine(BetterLuckNextTime());
            }

            return;
        }
        
        _audioSource.PlayOneShot(pickUpSoundEffect);

        if (!other.gameObject.GetComponent<CharacterBase>().isFox)
        {
            int random = Random.Range(0, chickenTraps.Count);
            random = 3;

            if (random < 3)
            {
                //assigning random from egg shells & rotten egg
                other.gameObject.GetComponent<ChickenBehaviour>().trapSlot = chickenTraps[random];
                Sprite icon = chickenTraps[random].GetComponent<TrapBlueprint>().trapIcon;
                HUDManager.Instance.ShowItemImage(icon);
                other.gameObject.GetComponent<ChickenBehaviour>().blueprintIndex = random;
            }
            else if (random == 3)
            {
                // assigning decoy chicken (no blueprint)
                ChickenBehaviour chick = other.gameObject.GetComponent<ChickenBehaviour>();
                chick.photonView.RPC("RPC_ToggleDecoy", RpcTarget.AllBufferedViaServer, true);
                
                //other.gameObject.GetComponent<ChickenBehaviour>().hasDecoy = true;
            }

            other.gameObject.GetComponent<ChickenBehaviour>().hasTrap = true;

            if (gameObject != null)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("Assigning FOX trap");
            if (NewGameManager.Instance.canBeFakeLever)
            {
                int random = Random.Range(0, 2);

                if (random == 0)
                {
                    //assigning the false lever
                    Debug.Log("Oh lord he has the lever");
                    other.gameObject.GetComponent<CharacterBase>().hasLever = true;
                    other.gameObject.GetComponent<CharacterBase>().hasTrap = true;

                    if (gameObject != null)
                    {
                        PhotonNetwork.Destroy(gameObject);
                    }
                }
                else
                {
                    // assigning the glue tub
                    other.gameObject.GetComponent<FoxBehaviour>().hasTrap = true;
                    other.gameObject.GetComponent<FoxBehaviour>().trapSlot = tubGluePrefab;
                    Sprite icon = tubGluePrefab.GetComponent<TrapBlueprint>().trapIcon;
                    HUDManager.Instance.ShowItemImage(icon);

                    if (gameObject != null)
                    {
                        PhotonNetwork.Destroy(gameObject);
                    }
                }
            }
            else
            {
                // assigning the glue tub
                other.gameObject.GetComponent<FoxBehaviour>().hasTrap = true;
                other.gameObject.GetComponent<FoxBehaviour>().trapSlot = tubGluePrefab;
                Sprite icon = tubGluePrefab.GetComponent<TrapBlueprint>().trapIcon;
                HUDManager.Instance.ShowItemImage(icon);

                if (gameObject != null)
                {
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }
}

