using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMenuHandler : MonoBehaviour
{
    private bool isFirstClick = true;
    private Animator m_animator;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (isFirstClick)
            {
                m_animator.SetInteger("PanelState", 1);
                isFirstClick = false;
            }
        }
    }
}
