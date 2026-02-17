using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExpSpawnManager : MonoBehaviour
{
    public static ExpSpawnManager Instance; // 싱글톤

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
        Debug.Log("SpawnExp 함수 실행");

        GameObject obj = ObjectPoolManager.instance.GetGo("Exp");
        if (obj != null)
        {
            obj.transform.position = spawn_position;
            Debug.Log("Exp 생성");
        }
    }

}
