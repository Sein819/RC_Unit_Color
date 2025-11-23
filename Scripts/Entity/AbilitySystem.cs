using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySystem : MonoBehaviour
{
    Player player;

    [Header("쿨타임 설정 (초)")]
    public float berserkCooldown = 60f;
    public float reflectCooldown = 60f;
    public float doubleStrikeCooldown = 30f;
    public float chargeCooldown = 60f;

    float lastBerserkTime;
    float lastReflectTime;
    float lastDoubleStrikeTime;
    float lastChargeTime;

    bool healthRegenActivate;

    [Header("체력 회복 관련")]
    public float healthRegenCooldown = 40f; // 40초마다 회복
    float lastHealthRegenTime;

    // 부활 패시브 (초록색 5번)
    [HideInInspector]
    public bool revive;

    void Awake()
    {
        player = gameObject.GetComponent<Player>();
    }

    void Start()
    {
        healthRegenActivate = false;

        lastHealthRegenTime = -99;
        lastBerserkTime = -99;
        lastReflectTime = -99;
        lastDoubleStrikeTime = -99;
        lastChargeTime = -99;

        revive=false;
    }

    void Update()
    {
        // ─────────────────────────────
        // 체력 자동 회복 (초록색 3번)
        // ─────────────────────────────
        if (healthRegenActivate && Time.time - lastHealthRegenTime >= healthRegenCooldown)
        {
            float regenAmount = player.maxHp * 0.1f;
            player.hp = Mathf.Min(player.hp + regenAmount, player.maxHp);
            lastHealthRegenTime = Time.time;
            Debug.Log($"체력 회복 발동! +{regenAmount} (현재 체력: {player.hp})");
        }
    }


    // ─────────────────────────────
    // 흑백
    // ─────────────────────────────
    public void Black1()
    {
        Debug.Log($"흑백 스킬 발동! (적을 동료로 전환)");
        // 실제 AI/팀 전환은 Enemy 스크립트에서 구현 필요
    }


    // ─────────────────────────────
    // 빨간색
    // ─────────────────────────────

    // 0번: 강하게 치기 (이미 존재)
    public void Red1()
    {
        GameManager.instance.redSkill1Activate = true;
        Debug.Log("강하게 치기 패시브 적용");
    }

    // 1번: 광전사 (이미 존재)
    public void Red2()
    {
        if (Time.time - lastBerserkTime < berserkCooldown) return;

        StartCoroutine(BerserkRoutine());
        lastBerserkTime = Time.time;
    }

    IEnumerator BerserkRoutine()
    {
        player.berserkerActivate = true;
        Debug.Log("🔥 광전사 발동! 공격력 1.5배 (5초간)");
        yield return new WaitForSeconds(5f);

        player.berserkerActivate = false;
        Debug.Log("🔥 광전사 종료");
    }

    // 2번: 체력 소모 + 강공격 (신규)
    public void Red3()
    {
        if (player.hp <= 50f)
        {
            Debug.Log("❌ 체력이 부족하여 강공격 불가!");
            return;
        }

        // 체력 소모
        player.hp -= 50f;

        // 데미지 2배 적용
        player.redFinalActive = true;

        Debug.Log("🔴 체력 소모 강공격 발동! 체력 -50, 다음 공격 2배!");
    }

    // ─────────────────────────────
    // 초록색
    // ─────────────────────────────

    // 3번: 체력 회복 (이미 존재)
    public void Green1()
    {
        healthRegenActivate = true;
        Debug.Log("🟩 체력 회복 패시브 활성화");
    }

    // 4번: 데미지 반사 (이미 존재)
    public void Green2()
    {
        if (Time.time - lastReflectTime < reflectCooldown) return;

        StartCoroutine(ReflectRoutine());
        lastReflectTime = Time.time;
    }

    private IEnumerator ReflectRoutine()
    {
        player.reflect = true;
        Debug.Log("🟩 데미지 반사 발동 (5초간)");
        yield return new WaitForSeconds(5f);

        player.reflect = false;
        Debug.Log("🟩 데미지 반사 종료");
    }

    // 5번: 부활 (신규)
    public void Green3()
    {
        revive = true;
        Debug.Log("🟩 부활 패시브 적용됨 (사망 시 1회 자동 부활)");
    }

    public void Revive()
    {
        revive = false;

        player.hp = player.maxHp * 0.5f;
        Debug.Log("🟩 부활 발동! 체력 50%로 부활!");
    }


    // ─────────────────────────────
    // 파랑색
    // ─────────────────────────────

    // 6번: 더블 카운터 (기존)
    public void Blue1()
    {
        if (Time.time - lastDoubleStrikeTime < doubleStrikeCooldown) return;

        StartCoroutine(DoubleStrikeRoutine());
        lastDoubleStrikeTime = Time.time;
    }

    private IEnumerator DoubleStrikeRoutine()
    {
        player.attackSpeed += 50;
        Debug.Log("💥 공격속도 +50% (5초간)");

        yield return new WaitForSeconds(5f);
        player.attackSpeed -= 50;
        Debug.Log("💥 더블 카운터 종료");
    }

    // 7번: 돌격 (기존)
    public void Blue2()
    {
        if (Time.time - lastChargeTime < chargeCooldown) return;

        StartCoroutine(Charge());
        lastChargeTime = Time.time;
    }

    IEnumerator Charge()
    {
        player.moveSpeed += 20;
        Debug.Log("💨 돌격! 이동속도 +20% (5초간)");

        yield return new WaitForSeconds(5f);
        player.moveSpeed -= 20;
        Debug.Log("💨 돌격 종료");
    }

    // 8번: 적 이동속도 30% 감소 (신규)
    public void Blue3()
    {
        GameManager.instance.slowEnemy=true;
        Debug.Log("🔵 적 이동속도 30% 감소 패시브 적용됨");
    }


    // ─────────────────────────────
    // 스킬 보유 검사
    // ─────────────────────────────
    bool hasSkill(int type)
    {
        for (int i = 0; i < 4; i++)
        {
            if (player.skills[i] == type) return true;
        }
        return false;
    }
}