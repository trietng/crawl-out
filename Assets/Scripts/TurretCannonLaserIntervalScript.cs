using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCannonLaserIntervalScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject laser;
    public int interval = 5;
    private LaserScript laserScript;
    private Vector2 firingDirection;

    void Start()
    {
        var _laser = Instantiate(laser, transform);
        laserScript = _laser.GetComponent<LaserScript>();
        laserScript.damage = 999;
        laserScript.ApplyColorFiler(WeaponScript.WeaponType.LaserIII);
        // Calculate the firing direction from the turret's rotation
        firingDirection = gameObject.transform.rotation * Vector2.up;
        StartCoroutine(CyclingFire());
    }

    IEnumerator CyclingFire()
    {
        while (true)
        {
            laserScript.gameObject.SetActive(true);
            laserScript.Fire<TurretCannonLaserScript>(gameObject.transform.position, firingDirection, 1);
            yield return new WaitForSeconds(interval);
            laserScript.gameObject.SetActive(false);
            yield return new WaitForSeconds(interval);
        }
    }
}
