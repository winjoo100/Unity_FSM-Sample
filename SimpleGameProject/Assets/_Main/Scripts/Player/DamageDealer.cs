using UnityEngine;
using System.Collections.Generic;

public class DamageDealer : MonoBehaviour
{
    bool canDealDamage;                         // 공격 가능 여부를 나타내는 변수
    List<GameObject> hasDealDamage;             // 이미 피해를 준 오브젝트를 저장하는 리스트 (중복피해 방지)

    [SerializeField] float weaponLength;        // 무기의 공격 범위 (RayCast의 길이)
    [SerializeField] float weaponDamage;        // 무기의 공격력

    private void Start()
    {
        canDealDamage = false;                      // 처음에는 공격이 불가능한 상태
        hasDealDamage = new List<GameObject>();     // 공격한 대상 리스트 초기화
    }

    private void Update()
    {
        // 공격이 활성화 된 경우
        if(canDealDamage)
        {
            RaycastHit hit;

            // 9번 레이어(Enemy) 에 해당하는 오브젝트만 검사
            int layerMask = 1 << 9;
            if(Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
            {
                // 해당 오브젝트가 이전에 피해를 받은적이 없는 경우
                if(hit.transform.TryGetComponent(out Enemy enemy) && !hasDealDamage.Contains(hit.transform.gameObject))
                {
                    enemy.TakeDamage(weaponDamage);

                    // 해당 오브젝트를 리스트에 추가해서 중복 피해 방지
                    hasDealDamage.Add(hit.transform.gameObject);

                    GameObject vfx = VFXObjectPool.instance.GetPoolObj(VFXPoolObjType.PlayerAttack_VFX);
                    vfx.SetActive(true);
                    vfx.transform.position = hit.transform.position + Vector3.up;
                }
            }
        }
    }

    public void StartDealDamage()
    {
        canDealDamage = true;
        hasDealDamage.Clear();
    }

    public void EndDealDamage()
    {
        canDealDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}
