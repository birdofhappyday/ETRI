using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviour
{
    public bool m_trigger;
    public WidthButton m_width;
    public Image m_image;

    private float m_flickerTime = 0.4f;
    private float m_time;
    private bool m_currentSelect;
    private Color m_flickerColor;
    private Color m_baseColor;
    private Color m_currentColor = Color.green;
    private Color m_selectedColor = Color.red;

    public bool CurrentSelectBool
    {
        get { return m_currentSelect; }
        set { m_currentSelect = value; }
    }

    public virtual void PassButton()
    {
        m_currentSelect = false;
        m_time = 0.0f;
    }

    public void SetColor(Color m_color)
    {
        m_image.color = m_color;
    }

    /// <summary>
    /// 현재 선택되어 있는 버튼
    /// </summary>
    public void CurrentSelect()
    {
        m_currentSelect = false;
        if (!m_trigger)
        {
            NormalButton temp_NormalButton = GetComponent<NormalButton>();
            if (temp_NormalButton.Select)
            {
                m_flickerColor = Color.red;
            }
            else
            {
                m_flickerColor = m_baseColor;
            }
        }
        else
        {
            m_flickerColor = m_baseColor;
        }
        m_currentSelect = true;
    }

    /// <summary>
    /// 버튼 초기화.
    /// </summary>
    public virtual void Init()
    {
        m_baseColor = m_image.color;
    }

    public virtual void BaseState()
    {
        m_image.color = m_baseColor;
    }

    public void AfterInit()
    {
        m_image.color = m_baseColor;
    }

    /// <summary>
    /// 버튼 선택시 행동.
    /// </summary>
    public virtual void ButtonSelect()
    {
        m_image.color = m_selectedColor;
    }

    public void Update()
    {
        if(m_currentSelect)
        {

            m_time += Time.deltaTime;

            //0.4f
            if (m_time < m_flickerTime)
            {
                m_image.color = m_currentColor;
            }

            else if (m_time > m_flickerTime)
            { 
                m_image.color = m_flickerColor;

                if(m_time > m_flickerTime * 2)
                {
                    m_time = 0.0f;
                }
            }

        }
    }
}