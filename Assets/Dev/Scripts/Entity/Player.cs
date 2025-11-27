using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public FloatingJoystick joy;
    public GameObject slash;
    public Slider hpUI;

    Rigidbody2D rb;
    SpriteRenderer sr;
    MaterialPropertyBlock mpb;

    //효과음 관련
    private AudioSource audioPlayer;
    private float lastMoveSfxTime = 0f;
    public AudioClip[] basicAudios;
    public AudioClip moveSfx;
    public AudioClip attackSfx;
    public float moveSfxInterval = 0.25f; //발걸음 주기

    public float maxHp;
    public float hp;
    public float attackPower;
    public float attackSpeed;
    public float moveSpeed;
    [HideInInspector]
    public bool dead;
    [HideInInspector]
    public bool reflect;
    [HideInInspector]
    public bool berserkerActivate;
    public int[] skills;
    [HideInInspector]
    public bool isCasino;
    [HideInInspector]
    public bool redFinalActive;
    public int finalSkillType;

    float timer;
    float attackCd;
    float lastAttackTime;
    float lastTapTime = 0f;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();

        audioPlayer = GetComponent<AudioSource>(); 

        maxHp=150;
        hp=maxHp;
        attackPower=100;
        attackSpeed=100;
        moveSpeed=100;
        dead=false;
        reflect=false;
        berserkerActivate=false;
        isCasino=false;
        redFinalActive=false;
        finalSkillType=-999;

        timer=0;
        attackCd=0.7f;
        lastAttackTime=-999;

        for(int i=0;i<4;i++){
            skills[i]=-1;
        }
    }
    
    void Update(){
        timer+=Time.deltaTime;

        if(dead||isCasino) return;
        ActivateFianlSkill();
    }

    void FixedUpdate(){
        if(dead||isCasino) return;

        Move();
    }

    //이동
    void Move(){
        float h = joy.Horizontal;
        float v = joy.Vertical;

        float kh = Input.GetAxisRaw("Horizontal");
        float kv = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(kh) > Mathf.Abs(h)) h = kh;
        if (Mathf.Abs(kv) > Mathf.Abs(v)) v = kv;

        Vector2 input = new Vector2(h, v);
        input = input.normalized;

        Vector2 nextPos = input * 3 * Time.fixedDeltaTime * moveSpeed / 100;
        rb.MovePosition(rb.position + nextPos);

        if (input != Vector2.zero){
            if(input.x > 0) sr.flipX = false;
            else if(input.x < 0) sr.flipX = true;
           
            if (Time.time - lastMoveSfxTime > moveSfxInterval) //이동 효과음
            {
                audioPlayer.PlayOneShot(basicAudios[0]);
                lastMoveSfxTime = Time.time;
            }

            Transform direction = transform.Find("PlayerDirection");
            float targetAngle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            direction.rotation = Quaternion.Lerp(direction.rotation, targetRotation, Time.fixedDeltaTime * 15);
        }
        else if(timer > lastAttackTime + attackCd / attackSpeed * 100){
            Attack();
            lastAttackTime = timer;
        }
    }

    //공격
    void Attack(){

        audioPlayer.PlayOneShot(basicAudios[1]); // 효과음

        //공격 애니메이션
        GameObject prefab = Instantiate(slash,new Vector2(transform.position.x,transform.position.y+0.4f),transform.rotation);
        if(redFinalActive){
            prefab.GetComponent<Slash>().redFinalActive = true;
            redFinalActive=false;
        }
    }

    //피해 입기
    public IEnumerator HitColor(){
        hpUI.value = (float)hp/maxHp;

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
        //사망 애니메이션
        AbilitySystem skill = gameObject.GetComponent<AbilitySystem>();
        if(skill.revive){
            skill.Revive();
            return;
        }
        dead=true;
        StartCoroutine(GameManager.instance.GameOver());
    }

    
    //최종 스킬 발동
    void ActivateFianlSkill(){
        if (Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began){
                if (Time.time - lastTapTime < 0.2f){
                    UseFinalSkill();
                }
                lastTapTime = Time.time;
            }
        }
        else{
            if (Input.GetMouseButtonDown(0)){
                if (Time.time - lastTapTime < 0.2f){
                    UseFinalSkill();
                }
                lastTapTime = Time.time;
            }
        }
    }

    void UseFinalSkill(){
        AbilitySystem skill = gameObject.GetComponent<AbilitySystem>();

        if(finalSkillType==-1){ //흑백 최종
            Debug.Log("흑백 최종 스킬 발동");
        }
        else if(finalSkillType==0){ //빨강 최종
            skill.Red3();
            Debug.Log("빨강 최종 스킬 발동");
        }
        // else if(finalSkillType==1){ //초록 최종
        //     Debug.Log("초록 최종 스킬 발동");
        // }
        // else if(finalSkillType==2){ //파랑 최종
        //     Debug.Log("파랑 최종 스킬 발동");
        // }
        else if(finalSkillType==3){ //노랑 최종
            Debug.Log("노랑 최종 스킬 발동");
        }
        else if(finalSkillType==4){ //자홍 최종
            Debug.Log("자홍 최종 스킬 발동");
        }
        else if(finalSkillType==5){ //청록 최종
            Debug.Log("청록 최종 스킬 발동");
        }
        else if(finalSkillType==6){ //흰색 최종
            Debug.Log("흰색 최종 스킬 발동");
        }
    }
}