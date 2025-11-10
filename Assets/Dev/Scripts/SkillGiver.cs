using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGiver : MonoBehaviour
{
    public int type;

    bool isUsed;

    void Start(){
        isUsed=false;
    }

    void OnEnable(){
        isUsed=false;
    }

    void Casino(){
        Debug.Log("스킬 획득");
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
