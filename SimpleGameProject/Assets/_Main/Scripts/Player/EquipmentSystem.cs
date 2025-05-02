using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    [SerializeField] GameObject weaponInHand;
    [SerializeField] GameObject weaponInSheath;

    private void Start()
    {
        weaponInHand.SetActive(false);
        weaponInSheath.SetActive(true);
    }

    public void DrawWeapon()
    {
        weaponInHand.SetActive(true);
        weaponInSheath.SetActive(false);
    }

    public void SheathWeapon()
    {
        weaponInSheath.SetActive(true);
        weaponInHand.SetActive(false);
    }

    public void StartDealDamage()
    {
        weaponInHand.GetComponentInChildren<DamageDealer>().StartDealDamage();
    }

    public void EndDealDamage()
    {
        weaponInHand.GetComponentInChildren<DamageDealer>().EndDealDamage();
    }
}
