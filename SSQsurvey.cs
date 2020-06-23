using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSQsurvey : MonoBehaviour
{
    public Survey[] m_survey;

    private int m_currentPage;

    public void NextPage()
    {
        m_survey[m_currentPage].gameObject.SetActive(false);
        m_currentPage++;
        m_survey[m_currentPage].Init();
        m_survey[m_currentPage].gameObject.SetActive(true);
    }

    public void SSQStart()
    {
        m_currentPage = 0;
        m_survey[m_currentPage].Init();
        m_survey[m_currentPage].gameObject.SetActive(true);
    }

    public void GameOver()
    {
        m_survey[m_currentPage].gameObject.SetActive(false);
        
        //
        //Image
        //

    }
}
