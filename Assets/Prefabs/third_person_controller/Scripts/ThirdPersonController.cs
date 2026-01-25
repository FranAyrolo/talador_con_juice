using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{

    [Tooltip("Speed ​​at which the character moves. It is not affected by gravity or jumping.")]
    public float velocity = 5f;
    [Tooltip("This value is added to the speed value while the character is sprinting.")]
    public float sprintAdittion = 3.5f;
    [Tooltip("Force that pulls the player down. Changing this value causes all movement, jumping and falling to be changed as well.")]
    public float gravity = 9.8f;
    [Tooltip("Multiplicador de la velocidad de rotación de los brazos")]
    public float swingMultiplier = 1.0f;
    [Tooltip("Inclinación máxima de los brazos")]
    public float maxDollyPitch = 70f;
    [Tooltip("Rotación máxima de los brazos")]
    public float maxDollyRotation = 70f;

    //Aca van los hombros y codo para poder animar
    //Nada de esto hace falta que sea publico, lo puedo atrapar por código hardcodeando con la jerarquía
    [Tooltip("Transform Hombro Derecho")]
    public Transform transRightShoulder = null;
    [Tooltip("Transform Codo Derecho")]
    public Transform transRightElbow = null;
    [Tooltip("Transform Hombro Izquierdo")]
    public Transform transLeftShoulder = null;
    [Tooltip("Transform Codo Izquierdo")]
    public Transform transLeftElbow = null;
    [Tooltip("Transform Codo Izquierdo")]
    public Transform armsDolly = null;

    Transform transBaseRight = null;
    Transform transBaseLeft = null;
    Transform lookAtRight = null;
    Transform lookAtLeft = null;
    

    // Player states
    bool isSprinting = false;

    // Inputs
    float inputHorizontal;
    float inputVertical;
    bool inputSprint;
    bool inputLeftClick;
    bool inputRightClick;
    float mouseHorizontalSpeed;
    float mouseVerticalSpeed;
    float dollyRotation;
    float armsPitch;

    Animator animator;
    CharacterController cc;
    

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (transRightShoulder == null || transRightElbow == null || 
                transLeftShoulder == null || transLeftElbow == null ||
                    armsDolly == null)
        {
            Debug.LogError("NO PUSISTE ALGÚN CODO U HOMBRO");
            //Este quit() no hace nada por algún motivo, y no creo que John Unity esté al tanto
            Application.Quit();
        }

        transBaseRight = transform.Find("DollyBrazos/TransBaseDerecho");
        transBaseLeft = transform.Find("DollyBrazos/TransBaseIzquierdo");
        lookAtRight = transform.Find("DollyBrazos/TransBaseDerecho/LookAtDerecho");
        lookAtLeft = transform.Find("DollyBrazos/TransBaseIzquierdo/LookAtIzquierdo");
    }


    private void LateUpdate() 
    {
        //acomodar el brazo derecho
        transRightShoulder.position = transBaseRight.position;
        Vector3 direction = lookAtRight.position - transBaseRight.position;
        //el hombro del modelo tiene la rotación de base para cualquier lado, este -90 -90 lo endereza
        Quaternion lookRot = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, -90);
        transRightShoulder.rotation = lookRot;

        transLeftShoulder.position = transBaseLeft.position;
        transLeftShoulder.rotation = Quaternion.LookRotation(lookAtLeft.position - transBaseLeft.position) * Quaternion.Euler(0, -270, 90);

    }

    void Update()
    {

        // Input checkers
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
        inputSprint = Input.GetAxis("Fire3") == 1f;
        inputLeftClick = Input.GetAxis("Fire1") == 1f;
        mouseHorizontalSpeed = Input.GetAxis("Mouse X");
        mouseVerticalSpeed = Input.GetAxis("Mouse Y");

        dollyRotation = Mathf.Clamp(dollyRotation - mouseHorizontalSpeed * swingMultiplier, -maxDollyRotation, maxDollyRotation);
        armsDolly.localRotation = Quaternion.Euler(0f, 0f, dollyRotation);

        armsPitch = Mathf.Clamp(armsPitch - mouseVerticalSpeed * swingMultiplier, -maxDollyPitch, maxDollyPitch);
        transBaseRight.localRotation = Quaternion.Euler(armsPitch, 0f, 0f);
        transBaseLeft.localRotation = Quaternion.Euler(armsPitch, 0f, 0f);
        

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