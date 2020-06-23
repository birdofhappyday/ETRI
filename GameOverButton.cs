using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverButton : BaseButton
{
    public SSQsurvey m_ssqSurvey;

    public override void ButtonSelect()
    {
        base.PassButton();
        base.m_width.m_survey.AfterInit();
        base.m_width.m_survey.SSQ_SelectedNumber();
        base.m_width.m_baseButton.Remove(this);
        TriggerManager.Instance.list_Num++;
        TriggerManager.Instance.contents_Num++;
        TriggerManager.Instance.Action();

        SurveyManager.Instance.SSQEnd();
        m_ssqSurvey.GameOver();

        //if (base.m_width.m_survey.AllSelectCheck())
        //{
        //    base.m_width.m_survey.gameObject.SetActive(false);
        //}
    }

    public override void PassButton()
    {
        base.PassButton();
        base.BaseState();
    }
}
