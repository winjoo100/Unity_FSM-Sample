using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float maxHealth = 5;
    [SerializeField] float curHealth = 5;

    [Header("Combat")]
    [SerializeField] float attackCoolDown = 3f;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float aggroRange = 8f;

    GameObject player;
    NavMeshAgent agent;
    Animator animator;
    float timePassed;
    float newDestinationCoolDown = 0.5f;

    Vector3 spawnPosition;

    // --------------------------------------------------

    KD_System kd_System;

    // --------------------------------------------------

    public Action<float, float> OnHealthChange;        // 체력 변경 이벤트
    public Action<Transform, Vector3> OnRespawn;       // 리스폰 이벤트

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        kd_System = player.GetComponent<KD_System>();

        spawnPosition = this.transform.position;
        curHealth = maxHealth;
    }

    private void Update()
    {
        animator.SetFloat("speed", agent.velocity.magnitude / agent.speed);

        if (timePassed >= attackCoolDown)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
            {
                animator.SetTrigger("attack");
                timePassed = 0;
            }
        }
        timePassed += Time.deltaTime;

        if (newDestinationCoolDown <= 0 && Vector3.Distance(player.transform.position, transform.position) <= aggroRange)
        {
            newDestinationCoolDown = 0.5f;
            agent.SetDestination(player.transform.position);
        }
        newDestinationCoolDown -= Time.deltaTime;

        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(targetPosition);
    }

    private void OnEnable()
    {
        InitState();
    }

    public void TakeDamage(float damageAmount)
    {
        curHealth -= damageAmount;
        animator.SetTrigger("damage");

        InvokeHealthChange(maxHealth, curHealth);

        if (curHealth <= 0)
        {
            Die();
        }
    }

    public void StartDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>().StartDealDamage();
    }

    public void EndDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>().EndDealDamage();
    }

    private void Die()
    {
        kd_System.AddKillCount();
        InvokeRespawn(this.transform, spawnPosition);

        SoundManager.Instance.PlaySFX("Enemy_Die");

        this.gameObject.SetActive(false);
    }

    private void InitState()
    {
        curHealth = maxHealth;
        InvokeHealthChange(maxHealth, curHealth);
    }

    private void OnDrawGizmos()
    {
        // 공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 어그로 범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }

    private void InvokeHealthChange(float maxhp, float curhp)
    {
        OnHealthChange?.Invoke(maxhp, curhp);
    }

    private void InvokeRespawn(Transform spawnTransform, Vector3 spawnPosition)
    {
        OnRespawn?.Invoke(spawnTransform, spawnPosition);
    }
}
