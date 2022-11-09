using System.Collections.Generic;
using UnityEngine;

public delegate void METHOD();

public class Timer : MonoBehaviour
{
    private float delay;
    private float InitedInterval;
    private float Currentinterval;
    private int TimerId;
    private int InitedLoopCount; //if LoopCount < 0 ===> infinite Loop
    private int RemainLoopCount;
    private  bool FirstPlay = true;
    
    METHOD method;

    public float Getdelay() { return delay; }
    public float Getinterval() { return InitedInterval; }
    public int GetTimerId() { return TimerId; }

    public float GetRemainToStart() { return Currentinterval; }

    public int GetLoopCount() { return InitedLoopCount; }

    public int GetRemainLoopCount() { return RemainLoopCount; }

    public void Init(METHOD func, float dly = 0, float interv = 0, int Id = -1, int LoopCnt = -1)
    {
        method = func;
        delay = dly;
        InitedInterval = Currentinterval = interv;
        TimerId = Id;
        InitedLoopCount = RemainLoopCount = LoopCnt;
    }

    public void UnbindTimer()
    {
        TimerManager.instance.DeleteTimer(TimerId);
        Destroy(this);
    }
    void Update()
    {
        if(delay <= 0)
        {
            if (FirstPlay)
            {
                method();
                FirstPlay = false;
                //Debug.Log("Timer Id:" + TimerId + " Is Activated");
                if(InitedLoopCount == 0)
                {
                    UnbindTimer();
                }
            }
            else if (Currentinterval <= 0)
            {
                method();
               //Debug.Log("Timer Id:" + TimerId + " Is Activated");
                if (RemainLoopCount == 0)
                {
                    UnbindTimer();
                }
                else
                {
                    Currentinterval = InitedInterval;
                }
            }
            else
            {
                Currentinterval -= Time.deltaTime;
            }
        }
        else
        {
            delay -= Time.deltaTime;
        }
        //Debug.Log("Timer Delay Tracer:" + delay);
    }
}

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;
    private List<Timer> TimerAL;

    int TimerIdGenerated = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one TimerManger in scene");
        }
        else
        {
            instance = this;
        }
        TimerAL = new List<Timer>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTimer(METHOD func ,float dly =0,float interv = 0, int LoopCnt = -1)
    {
        Timer tmp = this.gameObject.AddComponent<Timer>();
        tmp.Init(func, dly, interv, TimerIdGenerated, LoopCnt);
        TimerIdGenerated++;
        //Debug.Log("Parsed Param: dly" + dly + " interv:" + interv + " TimerIdGenerated:" + TimerIdGenerated + " LoopCnt:" + LoopCnt);
        TimerAL.Add(tmp);
    }

    private Timer FindTimer(int id)
    {
        if (TimerAL.Count <= 0) return null;

        for (int i = 0; i < TimerAL.Count; i++)
        {
            if (TimerAL[i].GetTimerId() == id) return TimerAL[i];
        }

        return null;
    }

    public void DeleteTimer(int id)
    {
        if (TimerAL.Count <= 0) return;

        for(int i=0;i< TimerAL.Count;i++)
        {
            if (TimerAL[i].GetTimerId() == id)TimerAL.RemoveAt(i);
        }
    }

}
