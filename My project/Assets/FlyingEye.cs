using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public List<Vector2> PatrollPoint;
    private AIMoveMentHandler m_MovementComp;
    private int CurrtentPatrollIdx = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_MovementComp = GetComponent<AIMoveMentHandler>();
        PatrollPoint = new List<Vector2>();
        PatrollPoint.Add(new Vector2(-18f, -0.28f));
        PatrollPoint.Add(new Vector2(-14f, -0.28f));
        TimerManager.instance.SetTimer(SetPatroll, 5, 5, -1);
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
}
