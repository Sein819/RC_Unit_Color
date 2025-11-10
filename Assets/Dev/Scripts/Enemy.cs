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

    // 플레이어 추적 관련 변수 추가
    public Transform player;          // 플레이어 Transform 연결용
    public float moveSpeed = 2f;      // 적 이동 속도
    public float attackRange = 1.5f;  // 플레이어와의 공격 거리

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
    }

    void Start()
    {
        maxHp = 20;
        hp = maxHp;
        attackPower = 100;
        attackSpeed = 100;
        dead = false;

        timer = 0;
        attackCd = 0.7f;
        lastAttackTime = -999;

        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (dead) return;

        timer += Time.deltaTime;

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
            }
            // 공격 범위 안이면 공격 시도
            else if (timer > lastAttackTime + attackCd / attackSpeed * 100)
            {
                Attack();
                lastAttackTime = timer;
            }

            // 플레이어 방향에 따라 스프라이트 좌우 반전
            if (player.position.x < transform.position.x)
                sr.flipX = true;
            else
                sr.flipX = false;
        }
        // 플레이어 추적 기능 끝
    }

    void Attack()
    {
        // 공격 애니메이션
        Instantiate(attackPrefab, new Vector2(transform.position.x, transform.position.y + 0.4f), transform.rotation);
    }

    //피해 입기
    public IEnumerator HitColor()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        sr.color = Color.white;
    }

    //사망
    public void Die()
    {
        dead = true;
        GameManager.instance.EnmeyDie();
        Destroy(gameObject);
    }
}
