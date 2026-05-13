using UnityEngine;

public class arrow : MonoBehaviour
{
    public Enemy enemy;
    
    public float speed = 10f;
    public int damage = 5;

    private Vector2 direction;

    public void Start()
    {
        Vector2 dir = (GameManager.instance.player.transform.position - transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle-45);
        direction = dir.normalized;

        Destroy(gameObject,2f);
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    //데미지
    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player"){
            Player playerScript = collision.gameObject.GetComponent<Player>();
            if(playerScript.dead) {
                Destroy(gameObject);
                return;
            }

            if(playerScript.reflect){
                if(enemy.dead||enemy.immune>0) {
                    Destroy(gameObject);
                    return;
                }
                enemy.hp -=damage;
                if(enemy.hp<=0) enemy.Die();
                enemy.StartCoroutine(enemy.HitColor());
                Destroy(gameObject);
                return;
            }

            playerScript.hp-=damage;
            if(playerScript.hp<=0) playerScript.Die();
            playerScript.StartCoroutine(playerScript.HitColor());
            Destroy(gameObject);
        }
    }
}
