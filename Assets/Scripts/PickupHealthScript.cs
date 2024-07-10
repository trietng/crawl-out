using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHealthScript : MonoBehaviour
{
    private Transform itemTransform;

    void Start()
    {
        itemTransform = transform.GetChild(0).transform;
        StartCoroutine(PickupItemAnimation());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int d = PlayerScript.Instance.maxHealth - PlayerScript.Instance.currentHealth;
            if (d > 0)
            {
                PlayerScript.Instance.currentHealth += Math.Min(10, d);
                PlayerUIScript.Instance.UpdateHealthText(PlayerScript.Instance.currentHealth);
                Destroy(gameObject);
            }
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
