using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Vector2 moveDir;
    public float moveSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        this.transform.position += (Vector3)moveDir * moveSpeed;
    }
}
