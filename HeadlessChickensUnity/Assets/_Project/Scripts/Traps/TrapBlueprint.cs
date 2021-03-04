using Photon.Pun;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrapBlueprint : MonoBehaviourPunCallbacks
{
    //this script goes on the trap blueprint
    
    public GameObject actualTrapPrefab;

    public Camera _cam;
    private RaycastHit _hit;
    private Vector3 _movePoint;

    private PlayerInput _playerControls;

    private Vector3 _newPosition;
    
    private InputControls _controls;


    private void Awake()
    {
        _controls = new InputControls();
        _controls.Enable();
        _controls.Player.TrapInteract.performed += ctx => SetDown();
    }

    private void Start()
    {
        _cam = Camera.main;

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
        if (gameObject != null)
        {
            gameObject.transform.GetComponentInParent<CharacterBase>().trapSlot = null;
            gameObject.transform.GetComponentInParent<CharacterBase>().hasTrap = false;
            gameObject.transform.GetComponentInParent<CharacterBase>().isBlueprintActive = false;
            
            photonView.RPC("RPC_SpawnTrap", RpcTarget.AllViaServer);
            photonView.RPC("RPC_DestroySelf", RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    public void RPC_SpawnTrap()
    {
        PhotonNetwork.Instantiate(actualTrapPrefab.name,
                new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z),
                gameObject.transform.rotation, 0);
        
    }

    [PunRPC]
    public void RPC_DestroySelf()
    {
        if (gameObject != null)
        {
            _controls.Disable();
            PhotonNetwork.Destroy(gameObject);
        }
    }
}