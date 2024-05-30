using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UI_BulletScript : MonoBehaviour
{
    Text bulletCountText;
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

    internal void PlayFireSound(Vector2 dir)
    {
        if (dir.x > 0) GetComponentInChildren<AudioSource>().panStereo = 0.65f;
        else GetComponentInChildren<AudioSource>().panStereo = -0.65f;
        GetComponentInChildren<AudioSource>().Play();
    }
}
