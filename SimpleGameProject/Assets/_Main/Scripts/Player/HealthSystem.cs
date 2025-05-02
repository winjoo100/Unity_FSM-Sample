using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("체력")]
    [SerializeField] float maxHp = 5;
    [SerializeField] float curHp = 5;
    Animator animator;

    //--------------------------------------------------

    KD_System kd_System;
    PlayerPositionInit p_posInit;

    //--------------------------------------------------

    public event Action<float, float> OnHealthChange;   // 체력 변경 이벤트

    private void Awake()
    {
        kd_System = GetComponent<KD_System>();
        p_posInit = GetComponent<PlayerPositionInit>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        curHp = maxHp;
    }

    public void TakeDamage(float damageAmount)
    {
        curHp -= damageAmount;
        animator.SetTrigger("damage");
        SoundManager.Instance.PlaySFX("Dialogue_Hit");

        InvokeHealthChange(maxHp, curHp);

        if (curHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        curHp = maxHp;
        kd_System.AddDeathCount();
        p_posInit.Respawn(height: 2);

        SoundManager.Instance.PlaySFX("Dialogue_Die");

        InvokeHealthChange(maxHp, curHp);
    }

    /// <summary>
    /// hp 변경 이벤트 호출
    /// </summary>
    /// <param name="maxHp"></param>
    /// <param name="curHp"></param>
    public void InvokeHealthChange(float maxHp, float curHp)
    {
        OnHealthChange?.Invoke(maxHp, curHp);
    }
}
