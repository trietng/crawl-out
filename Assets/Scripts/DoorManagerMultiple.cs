using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManagerMultiple : MonoBehaviour
{
    public bool isLocked = true;
    private GameObject mainRoomSwitch;
    public string roomSwitch;

    void Start()
    {
        mainRoomSwitch = GameObject.Find(roomSwitch);
        UpdateDoorState();
    }

    void Update()
    {
        UpdateDoorState();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (KeyManagerMultiple.collectedKeys >= 4)
            {
                isLocked = false;
            }
            else
            {
                isLocked = true;
            }
        }
    }

    private void UpdateDoorState()
    {
        if (mainRoomSwitch != null)
        {
            mainRoomSwitch.SetActive(!isLocked);
        }
    }
}
