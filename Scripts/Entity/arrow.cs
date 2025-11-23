using UnityEngine;

public class arrow : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 5;

    private Vector2 direction;

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player playerScript = collision.GetComponent<Player>(); // 수정
        if (playerScript != null)
        {
            playerScript.TakeDamage(damage); // hp 감소 및 HitColor 실행
            Destroy(gameObject);
        }
    }
}
