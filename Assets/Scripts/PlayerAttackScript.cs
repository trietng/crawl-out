using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
    {

        [NonSerialized] public Camera mainCam;
        Vector2 currMousePoint;
        Animator anim;
        [SerializeField] GameObject bullet;
        [SerializeField] GameObject slash;
        [SerializeField] GameObject laser;
        [SerializeField] float alignFirePos;
        private float fireTimer;
        private float fireCounter;
        private float bulletSpeed;
        private float bulletRange;
        // private int damageAmount = 10; // Adjust damage amount as needed
        private static readonly KeyCode[] fireModeKeys = {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3
        };

        public GameObject weaponPrefab;

        private GameObject[] weapons = new GameObject[3];

        [NonSerialized] public int currentWeaponIndex;
        public static PlayerAttackScript Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                anim = GetComponent<Animator>();
                mainCam = Camera.main;
                currentWeaponIndex = 1;
                for (int i = 0; i < weapons.Length; i++)
                {
                    weapons[i] = Instantiate(weaponPrefab, transform);
                    weapons[i].GetComponent<WeaponScript>().MakeInventory();
                }
                weapons[0].GetComponent<WeaponScript>().weaponType = WeaponScript.WeaponType.None;
                weapons[1].GetComponent<WeaponScript>().weaponType = WeaponScript.WeaponType.Melee;
                weapons[2].GetComponent<WeaponScript>().weaponType = WeaponScript.WeaponType.LaserI;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            ChangeWeapon();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("WeaponItem"))
            {
                var weaponScript = collision.gameObject.GetComponent<WeaponScript>();
                var weaponType = weaponScript.weaponType;
                if (weaponType.ToString().StartsWith("Laser"))
                {
                    WeaponScript.SwapWeapon(weapons[2], collision.gameObject);
                    PlayerUIScript.Instance.UpdateWeaponImage(weaponType);
                    if (weaponScript.weaponType == WeaponScript.WeaponType.None)
                    {
                        Destroy(collision.gameObject);
                    }
                }
                else
                {
                    WeaponScript.SwapWeapon(weapons[0], collision.gameObject);
                    PlayerUIScript.Instance.UpdateWeaponImage(weaponType);
                    if (weaponScript.weaponType == WeaponScript.WeaponType.None)
                    {
                        Destroy(collision.gameObject);
                    }
                    
                }
                ChangeWeapon();
            }
        }

        // Update is called once per frame
        void Update()
        {
            foreach (KeyCode key in fireModeKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    currentWeaponIndex = key - KeyCode.Alpha1;
                    ChangeWeapon();
                }
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;
                ChangeWeapon();
            }
            WeaponScript.WeaponType weapon = weapons[currentWeaponIndex].GetComponent<WeaponScript>().weaponType;
            bool keyEvent = weapon == WeaponScript.WeaponType.Auto ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);
            if (keyEvent && weapon != WeaponScript.WeaponType.None)
            {
                if (fireCounter > fireTimer && gameObject.GetComponent<PlayerScript>().IsAlive())
                {
                    currMousePoint = mainCam.ScreenToWorldPoint(Input.mousePosition);
                    if (weapon != WeaponScript.WeaponType.Melee && weapon != WeaponScript.WeaponType.None)
                    {
                        anim.SetTrigger("isShooting");
                    }
                    StartCoroutine(Attack(currMousePoint - ((Vector2)gameObject.transform.position + Vector2.up * alignFirePos)));
                    fireCounter = 0;
                }
            }
            fireCounter += Time.deltaTime;
        }
        private void ChangeWeapon()
        {
            var bulletScript = bullet.GetComponent<BulletScript>();
            var weaponType = weapons[currentWeaponIndex].GetComponent<WeaponScript>().weaponType;
            switch (weaponType)
            {
                case WeaponScript.WeaponType.Melee:
                    fireTimer = 0.4f;
                    break;
                case WeaponScript.WeaponType.Single:
                    fireTimer = 0.5f;
                    bulletSpeed = 40f;
                    bulletRange = 12f;
                    break;
                case WeaponScript.WeaponType.Burst:
                    fireTimer = 0.5f;
                    bulletSpeed = 20f;
                    bulletRange = 12f;
                    break;
                case WeaponScript.WeaponType.Spread:
                    fireTimer = 0.5f;
                    bulletSpeed = 30f;
                    bulletRange = 8f;
                    break;
                case WeaponScript.WeaponType.Auto:
                    fireTimer = 0.15f;
                    bulletSpeed = 20f;
                    bulletRange = 16f;
                    break;
                case WeaponScript.WeaponType.LaserI:
                case WeaponScript.WeaponType.LaserII:
                case WeaponScript.WeaponType.LaserIII:
                    fireTimer = 0.5f;
                    break;
            }
            bulletScript.ApplyColorFiler(weaponType);
            PlayerUIScript.Instance.UpdateAudioClip(weaponType);
            PlayerUIScript.Instance.ChangeWeaponSelection(currentWeaponIndex);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(gameObject.transform.position, currMousePoint);
        }
        IEnumerator Attack(Vector2 dir)
        {
            // if (GameManager.Instance.bulletCount < shotCount) yield break;
            // GameManager.Instance.bulletCount -= shotCount;
            // BulletUIScript.Instance.UpdateBulletCountText();
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
            // StartCoroutine(GameManager._instance.TempRemoveCollider(gameObject, 0.1f));
            // StartCoroutine(tempSlowPlayer(0.1f));
            // GameManager._instance.TempRemoveCollider(gameObject, 3f);
            var weapon = weapons[currentWeaponIndex].GetComponent<WeaponScript>();
            var weaponType = weapon.weaponType;
            if (weaponType == WeaponScript.WeaponType.Melee)
            {
                GameObject _slash = Instantiate(
                    slash,
                    (Vector2)gameObject.transform.position + Vector2.up * alignFirePos + dir * 1.25f,
                    Quaternion.identity);
                _slash.GetComponent<SlashScript>().Fire(dir);
            }
            else if (weaponType.ToString().StartsWith("Laser"))
            {
                GameObject _laser = Instantiate(laser);
                var origin = (Vector2)gameObject.transform.position + Vector2.up * alignFirePos + dir * 0.8f;
                _laser.GetComponent<LaserScript>().Fire(origin, dir, weapon.shotCount);
                PlayerUIScript.Instance.PlayFireSound(dir);
            }
            else
            {
                for (int i = 0; i < weapon.shotCount; i++)
                {
                    var curDir = dir;
                    GameObject _bullet = Instantiate(bullet, (Vector2)gameObject.transform.position + Vector2.up * alignFirePos + curDir * 0.8f, Quaternion.identity);
                    switch (weaponType)
                    {
                        case WeaponScript.WeaponType.Spread:
                            curDir = Quaternion.Euler(0, 0, (i - 1) * 10) * curDir;
                            break;
                        case WeaponScript.WeaponType.Burst:
                            break;
                        case WeaponScript.WeaponType.Auto:
                            curDir = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-5, 5)) * curDir;
                            break;
                    }
                    _bullet.GetComponent<BulletScript>().Fire(curDir, bulletSpeed, bulletRange);
                    PlayerUIScript.Instance.PlayFireSound(curDir);
                    if (weaponType == WeaponScript.WeaponType.Burst)
                    {
                        // delay between shots
                        yield return new WaitForSeconds(0.2f);
                    }
                }
            }
        }
        IEnumerator tempSlowPlayer(float sec)
        {
            var playerScript = GetComponent<PlayerScript>();
            playerScript.speed = 0;
            for (int i = 0; i < 1; i++)
            {
                yield return new WaitForSeconds(sec);
            }
            playerScript.speed = playerScript.maxSpeed;
        }
    }