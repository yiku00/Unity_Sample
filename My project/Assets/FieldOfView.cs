using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius = 2.5f;
    [Range(1,360)]public float angle = 45f;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;
    public int Direction = 1;
    public GameObject PlayerRef;

    public bool CanSeePlayer { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        PlayerRef = GameObject.FindGameObjectWithTag("Player");
        targetLayer = LayerMask.GetMask("Player");
        obstructionLayer = LayerMask.GetMask("Obstruction");
        StartCoroutine(FOVCheck());
    }

    private IEnumerator FOVCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            FOV();
        }
    }

    private void FOV()
    {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        if (rangeCheck.Length > 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector2 directionTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(Direction*transform.right, directionTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);
                if (!Physics2D.Raycast(transform.position, directionTarget, distanceToTarget, obstructionLayer))
                    CanSeePlayer = true;
                else
                    CanSeePlayer = false;
            }
            else
                CanSeePlayer = false;
        }
        else if (CanSeePlayer)
            CanSeePlayer = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
        Vector3 angle1 = DirecionFromAngle(Direction*90, -angle / 2);
        Vector3 angle2 = DirecionFromAngle(Direction*90, angle / 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + angle1 * radius);
        Gizmos.DrawLine(transform.position, transform.position + angle2 * radius);

        if(CanSeePlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, PlayerRef.transform.position);
        }
    }

    private Vector2 DirecionFromAngle(float eulerY,float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));

    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<SpriteRenderer>().flipX)
            Direction = -1;
        else
            Direction = 1;
    }
}
