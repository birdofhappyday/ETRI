using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalButton : BaseButton
{
    private bool m_select;

    public bool Select
    {
        get { return m_select; }
        set { m_select = value; }
    }
    
    public override void Init()
    {
        base.Init();
        m_select = false;
    }

    public override void BaseState()
    {
        base.BaseState();
        m_select = false;
    }

    public override void ButtonSelect()
    {
        if(!m_select)
        {
            m_width.Select = true;
            m_select = true;
            base.ButtonSelect();
            base.CurrentSelect();
        }
        else
        {
            m_width.Select = false;
            m_select = false;
            base.CurrentSelect();
        }
    }

    public override void PassButton()
    {
        base.PassButton();

        if (!m_select)
        {
            base.BaseState();
        }
        else
        {
            base.ButtonSelect();
        }
    }
}
