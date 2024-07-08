using UnityEngine;
using UnityEngine.UI;


public class BulletUIScript : MonoBehaviour
{
    Text bulletCountText;
    Image bulletImage;
    [SerializeField] AudioClip pistolFireSound;
    [SerializeField] AudioClip shotgunFireSound;
    [SerializeField] AudioClip machineGunFireSound;

    public static BulletUIScript Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            bulletCountText = GetComponentInChildren<Text>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        bulletCountText.text = GameManager.Instance.bulletCount.ToString();
    }

    public void UpdateBulletCountText()
    {
        bulletCountText.text = GameManager.Instance.bulletCount.ToString();
    }
    public void UpdateBulletImage(PlayerAttack.FireMode fireMode)
    {
        GetComponentInChildren<Image>().sprite = GameManager.Instance.weaponSprites[(int)fireMode];
    }
    public void UpdateAudioClip(PlayerAttack.FireMode fireMode)
    {
        switch (fireMode)
        {
            case PlayerAttack.FireMode.Single:
            case PlayerAttack.FireMode.Burst:
                GetComponentInChildren<AudioSource>().clip = pistolFireSound;
                break;
            case PlayerAttack.FireMode.Spread:
                GetComponentInChildren<AudioSource>().clip = shotgunFireSound;
                break;
            case PlayerAttack.FireMode.Auto:
                GetComponentInChildren<AudioSource>().clip = machineGunFireSound;
                break;
        }
    }
    internal void PlayFireSound(Vector2 dir)
    {
        if (dir.x > 0) GetComponentInChildren<AudioSource>().panStereo = 0.65f;
        else GetComponentInChildren<AudioSource>().panStereo = -0.65f;
        GetComponentInChildren<AudioSource>().Play();
    }
}