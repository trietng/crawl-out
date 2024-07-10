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
        foreach (var hit in hits)
        {
            // TODO: hit enemies here
            if (hit.collider.name == "walls")
            {
                GetComponent<LineRenderer>().SetPosition(1, hit.point);
                break;
            }
        }
    }
}