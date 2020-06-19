
using UnityEngine;
using XInputDotNetPure;
using System;

public enum GamePadType
{
    Once,
    Continuosly,
}

public class GamePadManager : MonoSingleTon<GamePadManager>
{
    public GamePadType m_DPadType = GamePadType.Continuosly;
    public GamePadType m_TriggerType = GamePadType.Continuosly;
    public GamePadType m_TumpStick = GamePadType.Continuosly;

    private bool iseActive = false;
    private bool playerIndexSet = false;
    private GamePadState state;
    private GamePadState prevState;
    private PlayerIndex playerIndex;

    private Action RightTrigger = null;
    private Action RightUnTrigger = null;
    private Action LeftTrigger = null;
    private Action LeftUnTrigger = null;
    private Action DpadUp = null;
    private Action DpadUnUp = null;
    private Action DpadDown = null;
    private Action DpadUnDown = null;
    private Action DpadRight = null;
    private Action DpadUnRight = null;
    private Action DpadLeft = null;
    private Action DpadUnLeft = null;
    private Action RightTump = null;
    private Action RightUnTump = null;
    private Action LeftTump = null;
    private Action LeftUnTump = null;

    private bool isTriggerRight = false;
    private bool isTriggerLeft = false;
    private bool isDpadUp = false;
    private bool isDpadDown = false;
    private bool isDpadRight = false;
    private bool isDpadLeft = false;
    private bool isRightTumpStick = false;
    private bool isLeftTumpStick = false;

    public bool IseActive { get { return iseActive; } set { iseActive = value; } }
    public int RightTumpState
    {
        get
        { if (state.ThumbSticks.Right.Y > 0)
                return 1;
            else if (state.ThumbSticks.Right.Y < 0)
                return -1;
            else
                return 0;
        }
    }

    private GamePadManager() { }

    private void Update()
    {
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);
        #region XBPX Controller update
        if (prevState.IsConnected)
        {
            if (iseActive)
            {
                #region Trigger

                if (GamePadType.Continuosly == m_TriggerType)
                {
                    if (1 == state.Triggers.Right)
                    {
                        if (null != RightTrigger)
                            RightTrigger();
                    }
                    else
                    {
                        if (null != RightUnTrigger)
                            RightUnTrigger();
                    }

                    if (1 == state.Triggers.Left)
                    {
                        if (null != LeftTrigger)
                            LeftTrigger();
                    }
                    else
                    {
                        if (null != LeftUnTrigger)
                            LeftUnTrigger();
                    }
                }

                else
                {
                    if (1 == state.Triggers.Right)
                    {
                        if (!isTriggerRight)
                        {
                            if (null != RightTrigger)
                                RightTrigger();

                            isTriggerRight = true;
                        }
                    }
                    else
                    {
                        if (isTriggerRight)
                        {
                            if (null != RightUnTrigger)
                                RightUnTrigger();

                            isTriggerRight = false;
                        }
                    }

                    if (1 == state.Triggers.Left)
                    {
                        if (!isTriggerLeft)
                        {
                            if (null != LeftTrigger)
                                LeftTrigger();

                            isTriggerLeft = true;
                        }
                    }
                    else
                    {
                        if (isTriggerLeft)
                        {
                            if (null != LeftUnTrigger)
                                LeftUnTrigger();

                            isTriggerLeft = false;
                        }
                    }
                }

                #endregion

                #region Dpad

                if (GamePadType.Once == m_DPadType)
                {
                    if (ButtonState.Pressed == state.DPad.Up)
                    {
                        if (!isDpadUp)
                        {
                            if (null != DpadUp)
                                DpadUp();

                            isDpadUp = true;
                        }
                    }
                    else
                    {
                        if (isDpadUp)
                        {
                            if (null != DpadUnUp)
                                DpadUnUp();

                            isDpadUp = false;
                        }
                    }

                    if (ButtonState.Pressed == state.DPad.Down)
                    {
                        if (!isDpadDown)
                        {
                            if (null != DpadDown)
                                DpadDown();

                            isDpadDown = true;
                        }
                    }
                    else
                    {
                        if (isDpadDown)
                        {
                            if (null != DpadUnDown)
                                DpadUnDown();

                            isDpadDown = false;
                        }
                    }

                    if (ButtonState.Pressed == state.DPad.Right)
                    {
                        if (!isDpadRight)
                        {
                            if (null != DpadRight)
                                DpadRight();

                            isDpadRight = true;
                        }
                    }
                    else
                    {
                        if (isDpadRight)
                        {
                            if (null != DpadUnRight)
                                DpadUnRight();

                            isDpadRight = false;
                        }
                    }

                    if (ButtonState.Pressed == state.DPad.Left)
                    {
                        if (!isDpadLeft)
                        {
                            if (null != DpadLeft)
                                DpadLeft();

                            isDpadLeft = true;
                        }
                    }
                    else
                    {
                        if (isDpadLeft)
                        {
                            if (null != DpadUnLeft)
                                DpadUnLeft();

                            isDpadLeft = false;
                        }
                    }
                }

                else
                {
                    if (ButtonState.Pressed == state.DPad.Up)
                    {
                        if (null != DpadUp)
                            DpadUp();
                    }
                    else
                    {
                        if (null != DpadUnUp)
                            DpadUnUp();
                    }

                    if (ButtonState.Pressed == state.DPad.Down)
                    {
                        if (null != DpadDown)
                            DpadDown();
                    }
                    else
                    {
                        if (null != DpadUnDown)
                            DpadUnDown();
                    }

                    if (ButtonState.Pressed == state.DPad.Right)
                    {
                        if (null != DpadRight)
                            DpadRight();
                    }
                    else
                    {
                        if (null != DpadUnRight)
                            DpadUnRight();
                    }

                    if (ButtonState.Pressed == state.DPad.Left)
                    {
                        if (null != DpadLeft)
                            DpadLeft();
                    }
                    else
                    {
                        if (null != DpadUnLeft)
                            DpadUnLeft();
                    }
                }

                #endregion

                #region TumpStick

                if (GamePadType.Continuosly == m_TumpStick)
                {
                    if (0 != state.ThumbSticks.Right.X || 0 != state.ThumbSticks.Right.Y)
                    {
                        if (null != RightTump)
                            RightTump();
                    }

                    else
                    {
                        if (null != RightUnTump)
                            RightUnTump();
                    }

                    if (0 != state.ThumbSticks.Left.X || 0 != state.ThumbSticks.Left.Y)
                    {
                        if (null != LeftTump)
                            LeftTump();
                    }

                    else
                    {
                        if (null != LeftUnTump)
                            LeftUnTump();
                    }
                }

                else
                {
                    if (0 != state.ThumbSticks.Right.X || 0 != state.ThumbSticks.Right.Y)
                    {
                        if (!isRightTumpStick)
                        {
                            if (null != RightTump)
                                RightTump();

                            isRightTumpStick = true;
                        }
                    }

                    else
                    {
                        if (isRightTumpStick)
                        {
                            if (null != RightUnTump)
                                RightUnTump();

                            isRightTumpStick = false;
                        }
                    }

                    if (0 != state.ThumbSticks.Left.X || 0 != state.ThumbSticks.Left.Y)
                    {
                        if (!isLeftTumpStick)
                        {
                            if (null != LeftTump)
                                LeftTump();

                            isLeftTumpStick = true;
                        }
                    }

                    else
                    {
                        if (isLeftTumpStick)
                        {
                            if (null != LeftUnTump)
                                LeftUnTump();

                            isLeftTumpStick = false;
                        }
                    }
                }

                #endregion
            }
        }

        #endregion
    }

    #region XBOX Controller Action Setting

    public void InitAllXboxpad()
    {
        IseActive = false;

        InitRightTriggerClicked();
        InitRightTriggerUnClicked();
        InitLeftTriggerClicked();
        InitLeftTriggerUnClicked();
        InitDapUp();
        InitDpadUnUp();
        InitDpadDown();
        InitDpadUnDown();
        InitDpadRight();
        InitDpadUnRight();
        InitDpadLeft();
        InitDpadUnLeft();
        InitRightTump();
        InitRightUnTump();
        InitLeftTump();
        InitLeftunTump();
    }

    public void SetRightTriggerClicked(Action function)
    {
        RightTrigger += function;
    }

    public void SetRightTriggerClickedOff(Action function)
    {
        RightTrigger -= function;
    }

    public void InitRightTriggerClicked()
    {
        RightTrigger = null;
    }

    public void SetRightTriggerUnClicked(Action function)
    {
        RightUnTrigger += function;
    }

    public void SetRightTriggerUnClickedOff(Action function)
    {
        RightUnTrigger -= function;
    }

    public void InitRightTriggerUnClicked()
    {
        RightUnTrigger = null;
    }

    public void SetLeftTriggerClicked(Action function)
    {
        LeftTrigger += function;
    }

    public void SetLeftTriggerClickedOff(Action function)
    {
        LeftTrigger -= function;
    }

    public void InitLeftTriggerClicked()
    {
        LeftTrigger = null;
    }

    public void SetLeftTriggerUnClicked(Action function)
    {
        LeftUnTrigger += function;
    }

    public void SetLeftTriggerUnClickedOff(Action function)
    {
        LeftUnTrigger -= function;
    }

    public void InitLeftTriggerUnClicked()
    {
        LeftUnTrigger = null;
    }

    public void SetDpadUp(Action function)
    {
        DpadUp += function;
    }

    public void SetDpadUpOff(Action function)
    {
        DpadUp -= function;
    }

    public void InitDapUp()
    {
        DpadUp = null;
    }

    public void SetDpadUnUp(Action function)
    {
        DpadUnUp += function;
    }

    public void SetDpadUnUpOff(Action function)
    {
        DpadUnUp -= function;
    }

    public void InitDpadUnUp()
    {
        DpadUnUp = null;
    }

    public void SetDpadDown(Action function)
    {
        DpadDown += function;
    }

    public void SetDpadDownOff(Action function)
    {
        DpadDown -= function;
    }

    public void InitDpadDown()
    {
        DpadDown = null;
    }

    public void SetDpadUnDown(Action function)
    {
        DpadUnDown += function;
    }

    public void SetDpadUnDownOff(Action function)
    {
        DpadUnDown -= function;
    }

    public void InitDpadUnDown()
    {
        DpadUnDown = null;
    }

    public void SetDpadRight(Action function)
    {
        DpadRight += function;
    }

    public void SetDpadRightOff(Action function)
    {
        DpadRight -= function;
    }

    public void InitDpadRight()
    {
        DpadRight = null;
    }

    public void SetDpadUnRight(Action function)
    {
        DpadUnRight += function;
    }

    public void SetDpadUnRightOff(Action function)
    {
        DpadUnRight -= function;
    }

    public void InitDpadUnRight()
    {
        DpadUnRight = null;
    }

    public void SetDpadLeft(Action function)
    {
        DpadLeft += function;
    }

    public void SetDpadLeftOff(Action function)
    {
        DpadLeft -= function;
    }

    public void InitDpadLeft()
    {
        DpadLeft = null;
    }

    public void SetDpadUnLeft(Action function)
    {
        DpadUnLeft += function;
    }

    public void SetDpadUnLeftOff(Action function)
    {
        DpadUnLeft -= function;
    }

    public void InitDpadUnLeft()
    {
        DpadUnLeft = null;
    }

    public void SetRightTump(Action function)
    {
        RightTump += function;
    }

    public void SetRightTumpOff(Action function)
    {
        RightTump -= function;
    }

    public void InitRightTump()
    {
        RightTump = null;
    }

    public void SetRightUnTump(Action function)
    {
        RightUnTump += function;
    }

    public void SetRightUnTumpOff(Action function)
    {
        RightUnTump -= function;
    }

    public void InitRightUnTump()
    {
        RightUnTump = null;
    }

    public void SetLeftTump(Action function)
    {
        LeftTump += function;
    }

    public void SetLeftTumpOff(Action function)
    {
        LeftTump -= function;
    }

    public void InitLeftTump()
    {
        LeftTump = null;
    }

    public void SetLeftUnTump(Action function)
    {
        LeftUnTump += function;
    }

    public void SetLeftUnTumpOff(Action function)
    {
        LeftUnTump -= function;
    }

    public void InitLeftunTump()
    {
        LeftUnTump = null;
    }

    #endregion
}
