using UnityEngine;
using System;

public class TestMonster : PoolAble
{
    public static Action<Vector2> OnEnemyDeath;

    public void Die()
    {
        OnEnemyDeath?.Invoke(transform.position);
        //ReleaseObject();
    }
}