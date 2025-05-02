using System;
using UnityEngine;

public class KD_System : MonoBehaviour
{
    [Header("현재 킬 / 데스")]
    public int killCount = 0;
    public int deathCount = 0;

    //--------------------------------------------------

    public Action<int> OnKillCountChange;
    public Action<int> OnDeathCountChange;

    public void AddKillCount()
    {
        ++killCount;
        InvokeKillChange();
    }

    public void AddDeathCount()
    {
        ++deathCount;
        InvokeDeathChange();
    }

    public void InvokeKillChange()
    {
        OnKillCountChange?.Invoke(killCount);
    }

    public void InvokeDeathChange()
    {
        OnDeathCountChange?.Invoke(deathCount);
    }
}
