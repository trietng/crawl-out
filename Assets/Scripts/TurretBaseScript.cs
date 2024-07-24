using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBaseScript : MonoBehaviour
{
    public int health = 1;
    bool destroyed = false;

    private string[] LaserComponents = { "TurretCannonLaser", "TurretCannonSingleSmart" };
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
            Transform turret = null;
            for (int i = 0; i < LaserComponents.Length; ++i) {
                turret = transform.parent.Find(LaserComponents[i]);
                if (turret == null) continue;
                TurretLaserBaseScript test = turret.GetComponent(LaserComponents[i] + "Script") as TurretLaserBaseScript;
                if (test != null) test.DisableAttack();
                break;
            }

            for (int i = 0; i < transform.parent.childCount; i++)
            {
                Transform t = transform.parent.GetChild(i);
                if (t != transform)
                {
                    if (t != turret) t.gameObject.SetActive(false);
                }
            }
            GameManager.Instance.PlayTurretExplosion();
        }
    }
}
