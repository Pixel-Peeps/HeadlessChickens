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
        
        [Header("Mesh and Materials")]
        [SerializeField] Renderer chickenMesh;
        [SerializeField] Material caughtMat;
        [SerializeField] Material normalMat;
        
        [Header("Hiding")]
        public HidingSpot hidedSpot;
        public Transform currentHidingSpot;

        [Header("Following")]
        public ChickenBehaviour chickToFollow;
        public ChickenBehaviour currentFollow;
        public int chickToFollowID;
        [FormerlySerializedAs("followIntID")] public int followInt;

        public Transform escapeLocation;

        public InputControls controls;

        public bool spectating;
        
        private new void Awake()
        {
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

        [PunRPC]
        public void ChickenCaptured()
        {
            if (hasBeenCaught || alreadyEscaped) return;
            
            chickenMesh.GetComponent<Renderer>().sharedMaterial = caughtMat;
            
            NewGameManager.Instance.chickensCaught++;
            HUDManager.Instance.UpdateChickCounter();
            
            hasBeenCaught = true;
            NewGameManager.Instance.CheckForFinish();

            chickenManager.activeChicks.Remove(this);
        }

        [PunRPC]
        public void RestoreHead()
        {
            chickenMesh.GetComponent<Renderer>().sharedMaterial = normalMat;
            hasBeenCaught = false;
        }

        /*############################################
        *           CHICKEN ESCAPED / SPEC CAM       *
        * ###########################################*/
        
        #region CHICKEN ESCAPED / SPEC CAM
        
        [PunRPC]
        public void ChickenEscaped()
        {
            if (!photonView.IsMine || alreadyEscaped || hasBeenCaught) return;
            
            //NewGameManager.Instance.chickensEscaped++;
            //HUDManager.Instance.UpdateChickCounter();
            
            //NewGameManager.Instance.CheckForFinish();

            chickenManager.photonView.RPC("UpdateActiveList", RpcTarget.AllViaServer, photonView.ViewID);
            chickenManager.photonView.RPC("UpdateEscapedList", RpcTarget.AllViaServer, photonView.ViewID);

            // Switch camera to an active chick in level
            SwitchToObserverCam();

            photonView.RPC("UpdateAlreadyEscaped", RpcTarget.AllBufferedViaServer);
            // alreadyEscaped = true;

            chickenManager.photonView.RPC("UpdateEscapedChickCam", RpcTarget.AllViaServer, photonView.ViewID);

            // chickenMesh.enabled = false;
            
            // Disable rigidbody and player controls after escaping the level
            _rigidbody.isKinematic = true;
            _controller.enabled = false;

            // Place chick mesh in away from the game map after escape
            transform.position = escapeLocation.position;


        }

        [PunRPC]
        public void UpdateAlreadyEscaped()
        {
            alreadyEscaped = true;
        }

        [PunRPC]
        public void UpdateChickToFollow(int ID)
        {
            spectating = true;
            controls.Spectator.Enable();
            
            HUDManager.Instance.EnableSpectatorHUD();
            
            // if you are currently following a chick, store that chicken before assigning a new one
            currentFollow = 
                chickToFollow != null ? chickToFollow : null;
            
            // Assign the new chick that you are going to follow
            chickToFollowID = ID;
            chickToFollow = PhotonView.Find(chickToFollowID).GetComponent<ChickenBehaviour>();
            
            // Update HUD of chick that is being followed
            HUDManager.Instance.UpdateSpectatorHUD(chickToFollow.photonView.Owner.NickName);
            
            Debug.Log("<color=lime>" + photonView.Owner.NickName + "s currentFollow is: " + currentFollow.photonView.Owner.NickName + "</color>");
            Debug.Log("<color=cyan>" + photonView.Owner.NickName + "s Follow Changing to: " + chickToFollow.photonView.Owner.NickName + "</color>");
            
        }
        
        public void SwitchToObserverCam()
        {
            while (true)
            {
                // if (!photonView.IsMine) return;
                
                // Pick a random chick from the currently active chicks
                followInt = UnityEngine.Random.Range(0, chickenManager.activeChicks.Count);
                chickToFollowID = chickenManager.activeChicks[followInt].photonView.ViewID;
                
                // if the found chick is yours find another - in case of list update delay.
                if (chickToFollowID == photonView.ViewID)
                {
                    continue;
                }
                
                // Update who you are currently following
                photonView.RPC("UpdateChickToFollow", RpcTarget.AllViaServer, chickToFollowID);
                Debug.Log("<color=cyan>Following " + chickToFollow.photonView.Owner.NickName + "</color>");
                
                // Switch your camera
                photonView.RPC("RPC_CamSwitch", RpcTarget.AllViaServer, photonView.ViewID);
                
                break;
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
            if (currentFollow != null) currentFollow.playerCam.gameObject.SetActive(false);
            
            // Make sure your camera is turned off
            if (!alreadyEscaped) playerCam.gameObject.SetActive(false);
            
            // Activate camera of chick you are going to follow next
            chickToFollow.playerCam.gameObject.SetActive(true);
        }

        public void NextSpecCam()
        {
            if (!spectating) return;
            
            
            if (followInt == chickenManager.activeChicks.Count - 1)
            {
                followInt = - 1;
            }

            followInt++;
            
            // Chicken you are going to follow next
            chickToFollowID = chickenManager.activeChicks[followInt].photonView.ViewID;
            
            // Update who you are currently following
            photonView.RPC("UpdateChickToFollow", RpcTarget.AllViaServer, chickToFollowID);
            
            // Switch your camera
            photonView.RPC("RPC_CamSwitch", RpcTarget.AllViaServer, photonView.ViewID);
        }

        public void PreviousSpecCam()
        {
            if (!spectating) return;
            
            if (followInt == 0)
            {
                followInt = chickenManager.activeChicks.Count;
            }

            followInt--;
            
            // Chicken you are going to follow next
            chickToFollowID = chickenManager.activeChicks[followInt].photonView.ViewID;
            
            // Update who you are currently following
            photonView.RPC("UpdateChickToFollow", RpcTarget.AllViaServer, chickToFollowID);
            
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
            
            // log position before entering hiding as a return position when leaving
            positionBeforeHiding = transform.position;

            // Disable Mesh
            chickenMesh.enabled = false;

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
            }

            playerCam.gameObject.SetActive(false);
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
            chickenMesh.enabled = true;

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
    }
}
