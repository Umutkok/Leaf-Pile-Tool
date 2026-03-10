using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    public float playerSpeed = 5.0f;
    public float jumpHeight = 1.5f;
    public float gravityValue = -9.81f;
    public float rotationSpeed = 6f;
    private Vector3 targetDirection;


    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;


    private CharacterController controller;
    private Vector2 m_moveAmt;
    private bool groundedPlayer; 
    private Vector3 playerVelocity;
    private bool PlayerStoped;

    private Vector3 newDir;
    private Vector3 delayedDir;
    public LeafOriginScript leafOriginScript;
    public LeafPileScript leafPileScript;


    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        m_moveAmt = moveAction.action.ReadValue<Vector2>();

        // Rotation
        if (playerVelocity.sqrMagnitude > 0.01f)
        {
            newDir = Vector3.Slerp(
                transform.forward,
                new Vector3(playerVelocity.x, 0, playerVelocity.z),
                rotationSpeed * Time.deltaTime
            );

            transform.rotation = Quaternion.LookRotation(newDir);
        }

        // Jump
        if (jumpAction.action.triggered && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        // Gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
        
        //Final Move
        playerVelocity.x = m_moveAmt.x * playerSpeed;
        playerVelocity.z = m_moveAmt.y * playerSpeed;

        controller.Move(playerVelocity * Time.deltaTime);

        if (leafPileScript.InsidePile) // depandesy var yani birden fazla pile olacağı için manager tarzi bir script den çek
        {
            LeafPile();
        }


    }

    private void LeafPile()
    {   
        
        delayedDir = Vector3.Lerp(delayedDir, playerVelocity, 2f);
        leafOriginScript.SetOppositeMove(delayedDir);

        // If player stops
        if ( !PlayerStoped && !moveAction.action.inProgress)
        {
            leafOriginScript.StopFollowing();
            PlayerStoped = true;
        }

        if ( PlayerStoped && moveAction.action.inProgress)
        {
            leafOriginScript.StartFollowing(this.transform);
            PlayerStoped = false;
        }
    }
}