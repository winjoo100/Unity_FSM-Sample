using UnityEngine;
using UnityEngine.Rendering;

public class CursorController : MonoBehaviour
{
    bool isCursorLock;

    void Start()
    {
        isCursorLock = true;
        SetCurserState(isCursorLock);
    }

    void Update()
    {
        // TODO : ��ǲ �ý������� ����
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            isCursorLock = !isCursorLock;
            SetCurserState(isCursorLock);
        }
    }

    void SetCurserState(bool look = false)
    {
        if(look)    // Ŀ�� ��
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else        // Ŀ�� �� ����
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
