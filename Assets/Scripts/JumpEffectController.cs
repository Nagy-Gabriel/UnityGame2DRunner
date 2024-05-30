using UnityEngine;

public class JumpEffectController : MonoBehaviour
{
    public ParticleSystem jumpEffect;
    public Transform groundCheck;
    public float checkRadius = 0.1f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on " + gameObject.name);
        }

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on " + gameObject.name);
        }

        if (jumpEffect == null)
        {
            Debug.LogError("JumpEffect particle system not assigned in the Inspector.");
        }

        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck transform not assigned in the Inspector.");
        }
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        Debug.Log("Is Grounded: " + isGrounded);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector2(0f, 10f), ForceMode2D.Impulse);
            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
            PlayJumpEffect();
            PlayJumpSound();
        }

        if (rb.velocity.y < 0 && !isGrounded)
        {
            if (animator != null)
            {
                animator.SetTrigger("Fall");
            }
        }
    }

    void PlayJumpEffect()
    {
        if (jumpEffect != null)
        {
            Debug.Log("Playing Jump Effect");
            jumpEffect.Play();
        }
        else
        {
            Debug.Log("Jump Effect is null");
        }
    }

    void PlayJumpSound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            Debug.Log("Playing Jump Sound");
        }
        else
        {
            Debug.Log("Jump Sound or AudioSource is null");
        }
    }
}
