using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBaseScript : MonoBehaviour
{
    public int health = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        // TODO: Add explosion effect
        health -= damage;
        if (health <= 0)
        {
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (transform.parent.GetChild(i) != transform)
                {
                    transform.parent.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
}
