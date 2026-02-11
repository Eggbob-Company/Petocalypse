using UnityEngine;

public class TestBullet : PoolAble
{

    private void OnEnable()
    {
        // 활성화될 때마다 2초 후 반환 예약
        Invoke(nameof(ReturnToPool), 2f);
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