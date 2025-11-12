using System.Collections;
using UnityEngine;

public class AbilitySystem : MonoBehaviour
{
    [Header("기본 능력치")]
    public float baseAttackPower = 100f;
    public float baseHealth = 1000f;
    public float baseMoveSpeed = 5f;

    private float currentAttackPower;
    private float currentHealth;
    private float currentMoveSpeed;

    [Header("쿨타임 설정 (초)")]
    public float berserkCooldown = 60f;
    public float reflectCooldown = 60f;
    public float doubleStrikeCooldown = 30f;
    public float chargeCooldown = 60f;

    private float lastBerserkTime;
    private float lastReflectTime;
    private float lastDoubleStrikeTime;
    private float lastChargeTime;

    private bool isBerserkActive = false;
    private bool isReflectActive = false;
    private bool isDoubleStrikeActive = false;
    private bool isChargeActive = false;

    [Header("체력 회복 관련")]
    public float healthRegenInterval = 40f; // 40초마다 회복
    public float healthRegenPercent = 0.1f; // 10% 회복
    private float lastHealthRegenTime;

    void Start()
    {
        // 빨간색 1번: 강하게 치기 (영구 패시브 공격력 10% 증가)
        currentAttackPower = baseAttackPower * 1.1f;
        currentHealth = baseHealth;
        currentMoveSpeed = baseMoveSpeed;

        lastHealthRegenTime = Time.time;
        Debug.Log("능력 시스템 시작됨: 강하게 치기 패시브 적용 (공격력 +10%)");
    }

    void Update()
    {
        // 초록색 1번: 체력 회복 (40초마다 체력 10% 회복)
        if (Time.time - lastHealthRegenTime >= healthRegenInterval)
        {
            float regenAmount = baseHealth * healthRegenPercent;
            currentHealth = Mathf.Min(currentHealth + regenAmount, baseHealth);
            lastHealthRegenTime = Time.time;
            Debug.Log($"체력 회복 발동! +{regenAmount} (현재 체력: {currentHealth})");
        }

        // 파랑색 2번: 돌격 지속시간(5초) 끝나면 이동속도 복원
        if (isChargeActive && Time.time - lastChargeTime >= 5f)
        {
            currentMoveSpeed = baseMoveSpeed;
            isChargeActive = false;
            Debug.Log("돌격 종료! 이동속도 복원");
        }
    }

    // ─────────────────────────────
    // 흑백 능력
    // ─────────────────────────────
    public void ActivateBlackWhiteAbility(GameObject targetEnemy)
    {
        // 예시: 적을 플레이어 동료로 만들기
        Debug.Log($"{targetEnemy.name}이(가) 플레이어의 동료가 되었습니다!");
        // 실제로는 targetEnemy의 AI, 팀 정보 수정 필요
    }

    // ─────────────────────────────
    // 빨간색 능력
    // ─────────────────────────────
    public void ActivateBerserk()
    {
        if (Time.time - lastBerserkTime < berserkCooldown)
        {
            Debug.Log("⚠️ 광전사 쿨타임입니다!");
            return;
        }
        StartCoroutine(BerserkRoutine());
        lastBerserkTime = Time.time;
    }

    private IEnumerator BerserkRoutine()
    {
        isBerserkActive = true;
        float originalAttack = currentAttackPower;
        currentAttackPower *= 1.5f;
        Debug.Log("🔥 광전사 발동! 공격력 +50% (5초간)");
        yield return new WaitForSeconds(5f);
        currentAttackPower = originalAttack;
        isBerserkActive = false;
        Debug.Log("🔥 광전사 종료");
    }

    // ─────────────────────────────
    // 초록색 능력
    // ─────────────────────────────
    public void ActivateReflect()
    {
        if (Time.time - lastReflectTime < reflectCooldown)
        {
            Debug.Log("⚠️ 데미지 반사 쿨타임입니다!");
            return;
        }
        StartCoroutine(ReflectRoutine());
        lastReflectTime = Time.time;
    }

    private IEnumerator ReflectRoutine()
    {
        isReflectActive = true;
        Debug.Log("🟩 데미지 반사 발동 (5초간)");
        yield return new WaitForSeconds(5f);
        isReflectActive = false;
        Debug.Log("🟩 데미지 반사 종료");
    }

    // ─────────────────────────────
    // 파랑색 능력
    // ─────────────────────────────
    public void ActivateDoubleStrike()
    {
        if (Time.time - lastDoubleStrikeTime < doubleStrikeCooldown)
        {
            Debug.Log("⚠️ 한 대 더 때리기 쿨타임입니다!");
            return;
        }
        StartCoroutine(DoubleStrikeRoutine());
        lastDoubleStrikeTime = Time.time;
    }

    private IEnumerator DoubleStrikeRoutine()
    {
        isDoubleStrikeActive = true;
        float originalAttack = currentAttackPower;
        currentAttackPower *= 2f;
        Debug.Log("💥 한 대 더 때리기! 공격력 2배 (5초간)");
        yield return new WaitForSeconds(5f);
        currentAttackPower = originalAttack;
        isDoubleStrikeActive = false;
        Debug.Log("💥 한 대 더 때리기 종료");
    }

    public void ActivateCharge()
    {
        if (Time.time - lastChargeTime < chargeCooldown)
        {
            Debug.Log("⚠️ 돌격 쿨타임입니다!");
            return;
        }
        currentMoveSpeed = baseMoveSpeed * 1.2f;
        isChargeActive = true;
        lastChargeTime = Time.time;
        Debug.Log("💨 돌격! 이동속도 +20% (5초간)");
    }

    // ─────────────────────────────
    // 데미지 처리 예시
    // ─────────────────────────────
    public void TakeDamage(float damage, GameObject attacker = null)
    {
        if (isReflectActive && attacker != null)
        {
            Debug.Log($"🪞 데미지 반사! {attacker.name}에게 {damage} 피해 반사");
            // 실제로는 attacker.TakeDamage(damage) 같은 함수 호출 필요
        }

        currentHealth -= damage;
        Debug.Log($"피해 받음: {damage} (남은 체력: {currentHealth})");

        if (currentHealth <= 0)
        {
            Debug.Log("💀 사망!");
        }
    }
}
