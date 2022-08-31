using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zinputTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    Debug.Log("GetKeyDown");
        //}

        //if(Input.GetKeyUp(KeyCode.Space))
        //{
        //    Debug.Log("GetKeyUp");
        //}

        //if (Input.GetKey(KeyCode.Space))
        //{
        //    Debug.Log("GetKey");
        //}

        float movex = 0f;
        float movez = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            movex -= 1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movex += 1f;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            movez += 1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            movez -= 1f;
        }

        transform.Translate(new Vector3(movex, movez, 0f) * Time.deltaTime);
    }
}
