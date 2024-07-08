using System;
using System.Collections;
using UnityEngine;

namespace PlayerAttack
{
    public enum FireMode
    {
        Melee,
        Single,
        Burst,
        Spread,
        Auto
    }
    public class PlayerAttackScript : MonoBehaviour
    {

        [NonSerialized] public Camera mainCam;
        Vector2 currMousePoint;
        Animator anim;
        [SerializeField] GameObject bullet;
        [SerializeField] GameObject slash;
        [SerializeField] float alignFirePos;
        private float fireTimer;
        private float fireCounter;
        private float bulletSpeed;
        private float bulletRange;
        // private int damageAmount = 10; // Adjust damage amount as needed
        private static readonly KeyCode[] fireModeKeys = {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5
        };
        private FireMode fireMode;
        private int shotCount;
        public static PlayerAttackScript Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                anim = GetComponent<Animator>();
                mainCam = Camera.main;
                fireMode = FireMode.Single;
                UpdateFireMode();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        // Update is called once per frame
        void Update()
        {
            foreach (KeyCode key in fireModeKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    fireMode = (FireMode)(key - KeyCode.Alpha1);
                    UpdateFireMode();
                }
            }
            bool keyEvent = fireMode == FireMode.Auto ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);
            if (keyEvent)
            {
                if (fireCounter > fireTimer && gameObject.GetComponent<PlayerScript>().IsAlive())
                {
                    currMousePoint = mainCam.ScreenToWorldPoint(Input.mousePosition);
                    if (fireMode != FireMode.Melee)
                    {
                        anim.SetTrigger("isShooting");
                    }
                    StartCoroutine(Attack(currMousePoint - ((Vector2)gameObject.transform.position + Vector2.up * alignFirePos)));
                    fireCounter = 0;
                }
            }
            fireCounter += Time.deltaTime;
        }
        private void UpdateFireMode()
        {
            var bulletScript = bullet.GetComponent<BulletScript>();
            switch (fireMode)
            {
                case FireMode.Melee:
                    fireTimer = 0.4f;
                    shotCount = 0;
                    break;
                case FireMode.Single:
                    fireTimer = 0.5f;
                    bulletSpeed = 40f;
                    bulletRange = 12f;
                    shotCount = 1;
                    break;
                case FireMode.Burst:
                    fireTimer = 0.5f;
                    bulletSpeed = 20f;
                    bulletRange = 12f;
                    shotCount = 2;
                    break;
                case FireMode.Spread:
                    fireTimer = 0.5f;
                    bulletSpeed = 30f;
                    bulletRange = 8f;
                    shotCount = 3;
                    break;
                case FireMode.Auto:
                    fireTimer = 0.15f;
                    bulletSpeed = 20f;
                    bulletRange = 16f;
                    shotCount = 1;
                    break;
            }
            bulletScript.ApplyColorFiler(fireMode);
            // BulletUIScript.Instance.UpdateBulletImage(fireMode);
            // BulletUIScript.Instance.UpdateAudioClip(fireMode);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(gameObject.transform.position, currMousePoint);
        }
        IEnumerator Attack(Vector2 dir)
        {
            if (GameManager.Instance.bulletCount < shotCount) yield break;
            GameManager.Instance.bulletCount -= shotCount;
            BulletUIScript.Instance.UpdateBulletCountText();
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
            if (fireMode == FireMode.Melee)
            {
                GameObject _slash = Instantiate(
                    slash,
                    (Vector2)gameObject.transform.position + Vector2.up * alignFirePos + dir * 1.25f,
                    Quaternion.identity);
                StartCoroutine(GameManager.Instance.TempRemoveCollider(_slash, 0.1f));
                _slash.GetComponent<SlashScript>().Fire(dir);
            }
            else
            {
                for (int i = 0; i < shotCount; i++)
                {
                    var curDir = dir;
                    GameObject _bullet = Instantiate(bullet, (Vector2)gameObject.transform.position + Vector2.up * alignFirePos + curDir * 0.8f, Quaternion.identity);
                    switch (fireMode)
                    {
                        case FireMode.Spread:
                            curDir = Quaternion.Euler(0, 0, (i - 1) * 10) * curDir;
                            break;
                        case FireMode.Burst:
                            break;
                        case FireMode.Auto:
                            curDir = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-5, 5)) * curDir;
                            break;
                    }
                    StartCoroutine(GameManager.Instance.TempRemoveCollider(_bullet, 0.05f));
                    _bullet.GetComponent<BulletScript>().Fire(curDir, bulletSpeed, bulletRange);
                    BulletUIScript.Instance.PlayFireSound(curDir);
                    if (fireMode == FireMode.Burst)
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
}