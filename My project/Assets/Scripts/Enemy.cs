using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour, IDamageable
{
    
    public GameObject patrolPointA;
    public GameObject patrolPointB;
    public float speed;

    public float HP;
    

    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;

    // Start is called before the first frame update
    void Start()
    {
        EnemyManager.Instance.RegisterEnemy(this);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = patrolPointB.transform;
        anim.SetBool("isRunning", true);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == patrolPointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == patrolPointB.transform)
        {
            Flip();
            currentPoint = patrolPointA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == patrolPointA.transform)
        {
            Flip();
            currentPoint = patrolPointB.transform;
        }
        
        if (HP <= 0)
        {
            
            Destroy(gameObject);
        }
    }

    // enemy flip
    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(patrolPointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(patrolPointB.transform.position, 0.5f);
        Gizmos.DrawLine(patrolPointA.transform.position, patrolPointB.transform.position);
    }
    
    public float GetHp()
    {
        return HP;
    }

    public void RemoveHP(float damage)
    {
        HP -= damage;
    } 
    
    void OnDestroy()
    {
        EnemyManager.Instance.UnregisterEnemy(this);
    }

    public void SetHp(float newHp)
    {
        HP = newHp;
    }
}
