using System;
using System.Collections.Generic;
using UnityEngine;

public class FighterPlane : Monster
{
    public CustomPath m_customPath;

    //private Vector3 m_currentTargetVectordir;
    private CustomPath m_pathdata;
    private int m_customIndex;
    private float m_rotSpeed;
    private int m_attackCycle = 0;
    private CoroutineCommand m_moveCommand;
    private CoroutineCommand m_attackCommand;

    #region State Func

    public override void Attack()
    {
        //if (AILogic.TargetDistance(this.transform, CurrentTarget) <= skills[0].SkillRange)
        //{
        //    m_currentTargetVectordir = (CurrentTarget.position - transform.position).normalized;
        //    float angle = Mathf.Acos(Vector3.Dot(new Vector3(m_currentTargetVectordir.x, m_currentTargetVectordir.y, m_currentTargetVectordir.z), transform.forward)) * Mathf.Rad2Deg;
        //    //if (Team == TeamFlag.Boss)
        //    //    Debug.Log(4);
        //    // 0의 근사치로 잡힐경우 처리가 애매해져 float.IsNaN(angle) 추가 
        //    if (angle < SightAngle || float.IsNaN(angle))
        //    {
        //        if (!skills[0].IsCooltime())
        //        {
        //            if (skills[0] != null)
        //            {
        //                Debug.Log("공격");
        //                WeaponPosition.transform.forward = (CurrentTarget.position - WeaponPosition.transform.position).normalized;
        //                AssetManager.Projectile.Retrieve(skills[0].SkillName, this.WeaponPosition, this.gameObject);
        //                skills[0].Reset(this);
        //            }
        //        }
        //        else
        //        {
        //            RotateUpdate(this.transform, CurrentTarget.position, m_rotSpeed);
        //        }
        //    }
        //    else
        //    {
        //        RotateUpdate(this.transform, CurrentTarget.position, m_rotSpeed);
        //    }
        //}
        //else
        //{
        //    RotateUpdate(this.transform, CurrentTarget.position, m_rotSpeed);
        //}

        //if (!skills[0].IsCooltime())
        //{
        //    if (skills[0] != null)
        //    {
        //        Debug.Log("공격");
        //        WeaponPosition.transform.forward = (CurrentTarget.position - WeaponPosition.transform.position).normalized;
        //        AssetManager.Projectile.Retrieve(skills[0].SkillName, this.WeaponPosition, this.gameObject);
        //        skills[0].Reset(this);
        //    }
        //}

        if (null != m_attackCommand)
            CoroutineManager.Instance.Unregister(m_attackCommand);

        m_attackCommand = CoroutineManager.Instance.Register(TimeAttack());
    }

    public override void Move()
    {
        //곡선으로 이동하는 것을 보이게 하기 위해 노드를 3개 사용한다.
        //노드 수를 홀수로 맞춰놓아야 이용이 가능하다.
        if (m_customIndex < m_customPath.m_nodeCount - 1)
        {
            m_pathdata = BetweenMove(m_customPath, m_customIndex);
            if (null != m_moveCommand)
                CoroutineManager.Instance.Unregister(m_moveCommand);
            m_moveCommand = CoroutineManager.Instance.Register(m_pathdata.FinishActionPutOnPath(this.transform, m_pathdata, true, () => Attack()));
            m_customIndex += 2;
        }
        else
        {
            m_customPath.m_nodes = GetReaptPatrolPath(m_customPath);
            m_customPath.NodeReset();
            m_customIndex = 0;
            m_pathdata = BetweenMove(m_customPath, m_customIndex);
            if (null != m_moveCommand)
                CoroutineManager.Instance.Unregister(m_moveCommand);
            m_moveCommand = CoroutineManager.Instance.Register(m_pathdata.FinishActionPutOnPath(this.transform, m_pathdata, true, () => Attack()));
            m_customIndex += 2;
        }
    }

    public override void Search()
    {
        //목표 설정.
        //플레이어의 위치를 알고 있다고 가정한 상태임.
        if(null != GameData.Instance.Camera)
            CurrentTarget = GameData.Instance.Camera.transform;

    }

    public override void Idle()
    {
        if (null != CurrentTarget)
        {
            RotateUpdate(this.transform, CurrentTarget.transform.position, m_rotSpeed);
        }
    }

    public override void Dead()
    {
        OnRestore();
    }

    #endregion

    private IEnumerator<CoroutinePhase> TimeAttack()
    {
        m_attackCycle++;

        yield return Suspend.Do(skills[0].Cooltime());


        if (null != CurrentTarget)
        {
            if (!skills[0].IsCooltime())
            {
                if (AILogic.TargetDistance(this.transform, CurrentTarget) <= skills[0].SkillRange)
                {
                    if (skills[0] != null)
                    {
                        //Debug.Log("공격");
                        WeaponPosition.transform.forward = (CurrentTarget.position - WeaponPosition.transform.position).normalized;
                        AssetManager.Projectile.Retrieve(skills[0].SkillName, this.WeaponPosition, this.gameObject);
                        skills[0].Reset(this);
                    }
                }
            }
        }

        if (null != m_customPath)
        {
            if (m_attackCycle > m_customPath.m_nodes[m_customIndex].AttackCycle)
                Move();
            else
                Attack();
        }
        else
        {
            Attack();
        }
    }

    protected override void MonsterStart()
    {
        
    }

    protected override void MonsterEnd()
    {
        if (null != m_moveCommand)
            CoroutineManager.Instance.Unregister(m_moveCommand);
    }

    public void Start()
    {
        CharacterSetting();

        m_customIndex = 0;
        m_rotSpeed = MoveSpeedTimeDelta * 20f;
        if (null != m_customPath)
        {
            m_pathdata = new CustomPath();
            m_pathdata.m_nodeCount = 2;
            Move();
        }
        else
        {
            Attack();
        }
    }

    public void Update()
    {
        if (null == CurrentTarget)
        {
            Search();
        }

        UpdateSkill();
    }
}
