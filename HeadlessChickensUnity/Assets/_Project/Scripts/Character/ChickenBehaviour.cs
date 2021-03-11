using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using Photon.Pun;
using PixelPeeps.HeadlessChickens.Network;
using PixelPeeps.HeadlessChickens.UI;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.Serialization;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character

{
    public class ChickenBehaviour : CharacterBase
    {
        public ChickenManager chickenManager;
        public Vector3 positionBeforeHiding;

        [Header("Modes")]
        [SerializeField] GameObject normalChick;
        [SerializeField] GameObject headlessChick;

        [Header("Mesh and Materials")]
        [SerializeField] GameObject chickenMesh;
        [SerializeField] Material caughtMat;
        [SerializeField] Material normalMat;
        
        
        

        [Header("Following")]
        public ChickenBehaviour currentFollow;
        public ChickenBehaviour chickWasFollowing;
        public int currentFollowID;
        [FormerlySerializedAs("followIntID")] public int followInt;

        public Transform escapeLocation;
        public float decoyDuration = 4f;

        public InputControls controls;

        public bool spectating;
        public bool isLastChick = false;

        [Header("Glue Tub Effect")] private float origChickenSpeed;
        public float effectDuration = 4;
        
        [Header("Sound Effects")]
        public AudioClip chickenEscapedSFX;
        private AudioSource _audioSource;

       
        

        
        private new void Awake()
        {
            _audioSource = Camera.main.GetComponent<AudioSource>();
            base.Awake();
            controls = new InputControls();
            
            controls.Spectator.Disable();

            controls.Spectator.NextPlayer.performed += _ => NextSpecCam();
            controls.Spectator.PreviousPlayer.performed += _ => PreviousSpecCam();
        }
        
        private void Start()
        {
            spectating = false;
            // chickenMesh = GetComponent<Renderer>();
            chickenManager = FindObjectOfType<ChickenManager>();
            chickenManager.activeChicks.Add(this);
            escapeLocation = GameObject.FindGameObjectWithTag("Sanctuary").transform.GetChild(0);
        }

        // [PunRPC]
        public void ChickenCaptured()
        {
            if (hasBeenCaught || alreadyEscaped) return;

            Debug.Log("<color=green>" + photonView.Owner.NickName + " called ChickenCaptured()</color>");
            
            chickenManager.photonView.RPC("UpdateDeadList", RpcTarget.AllViaServer, photonView.ViewID);
            chickenManager.photonView.RPC("UpdateActiveList", RpcTarget.AllViaServer, photonView.ViewID);

            photonView.RPC("SwitchToHeadless", RpcTarget.AllBufferedViaServer, photonView.ViewID);
        }

        [PunRPC]
        private void SwitchToHeadless(int chickID)
        {
            HUDManager.Instance.UpdateChickCounter();

            if (photonView.ViewID == chickID)
            {
                normalChick.SetActive(false);
                headlessChick.SetActive(true);
                ToggleBP(false);
                hasTrap = false;
                // NewGameManager.Instance.chickensCaught++;

                hasBeenCaught = true;
                _controller.SwapAnimator();
            }
        }

        [PunRPC]
        public void RestoreHead()
        {
            normalChick.SetActive(true);
            headlessChick.SetActive(false);

            hasBeenCaught = false;
        }

        /*############################################
        *           CHICKEN ESCAPED / SPEC CAM       *
        * ###########################################*/
        
        #region CHICKEN ESCAPED / SPEC CAM
        
        // [PunRPC]
        public void ChickenEscaped()
        {

            if (alreadyEscaped || hasBeenCaught) return;

            // photonView.RPC("UpdateAlreadyEscaped", RpcTarget.AllBufferedViaServer);
            

            // NewGameManager.Instance.chickensEscaped++;
            

            // NewGameManager.Instance.CheckForFinish();


            if (photonView.IsMine)
            {
                // alreadyEscaped = true;

                // chickenManager.UpdateActiveList(photonView.ViewID);
                // chickenManager.UpdateEscapedList(photonView.ViewID);

                chickenManager.photonView.RPC("UpdateActiveList", RpcTarget.AllViaServer, photonView.ViewID);
                chickenManager.photonView.RPC("UpdateEscapedList", RpcTarget.AllViaServer, photonView.ViewID);

                // Switch camera to an active chick in level

                if(!isLastChick)
                {
                    _audioSource.PlayOneShot(chickenEscapedSFX);
                    SwitchToObserverCam();
                    chickenManager.UpdateEscapedChickCam(photonView.ViewID);
                       
                }

                // chickenManager.photonView.RPC("UpdateEscapedChickCam", RpcTarget.AllViaServer, chickToFollowID);

                // chickenMesh.enabled = false;
                // chickenManager.UpdateEscapedChickCam(photonView.ViewID);

            }
        }

        [PunRPC]
        public void MoveToSantuary(int chickID)
        {
            HUDManager.Instance.UpdateChickCounter();

            if (photonView.ViewID == chickID)
            {
                // alreadyEscaped = true;
                
                // Disable rigidbody and player controls after escaping the level
                _rigidbody.isKinematic = true;
                _controller.enabled = false;
                
                
                
                // Place chick mesh in away from the game map after escape
                transform.position = escapeLocation.position;
            }
        }

        [PunRPC]
        public void UpdateChickToFollow(int ID)
        {
            // if you are currently following a chick, store that chicken before assigning a new one
            chickWasFollowing = 
                currentFollow != null ? currentFollow : null;
            
            // Assign the new chick that you are going to follow
            currentFollowID = ID;
            currentFollow = PhotonView.Find(currentFollowID).GetComponent<ChickenBehaviour>();

            if (photonView.IsMine)
            {
                // Update HUD of chick that is being followed
                HUDManager.Instance.UpdateSpectatorHUD(currentFollow.photonView.Owner.NickName);
            }
            
            if(chickWasFollowing != null)
                Debug.Log("<color=lime>" + photonView.Owner.NickName + "s currentFollow is: " + chickWasFollowing.photonView.Owner.NickName + "</color>");
            
            Debug.Log("<color=cyan>" + photonView.Owner.NickName + "s Follow Changing to: " + currentFollow.photonView.Owner.NickName + "</color>");
            
        }
        
        public void SwitchToObserverCam()
        {
            while (true)
            {
                spectating = true;
                controls.Spectator.Enable();
            
                HUDManager.Instance.EnableSpectatorHUD();
                
                // if (!photonView.IsMine) return;
                
                // Pick a random chick from the currently active chicks
                followInt = UnityEngine.Random.Range(0, chickenManager.activeChicks.Count);
                currentFollowID = chickenManager.activeChicks[followInt].photonView.ViewID;
                
                // if the found chick is yours find another - in case of list update delay.
                if (currentFollowID == photonView.ViewID)
                {
                    continue;
                }
                
                // Update who you are currently following
                photonView.RPC("UpdateChickToFollow", RpcTarget.AllViaServer, currentFollowID);
                
                
                // Switch your camera
                photonView.RPC("RPC_CamSwitch", RpcTarget.AllViaServer, photonView.ViewID);

                photonView.RPC("MoveToSantuary", RpcTarget.AllBufferedViaServer, photonView.ViewID);
                return;
            }
        }

        [PunRPC]
        public void RPC_CamSwitch(int pVid)
        {
            if (!photonView.IsMine) return;
            
            // Turn of the cameras of all escaped chicks
            foreach(ChickenBehaviour chicken in chickenManager.escapedChicks)
            {
                chicken.playerCam.gameObject.SetActive(false);
            }
            
            // If you are currently following someone turn of their camera before activating the camera of new follow target
            if (chickWasFollowing != null) chickWasFollowing.playerCam.gameObject.SetActive(false);
            
            // Make sure your camera is turned off
            if (alreadyEscaped) playerCam.gameObject.SetActive(false);
            
            // Activate camera of chick you are going to follow next
            currentFollow.playerCam.gameObject.SetActive(true);
        }

        public void NextSpecCam()
        {
            if (!spectating) return;
            
            
            if (followInt >= chickenManager.activeChicks.Count - 1)
            {
                followInt = -1;
            }

            followInt++;
            
            // Chicken you are going to follow next
            currentFollowID = chickenManager.activeChicks[followInt].photonView.ViewID;
            
            // Update who you are currently following
            photonView.RPC("UpdateChickToFollow", RpcTarget.AllViaServer, currentFollowID);
            
            // Switch your camera
            photonView.RPC("RPC_CamSwitch", RpcTarget.AllViaServer, photonView.ViewID);
        }

        public void PreviousSpecCam()
        {
            if (!spectating) return;
            
            if (followInt <= 0)
            {
                followInt = chickenManager.activeChicks.Count;
            }

            followInt--;
            
            // Chicken you are going to follow next
            currentFollowID = chickenManager.activeChicks[followInt].photonView.ViewID;
            
            // Update who you are currently following
            photonView.RPC("UpdateChickToFollow", RpcTarget.AllViaServer, currentFollowID);
            
            // Switch your camera
            photonView.RPC("RPC_CamSwitch", RpcTarget.AllViaServer, photonView.ViewID);
        }
        #endregion

        protected override void Action()
        {
            throw new System.NotImplementedException();
        }

        /*########################
        *           HIDING       *
        * #######################*/
        
        #region HIDING
        
        public override void HidingInteraction(bool canAccessHiding, Transform hideSpot)
        {
            if (photonView.IsMine)
            {
                // lock hiding access if the hiding spot is in use
                if (!canAccessHiding && !isHiding)
                {
                    Debug.Log("Someone else is already in there!");
                    return;
                }
                
                // Enter and Leave hiding
                switch (isHiding)
                {
                    case true:
                        photonView.RPC("RPC_LeaveHiding", RpcTarget.AllViaServer, positionBeforeHiding);
                        // photonView.RPC("RPC_SetParent", RpcTarget.AllViaServer);
                        hidedSpot.photonView.RPC("RPC_ToggleAccess", RpcTarget.AllViaServer);
                        break;
                    case false:
                        currentHidingSpot = hideSpot;
                        photonView.RPC("RPC_EnterHiding", RpcTarget.AllViaServer, currentHidingSpot.GetComponent<PhotonView>().ViewID);
                        // photonView.RPC("RPC_SetParent", RpcTarget.AllViaServer);
                        break;
                }
            }
        }

        [PunRPC]
        private void RPC_EnterHiding(int hideViewID)
        {
            Debug.Log("hiding spot view id:: "+hideViewID);
            ToggleBP(false);
            // log position before entering hiding as a return position when leaving
            positionBeforeHiding = transform.position;

            // Disable Mesh
            chickenMesh.SetActive(false);

            // lock physics on entering hiding
            _rigidbody.isKinematic = true;
            
            isHiding = true;
            Debug.Log("after isHiding = true");
            SwitchState(EStates.Hiding);
            Debug.Log("after switch state to hiding");
            
            // Set the current hiding spot as a parent object
            currentHidingSpot = PhotonView.Find(hideViewID).gameObject.transform;
            gameObject.transform.SetParent(PhotonView.Find(hideViewID).gameObject.transform);
            hidedSpot = transform.GetComponentInParent<HidingSpot>();
            Debug.Log("after set parent");
            
            // move player into the hiding spot
            gameObject.transform.position = PhotonView.Find(hideViewID).gameObject.transform.position;
            Debug.Log("after gameObject.transform.position = currentHidingSpot.position");

            if (photonView.IsMine)
            {
                hidedSpot.EnableHidingCam();
                playerCam.gameObject.SetActive(false);
            }
        }

        [PunRPC]
        public void RPC_LeaveHiding(Vector3 leavePos)
        {
            isHiding = false;

            // gameObject.transform.SetParent(null);
            // photonView.RPC("RPC_SetParent", RpcTarget.AllViaServer);
            transform.SetParent(null);

            //photonView.transform.SetParent(null);
            photonView.transform.position = positionBeforeHiding; 
            
            // unlock physics on leaving
            _rigidbody.isKinematic = false;

            // Re-enable Mesh
            chickenMesh.SetActive(true);

            if (photonView.IsMine)
            {
                hidedSpot.DisableHidingCam();
            }

            SwitchState(EStates.Moving);

            if (photonView.IsMine)
            {
                playerCam.gameObject.SetActive(true);
            }
        }

        public IEnumerator DecoyCooldown()
        {
            Debug.Log("Starting decoy cooldown");
            yield return new WaitForSeconds(decoyDuration);
            
            photonView.RPC("RPC_ToggleDecoy", RpcTarget.AllViaServer, false);
        }
        
        
        [PunRPC]
        public void RPC_ToggleDecoy(bool newHasDecoy)
        {
            hasDecoy = newHasDecoy;
            
        }

        [PunRPC]
        private void RPC_SetParent()
        {
            // Set the hiding spot as parent when in hiding, remove the parent on leaving the hiding spot.
            switch (isHiding)
            {
                case true: 
                    transform.SetParent(currentHidingSpot);
                    hidedSpot = transform.GetComponentInParent<HidingSpot>();
                    break;
                case false:
                    transform.SetParent(null);
                    break;
            }
        }
        #endregion
        
        public void StartGlueTubEffect(){
            if (photonView.IsMine)
            {
                //effect
                var victim = gameObject.GetComponent<CharacterInput>();
                movementAffected = true;
                Debug.Log("Let's slow this baby down: " + victim.moveSpeed);
                origChickenSpeed = victim.moveSpeed;
                victim.moveSpeed /= 2;
                StartCoroutine(GlueEffectCoolDown());
            }
        }

        public IEnumerator GlueEffectCoolDown()
        {
            yield return new WaitForSeconds(effectDuration);
            var victim = gameObject.GetComponent<CharacterInput>();
            victim.moveSpeed = origChickenSpeed;
            movementAffected = false;
            Debug.Log("ok, that's enough: "+victim.moveSpeed);
        }
    }
}
