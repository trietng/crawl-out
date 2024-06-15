using UnityEngine;
using UnityEngine.UI;

public class UI_BulletScript : MonoBehaviour
{
    Text bulletCountText;
    [SerializeField] AudioClip pistolFireSound;
    [SerializeField] AudioClip shotgunFireSound;
    [SerializeField] AudioClip machineGunFireSound;
    private void Awake()
    {
        bulletCountText = GetComponentInChildren<Text>();
    }
    private void Start()
    {
        bulletCountText.text = ServiceScript._instance.bulletCount.ToString();
    }
    public void UpdateBulletCountText()
    {
        bulletCountText.text = ServiceScript._instance.bulletCount.ToString();
    }
    public void UpdateBulletImage(PlayerAttackScript.FireMode fireMode)
    {
        GetComponentInChildren<Image>().sprite = ServiceScript._instance.weaponSprites[(int)fireMode];
    }
    public void UpdateAudioClip(PlayerAttackScript.FireMode fireMode)
    {
        switch (fireMode)
        {
            case PlayerAttackScript.FireMode.Single:
            case PlayerAttackScript.FireMode.Burst:
                GetComponentInChildren<AudioSource>().clip = pistolFireSound;
                break;
            case PlayerAttackScript.FireMode.Spread:
                GetComponentInChildren<AudioSource>().clip = shotgunFireSound;
                break;
            case PlayerAttackScript.FireMode.Auto:
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
