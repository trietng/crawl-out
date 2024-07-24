using System.Collections;
using System.Linq;
using UnityEngine;

public class TurretCannonSingleSmartScript : TurretLaserBaseScript
{
    // Start is called before the first frame update
    public GameObject bullet;
    private BulletScript bulletScript;
    private GameObject playerGameObject;
    private Vector3 firingOrigin;
    private Vector3 firingDirection;
    public float rotateSpeed;
    public float cooldown;
    public static readonly float magicMultiplier = 0.8f;
    private static readonly float angleOffset = -90f;
    public int damage;
    public AudioClip firingSound;
    private float radius;

    void Start () {
        passAnimator(GetComponent<Animator>());
        radius = GetComponent<CircleCollider2D>().radius;
    }

    void Update()
    {
        if (playerGameObject != null)
        {
            // Rotate the turret to face the player
            firingDirection = (playerGameObject.transform.position - transform.position).normalized;
            // Calculate the angle to rotate the turret
            float angle = Mathf.Atan2(firingDirection.y, firingDirection.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle + angleOffset, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDestroyed() && playerGameObject == null && collision.gameObject.CompareTag("PlayerBound"))
        {
            playerGameObject = collision.gameObject;
            StartCoroutine(AttackPlayer());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (playerGameObject != null && collision.gameObject.CompareTag("PlayerBound"))
        {
            playerGameObject = null;
        }
    }

    bool IsPlayerInSight()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(firingOrigin, firingDirection, radius);
        return hits.SkipLast(1).All(hit => hit.collider.name != "walls");
    }

    IEnumerator AttackPlayer()
    {
        while (playerGameObject != null && !isDestroyed())
        {
            yield return new WaitForSeconds(cooldown);
            firingOrigin = gameObject.transform.position + (firingDirection * magicMultiplier);
            if (IsPlayerInSight())
            {
                var _bullet = Instantiate(bullet, firingOrigin, Quaternion.identity);
                bulletScript = _bullet.GetComponent<BulletScript>();
                bulletScript.damage = damage;
                bulletScript.ApplyColorFiler(Color.white);
                bulletScript.Fire(firingDirection, 40f, 12f, BulletScript.Shooter.Turret);
                GameManager.Instance.PlayNormalPitchSound(firingSound);
            }
        }
    }
}
