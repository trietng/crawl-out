using System;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    Animator anim;
    Rigidbody2D rigit;
    float hor, ver;
    Vector2 newPos;
    public float speed;
    private float memoryOfSpeed;
    [NonSerialized] public Camera mainCam;
    [SerializeField] GameObject bullet;
    [SerializeField] AudioClip footstepClip;
    [SerializeField] AudioClip gameOverClip;

    public AudioSource audi { get; private set; }

    [SerializeField] public int maxHealth;
    [NonSerialized] public int currentHealth;
    private int memoryOfHealth;

    public static PlayerScript Instance { get; private set; }

    [SerializeField] private float dashSpeedMultiplier = 2f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashCooldown = 1f;
    private bool isDashing;
    private float dashTime;
    private float dashCooldownTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            mainCam = Camera.main;
            anim = GetComponent<Animator>();
            rigit = GetComponent<Rigidbody2D>();
            audi = GetComponent<AudioSource>();
            memoryOfSpeed = speed;
            currentHealth = maxHealth;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim.SetFloat("dirX", 0);
        anim.SetFloat("dirY", -1);
        audi.clip = footstepClip;
        PlayerUIScript.Instance.UpdateHealthText(currentHealth);
        dashCooldownTime = dashCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        hor = Input.GetAxisRaw("Horizontal");
        ver = Input.GetAxisRaw("Vertical");
        anim.SetBool("isMoving", (hor != 0 || ver != 0) && IsAlive());

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && dashCooldownTime <= 0)
        {
            StartDash();
        }

        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                EndDash();
            }
        }
        else
        {
            dashCooldownTime -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (anim.GetBool("isMoving"))
        {
            if (!audi.isPlaying) audi.Play();
            Move(new Vector2(hor, ver));
        }
        else audi.Pause();
    }

    void Move(Vector2 dir)
    {
        anim.SetFloat("dirX", hor);
        anim.SetFloat("dirY", ver);
        newPos = rigit.transform.position;
        dir.Normalize();
        newPos.x += dir.x * speed * Time.deltaTime * (isDashing ? dashSpeedMultiplier : 1f);
        newPos.y += dir.y * speed * Time.deltaTime * (isDashing ? dashSpeedMultiplier : 1f);
        rigit.transform.position = newPos;
    }

    void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        dashCooldownTime = dashCooldown;
    }

    void EndDash()
    {
        isDashing = false;
    }

    public bool IsAlive()
    {
        // return GameManager._instance.State != GameManager.GameState.Dead;
        return currentHealth > 0;
    }

    public void TakeDamage(int damage)
    {
        if (!IsAlive()) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        PlayerUIScript.Instance.UpdateHealthText(currentHealth);
    }

    public void Die()
    {
        MakeInvincible();
        GameManager.Instance.TurnOffLight();
        anim.SetTrigger("isDead");
        GameManager.Instance.PlayLowPitchSound(gameOverClip);
        speed = 0;
        StartCoroutine(GameManager.Instance.TurnOffLight());
        GameManager.Instance.UpdateGameState(GameManager.GameState.Dead);
    }

    public void SaveHealth()
    {
        memoryOfHealth = currentHealth;
    }

    public void RestoreHealth()
    {
        currentHealth = memoryOfHealth;
        PlayerUIScript.Instance.UpdateHealthText(currentHealth);
    }

    public void MakeInvincible()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponentInChildren<CapsuleCollider2D>().enabled = false;
    }

    public void RevokeInvincibility()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponentInChildren<CapsuleCollider2D>().enabled = true;
    }

    public void Resurrect()
    {
        RestoreHealth();
        PlayerAttackScript.Instance.RestoreInventory();
        GameManager.Instance.UpdateGameState(GameManager.GameState.Nor);
        anim.Rebind();
        anim.Update(0f);
        speed = memoryOfSpeed;
    }
}
