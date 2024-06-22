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
    Camera mainCam;
    [SerializeField] GameObject bullet;
    [SerializeField] AudioClip footstepClip;
    [SerializeField] AudioClip gameOverClip;

    public AudioSource audi { get; private set; }
    
    private int maxHealth = 100;
    private int currentHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigit = GetComponent<Rigidbody2D>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        audi = GetComponent<AudioSource>();
        maxSpeed = speed;
        currentHealth = maxHealth;
        //  GameManager._instance.UpdateGameState(GameManager.GameState.Nor);
    }
    // Start is called before the first frame update
    void Start()
    {
        anim.SetFloat("dirX", 0);
        anim.SetFloat("dirY", -1);
        audi.clip = footstepClip;

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
            PlayerDead();
        }
    }
    
    public void PlayerDead()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponentInChildren<CapsuleCollider2D>().enabled = false;
        gameObject.GetComponentInChildren<Light2D>().intensity = 0;
        anim.SetTrigger("isDead");
        ServiceScript._instance.PlaySound(gameOverClip);
        speed = 0;
        StartCoroutine(ServiceScript._instance.TurnOffLight());
        GameManager._instance.UpdateGameState(GameManager.GameState.Dead);
        Invoke("ReloadSceneWhenPlayerDead", 3f);
    }
    public void ReloadSceneWhenPlayerDead()
    {
        GameManager._instance.ReloadScene();
    }
}
