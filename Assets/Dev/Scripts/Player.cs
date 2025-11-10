using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public FloatingJoystick joy;
    public GameObject slash;

    Rigidbody2D rb;
    SpriteRenderer sr;
    MaterialPropertyBlock mpb;

    public float maxHp;
    public float hp;
    public float attackPower;
    public float attackSpeed;
    [HideInInspector]
    public bool dead;
    
    float timer;
    float attackCd;
    float lastAttackTime;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
    }

    void Start(){
        maxHp=100;
        hp=maxHp;
        attackPower=100;
        attackSpeed=100;
        dead=false;

        timer=0;
        attackCd=0.7f;
        lastAttackTime=-999;
    }

    void Update(){
        timer+=Time.deltaTime;
    }

    void FixedUpdate(){
        if(dead) return;

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
            //방향 애니메이션

            Transform direction = transform.Find("PlayerDirection");
            float targetAngle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            direction.rotation = Quaternion.Lerp(direction.rotation, targetRotation, Time.fixedDeltaTime * 15);
        }
        else if(timer>lastAttackTime+attackCd/attackSpeed*100){
            Attack();
            lastAttackTime=timer;
        }
    }

    //공격
    void Attack(){
        //공격 애니메이션
        Instantiate(slash,new Vector2(transform.position.x,transform.position.y+0.4f),transform.rotation);
    }

    //피해 입기
    public IEnumerator HitColor(){
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat("_IsDamaged", 1f);
        sr.SetPropertyBlock(mpb);
        yield return new WaitForSeconds(0.3f);
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat("_IsDamaged", 0f);
        sr.SetPropertyBlock(mpb);
    }

    public void Die(){
        dead=true;
        //사망 애니메이션
        StartCoroutine(GameManager.instance.GameOver());
    }
}
