using UnityEngine;

public class ObjectCanvasController : MonoBehaviour
{
    private void Start()
    {
        // 스케일 반전
        this.transform.localScale = new Vector3(-1, 1, 1);
    }

    void Update()
    {
        //Vector3 targetPosition = Camera.main.transform.position;
        //targetPosition.y = transform.position.y; // Y값을 고정
        //transform.LookAt(targetPosition);

        
        transform.LookAt(Camera.main.transform);
    }
}
