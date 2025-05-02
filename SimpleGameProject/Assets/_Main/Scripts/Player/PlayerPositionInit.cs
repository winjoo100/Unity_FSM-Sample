using UnityEngine;

public class PlayerPositionInit : MonoBehaviour
{
    Vector3 playerInitPosition;

    //--------------------------------------------------

    CharacterController characterController;
    KD_System kd_System;

    private void Awake()
    {
        kd_System = GetComponent<KD_System>();
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        playerInitPosition = transform.position;
    }

    private void Update()
    {
        if (transform.position.y <= -50f)
        {
            Respawn();
            SoundManager.Instance.PlaySFX("Dialogue_Respawn");
        }
    }

    public void Respawn(float height = 5)
    {
        characterController.enabled = false; // 컨트롤러를 잠시 끄고
        transform.position = playerInitPosition + new Vector3(0, height, 0); // 위치 설정
        characterController.enabled = true; // 다시 활성화

        GameObject spawnVFX = VFXObjectPool.instance.GetPoolObj(VFXPoolObjType.PlayerRespawn_VFX);
        spawnVFX.transform.position = playerInitPosition;
        spawnVFX.SetActive(true);

        kd_System.AddDeathCount();
    }
}
