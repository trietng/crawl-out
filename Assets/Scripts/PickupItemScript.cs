using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItemScript : MonoBehaviour
{
    public enum ItemType
    {
        Health,
        Weapon
    }

    private Transform itemTransform;

    public ItemType itemType;

    void Start()
    {
        itemTransform = transform.GetChild(0).transform;
        StartCoroutine(PickupItemAnimation());
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (itemType == ItemType.Health)
            {
                // TODO: Add health to player
            }
            else if (itemType == ItemType.Weapon)
            {
                // TODO: Make player pick up weapon
            }
            Destroy(gameObject);
        }
    }


    private IEnumerator PickupItemAnimation()
    {
        while (true)
        {
            // itemTransform.Rotate(Vector3.up, 60 * Time.deltaTime, Space.World);
            itemTransform.position += new Vector3(0, Mathf.Sin(Time.time * 3) * 0.001f, 0);
            yield return null;
        }
    }
}
