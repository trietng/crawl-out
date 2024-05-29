using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}
