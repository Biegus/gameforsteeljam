using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoulderBounce : MonoBehaviour
{
    private bool bounced = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EdgeCollider2D>() && !bounced) //not work
        {
            bounced = true;
            var rbody = other.GetComponent<Rigidbody2D>();
            rbody.velocity = Vector2.up;
            gameObject.layer = 6;
        }
    }
}
