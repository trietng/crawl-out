using System.Collections;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    Camera mainCam;
    Vector2 currMousePoint;
    Animator anim;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject slash;
    [SerializeField] float alignFirePos;
    private float fireTimer;
    private float fireCounter;
    private int damageAmount = 10; // Adjust damage amount as needed

    private void Awake()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        anim = GetComponent<Animator>();
        fireCounter = Mathf.Infinity;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (fireCounter > fireTimer && gameObject.GetComponent<PlayerScript>().IsAlive())
            {
                currMousePoint = mainCam.ScreenToWorldPoint(Input.mousePosition);
                anim.SetTrigger("isShooting");
                StartCoroutine(Attack(currMousePoint - ((Vector2)gameObject.transform.position + Vector2.up * alignFirePos)));
                fireCounter = 0;
            }
        }
        fireCounter += Time.deltaTime;
    }

    IEnumerator Attack(Vector2 dir)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(gameObject.transform.position + (Vector3)dir.normalized * 0.8f, dir.normalized, 1.0f);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Zombie"))
            {
                hit.collider.GetComponent<ZombieScript>().TakeDamage(damageAmount);
            }
        }

        yield return null; // You can add delay or other logic here
    }
}
