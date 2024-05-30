using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform GFX;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float groundDistance = 0.25f;
    [SerializeField] private float jumpTime = 0.3f;
    [SerializeField] private int maxAirJumps = 4;
    [SerializeField] private float airJumpForce = 5f;
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private ParticleSystem invincibilityEffect; 
    //folosim serializeField ca sa ne apara variabilele private in inspector.

    private bool isGrounded = false;
    private bool isJumping = false;
    private float jumpTimer;
    private int airJumpsRemaining;
    private Animator animator;
    private Vector3 initialScale; //marimea initiala a caracterului

    public bool IsInvincible { get; private set; } = false; //pentru verificarea starii caracterului: invicibil sau nu
    private float invincibilityEndTime; 

    private void Start()
    {
        airJumpsRemaining = maxAirJumps;
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("isRunning", true); // caracterul alearga mereu

        if (invincibilityEffect != null)
        {
            invincibilityEffect.Stop();
        }

        initialScale = GFX.localScale;
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);
        animator.SetBool("isGrounded", isGrounded);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            rb.velocity = Vector2.up * jumpForce;
            jumpTimer = 0;
            animator.SetTrigger("Jump");
        }

        if (isJumping && Input.GetButton("Jump"))
        {
            if (jumpTimer < jumpTime)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimer += Time.deltaTime;
                animator.SetTrigger("Jump");
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            jumpTimer = 0;
        }

        if (!isGrounded && airJumpsRemaining > 0 && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, airJumpForce);
            airJumpsRemaining--;
            animator.SetTrigger("Jump");
        }

        if (isGrounded)
        {
            airJumpsRemaining = maxAirJumps;
        }

        //animatie cadere
        if (rb.velocity.y < 0 && !isGrounded)
        {
            animator.SetTrigger("Fall");
        }

        // animatie crouch + scadere scale
        if (isGrounded && Input.GetButton("Crouch"))
        {
            GFX.localScale = new Vector3(initialScale.x, crouchHeight, initialScale.z);
            animator.SetBool("isCrouching", true); 
        }
        else if (isGrounded && Input.GetButtonUp("Crouch"))
        {
            GFX.localScale = initialScale; // resetam initialScale, adica dimensiunea caracterului la 1
            animator.SetBool("isCrouching", false);
        }

        //verificam daca s-a terminat perioada de invicibilitate si o dezactivam
        if (IsInvincible && Time.time >= invincibilityEndTime)
        {
            DeactivateInvincibility();
        }
    }

    public void ActivateInvincibility(float duration)
    {
        IsInvincible = true;
        invincibilityEndTime = Time.time + duration;  //calculam timpul in care va expira perioada de inv.

        if (invincibilityEffect != null)
        {
            invincibilityEffect.Play(); //pornim efectul de invicibilitate
        }

        // programam dezactivarea invicibilitati dupa durata
        Invoke(nameof(DeactivateInvincibility), duration);
    }

    private void DeactivateInvincibility()
    {
        IsInvincible = false;

        if (invincibilityEffect != null)
        {
            invincibilityEffect.Stop(); //oprim efectul
        }
    }


}
