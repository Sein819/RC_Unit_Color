using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    MaterialPropertyBlock mpb;

    public float maxHp;
    public float hp;
    public float attackPower;
    public float attackSpeed;
    [HideInInspector]
    public bool dead;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();

        maxHp=100;
        hp=maxHp;
        attackPower=100;
        attackSpeed=100;
        dead=false;
    }

    void Update(){
        
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
        Destroy(gameObject);
    }
}
