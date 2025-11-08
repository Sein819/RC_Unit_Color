using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public ParticleSystem slashParticle;

    void Start(){
        var mainModule = slashParticle.main;
        mainModule.startRotation = Mathf.Deg2Rad * (-transform.rotation.eulerAngles.z+90);

        Destroy(gameObject,0.2f);
    }

    //데미지
    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player"){
            Player playerScript = collision.gameObject.GetComponent<Player>();
            if(playerScript.dead) return;
            playerScript.hp -= 5;
            if(playerScript.hp<=0) playerScript.Die();
            playerScript.StartCoroutine(playerScript.HitColor());
        }
    }
}
