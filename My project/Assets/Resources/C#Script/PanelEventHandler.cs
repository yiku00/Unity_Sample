using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelEventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Panel;

    public void OpenPanel()
    {
        if(Panel != null)
        {
            bool isActivate = Panel.activeSelf;
            Panel.SetActive(isActivate);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log("0");
        }
    }
}
