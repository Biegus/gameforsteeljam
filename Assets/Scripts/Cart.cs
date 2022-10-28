using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : MonoBehaviour
{
    [SerializeField]
    private Vector2 MoveDirection = Vector2.right;

    [SerializeField] 
    private float Speed = 1;


    void Update()
    {
        transform.position += (Vector3)(MoveDirection * (Speed * Time.deltaTime));
    }
}
