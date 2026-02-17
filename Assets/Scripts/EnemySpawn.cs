using UnityEngine;
using System;

 // PoolAble을 상속받음으로써 오브젝트 풀에 들어갈 수 있게 됨
public class EnemySpawn : PoolAble
{
    public static Action<Vector2> OnEnemyDeath;

    private void OnEnable()
    {
        // 활성화될 때마다 2초 후 반환 예약
        // Invoke(nameof(ReturnToPool), 2f);

        // 테스트용
        Invoke(nameof(Die), 2f);
    }

    public void Die()
    {
        OnEnemyDeath?.Invoke(transform.position);
        Debug.Log($"{gameObject.name} 사망 신호 발송 및 풀 반환");
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        // PoolAble에 정의된 ReleaseObject를 호출하여 풀로 복귀
        ReleaseObject();
    }

    private void OnDisable()
    {
        // 비활성화될 시 스폰 함수 취소
        CancelInvoke();
    }
}