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
    private void Awake()
    {
        isDead = false;
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        rigit = GetComponent<Rigidbody2D>();
        toggleCounter = 0;
        audi = GetComponent<AudioSource>();
        audi.clip = footstepClip;
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
    // Start is called before the first frame update
    void Start()
    {
        audi.Play();
        dirMove = MoveHorizontal ? Vector2.right : Vector2.up;
        attackCounter = Mathf.Infinity;
    }

    // Update is called once per frame
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
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.green;
        CapsuleCollider2D bound = (CapsuleCollider2D)gameObject.GetComponentInChildren(typeof(CapsuleCollider2D));
        Vector2 dir = MoveHorizontal ? (Vector2.right + alligVision) * anim.GetFloat("dirX") : (Vector2.up + alligVision) * anim.GetFloat("dirY");
        Gizmos.DrawWireCube(bound.bounds.center + (Vector3)dir, visionSize);


        CapsuleCollider2D _bound = (CapsuleCollider2D)gameObject.GetComponentInChildren(typeof(CapsuleCollider2D));
        Vector2 _dir = MoveHorizontal ? (Vector2.right * allignAttackBound) * anim.GetFloat("dirX") : (Vector2.up * allignAttackBound) * anim.GetFloat("dirY");
        Collider2D hit = Physics2D.OverlapCircle(bound.bounds.center + (Vector3)dir, attackRange);
        Gizmos.DrawWireSphere(_bound.bounds.center + (Vector3)_dir, attackRange);
    }
    void Chasing(GameObject target) 
    {
        if (target == null || isDead) return;
        Vector3 dir = target.transform.position - transform.position;
       // float _angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Deg2Rad;
        float _angle = MoveHorizontal ? (Vector2.Angle(dir, Vector2.right)) : (Vector2.Angle(dir, Vector2.up));
        if (_angle > 45 && _angle < 135)
        {
            Debug.Log(_angle);
            ChangeAxis(target.transform.position,MoveHorizontal);
        }
        if (dir.magnitude > 1) dir.Normalize();
        Vector2 pos = transform.position;
        pos.x += (speed + 2) * Time.deltaTime * dir.x;
        pos.y += (speed + 2) * Time.deltaTime * dir.y;
        transform.position = pos;
    }
    void ChangeAxis(Vector2 tar, bool fromHorToVer = true)
    {
        if (fromHorToVer)
        {
            alligVision = new Vector2(0, 1.5f);
            visionSize = new Vector2(4f, 6f);
            if (tar.y > transform.position.y) dirMove = Vector2.up;
            else dirMove = Vector2.down;
        }
        else
        {
            alligVision = new Vector2(1.5f, 0);
            visionSize = new Vector2(6, 4f);
            if (tar.x > transform.position.x) dirMove = Vector2.right;
            else dirMove = Vector2.left;
        }
        MoveHorizontal = !MoveHorizontal;
    }
    void CanAttack()
    {
        if (isDead) return;
        CapsuleCollider2D bound = (CapsuleCollider2D)gameObject.GetComponentInChildren(typeof(CapsuleCollider2D));
        Vector2 dir = MoveHorizontal ? (Vector2.right * allignAttackBound) * anim.GetFloat("dirX") : (Vector2.up * allignAttackBound) * anim.GetFloat("dirY");
        Collider2D[] hit = Physics2D.OverlapCircleAll(bound.bounds.center + (Vector3)dir, attackRange, playerLayer);
      //  Collider2D hit = Physics2D.OverlapCircle(bound.bounds.center + (Vector3)dir, attackRange, playerLayer);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i] != null) anim.SetTrigger("isAttack");
        }
        /*
        if (hit != null)
        {
                anim.SetTrigger("isAttack");
        } */
    }
    
    void AttackPlayer()
    {
        player.GetComponent<PlayerScript>().PlayerDead();
    }
}
