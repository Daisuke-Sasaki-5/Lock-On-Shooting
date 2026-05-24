using System;
using UnityEngine;
using UnityEngine.AI;

public class EneyAI : MonoBehaviour
{
    public Transform player;

    public float attackDistance = 3f;
    public int attackDamage = 10;

    public float attackCoolDdawn = 1.5f;

    private float attackTimer;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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
    }

    private void Attack()
    {
        // UŒ‚‚ÌƒAƒjƒ[ƒVƒ‡ƒ“‚ðŒã‚Å“ü‚ê‚é
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCoolDdawn)
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
