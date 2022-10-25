using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Bearded : MonoBehaviour
{
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private AudioManager_PrototypeHero m_audioManager;
    private AIMoveMentHandler m_MovementComp;
    public List<Vector2> PatrollPoint;
    private int CurrtentPatrollIdx = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_audioManager = AudioManager_PrototypeHero.instance;
        m_MovementComp = GetComponent<AIMoveMentHandler>();
        //m_MovementComp.MoveToLocation(new Vector2(-19f, 0f));
        PatrollPoint = new List<Vector2>();
        PatrollPoint.Add(new Vector2(-19f, 0.2f));
        PatrollPoint.Add(new Vector2(-14f, 0.2f));
        TimerManager.instance.SetTimer(SetPatroll, 5, 5, -1);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Mathf.Abs(m_body2d.velocity.x) > 0 )
        {
            m_animator.SetBool("Moving", true);
        }
        else
        {
            m_animator.SetBool("Moving", false);
        }

    }

    void ToggleTargetVector()
    {
        m_MovementComp.MoveToLocation(new Vector2(-12f, 0f));
    }

    void SetPatroll()
    {
        if(PatrollPoint.Count <= 0)
        {
            return;
        }
        m_MovementComp.MoveToLocation(PatrollPoint[CurrtentPatrollIdx % PatrollPoint.Count]);
        CurrtentPatrollIdx++;
    }
}
