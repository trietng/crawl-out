using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    bool isFire;
    Vector2 fireDir;
    float fireSpeed;
    Vector2 currPos;
    [SerializeField] float alignAngle;
    [SerializeField] AudioClip reloadBulletClip;

    private void Awake()
    {
        isFire = false;
    }

    private void Update()
    {
        if (isFire)
        {
            currPos = transform.position;
            currPos.x += fireDir.x * fireSpeed * Time.deltaTime;
            currPos.y += fireDir.y * fireSpeed * Time.deltaTime;
            transform.position = currPos;
        }
    }
    public void ApplyColorFiler(PlayerAttack.FireMode fireMode)
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        switch (fireMode)
        {
            case PlayerAttack.FireMode.Single:
            case PlayerAttack.FireMode.Burst:
                spriteRenderer.color = Color.red;
                break;
            case PlayerAttack.FireMode.Spread:
                spriteRenderer.color = Color.green;
                break;
            case PlayerAttack.FireMode.Auto:
                spriteRenderer.color = Color.yellow;
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
        if (collision.gameObject.CompareTag("PlayerBound") || collision.gameObject.CompareTag("PickupItem") || collision.gameObject.name.CompareTo("Confiner") == 0)
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