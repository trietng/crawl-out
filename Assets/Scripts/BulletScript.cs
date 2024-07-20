using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BulletScript : MonoBehaviour
{
    bool isFire;
    Vector2 fireDir;
    float fireSpeed;
    Vector2 currPos;
    [SerializeField] float alignAngle;
    [SerializeField] AudioClip reloadBulletClip;
    
    [NonSerialized] public int damage;
    private static readonly HashSet<string> whitelist = new()
    {
        "Door",
        "Key",
        "TurretDetector"
    };

    private static readonly HashSet<string> playerWhitelist = new()
    {
        "PlayerBound"
    };

    private static readonly HashSet<string> turretWhitelist = new()
    {
        "Zombie"
    };

    public enum Shooter
    {
        Player,
        Turret
    }
    private Shooter shooter;

    void Awake()
    {
        isFire = false;
    }

    void Update()
    {
        if (isFire)
        {
            currPos = transform.position;
            currPos.x += fireDir.x * fireSpeed * Time.deltaTime;
            currPos.y += fireDir.y * fireSpeed * Time.deltaTime;
            transform.position = currPos;
        }
    }
    public void ApplyColorFiler(WeaponScript.WeaponType weapon)
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var light2D = GetComponent<Light2D>();
        switch (weapon)
        {
            case WeaponScript.WeaponType.Single:
                spriteRenderer.color = Color.magenta;
                light2D.color = Color.magenta;
                break;
            case WeaponScript.WeaponType.Burst:
                spriteRenderer.color = Color.green;
                light2D.color = Color.green;
                break;
            case WeaponScript.WeaponType.Spread:
                spriteRenderer.color = Color.red;
                light2D.color = Color.red;
                break;
            case WeaponScript.WeaponType.Auto:
                spriteRenderer.color = Color.yellow;
                light2D.color = Color.yellow;
                break;
        }
    }

    public void ApplyColorFiler(Color color)
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var light2D = GetComponent<Light2D>();
        spriteRenderer.color = color;
        light2D.color = color;
    }

    public void Fire(Vector2 dir, float speed, float range, Shooter shooter)
    {
        GetComponent<Animator>().SetTrigger("fire");
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
        fireDir = dir;
        fireSpeed = speed;
        isFire = true;
        this.shooter = shooter;
        Invoke(nameof(Explode), range / speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool hit = true;
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hit = false;
        }
        switch (shooter)
        {
            case Shooter.Player:
                if (collision.gameObject.CompareTag("Player"))
                {
                    hit = false;
                }
                if (collision.gameObject.CompareTag("Turret"))
                {
                    var turretBaseScript = collision.gameObject.GetComponent<TurretBaseScript>();
                    turretBaseScript.TakeDamage(damage);
                }
                break;
            case Shooter.Turret:
                if (collision.gameObject.CompareTag("Turret"))
                {
                    hit = false;
                }
                break;
        }
        if (hit)
        {
            CollisionAction();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool hit = true;
        if (whitelist.Contains(collision.gameObject.tag) || 
            collision.gameObject.tag.EndsWith("Item") || 
            collision.gameObject.name.CompareTo("Confiner") == 0)
        {
            hit = false;
        }
        switch (shooter)
        {
            case Shooter.Player:
                if (collision.gameObject.CompareTag("Zombie"))
                {
                    var zombieScript = collision.gameObject.transform.parent.GetComponent<ZombieScript>();
                    hit = !zombieScript.isDead;
                    zombieScript.TakeDamage(damage);
                }
                if (playerWhitelist.Contains(collision.gameObject.tag))
                {
                    hit = false;
                }
                break;
            case Shooter.Turret:
                if (collision.gameObject.CompareTag("PlayerBound"))
                {
                    PlayerScript.Instance.TakeDamage(damage);
                }
                if (turretWhitelist.Contains(collision.gameObject.tag))
                {
                    hit = false;
                }
                break;
        }
        if (hit)
        {
            CollisionAction();
        }
    }

    private void CollisionAction()
    {
        fireSpeed = 0f;
        gameObject.GetComponent<Animator>().SetTrigger("explode");
    }

    public void Explode()
    {
        Destroy(gameObject);
    }
}