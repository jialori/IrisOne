using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0.005f;

    bool isAlive = true;

    void Start()
    {
        
    }

    void Update()
    {
        if (isAlive)
        {
            Vector2 move = Input.GetAxis("Vertical") * Vector2.up + Input.GetAxis("Horizontal") * Vector2.right;    
            this.transform.position += (Vector3)move * moveSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("monster"))
        {
            isAlive = false;
        }
    }

}
