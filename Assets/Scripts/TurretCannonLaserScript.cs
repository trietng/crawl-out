using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretCannonLaserScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject laser;
    private LaserScript laserScript;
    private Vector2 firingDirection;
    private Vector2 firingOrigin;
    public static readonly float magicMultiplier = 0.5f;
    public static readonly int damage = 999;

    void Start()
    {
        var _laser = Instantiate(laser, transform);
        laserScript = _laser.GetComponent<LaserScript>();
        laserScript.damage = damage;
        laserScript.ApplyColorFiler(WeaponScript.WeaponType.LaserIII);
        // Calculate the firing direction from the turret's rotation
        firingDirection = gameObject.transform.rotation * Vector2.up;
        print(firingDirection);
        firingOrigin = gameObject.transform.position.ConvertTo<Vector2>() + (firingDirection * magicMultiplier);
    }

    // Update is called once per frame
    void Update()
    {
    
        laserScript.Fire<TurretCannonLaserScript>(firingOrigin, firingDirection, 1);
    }
}
