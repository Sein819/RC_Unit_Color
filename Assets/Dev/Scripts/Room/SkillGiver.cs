using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillGiver : MonoBehaviour
{
    public GameObject casinoUI;
    public GameObject casinoRouletteUI;
    public Button[] casinoRouletteButton;
    public Sprite[] skillSprites;
    public Button[] skillButton;

    Player player;
    AbilitySystem skillScript;
    Image[] casinoRouletteImage = new Image[3];
    
    public int type;
    public float spinSpeed = 0.05f;
    public float spinDuration = 2f;

    bool isUsed;
    int[] rouletteResults;
    
    void Start(){
        player=GameManager.instance.player.GetComponent<Player>();
        skillScript=GameManager.instance.player.GetComponent<AbilitySystem>();

        casinoUI.SetActive(false);
        for(int i=0;i<3;i++){
            casinoRouletteButton[i].interactable=false;
            skillButton[i].gameObject.SetActive(false);
            casinoRouletteImage[i]=casinoRouletteButton[i].gameObject.GetComponent<Image>();
        }

        isUsed=false;
    }

    void OnEnable(){
        isUsed=false;
    }

    //도박장 창 활성화
    public void Casino(){
        casinoUI.SetActive(true);
    }

    //도박장 - 체력 회복 버튼
    public void CasinoHeal(){
        player.hp = Mathf.Min(player.hp + player.maxHp*0.2f, player.maxHp);
        casinoUI.SetActive(false);
    }

    //도박장 - 스킬 획득 버튼
    public void CasinoRoulette(){
        casinoUI.SetActive(false);
        casinoRouletteUI.SetActive(true);

        StartCoroutine(RouletteCoroutine());
    }

    IEnumerator RouletteCoroutine(){
        float elapsed = 0f;
        while (elapsed < spinDuration){
            for (int i = 0; i < 3; i++){
                int rand = Random.Range(0, skillSprites.Length);
                casinoRouletteImage[i].sprite = skillSprites[rand];
            }
            elapsed += spinSpeed;
            yield return new WaitForSeconds(spinSpeed);
        }

        //랜덤 결과 확정
        rouletteResults = new int[3];
        for (int i = 0; i < 3; i++){
            rouletteResults[i] = Random.Range(0, skillSprites.Length);
        }

        //순차 멈춤
        for (int i = 0; i < 3; i++){
            yield return new WaitForSeconds(0.5f);
            int result = rouletteResults[i];
            casinoRouletteImage[i].sprite = skillSprites[result];
        }

        for (int i = 0; i < 3; i++){
            casinoRouletteButton[i].interactable = true;
        }
    }

    //스킬 획득
    public void SelectCasinoSkill(int type){
        for(int i=0;i<3;i++){
            if(player.skills[i]!=-1) continue;

            player.skills[i]=rouletteResults[type];
            Debug.Log($"{rouletteResults[type]}번 스킬 획득");

            GetSkill(rouletteResults[type]);

            break;
        }

        casinoRouletteUI.SetActive(false);
        for(int i=0;i<3;i++){
            casinoRouletteButton[i].interactable=false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player"&&!isUsed){
            if(type==0){
                //스킬 획득
                Casino();
            }
            else{
                //최종 스킬
                Debug.Log("최종 스킬 획득");
            }
            isUsed=true;
        }
    }


    //스킬 획득
    void GetSkill(int type){
        int button=-1;
        for(int i=0;i<3;i++){
            if(!skillButton[i].gameObject.activeSelf) {
                button=i;
                break;
            }
        }
        if(button==-1){
            Debug.Log("남아있는 스킬 버튼 없음");
            return;
        }

        if(type==0){ //강하게 치기
            skillScript.Red1();
        }
        else if(type==1){ //광전사
            skillButton[button].onClick.AddListener(() => skillScript.Red2());
            SetActivateSkillButton(type, button);
        }
        else if(type==2){ //체력회복
            skillScript.Green1();
        }
        else if(type==3){ //데미지 반사
            skillButton[button].onClick.AddListener(() => skillScript.Green2());
            SetActivateSkillButton(type, button);
        }
        else if(type==4){ //한대 더 때리기
            skillButton[button].onClick.AddListener(() => skillScript.Blue1());
            SetActivateSkillButton(type, button);
        }
        else if(type==5){ //돌격
            skillButton[button].onClick.AddListener(() => skillScript.Blue2());
            SetActivateSkillButton(type, button);
        }
        else{
            Debug.Log("스킬 획득 정의X");
        }
    }

    void SetActivateSkillButton(int type, int button){
        skillButton[button].gameObject.GetComponent<Image>().sprite = skillSprites[type];
        skillButton[button].gameObject.SetActive(true);
    }
}
