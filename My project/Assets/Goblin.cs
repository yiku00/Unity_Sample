using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    public List<Vector2> PatrollPoint;
    private AIMoveMentHandler m_MovementComp;
    private Animator m_animator;
    private int CurrtentPatrollIdx = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_MovementComp = GetComponent<AIMoveMentHandler>();
        m_animator = GetComponent<Animator>();
        PatrollPoint = new List<Vector2>();
        PatrollPoint.Add(new Vector2(-18f, -0.28f));
        PatrollPoint.Add(new Vector2(-14f, -0.28f));
        TimerManager.instance.SetTimer(Attack2, 5, 5, -1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetPatroll()
    {
        if (PatrollPoint.Count <= 0)
        {
            return;
        }
        m_MovementComp.MoveToLocation(PatrollPoint[CurrtentPatrollIdx % PatrollPoint.Count]);
        CurrtentPatrollIdx++;
    }

    public void Attack2()
    {
        m_animator.Play("Attack2");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "PlayerAtt")
        {
            
            OnHit(collision);
        }
    }

    private void OnHit(Collision2D collision)
    {

        throw new NotImplementedException();
    }

    private void ApplyDamage(float dmg)
    {
        m_animator.Play("Hitted");
    }
}
