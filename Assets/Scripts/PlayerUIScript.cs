using UnityEngine;
using UnityEngine.UI;


public class PlayerUIScript : MonoBehaviour
{
    private Image bulletImage;
    private AudioSource bulletAudio;
    [SerializeField] AudioClip pistolFireSound;
    [SerializeField] AudioClip shotgunFireSound;
    [SerializeField] AudioClip machineGunFireSound;

    public static PlayerUIScript Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            bulletImage = GetComponentInChildren<Image>();
            bulletAudio = GetComponentInChildren<AudioSource>();
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
        bulletImage.sprite = GameManager.Instance.weaponSprites[(int)fireMode];
    }

    public void UpdateAudioClip(PlayerAttack.FireMode fireMode)
    {
        switch (fireMode)
        {
            case PlayerAttack.FireMode.Single:
            case PlayerAttack.FireMode.Burst:
                bulletAudio.clip = pistolFireSound;
                break;
            case PlayerAttack.FireMode.Spread:
                bulletAudio.clip = shotgunFireSound;
                break;
            case PlayerAttack.FireMode.Auto:
                bulletAudio.clip = machineGunFireSound;
                break;
        }
    }

    internal void PlayFireSound(Vector2 dir)
    {
        if (dir.x > 0) bulletAudio.panStereo = 0.65f;
        else bulletAudio.panStereo = -0.65f;
        bulletAudio.Play();
    }
}