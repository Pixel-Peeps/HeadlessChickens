using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;


namespace com.pixelpeeps.headlesschickens
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {

        public Camera _camera;
        //private Rigidbody rB;
        private nCharacterController charController;

        [Tooltip("The current Health of our player")]
        public float Health = 10f;

        public Text healthDisplay;
        
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        // Start is called before the first frame update
        void Awake()
        {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }

            charController = gameObject.GetComponent<nCharacterController>();
           // rB = gameObject.GetComponent<Rigidbody>();
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            
            if (photonView.IsMine)
            {
                //this.ProcessInputs();
                if (Health <= 0f)
                {
                    GameManager.Instance.LeaveRoom();
                }
            }

            healthDisplay.text = Health.ToString();
        }

        void Start()
        {
            #if UNITY_5_4_OR_NEWER
            // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            #endif

            //Camera _camera = Camera.main;
            //Camera _camera = gameObject.GetComponent<Camera>();


            //if (_camera != null)
            //{
                if (photonView.IsMine)
                {
                    _camera.gameObject.SetActive(true);
                   Rigidbody rB = gameObject.AddComponent<Rigidbody>();
                   rB.useGravity = false;
                   charController.enabled = true;


                   //_camera.gameObject.transform.SetParent(this.transform);
                   //_camera.gameObject.transform.position = new Vector3(-0.43f,1.8f,-4.4f);
                   //_camera.gameObject.transform.rotation = new Quaternion(14.6f, 0, 0,0);
                }
            }
           // else
           // {
          //      Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
          //  }
       // }
        
        #if UNITY_5_4_OR_NEWER
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }
        #endif
        
        #region IPunObservable implementation


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                //stream.SendNext(IsFiring);
                stream.SendNext(Health);
            }
            else
            {
                // Network player, receive data
                //this.IsFiring = (bool)stream.ReceiveNext();
                this.Health = (float)stream.ReceiveNext();
            }
            
        }
        
        

        #endregion
        
        #if !UNITY_5_4_OR_NEWER
        /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
        void OnLevelWasLoaded(int level)
        {
             this.CalledOnLevelWasLoaded(level);
        }
        #endif


        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }
        }
        /// <summary>
        /// MonoBehaviour method called when the Collider 'other' enters the trigger.
        /// Affect Health of the Player if the collider is a beam
        /// Note: when jumping and firing at the same, you'll find that the player's own beam intersects with itself
        /// One could move the collider further away to prevent this or check if the beam belongs to the player.
        /// </summary>
        void OnTriggerEnter(Collider other)
        {
            Debug.Log("Trigger enter");
            if (!photonView.IsMine)
            {
                return;
            }
            // We are only interested in Beamers
            // we should be using tags but for the sake of distribution, let's simply check by name.
            if (other.CompareTag("Player"))
            {
                Health -= 1f;
                Debug.Log(this.name + Health);
            }
            //Health -= 0.1f;
        }
        
#if UNITY_5_4_OR_NEWER
        public override void OnDisable()
        {
            // Always call the base to remove callbacks
            base.OnDisable ();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }
#endif
        
    }
}
