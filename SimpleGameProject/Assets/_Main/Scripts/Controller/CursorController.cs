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
        // TODO : 인풋 시스템으로 변경
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            isCursorLock = !isCursorLock;
            SetCurserState(isCursorLock);
        }
    }

    void SetCurserState(bool look = false)
    {
        if(look)    // 커서 락
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else        // 커서 락 해제
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
