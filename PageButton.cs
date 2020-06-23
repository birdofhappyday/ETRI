using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageButton : BaseButton
{
    public SSQsurvey m_base;

    public override void ButtonSelect()
    {
        if (base.m_width.m_survey.AllSelectCheck())
        {
            base.PassButton();
            base.m_width.m_survey.AfterInit();
            base.m_width.m_survey.SSQ_SelectedNumber();
            base.m_width.m_baseButton.Remove(this);
            m_base.NextPage();
        }
    }

    public override void PassButton()
    {
        base.PassButton();
        base.BaseState();
    }
}
