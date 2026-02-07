using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Dash")]
    public float dashForce = 15f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;

    [Header("Slide")]
    public float slideSpeed = 12f;
    public float slideDuration = 0.6f;
    public float slideCooldown = 1.2f;
    public float slideHeight = 0.6f;

    [Header("Camera")]
    public Transform cameraTransform;
    public Camera cam;
    public float slideCamDrop = -0.6f;
    public float slideFOV = 75f;
    public float camSmooth = 10f;

    [Header("Ground")]
    public LayerMask groundLayer;

    Rigidbody rb;
    Vector2 moveInput;
    bool isGrounded;

    // DASH
    bool isDashing;
    float dashTimer;
    float dashCooldownTimer;

    // SLIDE
    bool isSliding;
    float slideTimer;
    float slideCooldownTimer;

    float normalHeight;
    float normalCamY;
    float normalFOV;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        normalHeight = transform.localScale.y;
        normalCamY = cameraTransform.localPosition.y;
        normalFOV = cam.fieldOfView;
    }

    void Update()
    {
        // INPUT
        moveInput = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1;

        // GROUND CHECK
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.2f, groundLayer);

        // JUMP
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded && !isSliding)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }

        // DASH (TAP SHIFT)
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame && dashCooldownTimer <= 0f && !isSliding)
        {
            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
        }

        // SLIDE (HOLD CTRL)
        if (Keyboard.current.leftCtrlKey.wasPressedThisFrame && isGrounded && slideCooldownTimer <= 0f)
        {
            isSliding = true;
            slideTimer = slideDuration;
            slideCooldownTimer = slideCooldown;

            transform.localScale = new Vector3(
                transform.localScale.x,
                slideHeight,
                transform.localScale.z
            );
        }

        if (dashCooldownTimer > 0f) dashCooldownTimer -= Time.deltaTime;
        if (slideCooldownTimer > 0f) slideCooldownTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * moveInput.y + camRight * moveInput.x).normalized;

        // DASH
        if (isDashing)
        {
            rb.linearVelocity = new Vector3(
                moveDir.x * dashForce,
                rb.linearVelocity.y,
                moveDir.z * dashForce
            );

            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0f)
                isDashing = false;
        }
        // SLIDE
        else if (isSliding)
        {
            rb.linearVelocity = new Vector3(
                moveDir.x * slideSpeed,
                rb.linearVelocity.y,
                moveDir.z * slideSpeed
            );

            slideTimer -= Time.fixedDeltaTime;
            if (slideTimer <= 0f)
            {
                isSliding = false;
                transform.localScale = new Vector3(
                    transform.localScale.x,
                    normalHeight,
                    transform.localScale.z
                );
            }
        }
        // NORMAL MOVE
        else
        {
            rb.linearVelocity = new Vector3(
                moveDir.x * moveSpeed,
                rb.linearVelocity.y,
                moveDir.z * moveSpeed
            );
        }

        // CAMERA FX (slide only)
        float targetY = isSliding ? normalCamY + slideCamDrop : normalCamY;
        Vector3 camPos = cameraTransform.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, targetY, Time.fixedDeltaTime * camSmooth);
        cameraTransform.localPosition = camPos;

        float targetFOV = isSliding ? slideFOV : normalFOV;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.fixedDeltaTime * camSmooth);
    }
}
