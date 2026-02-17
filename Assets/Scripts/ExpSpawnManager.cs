using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExpSpawnManager : MonoBehaviour
{

    private void OnEnable()
    {   // 몬스터 사망 이벤트 구독
        EnemySpawn.OnEnemyDeath += SpawnExp;
    }

    private void OnDisable()
    {
        // 몬스터 사망 이벤트 구독 해제
        EnemySpawn.OnEnemyDeath -= SpawnExp;
    }

    private void SpawnExp(Vector2 spawn_position)
    {
        if (ObjectPoolManager.instance == null) return;

        GameObject obj = ObjectPoolManager.instance.GetGo("Exp");
        if (obj != null)
        {
            obj.transform.position = spawn_position;
        }
    }

}
