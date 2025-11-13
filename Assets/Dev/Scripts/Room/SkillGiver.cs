using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillGiver : MonoBehaviour
{
    public GameObject casinoUI;
    public GameObject casinoRouletteUI;
    public Button[] casinoRouletteButton;
    public Sprite[] skillSprites; // 스킬 종류별 이미지

    Player player;
    Image[] casinoRouletteImage = new Image[3];
    
    public int type;
    public float spinSpeed = 0.05f;
    public float spinDuration = 2f;

    bool isUsed;
    int[] rouletteResults;
    
    void Start(){
        player=GameManager.instance.player.GetComponent<Player>();

        casinoUI.SetActive(false);
        for(int i=0;i<3;i++){
            casinoRouletteButton[i].interactable=false;
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
    public void GetSkill(int type){
        if(rouletteResults[type]==0){
            Debug.Log("0번 스킬 획득");
        }
        else if(rouletteResults[type]==1){
            Debug.Log("1번 스킬 획득");
        }
        else if(rouletteResults[type]==2){
            Debug.Log("2번 스킬 획득");
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
}
