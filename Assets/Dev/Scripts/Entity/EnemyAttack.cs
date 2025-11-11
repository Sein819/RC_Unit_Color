using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public ParticleSystem slashParticle;
    public Collider2D col;

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
        else if(type==1) col.enabled=false;

        Destroy(gameObject,destroyTime);
    }

    void Update(){
        timer+=Time.deltaTime;

        if(type==1&&timer>1&&timer<1.2f) col.enabled=true;
        if(type==1&&timer>=1.2f) col.enabled=false;
    }

    //데미지
    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player"){
            Player playerScript = collision.gameObject.GetComponent<Player>();
            if(playerScript.dead) return;

            if(type==0) playerScript.hp -= 5;
            else if(type==1) playerScript.hp -= 8;

            if(playerScript.hp<=0) playerScript.Die();
            playerScript.StartCoroutine(playerScript.HitColor());
        }
    }
}
