//BetterUnity // First Person Controller Template

//1. Attach this script to the parent GameObject. (Character, Capsule)
//2. Attach a camera on a child GameObject, and assign it in the inspector.


using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSController : MonoBehaviour
{
    // Information in inspector
    [Header("")]
    
    [Header("2. Attach a camera on a child GameObject, and assign it below.")]
    [Header("1. Attach this script to the parent (Character, Capsule etc) GameObject. ")]
    [Header("A simple First Person controller template.")]

    [Header("")]

    // User inputted speed
    [Tooltip("Speed of movement")]
    [SerializeField] private float speed = 5.0f;

    [Tooltip("Speed of the jump")]
    [SerializeField] private float jumpForce = 5.0f;

    private float input_x;
    private float input_y;

    private Vector3 moveVelocity_horizontal;
    private Vector3 moveVelocity_vertical;
    private Vector3 moveVelocity;

    private Rigidbody rb;

    private float mouseRotation_Y;
    private float mouseRotation_X;

    private Vector3 rotation_player;
    private Vector3 rotation_camera;

    [Tooltip("Sensitivity of looking around")]
    [SerializeField] private float mouseSensitivity = 3.0f;

    private bool isCursorLocked = true;

    [Header("References")]
    [Tooltip("A camera attached to a child GameObject")] public Camera childCamera;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {

        //Fetch input and assign it to our variable
        input_x = Input.GetAxis("Horizontal");
        input_y = Input.GetAxis("Vertical");

        //Velocity for the movement according to the player's current position
        moveVelocity_horizontal = transform.right * input_x;
        moveVelocity_vertical = transform.forward * input_y;

        moveVelocity = (moveVelocity_horizontal + moveVelocity_vertical).normalized * speed; //Final normalized velocity in our desired speed.

        //Fetch and apply mouse movement
        mouseRotation_Y = Input.GetAxisRaw("Mouse X");
        rotation_player = new Vector3(0, mouseRotation_Y, 0) * mouseSensitivity;

        mouseRotation_X = Input.GetAxisRaw("Mouse Y");
        rotation_camera = new Vector3(mouseRotation_X, 0, 0) * mouseSensitivity;


        // Input for jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            moveVelocity = new Vector3(moveVelocity.x, 50 * jumpForce, moveVelocity.z);
            //m_Rigid.AddForce(new Vector3(0, jumpSpeed, 0));
        }   

        //Apply the velocities to our player rigidbody
        if (moveVelocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        }


        if (rotation_player != Vector3.zero)
        {
            //Rotate the camera of the player
            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation_player));
        }

        if (childCamera != null)
        {
            //Negate the child camera rotation so that our rotation is in XY and not just a lateral rotation
            childCamera.transform.Rotate(-rotation_camera);
        }

        CursorBehaviour();

    }

    //Function to deal with cursor focus.
    private void CursorBehaviour()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            isCursorLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isCursorLocked = true;
        }

        Cursor.lockState = isCursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isCursorLocked;
    }

}