using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 input;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    void Update(){
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;
    }

    void FixedUpdate(){
        rb.MovePosition(rb.position + input * 3f * Time.fixedDeltaTime);
    }
}
