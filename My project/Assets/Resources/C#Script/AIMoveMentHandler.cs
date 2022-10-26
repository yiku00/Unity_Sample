using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    protected GameObject m_GO;
    public virtual void execute() { }
}

public class JumpCommand : Command
{

    float jumpForce;
    public JumpCommand(GameObject Actor, float JumpForce)
    {
        m_GO = Actor;
        jumpForce = JumpForce;
    }
    public override void  execute()
    {
        if(m_GO == null)
        {
            return;
        }
       Rigidbody2D RB = m_GO.GetComponent<Rigidbody2D>();
        if(RB == null)
        {
            return;
        }
        RB.velocity = new Vector2(RB.velocity.x, jumpForce);
    }

    private void jump()
    {

    }
}

public class MoveX : Command
{
    private int m_facingDirection = 1;
    float m_speed = 1f;
    private Vector2 TargetLocation;

    public MoveX(GameObject Actor, float Speed, Vector2 Destination)
    {
        m_GO = Actor;
        m_speed = Speed;
        TargetLocation = Destination;
    }
    public override void execute()
    {
        if (m_GO == null)
        {
            return;
        }
        Rigidbody2D RB = m_GO.GetComponent<Rigidbody2D>();
        if (RB == null)
        {
            return;
        }

        CalculateDirection();

        RB.velocity = new Vector2((float)m_facingDirection * m_speed, TargetLocation.y);
    }

    private void CalculateDirection()
    {
        Rigidbody2D RB = m_GO.GetComponent<Rigidbody2D>();
        if (RB == null)
        {
            return;
        }

        if (Mathf.Abs(TargetLocation.x - m_GO.transform.position.x) < 0.25)
        {
            RB.velocity = new Vector2(0, 0);
            return;
        }
        else if (TargetLocation.x > m_GO.transform.position.x)
        {
            m_facingDirection = 1;
            if(m_GO.GetComponent<SpriteRenderer>() == null) { return; }
            m_GO.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (TargetLocation.x < m_GO.transform.position.x)
        {
            m_facingDirection = -1;
            if (m_GO.GetComponent<SpriteRenderer>() == null) { return; }
            m_GO.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
}

public class Stop : Command
{
    public Stop(GameObject Actor)
    {
        m_GO = Actor;
    }
    public override void execute()
    {
        Rigidbody2D RB = m_GO.GetComponent<Rigidbody2D>();
        if (RB == null)
        {
            return;
        }

        RB.velocity = new Vector2(0f,0f);
    }
}

public class AIMoveMentHandler : MonoBehaviour
{
    private Rigidbody2D m_body2d;
    float m_speed = 1f;
    public Vector2 TargetLocation;
    Queue<Command> CommandQueue;
    
    // Start is called before the first frame update

    void Start()
    {
        CommandQueue = new Queue<Command>();
        m_body2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleCommand();
    }

    public void MoveToLocation(Vector2 TargetLoc)
    {
        //MoverOrder = true;
        TargetLocation = new Vector2(TargetLoc.x, TargetLoc.y);
        MoveX MoveCommand = new MoveX(this.gameObject, m_speed, TargetLoc);
        CommandQueue.Enqueue(MoveCommand);
    }

    private void HandleMovement()
    {
        if(Mathf.Abs(TargetLocation.x - this.transform.position.x) < 0.25)
        {
            Stop StopCommand = new Stop(this.gameObject);
            CommandQueue.Enqueue(StopCommand);
        }
    }

    private void HandleCommand()
    {
        if (CommandQueue.Count > 0)
        {
            Command dmp = CommandQueue.Dequeue();
            dmp.execute();
        }
    }
}
