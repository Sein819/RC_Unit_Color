using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Enemy enemy;
    public ParticleSystem particle;
    public GameObject[] finalBossAttack2Sprite;
    Collider2D col;

    public int type;
    public float destroyTime;

    float timer;

    void Awake(){
        col=GetComponent<Collider2D>();
    }

    void Start(){
        if(type==0){
            var mainModule = particle.main;
            mainModule.startRotation = Mathf.Deg2Rad * transform.rotation.z;
        }
        else if(type==1||type==101) col.enabled=false;
        else if(type==102){
            col.enabled=false;
            Vector2 dir = (GameManager.instance.player.transform.position - transform.position);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            finalBossAttack2Sprite[1].SetActive(false);
            finalBossAttack2Sprite[0].SetActive(true);
        }

        Destroy(gameObject,destroyTime);
    }

    void Update(){
        timer+=Time.deltaTime;

        if(type==1&&timer>0.8f&&timer<1f) col.enabled=true;
        if(type==1&&timer>=1f) col.enabled=false;

        if((type==101||type==102)&&timer>=0.8f&&timer<1.8f) {
            col.enabled=true;
            if(type==102){
                finalBossAttack2Sprite[1].SetActive(true);
                finalBossAttack2Sprite[0].SetActive(false);
            }
        }
        if((type==101||type==102)&&timer>=1.8f){
            col.enabled=false;
            if(type==102){
                finalBossAttack2Sprite[1].SetActive(false);
            }
        }
    }

    //데미지
    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player"){
            Player playerScript = collision.gameObject.GetComponent<Player>();
            if(playerScript.dead) return;

            float damage;
            if(type==1) damage = 8;
            else if(type==101) damage = 10;
            else if(type==102) damage = 12;
            else damage=5;

            if(playerScript.reflect){
                if(enemy.dead||enemy.immune>0) return;
                enemy.hp -=damage;
                if(enemy.hp<=0) enemy.Die();
                enemy.StartCoroutine(enemy.HitColor());

                if(type!=101)return;
            }

            playerScript.hp-=damage;
            if(playerScript.hp<=0) playerScript.Die();
            playerScript.StartCoroutine(playerScript.HitColor());
        }
    }
}
