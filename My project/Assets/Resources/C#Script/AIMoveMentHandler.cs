using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveMentHandler : MonoBehaviour
{
    private Rigidbody2D m_body2d;
    float m_speed = 1f;
    private int m_facingDirection = 1;
    public Vector2 TargetLocation;
    private bool MoverOrder = false;
    // Start is called before the first frame update

    void Start()
    {

        m_body2d = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void Move(float Direction, float y)
    {
        m_body2d.velocity = new Vector2((float)m_facingDirection * m_speed, y);
    }

    public void MoveToLocation(Vector2 TargetLoc)
    {
        MoverOrder = true;
        TargetLocation = new Vector2(TargetLoc.x, TargetLoc.y);
    }

    private void HandleMovement()
    {
        if(MoverOrder == false)
        {
            return; //can define as method like "canmove"
        }

        if(Mathf.Abs(TargetLocation.x - m_body2d.position.x) + Mathf.Abs(TargetLocation.y - m_body2d.position.y) < 0.5)
        {
            TargetLocation = new Vector2();
            m_body2d.velocity = new Vector2(0, 0);
            MoverOrder = false;
            return;
        }
        else if (TargetLocation.x > m_body2d.position.x)
        {
            m_facingDirection = 1;
            GetComponent<SpriteRenderer>().flipX = false;
            m_body2d.velocity = new Vector2((float)m_facingDirection * m_speed, TargetLocation.y);
        }
        else if (TargetLocation.x < m_body2d.position.x)
        {
            m_facingDirection = -1;
            GetComponent<SpriteRenderer>().flipX = true;
            m_body2d.velocity = new Vector2((float)m_facingDirection * m_speed, TargetLocation.y);
        }


    }
}
