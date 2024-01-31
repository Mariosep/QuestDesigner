using QuestDesigner;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonStateMachine : StateMachine
{
    public Vector3 Velocity;
    public float MovementSpeed = 5f;
    public float JumpForce = 5f;
    public float CameraSensitivity  = 10f;
    public float SmoothRotation = 1f;
    public float xClamp = 90f;
    public Vector2 frameRotation;
    public Vector2 rotation;
    public CharacterController CharacterController { get; private set; }
    public Transform MainCamera { get; private set; }
    public InputManager inputManager;
    //public UIChannel uiChannel;    
    
    private void Start()
    {
        CharacterController = GetComponent<CharacterController>();
        MainCamera = Camera.main.transform;
        inputManager = ServiceLocator.Get<InputManager>();
        /*uiChannel = ServiceLocator.Get<UIChannel>();

        uiChannel.onMenuOpen += () => ChangeState(new UIState());
        uiChannel.onMenuClosed += () => ChangeState(new MoveState(this));*/
        
        rotation.x = transform.rotation.eulerAngles.y;
        
        ChangeState(new MoveState(this));
    }
}