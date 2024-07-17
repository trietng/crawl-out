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

        private GameObject weaponGameObject;

        public GameObject weaponPrefab;

        private readonly GameObject[] weapons = new GameObject[3];

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
                foreach (var weapon in weapons)
                {
                    weapon.GetComponent<WeaponScript>().UpdateDataState();
                }
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

        void OnTriggerStay2D(Collider2D collision)
        {
            weaponGameObject = collision.gameObject;
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            weaponGameObject = null;
        }

        // Update is called once per frame
        void Update()
        {
            int newWeaponIndex;
            foreach (KeyCode key in fireModeKeys)
            {
                if (HandleChangeInput(key, out newWeaponIndex))
                {
                    currentWeaponIndex = newWeaponIndex;
                    ChangeWeapon();
                }
            }
            if (HandleChangeInput(KeyCode.Tab, out newWeaponIndex))
            {
                currentWeaponIndex = newWeaponIndex;
                ChangeWeapon();
            }
            if (weaponGameObject != null && Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (weaponGameObject.CompareTag("WeaponItem"))
                {
                    var weaponScript = weaponGameObject.GetComponent<WeaponScript>();
                    var weaponType = weaponScript.weaponType;
                    GameObject oldWeapon = weaponType.ToString().StartsWith("Laser") ? weapons[2] : weapons[0];
                    var oldWeaponScript = oldWeapon.GetComponent<WeaponScript>();
                    WeaponScript.SwapWeapon(oldWeapon, weaponGameObject); 
                    PlayerUIScript.Instance.UpdateWeaponImage(weaponType);
                    PlayerUIScript.Instance.UpdateAmmoText(oldWeaponScript.ammoCount);
                    if (weaponScript.weaponType == WeaponScript.WeaponType.None)
                    {
                        Destroy(weaponGameObject);
                    }
                    ChangeWeapon();
                }
            }
            WeaponScript.WeaponType weapon = weapons[currentWeaponIndex].GetComponent<WeaponScript>().weaponType;
            bool keyEvent = weapon == WeaponScript.WeaponType.Auto ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);
            if (keyEvent && weapon != WeaponScript.WeaponType.None)
            {
                if (fireCounter > fireTimer && gameObject.GetComponent<PlayerScript>().IsAlive())
                {
                    currMousePoint = mainCam.ScreenToWorldPoint(Input.mousePosition);
                    StartCoroutine(Attack(currMousePoint - ((Vector2)gameObject.transform.position + Vector2.up * alignFirePos)));
                    fireCounter = 0;
                }
            }
            fireCounter += Time.deltaTime;
        }

        private bool HandleChangeInput(KeyCode key, out int newWeaponIndex)
        {
            bool shouldChange = false;
            if (Input.GetKeyDown(key))
            {
                if (key == KeyCode.Tab)
                {
                    newWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;
                    shouldChange = weapons[newWeaponIndex].GetComponent<WeaponScript>().weaponType != WeaponScript.WeaponType.None;
                    if (!shouldChange)
                    {
                        int index = weapons.Length - weapons.Skip(newWeaponIndex + 1).SkipWhile(w => w.GetComponent<WeaponScript>().weaponType == WeaponScript.WeaponType.None).Count();
                        if (index != weapons.Length)
                        {
                            newWeaponIndex = index;
                            shouldChange = true;
                        }
                    }
                }
                else {
                    newWeaponIndex = key - KeyCode.Alpha1;
                    shouldChange = weapons[newWeaponIndex].GetComponent<WeaponScript>().weaponType != WeaponScript.WeaponType.None;
                }
            }
            else
            {
                newWeaponIndex = -1;
            }
            return shouldChange;
        }

        private void ChangeWeapon()
        {
            var bulletScript = bullet.GetComponent<BulletScript>();
            var weapon = weapons[currentWeaponIndex].GetComponent<WeaponScript>();
            var weaponType = weapon.weaponType;
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
            var weapon = weapons[currentWeaponIndex].GetComponent<WeaponScript>();
            var weaponType = weapon.weaponType;
            if (WeaponScript.weaponGroups[WeaponScript.WeaponGroup.Ranged].Contains(weaponType))
            {
                if (weapon.ammoCount < weapon.shotCount)
                {
                    yield break;
                }
                weapon.ammoCount -= weapon.shotCount;
                PlayerUIScript.Instance.UpdateAmmoText(weapon.ammoCount);
            }
            if (weaponType != WeaponScript.WeaponType.Melee && weaponType != WeaponScript.WeaponType.None)
            {
                anim.SetTrigger("isShooting");
            }
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
            if (weaponType == WeaponScript.WeaponType.Melee)
            {
                GameObject _slash = Instantiate(
                    slash,
                    (Vector2)gameObject.transform.position + Vector2.up * alignFirePos + dir * 1.25f,
                    Quaternion.identity);
                _slash.GetComponent<SlashScript>().Fire(dir);
            }
            else if (WeaponScript.weaponGroups[WeaponScript.WeaponGroup.Laser].Contains(weaponType))
            {
                print(weapon.shotCount);
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
    }