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
    public GameObject enemy;

    float gameTime;
    float enemyAmount;

    void Awake(){
        if(instance==null){
            instance=this;
        }
        else{
            Destroy(gameObject);
        }
        gameTime=0;
        enemyAmount=0;
    }

    void Start(){
        SpawnEnemy(10);
    }

    void Update(){
        gameTime+=Time.deltaTime; 
    }

    void SpawnEnemy(int amount){
        for(int i=0;i<amount;i++){
            Instantiate(enemy,new Vector2(Random.Range(-5.5f,5.5f),Random.Range(-5.5f,5.5f)),transform.rotation);
            enemyAmount=amount;
        }
    }

    public void EnmeyDie(){
        enemyAmount--;
        if(enemyAmount<=0){
            SpawnEnemy(10);
        }
    }

    public IEnumerator GameOver(){
        yield return new WaitForSeconds(1);
        //씬 전환
    }
}
