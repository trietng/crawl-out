using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    bool isDead;
    public float speed;
    Animator anim;
    float hor;
    float ver;
    Vector2 newPos;
    Rigidbody2D rigit;
    Vector2 dirMove;
    [SerializeField] float toggleTimmer;
    float toggleCounter;
    GameObject player;
    [SerializeField] AudioClip footstepClip;
    [SerializeField] AudioClip bodyHitClip;
    [SerializeField] bool MoveHorizontal;

    [Header("Vision")]
    [SerializeField] Vector2 visionSize;
    [SerializeField] Vector2 alligVision;
    [SerializeField] float attackRange;
    [SerializeField] float allignAttackBound;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float attackTimmer = 0.4f;
    float attackCounter = 0;

    AudioSource audi;
    private int maxHealth = 50;
    private int currentHealth;

    private void Awake()
    {
        isDead = false;
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        rigit = GetComponent<Rigidbody2D>();
        toggleCounter = 0;
        audi = GetComponent<AudioSource>();
        audi.clip = footstepClip;
        currentHealth = maxHealth;

        if (MoveHorizontal)
        {
            alligVision = new Vector2(1.5f, 0);
            visionSize = new Vector2(6, 4f);
        }
        else
        {
            alligVision = new Vector2(0, 1.5f);
            visionSize = new Vector2(4f, 6f);
        }
    }

    void Start()
    {
        audi.Play();
        dirMove = MoveHorizontal ? Vector2.right : Vector2.up;
        attackCounter = Mathf.Infinity;
    }

    void Update()
    {
        if (toggleCounter > toggleTimmer)
        {
            if (!MoveHorizontal) ToggleDirInSameAxis(false);
            else ToggleDirInSameAxis();
            toggleCounter = 0;
        }
        toggleCounter += Time.deltaTime;

        if (Vision() != null)
        {
            toggleCounter = 0;
            Chasing(Vision().gameObject);
        }

        if (attackCounter > attackTimmer)
        {
            CanAttack();
            attackCounter = 0;
        }
        attackCounter += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Move(dirMove);
    }

    void Move(Vector2 dir)
    {
        if (isDead) return;
        anim.SetBool("isMoving", true);
        hor = dir.x == 0 ? 0 : Mathf.Sign(dir.x);
        ver = dir.y == 0 ? 0 : Mathf.Sign(dir.y);
        anim.SetFloat("dirX", hor);
        anim.SetFloat("dirY", ver);
        newPos = rigit.transform.position;
        dir.Normalize();
        newPos.x += dir.x * speed * Time.deltaTime;
        newPos.y += dir.y * speed * Time.deltaTime;
        rigit.transform.position = newPos;
    }

    void ToggleDirInSameAxis(bool isHor = true)
    {
        if (isHor) dirMove.x = 0 - dirMove.x;
        else dirMove.y = 0 - dirMove.y;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            HitZombie();
        }
    }

    public void HitZombie()
    {
        isDead = true;
        audi.priority = 0;
        ServiceScript._instance.PlaySound(bodyHitClip);
        anim.SetTrigger("isDead");
        speed = 0;
    }

    public void Dead()
    {
        Destroy(gameObject);
    }

    Collider2D Vision()
    {
        CapsuleCollider2D bound = (CapsuleCollider2D)gameObject.GetComponentInChildren(typeof(CapsuleCollider2D));
        Vector2 dir = MoveHorizontal ? (Vector2.right + alligVision) * anim.GetFloat("dirX") : (Vector2.up + alligVision) * anim.GetFloat("dirY");
        Collider2D hit = Physics2D.OverlapBox(bound.bounds.center + (Vector3)dir + (Vector3)alligVision, visionSize, 0, playerLayer);
        return hit;
    }

    void Chasing(GameObject target)
    {
        if (target == null || isDead) return;
        dirMove = (target.transform.position - transform.position).normalized;
    }

    void CanAttack()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < attackRange)
        {
            player.GetComponent<PlayerScript>().TakeDamage(10); // Adjust damage amount as needed
        }
    }
}
