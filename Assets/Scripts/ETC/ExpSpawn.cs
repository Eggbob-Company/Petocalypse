using UnityEngine;

public class ExpSpawn : PoolAble // 오브젝트 풀 사용을 위한 PoolAble 상속
{
    private void OnEnable()
    {
        
    }

    // Player와 충돌 감지 시 복귀
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("경험치 획득");
            // + 플레이어의 경험치 수치 증가시키는 코드
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        // PoolAble에 정의된 ReleaseObject를 호출하여 풀로 복귀
        ReleaseObject();
    }
}
