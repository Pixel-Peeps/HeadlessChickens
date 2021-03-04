using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using UnityEngine;


public class TrapPickUp : MonoBehaviourPunCallbacks
{

    //not using these anymore, but keeping JIC
    enum eTrapType
    {
        FalseLever,
        TubOfGlue,
        RottenEgg,
        BrokenEggshells,
        DecoyChick
    }

    [SerializeField] private List<GameObject> chickenTraps;

    [Header("Trap BLUEPRINTS")] public GameObject tubGluePrefab;
    public GameObject rottenEggPrefab;
    public GameObject eggShellPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        chickenTraps.Add(rottenEggPrefab);
        chickenTraps.Add(eggShellPrefab);
    }

    public void OnTriggerEnter(Collider other)
    {
        photonView.SetControllerInternal(other.gameObject.GetComponent<PhotonView>().Owner.ActorNumber);

        if (other.gameObject.GetComponent<CharacterBase>().hasTrap)
        {
            return;
        }

        if (!other.gameObject.GetComponent<CharacterBase>().isFox)
        {

            Debug.Log("Trap: it's a chicken");
            int random = Random.Range(0, chickenTraps.Count + 1);

            if (random < 3)
            {
                //assigning random from egg shells & rotten egg
                other.gameObject.GetComponent<ChickenBehaviour>().trapSlot = chickenTraps[random];
                Debug.Log("Assigning chick trap: " + chickenTraps[random].name);
            }
            /*else if (random == 3)
            {
                //assigning decoy chicken (no blueprint)
                other.gameObject.GetComponent<ChickenBehaviour>().hasDecoy = true;
            }
*/
            other.gameObject.GetComponent<ChickenBehaviour>().hasTrap = true;

            if (gameObject != null)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("Assigning FOX trap");
            // int random = Random.Range(0, 2);
            //
            // if (random == 0)
            // {
            //     //assigning the false lever
            //     Debug.Log("Oh lord he has the lever");
            //     other.gameObject.GetComponent<CharacterBase>().hasLever = true;
            //     other.gameObject.GetComponent<CharacterBase>().hasTrap = true;
            //
            //     if (gameObject != null)
            //     {
            //         PhotonNetwork.Destroy(gameObject);
            //     }
            // }
            // else
            // {
                //assigning the glue tub
                Debug.Log("Oh lord he has the glue");
                other.gameObject.GetComponent<FoxBehaviour>().hasTrap = true;
                other.gameObject.GetComponent<FoxBehaviour>().trapSlot = rottenEggPrefab;

                if (gameObject != null)
                {
                    PhotonNetwork.Destroy(gameObject);
                }
            //}
        }
    }
}
