using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAddon : MonoBehaviour
{
    private Rigidbody rb;
    bool targethit;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (targethit)
        {
            return;
        }
        else
        {
            targethit = true;
        }

        rb.isKinematic = true;
    }
}