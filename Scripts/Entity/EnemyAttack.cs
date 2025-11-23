using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Enemy enemy;
    public ParticleSystem slashParticle;
    Collider2D col;

    public int type;
    public float destroyTime;

    float timer;

    void Awake(){
        col=GetComponent<Collider2D>();
    }

    void Start(){
        if(type==0){
            var mainModule = slashParticle.main;
            mainModule.startRotation = Mathf.Deg2Rad * transform.rotation.z;
        }
        else if(type==1||type==101) col.enabled=false;

        Destroy(gameObject,destroyTime);
    }

    void Update(){
        timer+=Time.deltaTime;

        if(type==1&&timer>0.8f&&timer<1f) col.enabled=true;
        if(type==1&&timer>=1f) col.enabled=false;

        if(type==101&&timer>=1f&&timer<2f) col.enabled=true;
        if(type==101&&timer>=2f) col.enabled=false;
    }

    //데미지
    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player"){
            Player playerScript = collision.gameObject.GetComponent<Player>();
            if(playerScript.dead) return;

            float damage;
            if(type==1) damage = 8;
            else if(type==101) damage = 10;
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
