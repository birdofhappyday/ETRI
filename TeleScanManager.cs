using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;


public class TeleScanManager : SingleTon<TeleScanManager>
{
    public float findSec = 1.0f;

    [DllImport("user32.dll")]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    [DllImport("user32.dll")]
    static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern void keybd_event(byte vk, byte scan, int flags, ref int extrainfo);

    const byte WM_LEFT = 37;
    const byte WM_UP = 38;
    const byte WM_RIGHT = 39;
    const byte WM_DOWN = 40;


    public IntPtr notepadTextbox;
    public IntPtr hWnd;
    public IntPtr my;

    private bool flag = true;
    private bool up_first = true;
    private float time;
    private bool dwnFlag = false;
    const byte Down = 40;
    int info = 0;

    public bool m_active = false;
    // Use this for initialization
    void Start()
    {
        if (!m_active)
            return;

        hWnd = FindWindow(null, "TeleScan");
        //my = FindWindow("UnityContainerWndClass", null);

        my = FindWindow(null, "Scenario");
        if (my != null)
        {
        //    my = FindWindowEx(my, IntPtr.Zero, "UnityGUIViewWndClass", null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_active)
            return;

        if (!flag)
        {
            time += Time.deltaTime;
            if (time >= findSec)
            {
                flag = true;
                SetForegroundWindow(my);
                time = 0;

                if(dwnFlag)
                {
                    Application.Quit();
                }
            }
        }

        #region[TestKey]
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    SetForegroundWindow(hWnd);
        //    time = 0;

        //    keybd_event(Down, 0, 0, ref info);
        //    Debug.Log("키값보냄");
        //    flag = false;
        //}

        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    Debug.Log("Left");
        //    SetForegroundWindow(hWnd);
        //    time = 0;
        //    keybd_event(WM_LEFT, 0, 0, ref info);
        //    //LeftKey();
        //    flag = false;
        //}

        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    Debug.Log("Up");

        //    SetForegroundWindow(hWnd);
        //    time = 0;
        //    keybd_event(WM_UP, 0, 0, ref info);
        //    //UpKey();
        //    flag = false;
        //}

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    Debug.Log("Right");

        //    SetForegroundWindow(hWnd);

        //    time = 0;
        //    keybd_event(WM_RIGHT, 0, 0, ref info);
        //    //RightKey();
        //    flag = false;
        //}

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    Debug.Log("Down");
        //    SetForegroundWindow(hWnd);
        //    time = 0;
        //    keybd_event(WM_DOWN, 0, 0, ref info);
        //    //DownKey();
        //    flag = false;
        //}
        #endregion
    }

    public void LeftKey()
    {
        if (up_first)
        {
            DataManager.Instance.Stopstate1 = false;
            DataManager.Instance.Seq_Name++;
            DataManager.Instance.Key_TeleScan = "u";
            DataManager.Instance.Seq_Status = Seq_Status.R;
            DataManager.Instance.TextFlag = true;
            DataManager.Instance.Xbox_Init();

            SetForegroundWindow(hWnd);
            time = 0;
            keybd_event(WM_UP, 0, 0, ref info);
            flag = false;

            up_first = false;
        }
        else
        {
            DataManager.Instance.Stopstate1 = false;
            DataManager.Instance.Seq_Name++;
            DataManager.Instance.Key_TeleScan = "l";
            DataManager.Instance.Seq_Status = Seq_Status.R;
            DataManager.Instance.Xbox_Init();

            SetForegroundWindow(hWnd);
            time = 0;
            keybd_event(WM_LEFT, 0, 0, ref info);
            flag = false;
        }

        if (!m_active)
            return;


    }

    public void UpKey()
    {
        if (!m_active)
            return;

        SetForegroundWindow(hWnd);
        time = 0;
        keybd_event(WM_UP, 0, 0, ref info);
        flag = false;
    }

    public void RightKey()
    {
        DataManager.Instance.Stopstate1 = true;
        DataManager.Instance.Seq_Name++;
        DataManager.Instance.Key_TeleScan = "r";
        DataManager.Instance.Seq_Status = Seq_Status.N;

        if (!m_active)
            return;

        SetForegroundWindow(hWnd);
        time = 0;
        keybd_event(WM_RIGHT, 0, 0, ref info);
        flag = false;
    }

    public void DownKey()
    {
        DataManager.Instance.Key_TeleScan = "d";


        SetForegroundWindow(hWnd);
        time = 0;
        keybd_event(WM_DOWN, 0, 0, ref info);
        flag = false;

        if (!m_active)
            return;

        dwnFlag = true;
    }


}

