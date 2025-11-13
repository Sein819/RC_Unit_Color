using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public ParticleSystem slashParticle;
    Player player;

    void Awake(){
        player=GameManager.instance.player.GetComponent<Player>();
    }
    
    void Start(){
        float playerRotation = GameManager.instance.player.transform.Find("PlayerDirection").rotation.eulerAngles.z;
        var mainModule = slashParticle.main;
        mainModule.startRotation = Mathf.Deg2Rad * (-playerRotation+90);

        Destroy(gameObject,0.2f);
    }

    //데미지
    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Enemy"){
            Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
            if(enemyScript.dead||enemyScript.immune>0) return;
            enemyScript.hp -=player.attackPower/100*5*(player.berserkerActivate?1.5f:1);
            if(enemyScript.hp<=0) enemyScript.Die();
            enemyScript.StartCoroutine(enemyScript.HitColor());
        }
    }
}
