using Photon.Pun;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrapBlueprint : MonoBehaviourPunCallbacks
{
    //this script goes on the trap blueprint
    
    public GameObject actualTrapPrefab;

    //public Camera _cam;
    private RaycastHit _hit;
    private Vector3 _movePoint;

    private PlayerInput _playerControls;

    private Vector3 _newPosition;
    
    private InputControls _controls;


    public virtual void OnEnable()
    {
        //base.OnEnable();
        _controls = new InputControls();
        _controls.Enable();
        _controls.Player.TrapInteract.performed += ctx => SetDown();
    }

    private void Start()
    {
       // _cam = Camera.main;

        /*if (!(_cam is null))
        {
            Ray ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out _hit, 50000.0f, 1 << 11))
            {
                _newPosition = _hit.point;
            }
        }
        */
    }

    private void Update()
    { //leaving this here in case we need to return to a raycast solution
        /*
        Ray ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        // if (Physics.Raycast(ray, out _hit, 50000.0f, (1 << 11)))
        if (Physics.Raycast(ray, out _hit))
        {
            _newPosition = new Vector3(_hit.point.x, 0f, _hit.point.z);
            transform.position = _newPosition;
        }
        */
    }

    private void SetDown()
    {
        //gameObject != null &&
        if ( gameObject.transform.GetComponentInParent<CharacterBase>().isBlueprintActive)
        {
            gameObject.transform.GetComponentInParent<CharacterBase>().trapSlot = null;
            gameObject.transform.GetComponentInParent<CharacterBase>().hasTrap = false;
            gameObject.transform.GetComponentInParent<CharacterBase>().isBlueprintActive = false;
            
            
            photonView.RPC("RPC_SpawnTrap", RpcTarget.AllBufferedViaServer);
            //photonView.RPC("RPC_DestroySelf", RpcTarget.AllViaServer);
            _controls.Disable();
            gameObject.GetComponentInParent<CharacterBase>().ToggleBP(false);
            
        }
    }

    [PunRPC]
    public void RPC_SpawnTrap()
    {
        if (photonView.IsMine)
        {
            gameObject.GetComponentInParent<CharacterBase>().hasTrap = false;
            gameObject.GetComponentInParent<CharacterBase>().isBlueprintActive= false;
            PhotonNetwork.Instantiate(actualTrapPrefab.name,
                gameObject.transform.position,
                gameObject.transform.rotation, 0);
        }
    }

    [PunRPC]
    public void RPC_DestroySelf()
    {
        if (gameObject != null)
        {
            Debug.Log("destroying blueprint!");
            _controls.Disable();
           // PhotonNetwork.Destroy(gameObject);
           gameObject.GetComponentInParent<CharacterBase>().ToggleBP(false);
           gameObject.GetComponentInParent<CharacterBase>().isBlueprintActive= false;
        }
    }

    public virtual void OnDisable()
    {
        Debug.Log("destroying blueprint!");
        _controls.Disable();
        // PhotonNetwork.Destroy(gameObject);
        gameObject.GetComponentInParent<CharacterBase>().ToggleBP(false);
        gameObject.GetComponentInParent<CharacterBase>().isBlueprintActive= false;
    }
}