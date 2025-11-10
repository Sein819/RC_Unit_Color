using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject attackPrefab;

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
        maxHp=20;
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

        if(dead) return;

        //플레이어가 가까이 있으면
        if(timer>lastAttackTime+attackCd/attackSpeed*100){
            Attack();
            lastAttackTime=timer;
        }
    }

    void Attack(){
        //공격 애니메이션
        Instantiate(attackPrefab,new Vector2(transform.position.x,transform.position.y+0.4f),transform.rotation);
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

    //사망
    public void Die(){
        dead=true;
        GameManager.instance.EnmeyDie();
        Destroy(gameObject);
    }
}
