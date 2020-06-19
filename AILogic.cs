using UnityEngine;
using System;
using System.Collections.Generic;

    // NPC AI Logic
    /// <summary>
    /// AI State 관련 공통 함수
    /// </summary>
public static class AILogic
{
    // HP Percentage
    public static float GetHpPercentage(Monster owner)
    {
        return (owner.RemainHP * 100f / owner.Health);
    }

    public static bool CheckDead(Monster owner)
    {
        return !owner.IsAlive;
    }

    public static bool IsArrived(Vector3 pos, Vector3 targetPos)
    {
        return (pos - targetPos).magnitude < 0.01f;
    }

    public static float TargetDistance(Transform me, Transform target)
    {
        return Vector3.Distance(me.position, target.position);
    }

}