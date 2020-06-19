
using System;
using System.Collections.Generic;
using UnityEngine;

public class FlightBoss : Monster
{
    public float m_attacktime = 10f;
    public Transform[] m_weapon;
    public string[] m_Missile;

    private Skill CurSkill;
    private Vector3 SkillStartPos;
    private List<string> Skillset;
    private CoroutineCommand m_attackCommand;
    private CoroutineCommand m_timeAttackCommand;
    private Vector3 m_attackFoward;

    private int attackEmptyNum;

    #region 이동변수

    public int m_directionSize;
    public int m_directionLimit;

    private List<int> m_direction;
    private Vector3 m_destination;
    private int m_randomRangeY;
    private int m_randomRangeZ;
    private bool m_middle;
    private bool m_randomJudge;
    private bool m_move;

    #endregion

    #region Statue 함수

    public override void Attack()
    {
        if (0 == Skillset.Count)
        {
            for (int i = 0; i < m_Missile.Length; ++i)
            {
                Skillset.Add(m_Missile[i]);
            }
        }

        if (null != m_timeAttackCommand)
            CoroutineManager.Instance.Unregister(m_timeAttackCommand);

        if (null != CurrentTarget)
            m_timeAttackCommand = CoroutineManager.Instance.Register(TimeAttack());
    }

    public override void Dead()
    {
        if (null != m_timeAttackCommand)
            CoroutineManager.Instance.Unregister(m_timeAttackCommand);
        if (null != m_attackCommand)
            CoroutineManager.Instance.Unregister(m_timeAttackCommand);

        UIManager.Instance.Open("UIVictory");

        OnRestore();
    }

    public override void Idle()
    {
        if (null != CurrentTarget)
            RotateUpdate(this.transform, CurrentTarget.position, MoveSpeedTimeDelta * 50f);
    }

    public override void Move()
    {
        if (m_move)
        {
            m_destination = this.transform.position;
            m_destination.x += 100;

            if (!m_middle)
            {
                RandomDirection();
                m_destination.y += m_randomRangeY;
                m_destination.z += m_randomRangeZ;
            }
            else
            {
                m_destination.y = 0.1f;
                m_destination.z = 0.1f;
            }

            this.transform.position = Vector3.MoveTowards(this.transform.position, m_destination, MoveSpeed * Time.deltaTime);

            MoveJudge();
        }
    }

    #endregion

    public override void Search()
    {
        //목표 설정.
        //플레이어의 위치를 알고 있다고 가정한 상태임.
        if (null != GameData.Instance.Camera)
            CurrentTarget = GameData.Instance.Camera.transform;
    }

    protected override void MonsterEnd()
    {
        if (null != m_attackCommand)
            CoroutineManager.Instance.Unregister(m_attackCommand);

        if (null != m_timeAttackCommand)
            CoroutineManager.Instance.Unregister(m_timeAttackCommand);
    }

    protected override void MonsterStart()
    {

    }

    #region 공격스킬

    public IEnumerator<CoroutinePhase> Skill1(Action after = null)
    {
        //m_attackFoward = (GameData.Instance.Target[2].position - m_weapon[0].position).normalized;

        //m_weapon[0].forward = m_attackFoward;
        //AssetManager.Projectile.Retrieve(m_Missile, m_weapon[0], this.gameObject);

        //m_attackFoward = (GameData.Instance.Target[0].position - m_weapon[1].position).normalized;

        //m_weapon[1].forward = m_attackFoward;
        //AssetManager.Projectile.Retrieve(m_Missile, m_weapon[1], this.gameObject);

        //m_attackFoward = (GameData.Instance.Target[1].position - m_weapon[2].position).normalized;

        //m_weapon[2].forward = m_attackFoward;
        //AssetManager.Projectile.Retrieve(m_Missile, m_weapon[2], this.gameObject);

        //m_attackFoward = (GameData.Instance.Target[3].position - m_weapon[4].position).normalized;

        //m_weapon[4].forward = m_attackFoward;
        //AssetManager.Projectile.Retrieve(m_Missile, m_weapon[4], this.gameObject);

        //m_attackFoward = (GameData.Instance.Target[4].position - m_weapon[5].position).normalized;

        //m_weapon[5].forward = m_attackFoward;
        //AssetManager.Projectile.Retrieve(m_Missile, m_weapon[5], this.gameObject);

        skills[CurSkill.SkillID].Reset(this);

        yield return null;

        if (null != after)
        {
            after();
        }
    }

    public IEnumerator<CoroutinePhase> Skill2(Action after = null)
    {
        //m_attackFoward = (GameData.Instance.Target[2].position - m_weapon[0].position).normalized;

        //m_weapon[0].forward = m_attackFoward;
        //AssetManager.Projectile.Retrieve(m_Missile, m_weapon[0], this.gameObject);

        //m_attackFoward = (GameData.Instance.Target[0].position - m_weapon[1].position).normalized;

        //m_weapon[1].forward = m_attackFoward;
        //AssetManager.Projectile.Retrieve(m_Missile, m_weapon[1], this.gameObject);

        //m_attackFoward = (GameData.Instance.Target[3].position - m_weapon[4].position).normalized;

        //m_weapon[4].forward = m_attackFoward;
        //AssetManager.Projectile.Retrieve(m_Missile, m_weapon[4], this.gameObject);

        //skills[CurSkill.SkillID].Reset(this);

        yield return null;

        if (null != after)
        {
            after();
        }
    }

    public IEnumerator<CoroutinePhase> Skill3(Action after = null)
    {
        //m_attackFoward = (GameData.Instance.Target[2].position - m_weapon[0].position).normalized;

        //m_weapon[0].forward = m_attackFoward;
        //AssetManager.Projectile.Retrieve(m_Missile, m_weapon[0], this.gameObject);

        //m_attackFoward = (GameData.Instance.Target[0].position - m_weapon[1].position).normalized;

        //m_weapon[1].forward = m_attackFoward;
        //AssetManager.Projectile.Retrieve(m_Missile, m_weapon[1], this.gameObject);

        //m_attackFoward = (GameData.Instance.Target[1].position - m_weapon[2].position).normalized;

        //m_weapon[2].forward = m_attackFoward;
        //AssetManager.Projectile.Retrieve(m_Missile, m_weapon[2], this.gameObject);

        //m_attackFoward = (GameData.Instance.Target[5].position - m_weapon[3].position).normalized;

        //m_weapon[3].forward = m_attackFoward;
        //AssetManager.Projectile.Retrieve(m_Missile, m_weapon[3], this.gameObject);

        //m_attackFoward = (GameData.Instance.Target[3].position - m_weapon[4].position).normalized;

        //m_weapon[4].forward = m_attackFoward;
        //AssetManager.Projectile.Retrieve(m_Missile, m_weapon[4], this.gameObject);

        //m_attackFoward = (GameData.Instance.Target[4].position - m_weapon[5].position).normalized;

        //m_weapon[5].forward = m_attackFoward;
        //AssetManager.Projectile.Retrieve(m_Missile, m_weapon[5], this.gameObject);

        //m_attackFoward = (GameData.Instance.Target[6].position - m_weapon[6].position).normalized;

        //m_weapon[6].forward = m_attackFoward;
        //AssetManager.Projectile.Retrieve(m_Missile, m_weapon[6], this.gameObject);

        skills[CurSkill.SkillID].Reset(this);

        yield return null;

        if (null != after)
        {
            after();
        }
    }

    #endregion

    #region 공격함수

    private IEnumerator<CoroutinePhase> TimeAttack()
    {
        m_move = true;

        yield return Suspend.Do(m_attacktime);

        attackEmptyNum = UnityEngine.Random.Range(0, Skillset.Count);

        SkillStartPos = this.transform.position;
        SkillStartPos.y = 0;
        SkillStartPos.z = 0;
        m_weapon[0].position = SkillStartPos;

        AssetManager.Projectile.Retrieve(Skillset[attackEmptyNum], m_weapon[0], this.gameObject);
        Skillset.RemoveAt(attackEmptyNum);

        Attack();
    }

    private void AttackJudge()
    {
        m_timeAttackCommand = CoroutineManager.Instance.Register(TimeAttack());

        //CurSkill = skillManager.GetEnableSkillNumber();
        //if (null != CurSkill)
        //{
        //    BossSkillJudge(CurSkill.SkillID);
        //}
        //else
        //{
        //    if (null != m_timeAttackCommand)
        //        CoroutineManager.Instance.Unregister(m_timeAttackCommand);

        //    m_timeAttackCommand = CoroutineManager.Instance.Register(TimeAttack());
        //}
    }

    private void BossSkillJudge(int num)
    {
        m_move = false;

        switch (num)
        {
            case 0:
                if (null != m_attackCommand)
                    CoroutineManager.Instance.Unregister(m_attackCommand);

                m_attackCommand = CoroutineManager.Instance.Register(Skill1(Attack));
                break;
            case 1:
                if (null != m_attackCommand)
                    CoroutineManager.Instance.Unregister(m_attackCommand);

                m_attackCommand = CoroutineManager.Instance.Register(Skill2(Attack));
                break;
            case 2:
                if (null != m_attackCommand)
                    CoroutineManager.Instance.Unregister(m_attackCommand);

                m_attackCommand = CoroutineManager.Instance.Register(Skill3(Attack));
                break;

        }
    }

    public void AttackStop()
    {
        if (null != m_attackCommand)
            CoroutineManager.Instance.Unregister(m_attackCommand);

        if (null != m_timeAttackCommand)
            CoroutineManager.Instance.Unregister(m_timeAttackCommand);
    }
    #endregion

    #region 이동함수

    private void MoveInit()
    {
        RandomRangeInit();
        m_direction = new List<int>();
        m_direction.Add(m_directionLimit);
        m_direction.Add(-m_directionLimit);
        m_randomJudge = false;
    }

    private void RandomRangeInit()
    {
        m_randomRangeY = 0;
        m_randomRangeZ = 0;
        m_middle = false;
        m_randomJudge = false;
    }

    private void RandomDirection()
    {
        if (!m_randomJudge)
        {
            //m_directionInt = UnityEngine.Random.Range(0, 3);
            m_randomRangeY += UnityEngine.Random.Range(-m_directionLimit, m_directionLimit + 1);
            //m_directionInt = UnityEngine.Random.Range(0, 3);
            m_randomRangeZ += UnityEngine.Random.Range(-m_directionLimit, m_directionLimit + 1);
            m_randomJudge = true;
        }
    }

    private void MoveJudge()
    {
        if (m_middle)
        {
            if (Math.Abs(this.transform.position.y) <= 1 && Math.Abs(this.transform.position.z) <= 1)
            {
                m_middle = false;
                RandomRangeInit();
            }
        }
        else
        {
            if (Math.Abs(this.transform.position.y) >= m_directionLimit || Math.Abs(this.transform.position.z) >= m_directionLimit)
            {
                m_middle = true;
            }
        }
    }

    #endregion

    public void BossInit()
    {
        this.gameObject.SetActive(true);
        CharacterSetting();
        Search();
        MoveInit();
        Skillset = new List<string>();
        Attack();
        m_move = true;
    }

    public void Start()
    {
        GameData.Instance.FlightBoss = this;
        //BossInit();
    }

    public void Update()
    {
        Move();

        UpdateSkill();
    }
}

