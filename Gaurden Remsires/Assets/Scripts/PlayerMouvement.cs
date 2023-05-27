using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
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
}
