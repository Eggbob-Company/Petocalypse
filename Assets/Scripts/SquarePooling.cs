using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SquarePooling : MonoBehaviour
{
    private IObjectPool<SquarePooling> _pool;

    //풀 매니저가 사용할 함수 : 풀 참조 전달
    public void SetPool(IObjectPool<SquarePooling> pool)
    {
        _pool = pool;
    }

    // 일정 시간 뒤 자동으로 풀로 복귀
    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), 2f); //2프레임 후 복귀
    }

    private void ReturnToPool()
    {
        if (_pool != null)
        {
            _pool.Release(this);
        }
    }
}