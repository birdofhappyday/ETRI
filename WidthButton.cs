using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidthButton : MonoBehaviour
{
    public Survey m_survey;
    public List<BaseButton> m_baseButton;

    private bool m_select;
    private bool m_widthActive;
    private int m_selectNumber;

    public bool Select
    {
        get { return m_select; }
        set { m_select = value; }
    }

    public bool WidthActive
    {
        get { return m_widthActive; }
        set { m_widthActive = value; }
    }

    public int SelectNumber
    {
        get { return m_selectNumber; }
    }


    public void Init()
    {
        for (int i = 0; i < m_baseButton.Count; ++i)
        {
            m_baseButton[i].Init();
            m_select = false;
        }
        m_widthActive = false;
    }

    public void ButtonSelect(int num)
    {
        if (!m_baseButton[num].m_trigger)
        {
            if (!m_select)
            {
                m_baseButton[num].ButtonSelect();
                m_selectNumber = num;
            }
            else
            {
                if (num != m_selectNumber)
                {
                    m_baseButton[m_selectNumber].BaseState();
                    m_baseButton[num].ButtonSelect();
                    m_selectNumber = num;
                }

                else
                {
                    m_baseButton[num].ButtonSelect();
                }
            }
        }
        else
        {
            m_baseButton[num].ButtonSelect();
        }
    }

    public void BaseState()
    {
        m_baseButton[0].CurrentSelect();
        m_widthActive = true;
    }

    public void AffterInit()
    {
        for (int i = 0; i < m_baseButton.Count; ++i)
        {
            m_baseButton[i].AfterInit();
        }
        m_widthActive = false;
    }
}
