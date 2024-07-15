using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUIScript : MonoBehaviour
{
    private Image weaponRangedImage;
    private Image weaponLaserImage;
    private Image weaponSelectionImage;
    private AudioSource weaponAudioSource;
    private Text healthText;
    [SerializeField] AudioClip pistolFireSound;
    [SerializeField] AudioClip shotgunFireSound;
    [SerializeField] AudioClip machineGunFireSound;
    [SerializeField] AudioClip laserFireSound;

    public static PlayerUIScript Instance { get; private set; }

    private static float[] weaponSelectionPosXOffsets = new float[] { 32, 254, 354 };

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            var images = GetComponentsInChildren<Image>();
            weaponLaserImage = images.Where(x => x.name == "WeaponLaserImage").FirstOrDefault();
            weaponRangedImage = images.Where(x => x.name == "WeaponRangedImage").FirstOrDefault();
            weaponSelectionImage = images.Where(x => x.name == "WeaponSelectionImage").FirstOrDefault();
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

    public void ChangeWeaponSelection(int currentWeaponIndex)
    {
        // weaponRangedImage.sprite = GameManager.Instance.weaponSprites[];
        weaponSelectionImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(weaponSelectionPosXOffsets[currentWeaponIndex], weaponSelectionImage.rectTransform.anchoredPosition.y);
    }

    // Call this if you want to update the health text
    public void UpdateHealthText(int health)
    {
        healthText.text = health.ToString();
    }

    public void UpdateAudioClip(WeaponScript.WeaponType weapon)
    {
        switch (weapon)
        {
            case WeaponScript.WeaponType.Single:
                weaponAudioSource.clip = pistolFireSound;
                break;
            case WeaponScript.WeaponType.Burst:
                weaponAudioSource.clip = shotgunFireSound;
                break;
            case WeaponScript.WeaponType.Auto:
                weaponAudioSource.clip = machineGunFireSound;
                break;
            case WeaponScript.WeaponType.LaserI:
            case WeaponScript.WeaponType.LaserII:
            case WeaponScript.WeaponType.LaserIII:
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