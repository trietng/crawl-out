using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBaseScript : MonoBehaviour
{
    public int health = 1;
    bool destroyed = false;
    Animator anim;
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
        if (health <= 0 && !destroyed)
        {
            destroyed = true;
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (transform.parent.GetChild(i) != transform)
                {
                    Animator anim = transform.parent.GetChild(i).gameObject.GetComponent<Animator>();
                    if (anim != null && anim.GetBool("Alive") == true) {
                        Debug.Log("destroyed");
                        anim.SetBool("Alive", false);
                    }
                    else transform.parent.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
}
