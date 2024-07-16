using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public enum WeaponType
    {
        None,
        Melee,
        Single,
        Burst,
        Spread,
        Auto,
        LaserI,
        LaserII,
        LaserIII
    }

    private Transform itemTransform;
    private bool animate = true;

    [NonSerialized] public int ammoCount;
    [NonSerialized] public int damage;
    [NonSerialized] public int shotCount;

    public WeaponType weaponType;

    void Start()
    {
        UpdateDataState();
        itemTransform = transform.GetChild(0).transform;
        StartCoroutine(PickupItemAnimation());
    }

    void UpdateDataState()
    {
        switch (weaponType)
        {
            case WeaponType.Melee:
                ammoCount = 0;
                damage = 10;
                shotCount = 0;
                break;
            case WeaponType.Single:
                ammoCount = 10;
                damage = 10;
                shotCount = 1;
                break;
            case WeaponType.Burst:
                ammoCount = 12;
                damage = 7;
                shotCount = 2;
                break;
            case WeaponType.Spread:
                ammoCount = 6;
                damage = 6;
                shotCount = 3;
                break;
            case WeaponType.Auto:
                ammoCount = 24;
                damage = 3;
                shotCount = 1;
                break;
            case WeaponType.LaserI:
                ammoCount = 0;
                damage = 1;
                shotCount = 1;
                break;
            case WeaponType.LaserII:
            case WeaponType.LaserIII:
                ammoCount = 100;
                damage = 5;
                shotCount = 5;
                break;
        }
        if (weaponType != WeaponType.None)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GameManager.Instance.weaponSprites[(int)weaponType - 1];
        }
    }

    public void MakeInventory()
    {
        animate = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(x => x.enabled = false);
    }

    private IEnumerator PickupItemAnimation()
    {
        while (animate)
        {
            // itemTransform.Rotate(Vector3.up, 60 * Time.deltaTime, Space.World);
            itemTransform.position += new Vector3(0, Mathf.Sin(Time.time * 3) * 0.001f, 0);
            yield return null;
        }
    }

    internal static void SwapWeapon(GameObject oldWeapon, GameObject newWeapon)
    {
        var oldWeaponScript = oldWeapon.GetComponent<WeaponScript>();
        var newWeaponScript = newWeapon.GetComponent<WeaponScript>();
        (oldWeaponScript.weaponType, newWeaponScript.weaponType) = (newWeaponScript.weaponType, oldWeaponScript.weaponType);
        (oldWeaponScript.shotCount, newWeaponScript.shotCount) = (newWeaponScript.shotCount, oldWeaponScript.shotCount);
        (oldWeaponScript.damage, newWeaponScript.damage) = (newWeaponScript.damage, oldWeaponScript.damage);
        (oldWeaponScript.ammoCount, newWeaponScript.ammoCount) = (newWeaponScript.ammoCount, oldWeaponScript.ammoCount);
    }
}
