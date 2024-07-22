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

    private Animation animation;
    private Animator animator;

    void Start()
    {
        var _laser = Instantiate(laser, transform);
        laserScript = _laser.GetComponent<LaserScript>();
        laserScript.damage = TurretCannonLaserScript.damage;
        laserScript.ApplyColorFiler(WeaponScript.WeaponType.LaserIII);   
        firingDirection = gameObject.transform.rotation * Vector2.up;
        firingOrigin = gameObject.transform.position.ConvertTo<Vector2>() + (firingDirection * TurretCannonLaserScript.magicMultiplier);
        animator = GetComponent<Animator>();
        StartCoroutine(CyclingFire());
    }

    void Update()
    {
        if (laserScript.gameObject.activeSelf)
        {
            laserScript.Fire<TurretCannonLaserScript>(firingOrigin, firingDirection, 1);
        }
    }

    IEnumerator CyclingFire()
    {
        while (true)
        {
            animator.Play("Turret_02_MK3", 0, 0);
            animator.speed = 1f;
            laserScript.gameObject.SetActive(true);
            // animator.speed = 0f;
            yield return new WaitForSeconds(interval / 2);
            laserScript.gameObject.SetActive(false);
            animator.Play("Turret_02_MK3", 0, 0);
            animator.speed = 0f;
            yield return new WaitForSeconds(interval / 4);
        }
    }
}
