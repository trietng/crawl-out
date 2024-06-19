using UnityEngine;

public class SlashScript : MonoBehaviour
{
    public void Fire(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Invoke(nameof(Explode), 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            collision.gameObject.GetComponent<ZombieScript>().TakeDamage(10); // Adjust damage amount as needed
        }
    }

    public void Explode()
    {
        Destroy(gameObject);
    }
}
