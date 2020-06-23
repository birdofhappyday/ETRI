using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public enum Seq_name
{
    None,

    S113,
    D113,
    S108,
    D108,
    S116,
    D116,
    S109,
    D109,
    S102,
    D102,
    S106,
    D106,
    S104,
    D104,
    S112,
    D112,
    S117,
    D117,
    S114,
    D114,
    S107,
    D107,
    S118,
    D118,
    S101,
    D101,
    S105,
    D105,
    S110,
    D110,
    S111,
    D111,
    S103,
    D103,
    S115,
    D115,

    Q001,
    B001,

    S221,
    D221,
    S210,
    D210,
    S206,
    D206,
    S216,
    D216,
    S203,
    D203,
    S222,
    D222,
    S201,
    D201,
    S218,
    D218,
    S220,
    D220,
    S223,
    D223,
    S205,
    D205,
    S209,
    D209,
    S225,
    D225,
    S214,
    D214,
    S208,
    D208,
    S226,
    D226,
    S217,
    D217,
    S207,
    D207,
    S213,
    D213,
    S212,
    D212,
    S204,
    D204,
    S211,
    D211,
    S219,
    D219,
    S215,
    D215,
    S224,
    D224,
    S202,
    D202,

    Q002,
    B002,

    S005,
    D005,
    S008,
    D008,
    S002,
    D002,
    S001,
    D001,
    S003,
    D003,
    S006,
    D006,
    S007,
    D007,
    S004,
    D004,

    Q003,
}

public enum Seq_Status
{
    None,
    R,
    V,
    N
}

public class DataManager : SingleTon<DataManager>
{
    public Transform drone;
    public Transform droneXZ;
    public Transform cam;
    private bool m_captureFlag = true;
    private bool m_textFlag = false;
    public string digit_7number = "0000000";
    public string digit_6number = "000000";

    private bool Stopstate = false;
    private int frame = 0;
    private float fps = 0;
    private string float_digit_number = "0.000";
    private Seq_name seq_Name = Seq_name.None;
    private Seq_Status seq_Status = Seq_Status.None;
    private Vector3 lastDronePos;
    private Vector3 lastDronePosVel = Vector3.zero;
    private Vector3 lastDroneRot;
    private Vector3 lastDroneRotVel = Vector3.zero;
    private Vector3 lastCamPos;
    private Vector3 lastCamPosVel = Vector3.zero;
    private Vector3 lastCamRot;
    private Vector3 lastCamRotVel = Vector3.zero;
    private string Xbox_fw = "-";
    private string Xbox_left = "-";
    private string Xbox_right = "-";
    private string key_TeleScan = "-";
    private float Eye_x = 0;
    private float Eye_y = 0;

    #region property

    public int Frame
    {
        get { return frame; }
        set { frame = value; }
    }

    public string Digit_7number
    {
        get { return digit_7number; }
    }

    public string Digit_6number
    {
        get { return digit_6number; }
    }

    public string Digit_float_number
    {
        get { return float_digit_number; }
    }

    public Seq_name Seq_Name
    {
        get { return seq_Name; }
        set { seq_Name = value; }
    }

    public Vector3 LastDronePos
    {
        get { return lastDronePos; }
        set { lastDronePos = value; }
    }

    public Vector3 LastDronePosVel
    {
        get
        {
            return lastDronePosVel;
        }

        set
        {
            lastDronePosVel = value;
        }
    }

    public Vector3 LastDroneRot
    {
        get
        {
            return lastDroneRot;
        }

        set
        {
            lastDroneRot = value;
        }
    }

    public Vector3 LastDroneRotVel
    {
        get
        {
            return lastDroneRotVel;
        }

        set
        {
            lastDroneRotVel = value;
        }
    }

    public Vector3 LastCamPos
    {
        get
        {
            return lastCamPos;
        }

        set
        {
            lastCamPos = value;
        }
    }

    public Vector3 LastCamPosVel
    {
        get
        {
            return lastCamPosVel;
        }

        set
        {
            lastCamPosVel = value;
        }
    }

    public Vector3 LastCamRot
    {
        get
        {
            return lastCamRot;
        }

        set
        {
            lastCamRot = value;
        }
    }

    public Vector3 LastCamRotVel
    {
        get
        {
            return lastCamRotVel;
        }

        set
        {
            lastCamRotVel = value;
        }
    }

    public Seq_Status Seq_Status
    {
        get
        {
            return seq_Status;
        }

        set
        {
            seq_Status = value;
        }
    }

    public string Key_TeleScan
    {
        get
        {
            return key_TeleScan;
        }

        set
        {
            key_TeleScan = value;
        }
    }

    public float FPS
    {
        get
        {
            return fps;
        }

        set
        {
            fps = value;
        }
    }

    public float Eye_x1
    {
        get
        {
            return Eye_x;
        }

        set
        {
            Eye_x = value;
        }
    }

    public float Eye_y1
    {
        get
        {
            return Eye_y;
        }

        set
        {
            Eye_y = value;
        }
    }

    public string Xbox_fw1
    {
        get
        {
            return Xbox_fw;
        }

        set
        {
            Xbox_fw = value;
        }
    }

    public string Xbox_left1
    {
        get
        {
            return Xbox_left;
        }

        set
        {
            Xbox_left = value;
        }
    }

    public string Xbox_right1
    {
        get
        {
            return Xbox_right;
        }

        set
        {
            Xbox_right = value;
        }
    }

    public bool Stopstate1
    {
        get
        {
            return Stopstate;
        }

        set
        {
            Stopstate = value;
        }
    }

    public bool TextFlag
    {
        get { return m_textFlag; }
        set { m_textFlag = value; }
    }

    public bool CaptureFlag
    {
        get { return m_captureFlag; }
        set { m_captureFlag = value; }
    }

    #endregion

    public void TeleScan_Init()
    {
        key_TeleScan = "-";
    }

    public void Xbox_Init()
    {
        Xbox_fw = "0";
        Xbox_left = "0";
        Xbox_right = "0";
    }

    public void Xbox_Stop()
    {
        Xbox_fw = "-";
        Xbox_left = "-";
        Xbox_right = "-";
    }

    public void Xbox_Pad(string fw, string left, string right)
    {
        Xbox_fw = fw;
        Xbox_left = left;
        Xbox_right = right;
    }

    public string Day()
    {
        return DateTime.Now.ToString("yyyy-MM-dd");
    }

    public string NowTime()
    {
        return DateTime.Now.ToString("HH-mm-ss.fff");
    }

    public string CalString(Vector3 pos)
    {
        return pos.x.ToString(float_digit_number) + "," + pos.y.ToString(float_digit_number) + "," + pos.z.ToString(float_digit_number);
    }

    public Vector3 VelocityCalculator(Vector3 current, Vector3 last)
    {
        Vector3 velocity = (current - last) / Time.deltaTime;
        return velocity;
    }

    public Vector3 AccelCalculator(Vector3 current, Vector3 last)
    {
        Vector3 accel = (current - last) / Time.deltaTime;
        return accel;
    }
}
