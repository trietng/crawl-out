using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeaponScript : MonoBehaviour
{
    private Transform itemTransform;

    void Start()
    {
        itemTransform = transform.GetChild(0).transform;
        StartCoroutine(PickupItemAnimation());
    }

    void Update()
    {
          
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
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
