using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public Rigidbody2D pRigid;
    public GameObject[] enemy;
    public GameObject colorBoss;
    public GameObject finalBoss;
    public GameObject defaultRoom;
    public GameObject colorRoom;
    public GameObject[] color;
    public Material defaultMat;
    public Material enemyMat;
    public GameObject casino;
    public GameObject finalSkillGiver;

    Player playerScript;
    public float[] rgb;

    [HideInInspector]
    public bool redSkill1Activate;
    int redSkill1Scale;
    [HideInInspector]
    public bool slowEnemy;

    float gameTime;
    float enemyAmount;
    int roomCount;
    int curColor;

    void Awake(){
        if(instance==null){
            instance=this;
        }
        else{
            Destroy(gameObject);
        }

        playerScript=player.GetComponent<Player>();

        gameTime=0;
        enemyAmount=0;
        roomCount=1;

        redSkill1Activate=false;
        redSkill1Scale=0;
        slowEnemy=false;
    }

    void Start(){
        defaultRoom.SetActive(false);
        colorRoom.SetActive(false);
        casino.SetActive(false);
        finalSkillGiver.SetActive(false);
        for(int i=0;i<3;i++) color[i].SetActive(false);
        for(int i=0;i<3;i++) rgb[i]=0;
        defaultMat.SetFloat("_R",0);
        defaultMat.SetFloat("_G",0);
        defaultMat.SetFloat("_B",0);
        enemyMat.SetFloat("_R",0);
        enemyMat.SetFloat("_G",0);
        enemyMat.SetFloat("_B",0);

        SpawnEnemy(0,5+roomCount/3);
    }

    void Update(){
        gameTime+=Time.deltaTime; 
    }

    //적 소환
    void SpawnEnemy(int type, int amount){
        for(int i=0;i<amount;i++){
            if(type==1) Instantiate(colorBoss,new Vector2(0,4),transform.rotation);
            else if(type==101) Instantiate(finalBoss,new Vector2(0,4),transform.rotation);
            else Instantiate(enemy[Random.Range(0,enemy.Length)],new Vector2(Random.Range(-5.5f,5.5f),Random.Range(-5.5f,5.5f)),transform.rotation);
        }
        enemyAmount+=amount;
    }

    //적 사망
    public void EnmeyDie(){
        enemyAmount--;
        if(enemyAmount<=0){
            if((roomCount%2+roomCount/7)%2==1&&roomCount<28){
                colorRoom.SetActive(true);
            }
            else {
                if(curColor!=-1){
                    color[curColor].SetActive(true);
                }
                defaultRoom.SetActive(true);
            }
        }
    }

    //방 입장
    public void EnterRoom(int type){ //0=빨강, 1=초록, 2=파랑
        defaultRoom.SetActive(false);
        colorRoom.SetActive(false);
        casino.SetActive(false);
        finalSkillGiver.SetActive(false);
        for(int i=0;i<3;i++) color[i].SetActive(false);
        
        enemyAmount=0;
        roomCount++;
        curColor=type;
        player.transform.position=new Vector2(0,0);

        if(roomCount%7==0){
            player.transform.position=new Vector2(0,-3);
            if(roomCount==28){
                //최종 스킬 획득
                finalSkillGiver.SetActive(true);
                defaultRoom.SetActive(true);
            }
            else{
                //도박장
                casino.SetActive(true);
                defaultRoom.SetActive(true);
            }
        } 
        else if(roomCount==32){
            //최종 보스
            SpawnEnemy(101,1);
        }
        else if(roomCount==33){
            //게임 종료
            PlayerPrefs.SetInt("ClearCount",PlayerPrefs.GetInt("ClearCount")+1);
            if(rgb[0]==3) PlayerPrefs.SetInt("Achieve_Red",1);
            else if(rgb[1]==3) PlayerPrefs.SetInt("Achieve_Green",1);
            else if(rgb[2]==3) PlayerPrefs.SetInt("Achieve_Blue",1);
            else if(rgb[0]+rgb[1]+rgb[2]==0) PlayerPrefs.SetInt("Achieve_Black",1);
            else if(rgb[0]==rgb[1]&&rgb[2]==rgb[1]&&rgb[0]==1)PlayerPrefs.SetInt("Achieve_White",1);
            SceneManager.LoadScene("Menu");
        }
        else{
            if(type>=0){
                //색깔방
                if(roomCount%7==6){
                    //색보스
                    SpawnEnemy(1,1);
                }
            }
            else{
                //기본방
            } 
            SpawnEnemy(0,5+roomCount/5);
        }
    }

    //색 획득
    public void GetColor(int type){
        if(type==0){
            rgb[0]+=0.25f;
            defaultMat.SetFloat("_R",rgb[0]);
            enemyMat.SetFloat("_R",rgb[0]);
            playerScript.attackPower+=20+redSkill1Scale*2;

            if(redSkill1Activate) redSkill1Scale++;
        }
        else if(type==1){
            rgb[1]+=0.25f;
            defaultMat.SetFloat("_G",rgb[1]);
            enemyMat.SetFloat("_G",rgb[1]);
            playerScript.maxHp+=20;
            playerScript.hp*=playerScript.maxHp/(playerScript.maxHp-20);
        }
        else{
            rgb[2]+=0.25f;
            defaultMat.SetFloat("_B",rgb[2]);
            enemyMat.SetFloat("_B",rgb[2]);
            playerScript.attackSpeed+=20;
        }
    }

    //게임 종료
    public IEnumerator GameOver(){
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Menu");
    }
}
