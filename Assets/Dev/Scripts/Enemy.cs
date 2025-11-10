using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject attackPrefab;
    public GameObject dieEffect;

    Rigidbody2D rb;
    SpriteRenderer sr;
    MaterialPropertyBlock mpb;
    Animator anim;

    public int type;
    public float maxHp;
    public float hp;
    public float attackPower;
    public float attackSpeed;
    [HideInInspector]
    public bool dead;

    float timer;
    float attackCd;
    float lastAttackTime;
    bool isRunning;

    // 플레이어 추적 관련 변수 추가
    public Transform player;          // 플레이어 Transform 연결용
    public float moveSpeed;      // 적 이동 속도
    public float attackRange;  // 플레이어와의 공격 거리

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
        anim=GetComponent<Animator>();

        if(type==1){
            maxHp=50;
            attackPower=200;
            attackSpeed=25;
            moveSpeed=1.5f;
        }
        else{
            maxHp=10;
            attackPower=100;
            attackSpeed=50;
            moveSpeed=2;
        }
        hp=maxHp;
        dead=false;

        timer=0;
        attackCd=0.7f;
        lastAttackTime=-999;
        isRunning=false;
        attackRange = 0.7f;

        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
    }

    void Update(){
        if (dead) return;

        timer += Time.deltaTime;
        anim.SetBool("IsRunning", isRunning);

        // $$ 플레이어 추적 기능 시작
        if (player != null)
        {
            // 플레이어까지의 거리 계산
            float distance = Vector2.Distance(transform.position, player.position);

            // 공격 범위 밖이면 플레이어 쪽으로 이동
            if (distance > attackRange)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
                isRunning=true;
            }
            // 공격 범위 안이면 공격 시도
            else {
                isRunning=false;
                if (timer > lastAttackTime + attackCd / attackSpeed * 100)
                {
                    StartCoroutine(Attack());
                    lastAttackTime = timer;
                }
            }

            // 플레이어 방향에 따라 스프라이트 좌우 반전
            if (player.position.x < transform.position.x)
                sr.flipX = true;
            else
                sr.flipX = false;
        }
        // 플레이어 추적 기능 끝
    }

    IEnumerator Attack(){
        //공격 애니메이션
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.3f);
        Instantiate(attackPrefab,new Vector2(transform.position.x,transform.position.y+0.4f),transform.rotation);
    }

    //피해 입기
    public IEnumerator HitColor(){
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat("_IsDamaged", 1f);
        sr.SetPropertyBlock(mpb);
        yield return new WaitForSeconds(0.15f);
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat("_IsDamaged", 0f);
        sr.SetPropertyBlock(mpb);
    }

    //사망
    public void Die(){
        dead=true;
        Instantiate(dieEffect,transform.position,transform.rotation);
        GameManager.instance.EnmeyDie();
        Destroy(gameObject);
    }
}
