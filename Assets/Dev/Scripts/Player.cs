using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public FloatingJoystick joy;

    Rigidbody2D rb;
    Vector2 input;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    void Update(){
        
    }

    void FixedUpdate(){
        Move();
    }

    //이동
    void Move(){
        float h = joy.Horizontal;
        float v = joy.Vertical;

        input = new Vector2(h, v);

        if (input.magnitude > 1f) input.Normalize();

        Vector2 nextPos = input * 3 * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextPos);

        if (input != Vector2.zero){
            //방향
        }
        else{
            Attack();
        }
    }

    void Attack(){
    }

}
