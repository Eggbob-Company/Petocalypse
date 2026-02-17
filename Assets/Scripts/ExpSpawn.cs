using UnityEngine;

public class ExpSpawn : PoolAble // 오브젝트 풀 사용을 위한 PoolAble 상속
{
    private void OnEnable()
    {
        
    }

    private void ReturnToPool()
    {
        // PoolAble에 정의된 ReleaseObject를 호출하여 풀로 복귀
        ReleaseObject();
    }
}
