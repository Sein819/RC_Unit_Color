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
    Collider2D col;

    public int type;
    public float maxHp;
    public float hp;
    public float attackSpeed;
    [HideInInspector]
    public bool dead;
    [HideInInspector]
    public int immune;

    float timer;
    float attackCd;
    float lastAttackTime;
    bool isRunning;
    bool isAttacking;

    // 플레이어 추적 관련 변수 추가
    public float moveSpeed;      // 적 이동 속도
    public float attackRange;  // 플레이어와의 공격 거리

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
        anim=GetComponent<Animator>();
        col=GetComponent<Collider2D>();

        if(type==1){
            maxHp=50;
            attackSpeed=100;
            moveSpeed=1.5f;
            attackCd=4f;
            attackRange = 0.7f;
        }
        else if(type==101){
            maxHp=200;
            attackSpeed=100;
            moveSpeed=1.5f;
            attackCd=3f;
            attackRange = 3.5f;
        }
        else{
            maxHp=10;
            attackSpeed=50;
            moveSpeed=2;
            attackCd=0.7f;
            attackRange = 0.7f;
        }
        hp=maxHp;
        dead=false;

        timer=0;
        lastAttackTime=-999;
        isRunning=false;
        isAttacking=false;
        immune=0;
    }

    void Update(){
        if (dead) return;
        timer += Time.deltaTime;
        if(type!=1&&type!=101) anim.SetBool("IsRunning", isRunning);


        if (timer > lastAttackTime + attackCd / attackSpeed * 100 && (type==1||type==101)){
            StartCoroutine(Attack());
            lastAttackTime = timer+Random.Range(0f,1.5f);
        }
    }

    void FixedUpdate(){
        if (dead) return;
        Move();
    }

    //이동
    void Move(){
        Transform player = GameManager.instance.player.transform;
        // 플레이어까지의 거리 계산
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if(type!=1){
            if(type==101&&isAttacking) return;
            // 공격 범위 밖이면 플레이어 쪽으로 이동
            if (distance > attackRange)
            {
                if(isAttacking) return;
                Vector2 direction = (player.position - transform.position).normalized;
                rb.MovePosition(rb.position + direction * moveSpeed*Time.fixedDeltaTime);
                isRunning=true;
            }
            // 공격 범위 안이면 공격 시도
            else {
                isRunning=false;
                if (timer > lastAttackTime + attackCd / attackSpeed * 100&&type==0)
                {
                    StartCoroutine(Attack());
                    lastAttackTime = timer;
                }
            }
        }

        // 플레이어 방향에 따라 스프라이트 좌우 반전
        if (player.position.x < transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;
    }

    //공격
    IEnumerator Attack(){
        GameObject prefab;

        isAttacking=true;
        anim.SetTrigger("Attack");
        //기본 적
        if(type==0){
            yield return new WaitForSeconds(0.3f);
            prefab=Instantiate(attackPrefab,new Vector2(transform.position.x,transform.position.y+0.2f),Quaternion.Euler(new Vector3(0,0,sr.flipX?180:0)));
            prefab.GetComponent<EnemyAttack>().enemy=this;
            yield return new WaitForSeconds(0.2f);
        }
        //색보스1
        else if(type==1){
            immune+=1;
            col.enabled=false;
            for(float i=0;i<15;){
                transform.position+=new Vector3(0,Time.deltaTime*30,0);
                yield return null;
                i+=Time.deltaTime*30;
            }
            Vector2 pos = GameManager.instance.player.transform.position;
            prefab=Instantiate(attackPrefab,pos,transform.rotation);
            prefab.GetComponent<EnemyAttack>().enemy=this;

            yield return new WaitForSeconds(0.40f);
            transform.position=pos+new Vector2(0,10);
            for(float i=0;i<10;){
                transform.position-=new Vector3(0,Time.deltaTime*30,0);
                yield return null;
                i+=Time.deltaTime*30;
            }
            col.enabled=true;
            immune-=1;
        }
        //최종 보스
        else if(type==101){
            immune+=1;
            prefab=Instantiate(attackPrefab,GameManager.instance.player.transform.position,transform.rotation);
            prefab.GetComponent<EnemyAttack>().enemy=this;
            yield return new WaitForSeconds(1f);
            immune-=1;
        }
        isAttacking=false;
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
