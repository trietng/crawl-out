using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{

    Camera mainCam;
    Vector2 currMousePoint;
    Animator anim;
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] float allignFirePos;
    [SerializeField] float fireTimer;
    float fireCounter;
    GameObject UI_bullet;
    private void Awake()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        anim = GetComponent<Animator>();
        fireCounter = Mathf.Infinity;
        UI_bullet = GameObject.FindGameObjectWithTag("UI_Bullet");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (fireCounter > fireTimer && gameObject.GetComponent<PlayerScript>().IsAlive())
            {
                currMousePoint = mainCam.ScreenToWorldPoint(Input.mousePosition);
                anim.SetTrigger("isShooting");
                Shoot(currMousePoint - ((Vector2)gameObject.transform.position + Vector2.up * allignFirePos));
                fireCounter = 0;
            }
        }
        fireCounter += Time.deltaTime;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(gameObject.transform.position, currMousePoint);
    }
    void Shoot(Vector2 dir)
    {
        if (ServiceScript._instance.bulletCount <= 0) return;
        ServiceScript._instance.bulletCount--;
        UI_bullet.GetComponent<UI_BulletScript>().UpdateBulletCountText();
        if (dir.magnitude > 1) dir.Normalize();
        float angle = Vector2.Angle(Vector2.right, dir) < 90 ? Vector2.Angle(Vector2.right, dir) : 180 - Vector2.Angle(Vector2.right, dir);
        if (angle < 45)
        {
            anim.SetFloat("dirX", Mathf.Sign(dir.x));
            anim.SetFloat("dirY", Mathf.Sign(dir.y));
        }
        else
        {
            anim.SetFloat("dirX", 0);
            anim.SetFloat("dirY", Mathf.Sign(dir.y));

        }
        GameObject _bullet = Instantiate(bullet, (Vector2)gameObject.transform.position + Vector2.up * allignFirePos, Quaternion.identity);
        StartCoroutine(ServiceScript._instance.TempRemoveCollider(gameObject, 0.1f));
        StartCoroutine(tempSlowPlayer(0.1f));
        ServiceScript._instance.TempRemoveCollider(gameObject, 3f);
        _bullet.GetComponent<BulletScript>().Fire(dir, bulletSpeed);
    }
    IEnumerator tempSlowPlayer(float sec)
    {
        float oldSpeed = GetComponent<PlayerScript>().speed;
        GetComponent<PlayerScript>().speed = 0;
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(sec);
        }
        GetComponent<PlayerScript>().speed = oldSpeed;
     }
}
