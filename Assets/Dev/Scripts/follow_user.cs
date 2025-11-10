using UnityEngine;

public class ImageController : MonoBehaviour
{
    // 속도 조절용 변수
    public float moveSpeed = 5f;

    void Update()
    {
        // 방향키로 이미지 움직이기
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        transform.position += new Vector3(moveX, moveY, 0) * moveSpeed * Time.deltaTime;
    }
}