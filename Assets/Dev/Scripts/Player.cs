using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public FloatingJoystick joy;
    public GameObject slash;

    Rigidbody2D rb;
    
    float timer;
    float attackCd;
    float lastAttackTime;

    void Start(){
        rb = GetComponent<Rigidbody2D>();

        timer=0;
        attackCd=0.7f;
        lastAttackTime=-999;
    }

    void Update(){
        timer+=Time.deltaTime;
    }

    void FixedUpdate(){
        Move();
    }

    //이동
    void Move(){
        float h = joy.Horizontal;
        float v = joy.Vertical;

        Vector2 input = new Vector2(h, v);

        if (input.magnitude > 1f) input.Normalize();

        Vector2 nextPos = input * 3 * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextPos);

        if (input != Vector2.zero){
            Transform direction = transform.Find("PlayerDirection");
            float targetAngle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            direction.rotation = Quaternion.Lerp(direction.rotation, targetRotation, Time.fixedDeltaTime * 15);
        }
        else if(timer>lastAttackTime+attackCd){
            Attack();
            lastAttackTime=timer;
        }
    }

    //공격
    void Attack(){
        Instantiate(slash,new Vector2(transform.position.x,transform.position.y+0.4f),transform.rotation);
    }

}
