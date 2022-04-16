using UnityEngine;

public class PlayerController : MonoBehaviour {

    public enum PlayerStatus { Idle, Walk, Run, Jump }

    public float moveSpeed;
    public float jumpForce;
    public float speedupDuration;
    public float rotSpeed;
    public float groundSensorRadius;
    [HideInInspector]
    public float acceleration = 0f;
    [HideInInspector]
    public bool doJump = false;
    [HideInInspector]
    public bool onGround;
    public LayerMask groundLayer;
    public AnimationCurve speedCurve;
    public PlayerStatus status;

    private float h, v;
    private float speedTime;
    private bool velocityReset = false;
    private bool isPressing;
    private bool doInteract;
    private Vector2 direction;
    private Rigidbody rb;
    private Transform cam;
    private Quaternion newRot = Quaternion.identity;
    private Transform groundSensor;
    private GameManager gameManager;

    void Start() {
        gameManager = GameManager.instance;
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        groundSensor = transform.Find("sensors/ground");
    }


    void Update() {
        if (gameManager?.isTrading ?? false) { // FIX: debug
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            isPressing = false;
            acceleration = 0;
            return;
        }

        // assign data to variables
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        acceleration = (speedupDuration - speedTime) / speedupDuration;

        isPressing = new Vector2(h, v).magnitude != 0;

        // check for input
        if (isPressing) {
            speedTime -= Time.deltaTime;
            speedTime = Mathf.Clamp(speedTime, 0, speedupDuration);
        } else {
            speedTime = speedupDuration;
        }

        if (onGround && Input.GetKeyDown(KeyCode.Space)) {
            doJump = true;
        }

        if (!velocityReset && !isPressing) {
            rb.velocity = Vector3.up * rb.velocity.y; // <0, rb.velocity.y, 0>
            rb.angularVelocity = Vector3.zero;
            velocityReset = true;
        }

        // check sensors
        onGround = Physics.OverlapSphere(groundSensor.position, groundSensorRadius, groundLayer).Length > 0; // 1 because player is counted


        if (onGround) {
            status = (acceleration == 0) ? PlayerStatus.Idle : (acceleration < 1) ? PlayerStatus.Walk : PlayerStatus.Run;
        } else {
            status = PlayerStatus.Jump;
        }
    }

    void FixedUpdate() {
        if (isPressing) Move();

        if (doJump) Jump();

        rb.rotation = Quaternion.Lerp(rb.rotation, newRot, rotSpeed);
    }

    void Move() {
        var camUp = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;
        var camRight = Quaternion.Euler(0, 90, 0) * camUp;
        if (camUp == Vector3.zero) {
            camUp = cam.up;
            // camRight = cam.right;
            camRight = cam.right;
        }

        velocityReset = false;
        var newVelocity = ((camUp * v) + (camRight * h)).normalized * moveSpeed;
        rb.velocity = newVelocity * speedCurve.Evaluate(acceleration) + (transform.up * rb.velocity.y);

        float rotAngle = ((Mathf.Atan2(h, v) / Mathf.PI) * 180f);
        if (rotAngle < 0) rotAngle += 360f;
        newRot = Quaternion.Euler(Vector3.up * (rotAngle + cam.rotation.eulerAngles.y));
    }

    void Jump() {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        doJump = false;
    }

    private void OnDrawGizmos() {
        if (!Application.isPlaying) return;
        Gizmos.DrawWireSphere(groundSensor.position, groundSensorRadius);
    }
}
