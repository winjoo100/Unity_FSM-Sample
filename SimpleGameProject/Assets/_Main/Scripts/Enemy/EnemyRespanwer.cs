using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespanwer : MonoBehaviour
{
    //-------------------------------------------------- 이벤트 스크립트
    [Header("Event")]
    public List<Enemy> enemies;

    [Header("리스폰 시간")]
    public float respawnTime;
    private WaitForSeconds reSpawnSeconds;

    private float spawnWaitTime = 2f;
    private WaitForSeconds reSpawnWaitSeconds;

    private void Start()
    {
        reSpawnSeconds = new WaitForSeconds(respawnTime);
        reSpawnWaitSeconds = new WaitForSeconds(spawnWaitTime);

        foreach(Enemy enemy in enemies)
        {
            enemy.OnRespawn += Respawn;
        }
    }

    void Respawn(Transform spawnTransform, Vector3 spawnPosition)
    {
        StartCoroutine(CoroutineRespawn(spawnTransform, spawnPosition));
    }

    IEnumerator CoroutineRespawn(Transform spawnTransform, Vector3 spawnPosition)
    {
        yield return reSpawnSeconds;

        GameObject reSpawnVFX = VFXObjectPool.instance.GetPoolObj(VFXPoolObjType.EnemyRespawn_VFX);
        reSpawnVFX.SetActive(true);
        reSpawnVFX.transform.position = spawnPosition;

        if (reSpawnVFX.TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            // 이펙트의 효과음에 볼륨 설정
            audioSource.volume = SoundManager.Instance.SFXVolume;
        }

        yield return reSpawnWaitSeconds;

        spawnTransform.position = spawnPosition;
        spawnTransform.gameObject.SetActive(true);
    }
}
