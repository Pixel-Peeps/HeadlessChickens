using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using Photon.Pun;
using PixelPeeps.HeadlessChickens.Network;
using PixelPeeps.HeadlessChickens.UI;
using System.Collections.Generic;
using Cinemachine;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character

{
    public class ChickenBehaviour : CharacterBase
    {
        public ChickenManager chickenManager;
        public Vector3 positionBeforeHiding;
        
        [Header("Mesh and Materials")]
        [SerializeField] Renderer chickenMesh;
        [SerializeField] Material caughtMat;
        
        [Header("Hiding")]
        public HidingSpot hidedSpot;
        public Transform currentHidingSpot;

        [Header("Following")]
        public ChickenBehaviour chickToFollow;
        public int chickToFollowID;

        public Transform escapeLocation;

        private void Start()
        {
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
        public void ChickenEscaped()
        {
            if (!photonView.IsMine) return;

            if (alreadyEscaped || hasBeenCaught) return;
            
            Debug.Log(gameObject.name + " has escaped");
            
            NewGameManager.Instance.chickensEscaped++;
            HUDManager.Instance.UpdateChickCounter();
            
            
            NewGameManager.Instance.CheckForFinish();
            

            chickenManager.photonView.RPC("UpdateActiveList", RpcTarget.AllViaServer, photonView.ViewID);
            chickenManager.photonView.RPC("UpdateEscapedList", RpcTarget.AllViaServer, photonView.ViewID);

            SwitchToObserverCam();
            alreadyEscaped = true;
            chickenManager.UpdateEscapedChickCam(photonView.ViewID);

            // chickenMesh.enabled = false;
            _rigidbody.isKinematic = true;
            _controller.enabled = false;

            transform.position = escapeLocation.position;


        }

        public void SwitchToObserverCam()
        {
            if (!photonView.IsMine) return;

            var currentFollow = !chickToFollow ? chickToFollow : null;

            int randomInt = UnityEngine.Random.Range(0, chickenManager.activeChicks.Count);

            chickToFollowID = chickenManager.activeChicks[randomInt].photonView.ViewID;
            chickToFollow = PhotonView.Find(chickToFollowID).GetComponent<ChickenBehaviour>();

            if (currentFollow != null) currentFollow.playerCam.gameObject.SetActive(false);
            if(!alreadyEscaped) playerCam.gameObject.SetActive(false);
            Debug.Log("Following " + chickToFollow);
            chickToFollow.playerCam.gameObject.SetActive(true);

            // if chick is watching this cam, they call this method
        }

        protected override void Action()
        {
            throw new System.NotImplementedException();
        }
        
        public override void HidingInteraction(bool canAccessHiding, Transform hideSpot)
        {
            if (photonView.IsMine)
            {
                if (!canAccessHiding && !isHiding)
                {
                    Debug.Log("Someone else is already in there!");
                    return;
                }
                
                switch (isHiding)
                {
                    case true:
                        photonView.RPC("RPC_LeaveHiding", RpcTarget.AllViaServer, positionBeforeHiding);
                        photonView.RPC("RPC_SetParent", RpcTarget.AllViaServer);
                        hidedSpot.photonView.RPC("RPC_ToggleAccess", RpcTarget.AllViaServer);
                        break;
                    case false:
                        currentHidingSpot = hideSpot;
                        photonView.RPC("RPC_EnterHiding", RpcTarget.AllViaServer, currentHidingSpot.GetComponent<PhotonView>().ViewID);
                        photonView.RPC("RPC_SetParent", RpcTarget.AllViaServer);
                        
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
            Debug.Log("after set parent");
            
            // move player into the hiding spot
            gameObject.transform.position = PhotonView.Find(hideViewID).gameObject.transform.position;
            Debug.Log("after gameObject.transform.position = currentHidingSpot.position");
           
        }

        [PunRPC]
        public void RPC_LeaveHiding(Vector3 leavePos)
        {
            isHiding = false;

            // gameObject.transform.SetParent(null);
            photonView.RPC("RPC_SetParent", RpcTarget.AllViaServer);

            //photonView.transform.SetParent(null);
            photonView.transform.position = positionBeforeHiding; 
            
            // unlock physics on leaving
            _rigidbody.isKinematic = false;

            // Re-enable Mesh
            chickenMesh.enabled = true;
            
            SwitchState(EStates.Moving);
        }

        [PunRPC]
        private void RPC_SetParent()
        {
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
    }
}
