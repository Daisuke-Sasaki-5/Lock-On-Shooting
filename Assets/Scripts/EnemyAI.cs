using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;

    public float attackDistance = 3f;
    public int attackDamage = 10;

    public float attackCooldawn = 1.5f;

    private float attackTimer;
    private NavMeshAgent agent;

    private Animator animator;

    private bool isAttacking;

    [SerializeField] private Transform targetpoint;
    public Transform TargetPoint => targetpoint;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.stoppingDistance = attackDistance;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Player追跡
        if(distance > attackDistance )
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            isAttacking = false;

            animator.SetBool("IsMove", true);
        }
        else
        {
            // 攻撃距離
            agent.isStopped = true;
            animator.SetBool("IsMove", false);
            Attack();
        }
    }

    private void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldawn && !isAttacking)
        {
            attackTimer = 0;

            // 攻撃アニメーション再生
            isAttacking = true;
            animator.SetTrigger("Attack");
        }
    }

    public void DealDamage()
    {
        PlayerHealth health = player.GetComponent<PlayerHealth>();

        if(health != null)
        {
            health.TakeDamage(attackDamage);
        }
        Debug.Log("Enemy Attack");
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}
