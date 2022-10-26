using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectMenuHandler : MonoBehaviour
{
    private bool isFirstClick = true;
    private Animator m_animator;
    private AudioManager_PrototypeHero m_audioManager;
    private Button startbutton;
    private Button Loadbutton;
    private Button Settingbutton;
    private Button Exitbutton;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_audioManager = AudioManager_PrototypeHero.instance;
        m_audioManager.PlaySound("BGM");
        startbutton = transform.Find("Panel").Find("Select Menu").Find("Button1").GetComponent<Button>();
        Loadbutton = transform.Find("Panel").Find("Select Menu").transform.Find("Button2").GetComponent<Button>();
        Settingbutton = transform.Find("Panel").Find("Select Menu").transform.Find("Button3").GetComponent<Button>();
        Exitbutton = transform.Find("Panel").Find("Select Menu").transform.Find("Button4").GetComponent<Button>();

        startbutton.onClick.AddListener(OnClickBtn1);
        Loadbutton.onClick.AddListener(OnClickBtn2);
        Settingbutton.onClick.AddListener(OnClickBtn3);
        Exitbutton.onClick.AddListener(OnClickBtn4);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (isFirstClick)
            {
                m_animator.SetInteger("PanelState", 1);
                m_audioManager.PlaySound("UI_ConFirm");
                isFirstClick = false;
            }
        }
    }

    void OnClickBtn1()
    {
        Debug.Log("OnClickBtn1 Clicked");
        m_audioManager.PlaySound("UI_ConFirm");
        SceneManager.LoadScene("Resources/Character/Hero Knight - Pixel Art/Demo/Demo", LoadSceneMode.Single);

    }

    void OnClickBtn2()
    {
        Debug.Log("OnClickBtn2 Clicked");
        m_audioManager.PlaySound("UI_ConFirm");
    }

    void OnClickBtn3()
    {
        Debug.Log("OnClickBtn3 Clicked");
        m_audioManager.PlaySound("UI_ConFirm");
    }

    void OnClickBtn4()
    {
        Debug.Log("OnClickBtn4 Clicked");
        m_audioManager.PlaySound("UI_ConFirm");
        Application.Quit();
    }
}
