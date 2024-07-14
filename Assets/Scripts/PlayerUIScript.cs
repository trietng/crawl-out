using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUIScript : MonoBehaviour
{
    private Image weaponImage;
    private AudioSource weaponAudioSource;
    private Text healthText;
    [SerializeField] AudioClip pistolFireSound;
    [SerializeField] AudioClip shotgunFireSound;
    [SerializeField] AudioClip machineGunFireSound;
    [SerializeField] AudioClip laserFireSound;

    public static PlayerUIScript Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            var images = GetComponentsInChildren<Image>();
            weaponImage = images.Where(x => x.name == "WeaponImage").FirstOrDefault();
            weaponAudioSource = GetComponentInChildren<AudioSource>();
            healthText = GetComponentInChildren<Text>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }

    public void UpdateBulletImage(PlayerAttack.FireMode fireMode)
    {
        weaponImage.sprite = GameManager.Instance.weaponSprites[(int)fireMode];
    }

    // Call this if you want to update the health text
    public void UpdateHealthText(int health)
    {
        healthText.text = health.ToString();
    }

    public void UpdateAudioClip(PlayerAttack.FireMode fireMode)
    {
        switch (fireMode)
        {
            case PlayerAttack.FireMode.Single:
            case PlayerAttack.FireMode.Burst:
                weaponAudioSource.clip = pistolFireSound;
                break;
            case PlayerAttack.FireMode.Spread:
                weaponAudioSource.clip = shotgunFireSound;
                break;
            case PlayerAttack.FireMode.Auto:
                weaponAudioSource.clip = machineGunFireSound;
                break;
            case PlayerAttack.FireMode.Laser:
                weaponAudioSource.clip = laserFireSound;
                break;

        }
    }

    internal void PlayFireSound(Vector2 dir)
    {
        if (dir.x > 0) weaponAudioSource.panStereo = 0.65f;
        else weaponAudioSource.panStereo = -0.65f;
        weaponAudioSource.Play();
    }
}