using UnityEngine;

public class ObjectCanvasController : MonoBehaviour
{
    private void Start()
    {
        // ������ ����
        this.transform.localScale = new Vector3(-1, 1, 1);
    }

    void Update()
    {
        //Vector3 targetPosition = Camera.main.transform.position;
        //targetPosition.y = transform.position.y; // Y���� ����
        //transform.LookAt(targetPosition);

        
        transform.LookAt(Camera.main.transform);
    }
}
