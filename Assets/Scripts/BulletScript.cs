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
    private static readonly HashSet<string> onTriggerEnterTagsWhitelist = new() {
        "PlayerBound",
        "Door",
        "Key"
    };

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
    public void ApplyColorFiler(WeaponScript.WeaponType weapon) {
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
    public void Fire(Vector2 dir, float speed, float range)
    {
        GetComponent<Animator>().SetTrigger("fire");
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
        fireDir = dir;
        fireSpeed = speed;
        isFire = true;
        // GameManager._instance.PlaySound(reloadBulletClip);
        Invoke(nameof(Explode), range / speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool hit = true;
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Player"))
        {
            hit = false;
        }
        if (hit)
        {   
            CollisionAction();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool hit = true;
        if (onTriggerEnterTagsWhitelist.Contains(collision.gameObject.tag) || collision.gameObject.tag.EndsWith("Item") || collision.gameObject.name.CompareTo("Confiner") == 0)
        {
            hit = false;
        }
        if (collision.gameObject.CompareTag("Zombie"))
        {
            var zombieScript = collision.gameObject.transform.parent.GetComponent<ZombieScript>();
            hit = !zombieScript.isDead;
            zombieScript.TakeDamage(10);
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