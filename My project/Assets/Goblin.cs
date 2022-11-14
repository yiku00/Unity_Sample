using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInfo
{
public float damage;
public Collider2D DamagingObject;
    public DamageInfo()
    {

    }

}

public class Goblin : MonoBehaviour
{
    public List<Vector2> PatrollPoint;
    public List<Timer> MyTimer;
    private AIMoveMentHandler m_MovementComp;
    private Animator m_animator;
    private int CurrtentPatrollIdx = 0;
    private TimerHandler AttHendler;
    private BattleHelper m_BattleHelper;


    // Start is called before the first frame update
    void Start()
    {
        m_MovementComp = GetComponent<AIMoveMentHandler>();
        m_animator = GetComponent<Animator>();
        PatrollPoint = new List<Vector2>();
        PatrollPoint.Add(new Vector2(-18f, -0.28f));
        PatrollPoint.Add(new Vector2(-14f, -0.28f));

        AttHendler = new TimerHandler();
        AttHendler.SetTimer(Attack2, 5, 5, -1);
        //TimerManager.instance.SetTimer(Attack2, 5, 5, -1);


        m_BattleHelper = GetComponent<BattleHelper>();
        if (m_BattleHelper)
        {
            m_BattleHelper.TargetLayer = LayerMask.GetMask("Player");
            m_BattleHelper.Damage = 40f;
            m_BattleHelper.InitHp(100f);
            m_BattleHelper.CanBeDestroy = true;
        }


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
            //OnHit(collision);
        }
    }

    private void OnDestroy()
    {
        AttHendler.DeleteTimer();
    }

    public void CharaOnDead()
    {
        AttHendler.DeleteTimer();
    }


}
