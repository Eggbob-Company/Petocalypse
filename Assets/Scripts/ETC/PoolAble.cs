using UnityEngine;
using UnityEngine.Pool;
// 오브젝트풀을 사용할 오브젝트들이 모두 상속받게 해서 이 스크립트를 상속받은 오브젝트들만 오브젝트풀에 등록할 수 있게 함
// 자신이 돌려줘야 할 Pool을 저장할 프로퍼티와 자신의 게임 오브젝트를 넘겨줄 ReleaseObject 메서드를 정의함
public class PoolAble : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }

    public void ReleaseObject()
    {
        if(Pool != null) // 풀로 반납
        {
            Pool.Release(gameObject);
        }
        else // 풀에 소속되지 않은 오브젝트일 경우: 예외 처리로 그냥 파괴
        {
            Destroy(gameObject);
        }
    }
}