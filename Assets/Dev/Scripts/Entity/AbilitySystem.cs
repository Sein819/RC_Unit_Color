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
    
    void Awake(){
        player= gameObject.GetComponent<Player>();
    }

    void Start()
    {
        healthRegenActivate=false;

        lastHealthRegenTime = -99;
        lastBerserkTime=-99;
        lastReflectTime=-99;
        lastDoubleStrikeTime=-99;
        lastChargeTime=-99;
    }

    void Update()
    {
        // 초록색 1번: 체력 회복 (40초마다 체력 10% 회복)
        if (healthRegenActivate&& Time.time - lastHealthRegenTime >= healthRegenCooldown){
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
        // 예시: 적을 플레이어 동료로 만들기
        Debug.Log($"흑백 스킬 발동!");
        // 실제로는 targetEnemy의 AI, 팀 정보 수정 필요
    }

    // ─────────────────────────────
    // 빨간색
    // ─────────────────────────────
    //강하게 치기 - type 0
    public void Red1(){
        GameManager.instance.redSkill1Activate=true;
        Debug.Log("강하게 치기 패시브 적용");
    }

    //광전사 - type 1
    public void Red2(){ 
        if (Time.time - lastBerserkTime < berserkCooldown) return;

        StartCoroutine(BerserkRoutine());
        lastBerserkTime = Time.time;
    }

    IEnumerator BerserkRoutine(){
        player.berserkerActivate=true;
        Debug.Log("🔥 광전사 발동! 공격력 1.5배 (5초간)");
        yield return new WaitForSeconds(5f);

        player.berserkerActivate=false;
        Debug.Log("🔥 광전사 종료");
    }

    // ─────────────────────────────
    // 초록색
    // ─────────────────────────────
    //체력 회복 - type 2
    public void Green1(){
        healthRegenActivate=true;
    }

    //반사 - type 3
    public void Green2(){ 
        if (Time.time - lastReflectTime < reflectCooldown) return;

        StartCoroutine(ReflectRoutine());
        lastReflectTime = Time.time;
    }

    private IEnumerator ReflectRoutine(){
        player.reflect = true;
        Debug.Log("🟩 데미지 반사 발동 (5초간)");
        yield return new WaitForSeconds(5f);

        player.reflect = false;
        Debug.Log("🟩 데미지 반사 종료");
    }

    // ─────────────────────────────
    // 파랑색
    // ─────────────────────────────
    //더블 스트라이크 - type 4
    public void Blue1(){ 
        if (Time.time - lastDoubleStrikeTime < doubleStrikeCooldown) return;

        StartCoroutine(DoubleStrikeRoutine());
        lastDoubleStrikeTime = Time.time;
    }

    private IEnumerator DoubleStrikeRoutine(){
        player.attackSpeed+=50;
        Debug.Log("💥 한 대 더 때리기! 공격속도 +50% (5초간)");

        yield return new WaitForSeconds(5f);
        player.attackSpeed-=50;
        Debug.Log("💥 한 대 더 때리기 종료");
    }

    //돌격 - type 5
    public void Blue2(){ 
        if (Time.time - lastChargeTime < chargeCooldown) return;

        StartCoroutine(Charge());
        lastChargeTime = Time.time;
    }

    IEnumerator Charge(){
        player.moveSpeed+=20;
        Debug.Log("💨 돌격! 이동속도 +20% (5초간)");

        yield return new WaitForSeconds(5f);
        player.moveSpeed-=20;
        Debug.Log("💨 돌격! 이동속도 +20% 종료");
    }


    //스킬 보유 검사
    bool hasSkill(int type){
        for(int i=0;i<4;i++){
            if(player.skills[i]==type) return true;
        }
        return false;
    }
}
