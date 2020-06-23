using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextContentButton : BaseButton
{
    public override void ButtonSelect()
    {
        if (base.m_width.m_survey.AllSelectCheck())
        {
            base.PassButton();
            base.m_width.m_survey.AfterInit();
            base.m_width.m_survey.Score_SelectedNumber();
            base.m_width.m_baseButton.Remove(this);
            base.m_width.m_survey.m_pass = false;


            ////성민조건
            ////삭제 예정
            if (SurveyManager.Instance.trainingBuild && TriggerManager.Instance.contents_Num == 6)
            {
                SurveyManager.Instance.SurveyEnd();
                SurveyManager.Instance.SSQStart();
                SurveyManager.Instance.key_Active = true;
                return;
            }

            if (TriggerManager.Instance.contents_Num == 18 && TriggerManager.Instance.contents_Num != 0)
            {
                SurveyManager.Instance.SurveyEnd();
                SurveyManager.Instance.SSQStart();
                SurveyManager.Instance.key_Active = true;
            }
            else if (TriggerManager.Instance.contents_Num == 45 && TriggerManager.Instance.contents_Num != 0)
            {
                SurveyManager.Instance.SurveyEnd();
                SurveyManager.Instance.SSQStart();
                SurveyManager.Instance.key_Active = true;
            }
            else if (TriggerManager.Instance.contents_Num == 54 && TriggerManager.Instance.contents_Num != 0)
            {
                SurveyManager.Instance.SurveyEnd();
                SurveyManager.Instance.SSQStart();
                SurveyManager.Instance.key_Active = true;
            }
            else
            {
                SurveyManager.Instance.SurveyEnd();

                TriggerManager.Instance.list_Num++;
                TriggerManager.Instance.contents_Num++;
                TriggerManager.Instance.Action();

                //    //SurveyManager.Instance.SurveyEnd();
            }
        }
    }

    public override void PassButton()
    {
        base.PassButton();
        base.BaseState();
    }
}
