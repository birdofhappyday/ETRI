
using System;
using UnityEngine;

[Serializable]
public class Skill
{
    [SerializeField] private string _skillName;
    [SerializeField] private int    _skillID = 0;
    [SerializeField] private float  _coolTime = 1;
    [SerializeField] private float  _skillRange = 1;
    [SerializeField] private float  _activeHp_PctMin = 0;
    [SerializeField] private float  _activeHp_PctMax = 100;

    private bool    bActive;
    private float   currCoolTime;

    #region property

    public int      SkillID             { get { return _skillID; } }
    public string   SkillName           { get { return _skillName; } }
    public float    SkillRange          { get { return _skillRange; } }

    #endregion

    #region GetFunc

    public bool IsActive() { return bActive; }
    public bool IsCooltime() { return currCoolTime < _coolTime; }
    public float Cooltime() { return _coolTime; }

    #endregion

    public void Init(Monster owner)
    {
        bActive = false;
        currCoolTime = _coolTime;
    }

    public void Reset(Monster owner)
    {
        currCoolTime = 0f;
    }

    public void Update(float ownerHp_Pct)
    {
        bActive = (ownerHp_Pct > _activeHp_PctMin && ownerHp_Pct <= _activeHp_PctMax);
        if (!bActive)
            return;

        currCoolTime += Time.deltaTime;
    }
}
