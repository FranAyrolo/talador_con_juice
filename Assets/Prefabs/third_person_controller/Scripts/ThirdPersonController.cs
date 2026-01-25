using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{

    [Tooltip("Speed ​​at which the character moves. It is not affected by gravity or jumping.")]
    public float velocity = 5f;
    [Tooltip("This value is added to the speed value while the character is sprinting.")]
    public float sprintAdittion = 3.5f;
    [Tooltip("Force that pulls the player down. Changing this value causes all movement, jumping and falling to be changed as well.")]
    public float gravity = 9.8f;
    //Aca van los hombros y codo para poder animar
    [Tooltip("Transform Hombro Derecho")]
    public Transform trans_right_shoulder = null;
    [Tooltip("Transform Codo Derecho")]
    public Transform trans_right_elbow = null;
    [Tooltip("Transform Hombro Izquierdo")]
    public Transform trans_left_shoulder = null;

    [Tooltip("Transform Codo Izquierdo")]
    public Transform trans_left_elbow = null;

    // Player states
    bool isSprinting = false;

    // Inputs
    float inputHorizontal;
    float inputVertical;
    bool inputSprint;
    bool inputLeftClick;
    bool inputRightClick;

    Animator animator;
    CharacterController cc;


    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (trans_right_shoulder == null || trans_right_elbow == null || 
                trans_left_shoulder == null || trans_left_elbow == null)
        {
            Debug.LogError("NO PUSISTE ALGÚN CODO U HOMBRO");
            //Este quit() no hace nada por algún motivo, y no creo que John Unity esté al tanto
            Application.Quit();
        }
    }


    private void LateUpdate() 
    {
        trans_right_shoulder.Rotate(Vector3.up);
        trans_right_elbow.Rotate(Vector3.right);
        trans_left_shoulder.Rotate(Vector3.up);
        trans_left_elbow.Rotate(Vector3.left);
    }

    // Update is only being used here to identify keys and trigger animations
    void Update()
    {

        // Input checkers
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
        inputSprint = Input.GetAxis("Fire3") == 1f;
        inputLeftClick = Input.GetAxis("Fire1") == 1f;
        if (Input.GetMouseButton(0))
        {
            Debug.Log("hizo click");
        }


        // Unfortunately GetAxis does not work with GetKeyDown, so inputs must be taken individually
        //inputCrouch = Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton1);

        if ( cc.isGrounded && animator != null )
        {
            // Run
            float minimumSpeed = 0.9f;
            animator.SetBool("run", cc.velocity.magnitude > minimumSpeed );

            // Sprint
            isSprinting = cc.velocity.magnitude > minimumSpeed && inputSprint;
            animator.SetBool("sprint", isSprinting );

        }
    }


    // With the inputs and animations defined, FixedUpdate is responsible for applying movements and actions to the player
    private void FixedUpdate()
    {

        float velocityAdittion = 0;
        if ( isSprinting ) 
        {
            velocityAdittion = sprintAdittion;
        }
        // Direction movement
        float directionX = inputHorizontal * (velocity + velocityAdittion) * Time.deltaTime;
        float directionZ = inputVertical * (velocity + velocityAdittion) * Time.deltaTime;
        float directionY = 0;

        // Add gravity to Y axis
        //TODO sin botón de salto no me hace falta esto
        directionY -= gravity * Time.deltaTime;

        
        // --- Character rotation --- 
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Relate the front with the Z direction (depth) and right with X (lateral movement)
        forward *= directionZ;
        right *= directionX;

        if (directionX != 0 || directionZ != 0)
        {
            float angle = Mathf.Atan2(forward.x + right.x, forward.z + right.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
        }

        // --- End rotation ---

        
        Vector3 verticalDirection = Vector3.up * directionY;
        Vector3 horizontalDirection = forward + right;

        Vector3 movement = verticalDirection + horizontalDirection;
        cc.Move( movement );

    }

}