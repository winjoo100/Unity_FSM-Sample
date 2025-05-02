using UnityEngine;
using UnityEngine.UI;

public class EnemyUIController : MonoBehaviour
{
    //--------------------------------------------------
    [Header("Event")]
    public Enemy enemy;

    //--------------------------------------------------
    [Header("UI")]
    public Image hp_fill;

    private void Start()
    {
        enemy.OnHealthChange += ChangeHealth;
    }

    public void ChangeHealth(float maxhp, float curhp)
    {
        hp_fill.fillAmount = curhp / maxhp;
    }
}
