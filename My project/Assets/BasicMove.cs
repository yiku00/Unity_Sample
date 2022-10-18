using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMove : MonoBehaviour
{
    private Rigidbody2D RB2D;

    // Start is called before the first frame update
    void Start()
    {
        RB2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = 0;

        if(Input.GetKeyDown(KeyCode.Space) && RB2D.transform.position.y == 0)
        {
            RB2D.AddForce(new Vector3(0, 300f, 0f));
        }

        Debug.Log("moveX = " + moveX + " moveY = " + moveY);
        transform.Translate(new Vector3(moveX, moveY, 0f).normalized * Time.deltaTime * 3f);
    }
}
