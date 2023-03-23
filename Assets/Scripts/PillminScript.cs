using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class PillminScript : MonoBehaviour
{
    public ObstacleScript obstacleScript;
    
    public Transform target;
    private NavMeshAgent agent;

    [SerializeField] private float lookRadius = 15f;
    public Rigidbody rb;
    [SerializeField] private LayerMask enemyLayer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);
        }
    }

    private void OnCollisionEnter(Collision c)
    {
        //PillminDestroy();
        //Collider[] hitEnemy = Physics.OverlapSphere(rb.position, 0.5f, enemyLayer);

        if ((enemyLayer.value & 1<<c.gameObject.layer) == 1<<c.gameObject.layer)
            //c.gameObject.layer == enemyLayer)
        {
            c.gameObject.GetComponent<ObstacleScript>().DestroySelf();
            Destroy(gameObject);

            Debug.Log("Detect");
        }

    }

    private void PillminDestroy()
    {
        //Collider[] hitEnemy = Physics.OverlapSphere(rb.position, 0.5f, enemyLayer);

        //foreach(Collider enemy in hitEnemy)
        //{
        //    gameObject.GetComponent<ObstacleScript>().DestroySelf();
        //    Destroy(gameObject);
        //}
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, lookRadius);
    }
}
