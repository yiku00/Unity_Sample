using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    float h;
    float v;
    [SerializeField] float Speed;
    bool isHorizonMove;
    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        bool hDown = Input.GetButtonDown("Horizontal"); //수평방향 눌렀을 때 
        bool vDown = Input.GetButtonDown("Vertical");   //수직방향 눌렀을 때
        bool hUp = Input.GetButtonUp("Horizontal");     //수평방향 떼었을 때 이제는 수평 방향 
        bool vUp = Input.GetButtonUp("Vertical");       //수직방향 떼었을 때

        if (hDown || vUp)  
            isHorizonMove = true;
        else if (vDown || hUp)
            isHorizonMove = false;
    }

    void FixedUpdate()
    {
        Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
        rigid.velocity = moveVec * Speed;
    }
}
