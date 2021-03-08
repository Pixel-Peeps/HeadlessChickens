using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Photon.Pun;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using UnityEngine.UI;
using UnityEngine.VFX;


namespace com.pixelpeeps.headlesschickens
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public Camera _camera;
        private CharacterInput charController;
        private CharacterBlueprintToggle _characterBlueprintToggle;
        public GameObject vCam;

        [Tooltip("The current Health of our player")]
        public float Health = 10f;
        
        
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;
        
        void Awake()
        {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }

            charController = gameObject.GetComponent<CharacterInput>();
            _characterBlueprintToggle = gameObject.GetComponentInChildren<CharacterBlueprintToggle>();
            
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
           DontDestroyOnLoad(this.gameObject);
        }
        
        void Start()
        {
#if UNITY_5_4_OR_NEWER
            // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
            
            if (photonView.IsMine)
            {
                //inst a rigidbody, this may not be necessary but otherwise maybe later problems
                //Rigidbody rB = gameObject.AddComponent<Rigidbody>();
                //rB.useGravity = false;
                
                charController.enabled = true;
                _characterBlueprintToggle.enabled = true;
                vCam.SetActive(true);

                //photonView.RPC("RPC_ChangeHealth", RpcTarget.All);
               // healthDisplay.text = this.Health.ToString("0");
            }
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
                //what does this do?
                //this.ProcessInputs();
                
                if (Health <= 0f)
                {
                    Health = 30;
                    
                    //GameManager.Instance.LeaveRoom();
                }
            }
        }

#if UNITY_5_4_OR_NEWER
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }
#endif
       
        
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

#if UNITY_5_4_OR_NEWER
        public override void OnDisable()
        {
            // Always call the base to remove callbacks
            base.OnDisable ();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            //throw new System.NotImplementedException();
        }
#endif

    }
}
