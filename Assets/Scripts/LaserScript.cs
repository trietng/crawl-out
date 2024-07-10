using System.Linq;
using UnityEngine;


public class LaserScript : MonoBehaviour
{
    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    public void Fire(Vector2 origin, Vector2 dir)
    {
        // Set the start position of the laser
        lineRenderer.SetPosition(0, origin);
        // Ignore the Confiner object
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir)
        .Where(hit => hit.collider.name != "Confiner")
        .ToArray();
        lineRenderer.SetPosition(1, hits.Last().point);
        for (int i = 0; i < hits.Length - 1; i++)
        {
            // TODO: Damage enemies
            if (hits[i].collider.name == "walls")
            {
                lineRenderer.SetPosition(1, hits[i].point);
                break;
            }
        }
        Invoke(nameof(Explode), 0.2f);
    }

    void Explode()
    {
        Destroy(gameObject);
    }
}