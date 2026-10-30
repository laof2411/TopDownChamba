using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable
{
    [field:SerializeField] public int Health { get; set; }
    [field: SerializeField] public LayerMask GetHitBy { get; set; }

    [SerializeField] NavMeshAgent agent;
    [SerializeField] float detectionRadius;
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask playerMask;

    public bool waveEnemy;
    public bool canAttack;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (waveEnemy)
        {
            agent.SetDestination(FindAnyObjectByType<PlayerController>().transform.position);
        }
        else if (Physics.CheckSphere(transform.position, detectionRadius, playerMask))
        {
            agent.SetDestination(FindAnyObjectByType<PlayerController>().transform.position);
        }

        if(Physics.CheckSphere(transform.position, attackRadius, playerMask))
        {
            canAttack = false;
            StartCoroutine(ResetAttack());

            Transform playerTransform = FindAnyObjectByType<PlayerController>().transform;

            Ray ray = new Ray(transform.position + Vector3.up, playerTransform.position - transform.position);
            Debug.DrawRay(transform.position + Vector3.up, (playerTransform.position - transform.position) * 5);
            if(Physics.Raycast(ray, out RaycastHit playerInfo, 5, 3))
            {
                Debug.Log(playerInfo.transform.name);
                if (playerInfo.transform.TryGetComponent<IDamagable>(out IDamagable damagable))
                {
                    Debug.Log("Hola");
                    //3 is the player layer
                    damagable.Damage(1, 6);
                }
            }
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}
