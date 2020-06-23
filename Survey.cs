using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Survey : MonoBehaviour
{
    public List<WidthButton> m_width;
    public BaseButton m_nextButton;
    public bool m_pass = false;

    private PlayerIndex playerIndex;
    private GamePadState state;
    private GamePadState prevState;
    private bool playerIndexSet = false;
    private int m_currentSelect;
    private int m_currentWidth;
    private bool m_X_isAxisInUse;
    private bool m_Y_isAxisInUse;
    private bool m_Fire_InUse;
    private bool m_activeNextButton;

    private void Update()
    {
        if (!SurveyManager.Instance.key_Active)
            return;

        if (m_pass)
        {
            m_width[m_currentWidth].ButtonSelect(m_currentSelect);
            FireSelectButton();
        }


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

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (!m_X_isAxisInUse)
            {
                if (Input.GetAxisRaw("Horizontal") > +0.9)
                {
                    NextButton();
                }
                else if (Input.GetAxisRaw("Horizontal") < -0.9)
                {
                    PrievButton();
                }
                m_X_isAxisInUse = true;
            }
        }
        else
        {
            m_X_isAxisInUse = false;
        }
        //----------------------------------------
        if (Input.GetAxisRaw("Forward") != 0)
        {
            if (!m_Y_isAxisInUse)
            {
                if (Input.GetAxisRaw("Forward") > +0.9)
                {
                    PrievWidth();
                }
                else if (Input.GetAxisRaw("Forward") < -0.9)
                {
                    NextWidth();
                }
                m_Y_isAxisInUse = true;
            }
        }
        else
        {
            m_Y_isAxisInUse = false;
        }
        //------------------------------------

        if (state.Triggers.Right == 1)
        {
            if (!m_Fire_InUse)
            {
                m_width[m_currentWidth].ButtonSelect(m_currentSelect);
                FireSelectButton();
                Input.ResetInputAxes();
            }
            m_Fire_InUse = true;
        }
        else
        {
            m_Fire_InUse = false;
        }

        //////------------------------keyBoard
        if (Input.GetKeyDown(KeyCode.A))
        {
            PrievButton();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            NextButton();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            PrievWidth();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            NextWidth();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            m_width[m_currentWidth].ButtonSelect(m_currentSelect);
            FireSelectButton();
        }
    }

    public void Init()
    {
        if (SurveyManager.Instance.pass)
        {
            m_pass = true;
        }

        m_currentSelect = 0;
        m_currentWidth = 0;
        m_Fire_InUse = true;
        m_X_isAxisInUse = false;
        m_Y_isAxisInUse = false;
        m_activeNextButton = false;
        for (int i = 0; i < m_width.Count; ++i)
        {
            m_width[i].Init();
        }
        m_nextButton.Init();
        m_width[0].BaseState();
    }

    public void AfterInit()
    {
        for (int i = 0; i < m_width.Count; ++i)
        {
            m_width[i].AffterInit();
        }
    }

    /// <summary>
    /// 모든줄에 체크가 되었는지 확인한다.
    /// </summary>
    /// <returns></returns>
    public bool AllSelectCheck()
    {
        for (int i = 0; i < m_width.Count; ++i)
        {
            if (!m_width[i].Select) return false;
        }

        return true;
    }

    /// <summary>
    /// 마지막 m_width에 m_nextButton추가.
    /// </summary>
    public void ActiveNextButton()
    {
        m_width[m_width.Count - 1].m_baseButton.Add(m_nextButton);
        m_nextButton.m_width = m_width[m_width.Count - 1];
        m_activeNextButton = true;
    }

    /// <summary>
    /// 파이어버튼 눌렀을때 반응.
    /// </summary>
    public void FireSelectButton()
    {
        if (!m_activeNextButton)
        {
            if (AllSelectCheck())
            {
                ActiveNextButton();
                m_width[m_currentWidth].m_baseButton[m_currentSelect].PassButton();
                m_currentSelect = m_width[m_width.Count - 1].m_baseButton.Count - 1;
                m_currentWidth = m_width.Count - 1;
                m_width[m_currentWidth].m_baseButton[m_currentSelect].CurrentSelect();
                return;
            }

            else
            {
                if (m_width.Count != m_currentWidth + 1)
                {
                    if (!m_width[m_currentWidth + 1].WidthActive)
                    {
                        m_width[m_currentWidth].m_baseButton[m_currentSelect].PassButton();
                        m_currentWidth++;
                        m_width[m_currentWidth].BaseState();
                        m_currentSelect = 0;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 다음버튼을 가르킨다.
    /// </summary>
    public void NextButton()
    {
        if (m_width[m_currentWidth].m_baseButton.Count - 1 != m_currentSelect)
        {
            m_width[m_currentWidth].m_baseButton[m_currentSelect].PassButton();
            m_currentSelect++;
            m_width[m_currentWidth].m_baseButton[m_currentSelect].CurrentSelect();
        }
        else
        {
            m_width[m_currentWidth].m_baseButton[m_currentSelect].PassButton();
            m_currentSelect = 0;
            m_width[m_currentWidth].m_baseButton[m_currentSelect].CurrentSelect();
        }
    }

    /// <summary>
    /// 이전버튼을 가르킨다.
    /// </summary>
    public void PrievButton()
    {
        if (0 != m_currentSelect)
        {
            m_width[m_currentWidth].m_baseButton[m_currentSelect].PassButton();
            m_currentSelect--;
            m_width[m_currentWidth].m_baseButton[m_currentSelect].CurrentSelect();
        }
        else
        {
            m_width[m_currentWidth].m_baseButton[m_currentSelect].PassButton();
            m_currentSelect = m_width[m_currentWidth].m_baseButton.Count - 1;
            m_width[m_currentWidth].m_baseButton[m_currentSelect].CurrentSelect();
        }
    }

    /// <summary>
    /// 다음줄을 가르킨다.
    /// </summary>
    public void NextWidth()
    {
        if (m_width.Count > m_currentWidth + 1)
        {
            if (!m_width[m_currentWidth + 1].WidthActive)
            {
                return;
            }
            else
            {
                m_width[m_currentWidth].m_baseButton[m_currentSelect].PassButton();
                m_currentWidth++;
                m_width[m_currentWidth].m_baseButton[m_currentSelect].CurrentSelect();
            }
        }
    }

    /// <summary>
    /// 이전줄을 가르킨다.
    /// 맨 마지막에 위로 올라올때 m_nextButton에서 올라올수 있기에 예외처리.
    /// </summary>
    public void PrievWidth()
    {
        if (0 == m_currentWidth)
        {
            return;
        }
        else
        {
            m_width[m_currentWidth].m_baseButton[m_currentSelect].PassButton();
            m_currentWidth--;
            if (m_width[m_currentWidth].m_baseButton.Count - 1 < m_currentSelect)
            {
                m_currentSelect -= 1;
            }
            m_width[m_currentWidth].m_baseButton[m_currentSelect].CurrentSelect();
        }
    }

    public void Score_SelectedNumber()
    {
        SurveyManager.Instance.Score(m_width[0].SelectNumber);
    }

    public void SSQ_SelectedNumber()
    {
        SurveyManager.Instance.SSQScore(m_width[0].SelectNumber, m_width[1].SelectNumber, m_width[2].SelectNumber, m_width[3].SelectNumber);
    }
}
