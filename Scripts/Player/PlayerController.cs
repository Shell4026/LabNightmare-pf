using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] Transform cam;
    [SerializeField] Animator camAnim;
    [Header("스탯")]
    public float maxHp = 100.0f;
    public float speed = 2.0f;
    public float speedInAir = 1.0f;
    public float speedRun = 4.0f;
    public float speedSit = 1.0f;
    public float jumpPower = 1.0f;
    public float playerHeight = 1.6f;
    public float dragInGround = 10.0f;
    [Header("키 설정")]
    public KeyCode keyForward = KeyCode.W;
    public KeyCode keyBack = KeyCode.S;
    public KeyCode keyLeft = KeyCode.A;
    public KeyCode keyRight = KeyCode.D;
    public KeyCode keyJump = KeyCode.Space;
    public KeyCode keySit = KeyCode.LeftControl;
    public KeyCode keyRun = KeyCode.LeftShift;
    public KeyCode keyFlash = KeyCode.Q;
    public KeyCode keyRotate = KeyCode.R;
    [Header("조작")]
    public float mouseSensivityX = 10.0f;
    public float mouseSensivityY = 10.0f;
    [Header("경사로")]
    public float maxSlopeAngle = 45.0f;

    int forward = 0;
    int right = 0;

    Vector3 moveDir = Vector3.zero;

    float xRot = 0.0f;
    float yRot = 0.0f;
    Rigidbody rb;
    RaycastHit slopeHit;

    bool onAir = false;
    bool onJump = false;
    bool canJump = true;
    bool onGround = true;
    bool onSlope = false;
    bool walking = false;
    bool run = false;
    bool sit = false;
    bool rotate = false;

    bool tired = false;
    bool stop = false;
    bool stopCam = false;
    bool lockCursor = true;

    float limitSpeed = 2.0f;

    GameObject slopeObject;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        MoveControl();
        RotationControl();
    }
    void Update()
    {
        
        PlayerInput();
        GroundCheck();

        if (onAir)
            rb.drag = 0;
        else
            rb.drag = dragInGround;

        Control();
        SitControl();
        WalkingAnimation();
        
        SpeedLimit();
    }

    private void LateUpdate()
    {
        
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject == slopeObject)
        {
            if (onJump)
                return;

            if (Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.5f))
            {
                rb.AddForce(Vector3.down * 1.5f, ForceMode.Impulse);
            }
        }
    }
    void PlayerInput()
    {
        if (stop)
            return;

        forward = 0;
        right = 0;
        
        walking = false;
        run = false;
        rotate = false;

        if (Input.GetKey(keyForward))
            forward += 1;
        if (Input.GetKey(keyBack))
            forward -= 1;
        if (Input.GetKey(keyRight))
            right += 1;
        if (Input.GetKey(keyLeft))
            right -= 1;

        if (Input.GetKeyDown(keySit))
            SitToggle();

        if (Input.GetKey(keyRotate))
            rotate = true;

        if (!rotate)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensivityX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensivityY;

            yRot += mouseX;

            xRot -= mouseY;
            xRot = Mathf.Clamp(xRot, -90.0f, 90.0f);
        }

        if (!onAir)
        {
            if (forward != 0 || right != 0)
            {
                walking = true;
            }
            if (!tired)
            {
                if (Input.GetKey(keyRun))
                {
                    sit = false;
                    run = true;
                }
                if (Input.GetKey(keyJump))
                {
                    if (canJump)
                    {
                        canJump = false;
                        Jump();
                        Invoke(nameof(JumpTrue), 0.1f);
                    }
                }
            }
        }
    }
    void RotationControl()
    {
        if (stop)
            return;
        if (stopCam)
            return;
        if (rotate)
            return;
        
        transform.rotation = Quaternion.Euler(0, yRot, 0);
        cam.localRotation = Quaternion.Euler(xRot, 0, 0);
    }
    void GroundCheck()
    {
        onGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f);
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.2f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            onSlope = angle <= maxSlopeAngle && angle != 0;
            if (onSlope)
                slopeObject = slopeHit.collider.gameObject;
        }
        else
        {
            slopeObject = null;
            onSlope = false;
        }

        if (!onGround && !onSlope)
            onAir = true;
        else
            onAir = false;
    }
    void MoveControl()
    {
        float speed = this.speed;

        if (sit)
            speed = speedSit;

        moveDir = transform.forward * forward + transform.right * right;
        moveDir.y = 0.0f;
        moveDir.Normalize();

        if (onSlope && !onJump)
        {

            rb.AddForce(20 * speed * GetSlopeMoveDir(), ForceMode.Force);
        }
        else if(onGround)
            rb.AddForce(20 * speed * moveDir);
        else if(onAir)
            rb.AddForce(20 * speedInAir * moveDir);

        rb.useGravity = !onSlope;
    }

    void SpeedLimit()
    {
        if (canJump && !onAir)
        {
            limitSpeed = this.speed;
            if (sit)
                limitSpeed = speedSit;
            if (run)
                limitSpeed = speedRun;
        }

        if (onSlope && !onJump)
        {
            if (rb.velocity.sqrMagnitude > limitSpeed * limitSpeed)
            {
                rb.velocity = rb.velocity.normalized * limitSpeed;
            }
        }
        else
        {
            Vector3 flatv = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (flatv.sqrMagnitude > limitSpeed * limitSpeed)
            {
                Vector3 limitV = flatv.normalized * limitSpeed;
                rb.velocity = new Vector3(limitV.x, rb.velocity.y, limitV.z);
            }
        }
    }
    void Control()
    {
        if(lockCursor)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void LockCursor(bool b)
    {
        lockCursor = b;
    }
    void SitControl()
    {
        if (sit)
        {
            if (transform.localScale.y > 0.5f)
            {
                Vector3 scale = transform.localScale;
                scale.y -= Time.deltaTime;
                transform.localScale = scale;
            }
            else
            {
                Vector3 scale = transform.localScale;
                scale.y = 0.5f;
                transform.localScale = scale;
            }
        }
        else
        {
            if (transform.localScale.y < 1.0f)
            {
                Vector3 scale = transform.localScale;
                scale.y += Time.deltaTime;
                transform.localScale = scale;
            }
            else
            {
                Vector3 scale = transform.localScale;
                scale.y = 1.0f;
                transform.localScale = scale;
            }
        }
    }
    public void Jump()
    {
        sit = false;
        onJump = true;
        if (onGround)
            rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
        rb.AddForce(new Vector3(0.0f, jumpPower, 0.0f), ForceMode.Impulse);
    }
    void JumpTrue()
    {
        onJump = false;
        if (!Input.GetKey(keyJump))
        {
            canJump = true;
        }
        else
            Invoke(nameof(JumpTrue), 0.2f);
    }
    public void SitToggle()
    {
        sit = !sit;
    }
	
    public void StopCameraMoving(bool state)
    {
        stopCam = state;
    }
	
    Vector3 GetSlopeMoveDir()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal).normalized;
    }
	
    void WalkingAnimation()
    {
        if (camAnim == null)
            return;
        if (!onAir)
        {
            if (rb.velocity.sqrMagnitude > speed * speed / 9.0f)
            {
                camAnim.SetBool("walking", true);
            }
            else
            {
                camAnim.SetBool("walking", false);
            }
        }
        else
            camAnim.SetBool("walking", false);
    }

    public bool OnGround()
    {
        return onGround;
    }
    public bool OnSlope()
    {
        return onSlope;
    }
    public bool OnJump()
    {
        return onJump;
    }
    public bool OnAir()
    {
        return onAir;
    }
    public bool GetRun() 
    { 
        return run; 
    }
    public bool GetSit() 
    { 
        return sit; 
    }
    public bool GetWalking() 
    { 
        return walking; 
    }
    
    public void SetTired(bool b)
    {
        tired = b;
    }
    public bool IsTired()
    {
        return tired;
    }

    public void SetStop(bool b)
    {
        stop = b;
    }

    public bool GetStop()
    {
        return stop;
    }
}
