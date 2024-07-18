using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretCannonLaserIntervalScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject laser;
    public float interval;
    private LaserScript laserScript;
    private Vector2 firingDirection;
    private Vector2 firingOrigin;
    private static readonly float magicMultiplier = 0.5f;

    void Start()
    {
        var _laser = Instantiate(laser, transform);
        laserScript = _laser.GetComponent<LaserScript>();
        laserScript.damage = 999;
        laserScript.ApplyColorFiler(WeaponScript.WeaponType.LaserIII);
        // Calculate the firing direction from the turret's rotation
        firingDirection = gameObject.transform.rotation * Vector2.up;
        firingOrigin = gameObject.transform.position.ConvertTo<Vector2>() + (firingDirection * magicMultiplier);
        StartCoroutine(CyclingFire());
    }

    void Update()
    {
        if (laserScript.gameObject.activeSelf)
        {
            laserScript.Fire<TurretCannonLaserScript>(gameObject.transform.position, firingDirection, 1);
        }
    }

    IEnumerator CyclingFire()
    {
        while (true)
        {
            laserScript.gameObject.SetActive(true);
            yield return new WaitForSeconds(interval / 2);
            laserScript.gameObject.SetActive(false);
            yield return new WaitForSeconds(interval / 4);
        }
    }
}
