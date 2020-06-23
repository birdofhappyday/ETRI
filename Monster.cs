
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : BaseCharacter, Hittable
{
    public float Health;
    public float MoveSpeed;
    public float StoppingDistance;

    public float SearchRange = 10.0f;
    public float SightAngle = 60.0f;

    private float m_fRemainHealth;

    public SkillManager skillManager { get; private set; }

    #region Battle Property

    public Skill[] skills;
    public Transform hitEffect;

    [SerializeField]
    protected int m_combatCycle; // 공격 횟수

    protected Transform CurrentTarget;

    #endregion

    #region Move Property

    protected bool m_Moving;

    public float MoveSpeedTimeDelta { get { return MoveSpeed * Time.deltaTime; } }

    #endregion

    #region Searching Proprty

    protected bool m_Searching;
    protected bool m_HasTarget;

    #endregion

    #region Base Func

    protected override void CharacterSetting()
    {
        StateInit();
        m_fRemainHealth = Health;
        InitSkill();

        MonsterStart();
    }

    protected override void OnRestore()
    {
        MonsterEnd();

        CurrentTarget = null;

        OnHit(false);
        OnDead(false);

        base.OnRestore();
    }

    protected abstract void MonsterStart();
    protected abstract void MonsterEnd();

    #endregion

    #region Move Func

    public virtual void LookAtTarget(Vector3 targetPosition, float fRotationSpeed = 1.0f)
    {
        Vector3 targetPosXZ = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        transform.LookAt(targetPosXZ);
    }

    protected void RotateUpdate(Transform my, Vector3 target, float speed)
    {
        if (my.position == target)
        {
            return;
        }
        Vector3 dir = target - my.position;
        dir.Normalize();

        my.rotation = Quaternion.RotateTowards(my.rotation, Quaternion.LookRotation(dir), speed * Time.deltaTime);
    }

    //custompath에 reapt체크 된것만 찾아서 역순으로 반환한다.
    public List<PathData> GetReaptPatrolPath(CustomPath m_customPath)
    {
        List<PathData> path = m_customPath.m_nodes.FindAll(rhs => rhs.Repeat == true);

        if (path.Count == 1)
            Debug.LogError("ItweenPath : AutoPath Checkbox has one. : Recommend More than 1 or None Checkboxes [ex : 2, 3... or  0] ");

        m_customPath.m_nodeCount = path.Count;
        path.Reverse();
        return path;
    }

    //곡선으로 이동하는 것을 보이게 하기 위해 노드를 3개 사용한다.
    public CustomPath BetweenMove(CustomPath customPath, int index)
    {
        CustomPath m_pathData = new CustomPath();
        m_pathData.m_nodeCount = 3;
        m_pathData.m_nodes.Add(customPath.m_nodes[index]);
        m_pathData.m_nodes.Add(customPath.m_nodes[index + 1]);
        m_pathData.m_nodes.Add(customPath.m_nodes[index + 2]);

        return m_pathData;
    }

    #endregion

    #region Unity

    #endregion

    #region GetFunc

    public bool IsAlive
    {
        get { return (m_fRemainHealth > 0.0f); }
    }
    public float RemainHP
    {
        get { return m_fRemainHealth; }
        set { m_fRemainHealth = value; }
    }

    public static float GetHpPercentage(Monster owner)
    {
        return (owner.RemainHP * 100f / owner.Health);
    }

    #endregion

    /// <summary>
    /// 애니메이터 상태 설정
    /// </summary>
    /// <param name="state"></param>
    #region State Set Func (Animator State Change)
    public virtual void OnAppear(bool state = true) { ThisAnimator.SetBool("OnAppear", state); }
    public virtual void OnIdle(bool state = true) { ThisAnimator.SetBool("OnIdle", state); }
    public virtual void OnMove(bool state = true) { ThisAnimator.SetBool("OnMove", state); }
    public virtual void OnAttack(bool state = true) { ThisAnimator.SetBool("OnAttack", state); }
    public virtual void OnHit(bool state = true)    { ThisAnimator.SetBool("OnHit", state);    }
    public virtual void OnDead(bool state = true)
    {
        m_fRemainHealth = 0.0f;

        ThisAnimator.SetBool("OnDead", state);
    }
    public virtual void OnDisappear(bool state = true) { ThisAnimator.SetBool("OnDisappear", state); }


    public void StateInit()
    {
        OnAppear(false);
        OnIdle(false);
        OnMove(false);
        OnAttack(false);
        OnHit(false);
        OnDead(false);
    }
    #endregion

    #region State Func

    public abstract void Idle();
    public abstract void Search();
    public abstract void Move();
    public abstract void Attack();
    public abstract void Dead();
    #endregion

    #region Hit Func

    /// <summary>
    /// 공격 받았을때 데미지 설정
    /// </summary>
    /// <param name="Damage"></param>
    public void ReduceHealth(float Damage)
    {
        m_fRemainHealth -= Damage;
    }

    /// <summary>
    /// 몬스터 마다 설정을 위해서 가상함수로 HitEvent를 설정.
    /// </summary>
    /// <param name="hitPoint"></param>
    /// <param name="damage"></param>
    /// <param name="hitType"></param>
    /// <param name="isWeakPoint"></param>
    public virtual void HitEvent(Vector3 hitPoint, float damage = 0.0f, HitType hitType = HitType.NONE, bool isWeakPoint = false)
    {
        if (!IsAlive)
            return;

        OnHit(true);

        if (hitEffect != null)
        {
            EffectRetrieve(hitEffect.name, hitPoint);
        }

        ReduceHealth(damage);

        if (m_fRemainHealth <= 0.0f)
        {
            {
                OnDead();
            }
        }
    }

    /// <summary>
    /// 몬스터가 공격 받았을때 작동. HitEvent를 호출
    /// </summary>
    /// <param name="hitPoint"></param>
    /// <param name="damage"></param>
    /// <param name="hittype"></param>
    /// <param name="isWeakPoint"></param>
    public void Hit(Vector3 hitPoint, float damage = 0, HitType hittype = HitType.NONE, bool isWeakPoint = false)
    {
        HitEvent(hitPoint, damage, hittype, isWeakPoint);
    }

    #endregion

    #region Attack Func

    /// <summary>
    /// 몬스터가 생성되었을 때 skillManager를 초기화한다.
    /// </summary>
    protected virtual void InitSkill()
    {
        if (skillManager != null)
            skillManager.Init(this);

        else
        {
            skillManager = new SkillManager();
            skillManager.Init(this);
        }   
    }

    /// <summary>
    /// 스킬 쿨타임을 계산한다.
    /// </summary>
    protected virtual void UpdateSkill()
    {
        if (skillManager != null)
            skillManager.Update();
    }

    #endregion

    #region Sound & Effect Func

    public void EffectRetrieve(string name, Transform target)
    {
        Effect effect = AssetManager.Effect.Retrieve(name);
        effect.transform.position = target.position;
        effect.transform.rotation = target.rotation;
    }

    public void EffectRetrieve(string name, Vector3 target)
    {
        Effect effect = AssetManager.Effect.Retrieve(name);
        effect.transform.position = target;
    }

    public Effect EffectRetrieve_Return(string name, Transform target)
    {
        Effect effect = AssetManager.Effect.Retrieve(name);
        effect.transform.position = target.position;
        effect.transform.rotation = target.rotation;
        return effect;
    }

    public Effect EffectRetrieve_Return(string name, Vector3 target)
    {
        Effect effect = AssetManager.Effect.Retrieve(name);
        effect.transform.position = target;
        return effect;
    }
    #endregion

}