using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputActions inputActions;
    private InputActions.ActionMapActions actionMap;
    private Player player;

    private void Awake()
    {
        inputActions = new InputActions();
        actionMap = inputActions.ActionMap;

        player = GetComponent<Player>();

        //subscribe to Jump and Attack methods
        actionMap.Jump.started += context => player.Jump();
        actionMap.Attack.performed += context => player.PerformAttack();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
    private void FixedUpdate()
    {
        //pass read input to player movement
        player.Movement(actionMap.Movement.ReadValue<Vector2>());
    }
}
