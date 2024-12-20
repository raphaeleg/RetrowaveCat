using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal = 0f;
    private float speed = 10f;
    private float jumpPower = 24f;
    private float speedMod = 2f;
    private bool isFacingRight = true;
    private bool isDashing = false;
    private bool isCrouching = false;
    private bool isInAir = false;
    private bool isAirCooldown = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator;

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        Flip();
        if (Input.GetButtonDown("Jump")) {
            OnPressJump();
        }
        if (Input.GetButtonDown("Dash")) { SetDash(true); }
        else if (Input.GetButtonUp("Dash")) { SetDash(false); }
        if (Input.GetButtonDown("Crouch")) { isCrouching = !isCrouching; }

        if (Input.GetButtonDown("Sit")) { 
            animator.SetTrigger("sit");
            EventManager.TriggerEvent("Sit", isFacingRight ? 1 : -1);
        }
        if (Input.GetButtonDown("Meow")) {
            EventManager.TriggerEvent("Meow", 1);
        }
    }

    private void FixedUpdate()
    {
        float mod = 1f;
        if (isCrouching) { mod = 1/speedMod; }
        else if (isDashing) { mod = speedMod; }

        float velocityX = horizontal * speed * mod;

        rb.linearVelocity = new Vector2(
            velocityX, 
            rb.linearVelocity.y
        );
        animator.SetFloat("speed", Mathf.Abs(velocityX));

        if (!isAirCooldown && isInAir && IsGrounded()) { 
            isInAir = false;
            animator.SetBool("isJumping", false);
        }
    }

    private void OnPressJump()
    {
        Vector2 updateVel = rb.linearVelocity;
        
        if (IsGrounded())
        {
            updateVel.y = jumpPower;
        }
        if (rb.linearVelocity.y > 0f || isCrouching)
        {
            updateVel.y *= 0.5f;
        }
        rb.linearVelocity = updateVel;
        isInAir = true;
        isAirCooldown = true;
        animator.SetBool("isJumping", true);
        StartCoroutine("AirCountdown");
    }
    private IEnumerator AirCountdown()
    {
        yield return new WaitForSeconds(1f);
        isAirCooldown = false;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void SetDash(bool val)
    {
        isDashing = val;
        animator.SetBool("isDashing", val);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal > -0.1f || !isFacingRight && horizontal < 0.1f) {
            return;
        }

        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}