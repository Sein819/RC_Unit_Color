using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SkillGiver : MonoBehaviour
{
    public GameObject casinoUI;
    public GameObject casinoRouletteUI;
    public Button[] casinoRouletteButton;
    public Sprite[] skillSprites;
    public int[] typeOfRedSkill;
    public int[] typeOfGreenSkill;
    public int[] typeOfBlueSkill;
    public Button[] skillButton;
    public Sprite nothingSkillSprite;
    public Sprite blackSkillSprite;

    Player player;
    AbilitySystem skillScript;
    Image[] casinoRouletteImage = new Image[3];
    
    public int type;
    public float spinSpeed = 0.05f;
    public float spinDuration = 2f;

    bool isUsed;
    public List<int> skillOfColor=new List<int>(); //룰렛에 나올 수 있는 모든 스킬 타입 리스트
    int[] rouletteResults;
    
    void Start(){
        player=GameManager.instance.player.GetComponent<Player>();
        skillScript=GameManager.instance.player.GetComponent<AbilitySystem>();

        isUsed=false;
        if(type==1) return;
        
        casinoUI.SetActive(false);
        for(int i=0;i<3;i++){
            casinoRouletteButton[i].interactable=false;
            skillButton[i].gameObject.SetActive(false);
            casinoRouletteImage[i]=casinoRouletteButton[i].gameObject.GetComponent<Image>();
        }
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
        float[] rgb = GameManager.instance.rgb;
        float sum=rgb[0]+rgb[1]+rgb[2];
        
        //색 스킬 추출, 이미 있는 능력 제외
        skillOfColor.Clear();
        skillOfColor.Add(-1);
        if(sum==0&&Array.IndexOf(player.skills, -2)==-1) skillOfColor.Add(-2);
        else{
            for(int i=0; i<skillSprites.Length;i++){
                if(-1!=Array.IndexOf(player.skills, i)) continue;

                if(-1!=Array.IndexOf(typeOfRedSkill,i)&&rgb[0]==0) continue;
                else if(-1!=Array.IndexOf(typeOfGreenSkill,i)&&rgb[1]==0) continue;
                else if(-1!=Array.IndexOf(typeOfBlueSkill,i)&&rgb[2]==0) continue;

                skillOfColor.Add(i);
            }
        }

        //룰렛 연출
        while (elapsed < spinDuration){
            for (int i = 0; i < 3; i++){
                int type=ShowRouletteRandomSkill(sum,rgb);

                if(type==-1){
                    casinoRouletteImage[i].sprite=nothingSkillSprite;
                    continue;
                }
                else if(type==-2){
                    casinoRouletteImage[i].sprite=blackSkillSprite;
                    continue;
                }
                casinoRouletteImage[i].sprite = skillSprites[type];
            }
            elapsed += spinSpeed;
            yield return new WaitForSeconds(spinSpeed);
        }

        //랜덤 결과 확정
        rouletteResults = new int[3];
        for (int i = 0; i < 3;i++){
            int type=ShowRouletteRandomSkill(sum,rgb);
            rouletteResults[i] = type;
            if(type!=-1) skillOfColor.Remove(type);
        }

        //순차 멈춤
        for (int i = 0; i < 3; i++){
            yield return new WaitForSeconds(0.5f);
            int result = rouletteResults[i];

            if(result==-1) {
                casinoRouletteImage[i].sprite=nothingSkillSprite;
                continue;
            }
            else if(result==-2){
                casinoRouletteImage[i].sprite=blackSkillSprite;
                continue;
            }
            casinoRouletteImage[i].sprite = skillSprites[result];
        }

        for (int i = 0; i < 3; i++){
            if(rouletteResults[i]!=-1) casinoRouletteButton[i].interactable = true;
        }
    }

    //도박장 - 스킬 랜덤 출력
    int ShowRouletteRandomSkill(float sum, float[] rgb){
        if(sum == 0&&skillOfColor.Contains(-2)){
            if(UnityEngine.Random.Range(0, 10)==0) return -2;
            else return -1;
        }
        
        int resultType;
        do{
            if(UnityEngine.Random.Range(0, 10)==0){
                resultType=-1;
                break;
            }
            float randomValue = UnityEngine.Random.Range(0f, sum);

            if (randomValue < rgb[0]) resultType=typeOfRedSkill[UnityEngine.Random.Range(0,typeOfRedSkill.Length)];
            else{
                randomValue -= rgb[0];
                if (randomValue < rgb[1]) resultType=typeOfGreenSkill[UnityEngine.Random.Range(0,typeOfGreenSkill.Length)];
                else resultType=typeOfBlueSkill[UnityEngine.Random.Range(0,typeOfBlueSkill.Length)];
            }
        }while(!skillOfColor.Contains(resultType));

        return resultType;
    }

    //도박장 - 스킬 선택
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

    //도박장 - 스킬 선택 스킵
    public void CasinoSkipSkillSelect(){
        casinoRouletteUI.SetActive(false);
        for(int i=0;i<3;i++){
            casinoRouletteButton[i].interactable=false;
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
            Debug.LogError("남아있는 스킬 버튼 없음");
            return;
        }
        if(type==-2){ //흑백
            skillButton[button].onClick.AddListener(() => skillScript.Black1());
            SetActivateSkillButton(type, button);
        }
        else if(type==0){ //강하게 치기
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
            Debug.LogError("스킬 획득 정의X");
        }
    }

    void SetActivateSkillButton(int type, int button){
        if(type==-2) skillButton[button].gameObject.GetComponent<Image>().sprite = blackSkillSprite;
        else skillButton[button].gameObject.GetComponent<Image>().sprite = skillSprites[type];
        skillButton[button].gameObject.SetActive(true);
    }

    //최종 스킬
    void FinalSkill(){
        int type=JudgeFinalSkillColor();

        if(type==-1){ //흑백 최종
            Debug.Log("흑백 최종 스킬 획득");
        }
        else if(type==0){ //빨강 최종
            Debug.Log("빨강 최종 스킬 획득");
        }
        else if(type==1){ //초록 최종
            Debug.Log("초록 최종 스킬 획득");
        }
        else if(type==2){ //파랑 최종
            Debug.Log("파랑 최종 스킬 획득");
        }
        else if(type==3){ //노랑 최종
            Debug.Log("노랑 최종 스킬 획득");
        }
        else if(type==4){ //자홍 최종
            Debug.Log("자홍 최종 스킬 획득");
        }
        else if(type==5){ //청록 최종
            Debug.Log("청록 최종 스킬 획득");
        }
        else if(type==6){ //흰색 최종
            Debug.Log("흰색 최종 스킬 획득");
        }
    }

    //최종 스킬 색 판별
    int JudgeFinalSkillColor(){
        float[] rgb=GameManager.instance.rgb;
        int r=(int)(rgb[0]*5),g=(int)(rgb[1]*5),b=(int)(rgb[2]*5);
        int[] arr = { r,g,b };
        int max = Mathf.Max(arr);
        int min = Mathf.Min(arr);
        int mid = r + g + b - max - min;

        if (r == 0 && g == 0 && b == 0) return -1;

        // 단일색
        if (max - mid >= 3||mid==0){
            if (max == r) return 0; //빨
            if (max == g) return 1; //초
            return 2; //파
        }

        // 혼합색
        if ((max - mid <= 2) && (mid - min >= 2||min==0)){
            if (max == r && mid == g) return 3;  //노랑
            if (max == r && mid == b) return 4;  //자홍
            if (max == g && mid == b) return 5;  //청록
        }

        // 흰
        if (max - min <= 2&&min!=0) return 6; //흰

        // 기본 단일색
        if (max == r) return 0;
        if (max == g) return 1;
        return 2;
    }


    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player"&&!isUsed){
            if(type==0){
                //도박장
                Casino();
            }
            else{
                //최종 스킬
                FinalSkill();
            }
            isUsed=true;
        }
    }
}
