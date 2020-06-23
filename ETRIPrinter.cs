using UnityEngine;
using System.IO;
using System.Drawing;
using System;
using System.Runtime.InteropServices;
using System.Collections;

public class ETRIPrinter : MonoBehaviour
{
    public TestCapture depthCapture;
    public TestCapture depthCapture2;

    private IntPtr client;
    private StreamWriter write_1;
    private string textPath;
    // Use this for initialization

    // FPS
    private float accum = 0;

    //Calc
    private Vector3 dronePos;
    private Vector3 dronePosVel;
    private Vector3 dronePosAccel;
    private Vector3 droneRot;
    private Vector3 droneRotVel;
    private Vector3 droneRotAccel;
    private Vector3 camPos;
    private Vector3 camPosVel;
    private Vector3 camPosAccel;
    private Vector3 camRot;
    private Vector3 camRotVel;
    private Vector3 camRotAccel;
    private string Xbox_fw;
    private string Xbox_left;
    private string Xbox_right;
    private string m_teleScan;
    private Seq_name m_seq_Name;
    private Seq_Status m_seq_Status;
    private static double m_eye_x;
    private static double m_eye_y;
    private static bool m_found;
    private static double m_pupil_x;
    private static double m_pupil_y;
    private static double m_size_a;
    private static double m_size_b;
    private static double m_angle_degree;

    private bool stopState = false;

    private GazeClientExtern gazeClient;

    //Image

    public byte[] tt;
    private int image_width = 640, image_height = 480, image_channel = 0; //
    private bool texture_made = false, texture_copied = false;
    private object texture_lock = new object();
    private Texture2D tex,tex2;

    /// <summary>
    /// 붙어져있는 플러그인에 있는 함수로 유저의 눈을 쫓아서 실행된다.
    /// </summary>
    /// <param name="found"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="pupil_x"></param>
    /// <param name="pupil_y"></param>
    /// <param name="size_a"></param>
    /// <param name="size_b"></param>
    /// <param name="angle_degree"></param>
    void onGazePoint(bool found, double x, double y, double pupil_x, double pupil_y, double size_a, double size_b, double angle_degree)
    {
        m_eye_x = (float)x;
        m_eye_y = (float)y;

        m_found = found;
        m_pupil_x = pupil_x;
        m_pupil_y = pupil_y;
        m_size_a = size_a;
        m_size_b = size_b;
        m_angle_degree = angle_degree;
    }

    /// <summary>
    /// 붙어져 있는 플러그인에 있는 함수로 이미지를 전달해준다.
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="channel"></param>
    void onImage(byte[] buffer, int width, int height, int channel)
    {
        tt = buffer;

        // 텍스쳐 크기와 이미지 크기가 서로 다른경우 텍스쳐를 새롭게 만들어줘야함
        // 메인쓰레드에서 만들어야 안전하기에 여기서는 boolean 값만 변경
        if (image_channel != channel)
        {
            image_channel = channel;
            texture_made = false;
        }

        // 텍스쳐의 사이즈가 이미지 크기와 같다면 buffer에서 이미지를 복사해오고 copy가 되었다고 boolean값 변경
        if (texture_made)
        {
            lock (texture_lock)
            {
               // tex.LoadRawTextureData(buffer);
                texture_copied = true;
            }
        }
    }

    /// <summary>
    /// 전달받은 유저의 얼굴 텍스처를 새로 생성한 텍스쳐에 복사해서 아래 함수를 호출해서 저장시킨다.
    /// </summary>
    /// <param name="frame"></param>
    private void SaveEyeImage(float frame)
    {
        
        if (!texture_made)
        {
            tex = new Texture2D(image_width, image_height, TextureFormat.RGB24, false);
          //  tex2 = new Texture2D(160, 120, TextureFormat.RGB24, false);
           
            texture_made = true;
        }

        lock (texture_lock)
        {
            // 텍스쳐가 카피가 되었다면 apply 시켜줌
            if (texture_copied)
            {
                //System.DateTime time = System.DateTime.Now;

                string file = frame.ToString(DataManager.Instance.digit_7number) + "_" + DataManager.Instance.NowTime() + ".jpeg";

                  tex.LoadRawTextureData(tt);
                  texture_copied = false;

                //tex.Resize(64, 48);
                /*   int xx = 0, yy = 0;
                   for (int y = 0; y < image_height;)
                   {
                       for (int x = 0; x < image_width;)
                       {
                           UnityEngine.Color color= tex.GetPixel(x, y);
                           tex2.SetPixel(xx, yy, color);
                           x = x + 4;
                           xx++;
                       }
                       y = y + 4;
                       yy++;

                   }

                   tex2.Apply();*/
                  tex.Apply();

                 SaveTextureToFile(tex, file);
               // SaveTextureToFile(file);

            }
        }
    }

    /// <summary>
    /// 전달받은 유저의 얼굴 텍스처를 파일로 바꾸어서 저장시킨다.
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="fileName"></param>
    private void SaveTextureToFile(Texture2D texture, string fileName)
    {
        byte[] bytes;
        bytes = texture.EncodeToJPG();
        System.IO.FileStream fileSave;
        DirectoryInfo di = new DirectoryInfo(RockVR.Video.PathConfig.e_savePath + "/Eye/");

        if (di.Exists == false)
        {
            di.Create();
        }

        fileSave = new FileStream(RockVR.Video.PathConfig.e_savePath + "/Eye/" + fileName, FileMode.Create);

        System.IO.BinaryWriter binary;
        binary = new BinaryWriter(fileSave);
          binary.Write(bytes);
       // binary.Write(tt);
        fileSave.Close();
    }

    /// <summary>
    /// 시작시 저장할 폴더를 찾고 없으면 만든다음 각종 수치를 저장하기 위해 엑셀파일을 만들어 놓는다.
    /// </summary>
    void Start()
    {
        textPath = RockVR.Video.PathConfig.savePath + "/Text/";

        if (!Directory.Exists(textPath))
        {
            Directory.CreateDirectory(textPath);
        }

        if (null == DataManager.Instance.drone)
        {
            Debug.Log("Drone null!");
            return;
        }

        else if (null == DataManager.Instance.droneXZ)
        {
            Debug.Log("DroneXZ null!");
            return;
        }

        else if (null == DataManager.Instance.cam)
        {
            Debug.Log("Cam null!");
            return;
        }
        textPath = textPath + "/ETRIPrinter.csv";
        //tex = new Texture2D(image_width, image_height, TextureFormat.RGB24, false);
        write_1 = new StreamWriter(textPath, false);

        DataManager.Instance.LastDronePos = DataManager.Instance.drone.position;
        DataManager.Instance.LastDroneRot = new Vector3(DataManager.Instance.droneXZ.localEulerAngles.x, DataManager.Instance.drone.localEulerAngles.y, DataManager.Instance.droneXZ.localEulerAngles.z);


        DataManager.Instance.LastCamPos = DataManager.Instance.cam.position;
        DataManager.Instance.LastCamRot = DataManager.Instance.cam.eulerAngles;

        write_1.WriteLine("Frame,Date,Time,Seq_Name,Status,Key_TeleScan,Cam_Pos_x,"
                          + "Cam_Pos_y,Cam_Pos_z,Cam_Pos_Vel_x,Cam_Pos_Vel_y,Cam_Pos_Vel_z,Cam_Pos_Acc_x,Cam_Pos_Acc_y,Cam_Pos_Acc_z,"
                          + "Cam_Rot_x,Cam_Rot_y,Cam_Rot_z,Cam_Rot_Vel_x,Cam_Rot_Vel_y,Cam_Rot_Vel_z,Cam_Rot_Acc_x,Cam_Rot_Acc_y,Cam_Rot_Acc_z,"
                          + "Head_Pos_x,Head_Pos_y,Head_Pos_z,Head_Pos_Vel_x,Head_Pos_Vel_y,Head_Pos_Vel_z,Head_Pos_Acc_x,Head_Pos_Acc_y,Head_Pos_Acc_z,"
                          + "Head_Rot_x,Head_Rot_y,Head_Rot_z,Head_Rot_Vel_x,Head_Rot_Vel_y,Head_Rot_Vel_z,Head_Rot_Acc_x,Head_Rot_Acc_y,Head_Rot_Acc_z,"
                          + "Key_Pad_Fw,Key_Pad_left,Key_Pad_right,Eye_x,Eye_y,Found,Pupil_x,Pupil_y,Size_a,Size_b,Angle_degree,Fps");

        gazeClient = GazeClientExtern.GetInstance();
        gazeClient.StartTracking();
        gazeClient.SetGazePointCallback(onGazePoint);
        gazeClient.SetImageCallback(onImage);
        gazeClient.StartStreamImage(30);

        //30, 60
    }

    /// <summary>
    /// 상태 체크를 하면서 계속 계산을 돌린다.
    /// </summary>
    void Update()
    {
        if (!DataManager.Instance.TextFlag)
            return;

        if (null == DataManager.Instance.drone)
        {
            Debug.Log("Drone null!");
            return;
        }

        else if (null == DataManager.Instance.droneXZ)
        {
            Debug.Log("DroneXZ null!");
            return;
        }

        else if (null == DataManager.Instance.cam)
        {
            Debug.Log("Cam null!");
            return;
        }

        Calc();
        Data_Write();
        AfterCalc();
    }

    /// <summary>
    /// 중간에 앱을 꺼질 때를 대비해서 종료해야 하는 부분을 종료시킨다. 엑셀과 다른 작동하고 있는 플러그인을 종료한다.
    /// </summary>
    private void OnApplicationQuit()
    {
        if (null != write_1)
        {
            write_1.Close();
        }
        gazeClient.StopStreamImage();
        gazeClient.TerminateGazeTracker();
    }

    /// <summary>
    /// 저장할 수치를 계산해 놓는다.
    /// </summary>
    public void Calc()
    {
        //Saves the position of drone in current frame

        dronePos = DataManager.Instance.drone.position;

        //Calculates the velocity of drone by using the position of current frame and position of one-frame-before

        dronePosVel = DataManager.Instance.VelocityCalculator(dronePos, DataManager.Instance.LastDronePos);

        //Calculates the acceleration of drone by using the velocity calculated in current frame and velocity of one-frame-before

        dronePosAccel = DataManager.Instance.AccelCalculator(dronePosVel, DataManager.Instance.LastDronePosVel);

        //Uses same way
        DataManager.Instance.Frame++;

        m_seq_Name = DataManager.Instance.Seq_Name;
        m_seq_Status = DataManager.Instance.Seq_Status;

        m_teleScan = DataManager.Instance.Key_TeleScan;
        DataManager.Instance.TeleScan_Init();

        droneRot = new Vector3(DataManager.Instance.droneXZ.eulerAngles.x, DataManager.Instance.drone.eulerAngles.y, DataManager.Instance.droneXZ.eulerAngles.z);
        droneRotVel = DataManager.Instance.VelocityCalculator(droneRot, DataManager.Instance.LastDroneRot);
        droneRotAccel = DataManager.Instance.AccelCalculator(droneRotVel, DataManager.Instance.LastDroneRotVel);
        camPos = DataManager.Instance.cam.localPosition;
        camPosVel = DataManager.Instance.VelocityCalculator(camPos, DataManager.Instance.LastCamPos);
        camPosAccel = DataManager.Instance.AccelCalculator(camPosVel, DataManager.Instance.LastCamPosVel);
        camRot = DataManager.Instance.cam.localEulerAngles;
        camRotVel = DataManager.Instance.VelocityCalculator(camRot, DataManager.Instance.LastCamRot);
        camRotAccel = DataManager.Instance.AccelCalculator(camRotVel, DataManager.Instance.LastCamRotVel);


        DataManager.Instance.FPS = (1f / Time.unscaledDeltaTime);

        //accum += Time.timeScale / Time.deltaTime;
        //DataManager.Instance.FPS = accum / DataManager.Instance.Frame;

        stopState = DataManager.Instance.Stopstate1;

        if (!stopState)
        {
            Xbox_fw = DataManager.Instance.Xbox_fw1;
            Xbox_left = DataManager.Instance.Xbox_left1;
            Xbox_right = DataManager.Instance.Xbox_right1;
            DataManager.Instance.Xbox_Init();
        }
        else if (stopState)
        {
            Xbox_fw = DataManager.Instance.Xbox_fw1;
            Xbox_left = DataManager.Instance.Xbox_left1;
            Xbox_right = DataManager.Instance.Xbox_right1;
            DataManager.Instance.Xbox_Stop();
        }
    }

    /// <summary>
    /// 엑셀파일에 계산한 수치를 기록한다.
    /// </summary>
    public void Data_Write()
    {
        if (null != write_1)
        {
            if (!stopState)
            {
                write_1.WriteLine(DataManager.Instance.Frame.ToString(DataManager.Instance.Digit_6number) + ","
                                  + DataManager.Instance.Day() + ","
                                  + DataManager.Instance.NowTime() + ","
                                  + m_seq_Name.ToString() + ","
                                  + m_seq_Status.ToString() + ","
                                  + m_teleScan + ","
                                  + DataManager.Instance.CalString(camPos) + ","
                                  + DataManager.Instance.CalString(camPosVel) + ","
                                  + DataManager.Instance.CalString(camPosAccel) + ","
                                  + DataManager.Instance.CalString(camRot) + ","
                                  + DataManager.Instance.CalString(camRotVel) + ","
                                  + DataManager.Instance.CalString(camRotAccel) + ","
                                  + DataManager.Instance.CalString(dronePos) + ","
                                  + DataManager.Instance.CalString(dronePosVel) + ","
                                  + DataManager.Instance.CalString(dronePosAccel) + ","
                                  + DataManager.Instance.CalString(droneRot) + ","
                                  + DataManager.Instance.CalString(droneRotVel) + ","
                                  + DataManager.Instance.CalString(droneRotAccel) + ","


                                  + Xbox_fw + ","
                                  + Xbox_left + ","
                                  + Xbox_right + ","
                                  + m_eye_x + ","
                                  + m_eye_y + ","
                                  + m_found.ToString() + ","
                                  + m_pupil_x + ","
                                  + m_pupil_y + ","
                                  + m_size_a + ","
                                  + m_size_b + ","
                                  + m_angle_degree + ","
                                  + DataManager.Instance.FPS.ToString(DataManager.Instance.Digit_float_number));

                                  depthCapture.count = DataManager.Instance.Frame;
                                  depthCapture2.count = DataManager.Instance.Frame;
                                  depthCapture.Captuere();
                                  depthCapture2.Captuere();

                                  SaveEyeImage(DataManager.Instance.Frame);
    
            }
            else
            {
                write_1.WriteLine(DataManager.Instance.Frame.ToString(DataManager.Instance.Digit_6number) + ","
                                  + DataManager.Instance.Day() + ","
                                  + DataManager.Instance.NowTime() + ","
                                  + m_seq_Name.ToString() + ","
                                  + m_seq_Status.ToString() + ","
                                  + m_teleScan + ","
                                                                                                            + "-,-,-,"
                                                                                                            + "-,-,-,"
                                                                                                            + "-,-,-,"
                                                                                                            + "-,-,-,"
                                                                                                            + "-,-,-,"
                                                                                                            + "-,-,-,"
                                                                                                            + "-,-,-,"
                                                                                                            + "-,-,-,"
                                                                                                            + "-,-,-,"
                                                                                                            + "-,-,-,"
                                                                                                            + "-,-,-,"
                                                                                                            + "-,-,-,"
                                                                                                            

                                                                                                            + "-,"
                                                                                                            + "-,"
                                                                                                            + "-,"
                                  + m_eye_x + ","
                                  + m_eye_y + ","
                                  + m_found.ToString() + ","
                                  + m_pupil_x + ","
                                  + m_pupil_y + ","
                                  + m_size_a + ","
                                  + m_size_b + ","
                                  + m_angle_degree + ","
                                  + DataManager.Instance.FPS.ToString(DataManager.Instance.Digit_float_number));

                                  SaveEyeImage(DataManager.Instance.Frame);
            }

            if ("d" == m_teleScan)
            {
                write_1.Close();
                write_1 = null;
            }
        }
    }

    /// <summary>
    /// 기록이 끝난 후 값을 현재 값으로 바꾼다.
    /// </summary>
    public void AfterCalc()
    {
        //Before moving on to next frame, updates the 'one_frame_before' data to use them at next frame's calculation
        DataManager.Instance.LastDronePos = dronePos;
        DataManager.Instance.LastDroneRot = droneRot;

        DataManager.Instance.LastDronePosVel = dronePosVel;
        DataManager.Instance.LastDroneRotVel = droneRotVel;

        DataManager.Instance.LastCamPos = camPos;
        DataManager.Instance.LastCamRot = camRot;

        DataManager.Instance.LastCamPosVel = camPosVel;
        DataManager.Instance.LastCamRotVel = camRotVel;
    }

    /// <summary>
    /// 정상 종료 됬을 경우에는 엑셀만 끈다.
    /// </summary>
    public void Close()
    {
        if (null != write_1)
        {
            write_1.Close();
            write_1 = null;
        }
    }
}


