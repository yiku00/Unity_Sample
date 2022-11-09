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

    void AE_BackFlipStart()
    {
        Rigidbody2D m_body2d = GetComponent<Rigidbody2D>();
        if (m_body2d == null)
            return;

        m_body2d.AddForce(new Vector2(m_MovementComp.GetDirection(), 0f) * 5f);

    }

    void AE_CutFront()
    {
        Rigidbody2D m_body2d = GetComponent<Rigidbody2D>();
        if (m_body2d == null || m_MovementComp == null)
            return;

        m_body2d.AddForce(new Vector2(m_MovementComp.GetDirection(), 0f) * 100f);
    }

    public void Attack2()
    {
        m_animator.Play("Attack2");
    }
}
