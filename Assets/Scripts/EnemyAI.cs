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

        // Player’ÇÕ
        if(distance > attackDistance )
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            // UŒ‚‹——£
            agent.isStopped = true;
            Attack();
        }

        animator.SetBool("IsMove", agent.velocity.magnitude > 0.1f);
    }

    private void Attack()
    {
        // UŒ‚‚ÌƒAƒjƒ[ƒVƒ‡ƒ“‚ðŒã‚Å“ü‚ê‚é
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldawn)
        {
            attackTimer = 0;

            PlayerHealth health = player.GetComponent<PlayerHealth>();

            if(health != null)
            {
                health.TakeDamage(attackDamage);
            }
            Debug.Log("Enemy Attack");
        }
    }
}
