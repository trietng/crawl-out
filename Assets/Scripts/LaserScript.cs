using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class LaserScript : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public GameObject lightSpot;
    [NonSerialized] public int damage;
    private GameObject lightSpotInstance;

    private static readonly Color orange = new(255, 165, 0);

    private static readonly Dictionary<WeaponScript.WeaponType, Gradient> gradients = new()
    {
        {
            WeaponScript.WeaponType.LaserI, 
            new Gradient()
            {
                colorKeys = new GradientColorKey[] { new(Color.blue, 1.0f) }
            }
        },
        {
            WeaponScript.WeaponType.LaserII, 
            new Gradient()
            {
                colorKeys = new GradientColorKey[] { new(orange, 1.0f) }
            }
        },
        {
            WeaponScript.WeaponType.LaserIII, 
            new Gradient()
            {
                colorKeys = new GradientColorKey[] { new(Color.white, 1.0f) }
            }
        }
    };

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    public void Fire<T>(Vector2 origin, Vector2 dir, int reflectCount)
    {
        lineRenderer.positionCount = reflectCount + 1;
        // Set the start position of the laser
        lineRenderer.SetPosition(0, origin);
        for (int i = 0; i < reflectCount; ++i) {
            // Ignore the Confiner object
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, 1000);
            if (hits.Length == 0) {
                continue;
            }
            RaycastHit2D destination = hits.Last();
            for (int j = 0; j < hits.Length - 1; j++)
            {
                if (hits[j].collider.gameObject.CompareTag("Zombie") && hits[j].collider.gameObject.transform.parent.TryGetComponent<ZombieScript>(out var zombie))
                {
                    zombie.TakeDamage(damage);
                }
                if (typeof(T) == typeof(TurretCannonLaserScript))
                {
                    if (hits[j].collider.gameObject.CompareTag("PlayerBound") && hits[j].collider.transform.parent.gameObject.TryGetComponent<PlayerScript>(out var player))
                    {
                        destination = hits[j];
                        player.TakeDamage(damage);
                        break;
                    }
                }
                else
                {
                    if (hits[j].collider.gameObject.CompareTag("Turret"))
                    {
                        destination = hits[j];
                        var turret = hits[j].collider.gameObject.transform.parent.GetComponentInChildren<TurretBaseScript>();
                        turret.TakeDamage(damage);
                        break;
                    }
                }
                if (hits[j].collider.name.CompareTo("walls") == 0)
                {
                    destination = hits[j];
                    break;
                }
            }
            lineRenderer.SetPosition(i + 1, destination.point);
            // Instantiate a light spot at the destination point if it doesn't already exist
            if (typeof(T) == typeof(TurretCannonLaserScript))
            {    
                if (GetComponentsInChildren<Light2D>().Length == 0)
                {
                    lightSpotInstance = Instantiate(lightSpot, destination.point, Quaternion.identity, gameObject.transform);
                }
                else
                {
                    lightSpotInstance.transform.position = destination.point;
                }
            }
            else
            {
                Instantiate(lightSpot, destination.point, Quaternion.identity, gameObject.transform);
            }
            origin = new Vector2(destination.point.x, destination.point.y);
            dir = Vector2.Reflect(dir, destination.normal);
            // Add a small offset to the origin to prevent the laser from hitting the same object again
            origin += dir * 0.01f;
        }
        if (typeof(T) == typeof(PlayerAttackScript))
        {            
            Invoke(nameof(Explode), 0.2f * reflectCount);
        }
    }

    public void FirePersist(Vector2 origin, Vector2 dir)
    {
        lineRenderer.positionCount = 2;
        // Set the start position of the laser
        lineRenderer.SetPosition(0, origin);
        // Ignore the Confiner object
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, 1000);
        if (hits.Length == 0) {
            return;
        }
        RaycastHit2D destination = hits.Last();
        for (int j = 0; j < hits.Length - 1; j++)
        {
            if (hits[j].collider.gameObject.CompareTag("Zombie") && hits[j].collider.gameObject.transform.parent.TryGetComponent<ZombieScript>(out var zombie))
            {
                zombie.TakeDamage(damage);
            }
            if (hits[j].collider.gameObject.CompareTag("Player") && hits[j].collider.gameObject.transform.parent.TryGetComponent<PlayerScript>(out var player))
            {
                print("Player hit");
                destination = hits[j];
                break;
            }
            if (hits[j].collider.name.CompareTo("walls") == 0)
            {
                destination = hits[j];
                break;
            }
        }
        lineRenderer.SetPosition(1, destination.point);
        // Instantiate a light spot at the destination point
        origin = new Vector2(destination.point.x, destination.point.y);
        dir = Vector2.Reflect(dir, destination.normal);
        // Add a small offset to the origin to prevent the laser from hitting the same object again
        origin += dir * 0.01f;
    }

    public void ApplyColorFiler(WeaponScript.WeaponType weapon)
    {
        var lineRenderer = GetComponent<LineRenderer>();
        switch (weapon)
        {
            case WeaponScript.WeaponType.LaserI:
                lineRenderer.colorGradient = gradients[WeaponScript.WeaponType.LaserI];
                lightSpot.GetComponent<Light2D>().color = Color.blue;
                break;
            case WeaponScript.WeaponType.LaserII:
                lineRenderer.colorGradient = gradients[WeaponScript.WeaponType.LaserII];
                lightSpot.GetComponent<Light2D>().color = orange;
                break;
            case WeaponScript.WeaponType.LaserIII:
                lineRenderer.colorGradient = gradients[WeaponScript.WeaponType.LaserIII];
                lightSpot.GetComponent<Light2D>().color = Color.white;
                break;
        }
    }

    void Explode()
    {
        Destroy(gameObject);
    }
}