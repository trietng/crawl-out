using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public bool isLocked = true;
    private GameObject mainRoomSwitch;
    public string roomSwitch;

    void Start()
    {
        isLocked = true;
        mainRoomSwitch = GameObject.Find(roomSwitch);
    }

    void Update()
    {
        if (isLocked && mainRoomSwitch != null)
        {
            mainRoomSwitch.SetActive(false);
        }
        else if (!isLocked && mainRoomSwitch != null)
        {
            mainRoomSwitch.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            isLocked = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            isLocked = true;
        }
    }
}
