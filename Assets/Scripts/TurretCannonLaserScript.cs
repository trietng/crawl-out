using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCannonLaserScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject laser;
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
    }

    // Update is called once per frame
    void Update()
    {
    
        laserScript.Fire<TurretCannonLaserScript>(gameObject.transform.position, firingDirection, 1);
    }
}
