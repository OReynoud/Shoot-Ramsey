
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMouvement : MonoBehaviour
{
    public static PlayerMouvement instance;
    public Vector3 playerDir;
    public float speed;
    public Transform gunPoint;

    public float shootingRate;
    private float shootingTimer;
    public float bulletSpeed;

    public PlayerBullets bulletPrefab;

    public int currentHealth;

    public int maxHealth;
    public Vector2 maxBounds;
    public Vector2 minBounds;

    public PlayerInputs controls;

    public Animator characterAnimator;

    public Animator gunAnimator;

    public float invincibilityTimer = 0;

    public Collider2D coll;

    public GameObject youDiedText;

    public GameObject restartButton;
    public Image winScreen;

    public Image[] healthBar;

    public bool isDead = false;

    public AudioClip hurt;

    public AudioClip shoot;

    public AudioClip baseMusic;

    public AudioClip winMusic;

    public AudioSource musicChannel;

    public AudioSource playerChannel;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this);
        }
        instance = this;
        controls = new PlayerInputs();
        controls.Enable();
        controls.Player.Enable();
        controls.Player.Move.performed += Move;
        controls.Player.Shoot.performed += Shoot;
        currentHealth = maxHealth;
        gunAnimator.SetFloat("Speed",1/0.2f);
        coll = GetComponent<Collider2D>();
    }

    private void Start()
    {
        musicChannel.Stop();
        musicChannel.clip = baseMusic;
        musicChannel.Play();
        musicChannel.volume = 0.15f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
        if (invincibilityTimer >= 0)
        {
            invincibilityTimer -= Time.fixedDeltaTime;
        }
        else
        {
            characterAnimator.SetBool("TakingDamage",false);
        }
        MovePlayer();
        ShootMissiles();
    }

    void MovePlayer()
    {
        if (controls.Player.Move.IsPressed())
        {
            transform.position += playerDir * speed;
        }
        if (transform.position.x > maxBounds.x)
        {
            transform.position = new Vector3(maxBounds.x, transform.position.y, transform.position.z);
        }
        if (transform.position.y > maxBounds.y)
        {
            transform.position = new Vector3(transform.position.x, maxBounds.y, transform.position.z);
        }
        if (transform.position.x < minBounds.x)
        {
            transform.position = new Vector3(minBounds.x, transform.position.y, transform.position.z);
        }
        if (transform.position.y < minBounds.y)
        {
            transform.position = new Vector3(transform.position.x, minBounds.y, transform.position.z);
        }
    }

    void ShootMissiles()
    {
        if (shootingTimer >= 0)
        {
            shootingTimer -= Time.fixedDeltaTime;
        }
        if (controls.Player.Shoot.IsPressed() && shootingTimer <= 0)
        {
            shootingTimer = shootingRate;
            Instantiate(bulletPrefab.gameObject, gunPoint.position, Quaternion.identity);
            playerChannel.Stop();
            playerChannel.clip = shoot;
            playerChannel.Play();
        }

        if (controls.Player.Shoot.IsPressed())
        {
            gunAnimator.SetBool("Attacking",true);
        }
        else
        {
            gunAnimator.SetBool("Attacking",false);
        }
    }

    void Move(InputAction.CallbackContext context)
    {
        playerDir = context.ReadValue<Vector2>().normalized;
    }

    void Shoot(InputAction.CallbackContext context)
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ennemy") && invincibilityTimer < 0)
        {
            Debug.Log("took damage");
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        currentHealth--;
        invincibilityTimer = 1;
        healthBar[currentHealth].color = Color.red;
        playerChannel.Stop();
        playerChannel.clip = hurt;
        playerChannel.Play();
        characterAnimator.SetBool("TakingDamage",true);
        if (currentHealth == 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        coll.enabled = false;
        isDead = true;
        youDiedText.transform.DOMoveY(youDiedText.transform.position.y - 150, 0.5f);
        restartButton.transform.DOMoveY(restartButton.transform.position.y + 100, 1f);
    }

    
    public void Restart()
    {
        SceneManager.LoadScene("Oscar");
    }
}
