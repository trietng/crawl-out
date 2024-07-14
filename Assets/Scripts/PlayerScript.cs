using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class PlayerScript : MonoBehaviour
{

    Animator anim;
    Rigidbody2D rigit;
    float hor, ver;
    Vector2 newPos;
    public float speed;
    [NonSerialized] public float maxSpeed;
    [NonSerialized] public Camera mainCam;
    [SerializeField] GameObject bullet;
    [SerializeField] AudioClip footstepClip;
    [SerializeField] AudioClip gameOverClip;

    public AudioSource audi { get; private set; }
    
    [SerializeField] public int maxHealth;
    [NonSerialized] public int currentHealth;

    public static PlayerScript Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            mainCam = Camera.main;
            anim = GetComponent<Animator>();
            rigit = GetComponent<Rigidbody2D>();
            audi = GetComponent<AudioSource>();
            maxSpeed = speed;
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
    }

    // Update is called once per frame
    void Update()
    {
        hor = Input.GetAxisRaw("Horizontal");
        ver = Input.GetAxisRaw("Vertical");
        anim.SetBool("isMoving", (hor != 0 || ver != 0) && IsAlive());
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
        newPos.x += dir.x * speed * Time.deltaTime;
        newPos.y += dir.y * speed * Time.deltaTime;
        rigit.transform.position = newPos;
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
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponentInChildren<CapsuleCollider2D>().enabled = false;
        GameManager.Instance.TurnOffLight();
        anim.SetTrigger("isDead");
        GameManager.Instance.PlaySound(gameOverClip);
        speed = 0;
        StartCoroutine(GameManager.Instance.TurnOffLight());
        GameManager.Instance.UpdateGameState(GameManager.GameState.Dead);
    }
}
