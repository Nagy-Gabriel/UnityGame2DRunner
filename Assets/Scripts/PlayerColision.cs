using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    AudioManager audioManager;
    PlayerMovement playerMovement; 

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        playerMovement = GetComponent<PlayerMovement>(); 
    }

    private void Start()
    {
        GameManager.Instance.onPlay.AddListener(ActivePlayer);
    }

    private void ActivePlayer()
    {
        gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Obstacle"))
        {
            if (playerMovement.IsInvincible)
            {
                Destroy(other.gameObject); // distrugem obiectele daca caracterul este invicibil
                audioManager.PlaySFX(audioManager.destroyObstacle); //sunet cand ne bagam in inamici
            }
            else
            {
                Die();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectible"))
        {
            CollectCollectible(other.gameObject);
        }
    }

    private void Die()
    {
        //verificam daca caracterul este invicibil iar daca nu este sa moara
        if (!playerMovement.IsInvincible)
        {
            gameObject.SetActive(false);
            GameManager.Instance.GameOver();
            audioManager.PlaySFX(audioManager.death);
        }
    }

    private void CollectCollectible(GameObject collectible)
    {
        // distrugem obiectul care ne da puterea
        Destroy(collectible);
        // sunetul de colcetare
        audioManager.PlaySFX(audioManager.collect);//efectul de colectare

        //activam invincibilitatea pentru cate secunde vrem
        playerMovement.ActivateInvincibility(6f); 
    }
}
