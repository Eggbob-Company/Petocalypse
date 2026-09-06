using UnityEngine;
using System.Collections.Generic;

public class ExpSpawn : PoolAble // 오브젝트 풀 사용을 위한 PoolAble 상속
{
    public static List<ExpSpawn> active_exps = new List<ExpSpawn>();
    public float follow_speed = 15f;
    
    private void OnEnable()
    {
        active_exps.Add(this);
    }

    private void OnDisable()
    {
        active_exps.Remove(this);
    }

    void Update()
    {
        if (InGameManager.instance.is_magnet_active) // 자석 효과 발동 중일 시
        {
            // 플레이어의 현재 위치로 이동
            transform.position = Vector2.MoveTowards(
                transform.position,
                Player.instance.transform.position,
                follow_speed * Time.deltaTime
            );
        }
    }

    // Player와 충돌 감지 시 복귀
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 경험치 추가
            InGameManager.instance.GetExp(5);

            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        // PoolAble에 정의된 ReleaseObject를 호출하여 풀로 복귀
        ReleaseObject();
    }
}
