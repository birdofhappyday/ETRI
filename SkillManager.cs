
using UnityEngine;
using System.Collections.Generic;

public class SkillManager
{
    protected Monster owner { get; private set; }

    private Skill[] m_skills;
    private List<Skill> m_currentSkills;

    public void Init(Monster m)
    {
        if (m == null)
            Debug.LogError("SkillManager.Init :<FAILED> Invalid Owner");

        owner = m;

        m_skills = owner.skills;

        foreach (var it in m_skills)
            it.Init(owner);
    }

    public Skill GetSkill(int skid)
    {
        for (int i = 0; i < m_skills.Length; i++)
        {
            if (m_skills[i].SkillID == skid)
            {
                return m_skills[i];
            }
        }

        return null;
    }

    public Skill GetEnableSkillNumber()
    {
        m_currentSkills = new List<Skill>();

        foreach (var it in owner.skills)
        {
            if (!it.IsActive())
                continue;

            if (it.IsCooltime())
                continue;

            m_currentSkills.Add(it);
        }

        if (0 == m_currentSkills.Count)
            return null;

        return m_currentSkills[Random.Range(0, m_currentSkills.Count)];
    }

    public void Update()
    {
        foreach (var it in owner.skills)
            it.Update(AILogic.GetHpPercentage(owner));
    }
}
