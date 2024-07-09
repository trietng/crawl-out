using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCScript : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;
    void Start()
    {
        var vcam = GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
