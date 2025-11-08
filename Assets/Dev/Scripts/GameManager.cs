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
    
    float gameTime;

    void Awake(){
        if(instance==null){
            instance=this;
        }
        else{
            Destroy(gameObject);
        }
        gameTime=0;
    }

    void Start(){
        
    }

    void Update(){
        gameTime+=Time.deltaTime; 
    }

    public IEnumerator GameOver(){
        yield return new WaitForSeconds(1);
        //씬 전환
    }
}
