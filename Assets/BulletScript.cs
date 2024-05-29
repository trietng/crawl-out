using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    bool isFire;
    Vector2 fireDir;
    float fireSpeed;
    Vector2 currPos;
    [SerializeField] float allignAngle;
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
    public void Fire(Vector2 dir, float speed)
    {
        GetComponent<Animator>().SetTrigger("fire");
        if (dir.x > 0) GetComponent<AudioSource>().panStereo = 0.65f;
        else GetComponent<AudioSource>().panStereo = -0.65f;

        GetComponent<AudioSource>().Play();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg  ;
        transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
        fireDir = dir;
        fireSpeed = speed;
        isFire = true;
        ServiceScript._instance.PlaySound(reloadBulletClip);

        Invoke("Explode", 1000);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        fireSpeed = 0f;
        gameObject.GetComponent<Animator>().SetTrigger("explode");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            collision.gameObject.transform.parent.GetComponent<ZombieScript>().HitZombie();
             gameObject.GetComponent<Animator>().SetTrigger("explode");
        }
    }
    public void Explode()
    {
        Destroy(gameObject);
    }
}