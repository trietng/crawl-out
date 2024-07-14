using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class LaserScript : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public GameObject lightSpot;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    public void Fire(Vector2 origin, Vector2 dir, int reflectCount)
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
                if (hits[j].collider.gameObject.CompareTag("Zombie"))
                {
                    if (hits[j].collider.gameObject.transform.parent.TryGetComponent<ZombieScript>(out var zombie))
                    {
                        zombie.TakeDamage(999);
                    }
                }
                if (hits[j].collider.name.CompareTo("walls") == 0)
                {
                    destination = hits[j];
                    break;
                }
            }
            lineRenderer.SetPosition(i + 1, destination.point);
            // Instantiate a light spot at the destination
            Instantiate(lightSpot, destination.point, Quaternion.identity, gameObject.transform);
            origin = new Vector2(destination.point.x, destination.point.y);
            dir = Vector2.Reflect(dir, destination.normal);
            // Add a small offset to the origin to prevent the laser from hitting the same object again
            origin += dir * 0.01f;
        }
        Invoke(nameof(Explode), 0.2f * reflectCount);
    }

    void Explode()
    {
        Destroy(gameObject);
    }
}