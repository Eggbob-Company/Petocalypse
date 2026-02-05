using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SquarePoolManager : MonoBehaviour
{
    //싱글톤 설정
    private static SquarePoolManager _instance;
    public static SquarePoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SquarePoolManager>();
            }
            return _instance;
        }
    }

    [SerializeField] private SquarePooling square_prefab;
    [SerializeField] private int max_pool_size = 10;

    private IObjectPool<SquarePooling> _pool;

    private void Awake()
    {
        //싱글톤 중복 방지
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;

        _pool = new ObjectPool<SquarePooling>(
            OnCreateItem,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            maxSize: max_pool_size
        );
    }

    private SquarePooling OnCreateItem()
    {
        var obj = Instantiate(square_prefab);
        obj.SetPool(_pool); //SquarePooling 스크립트에 풀 참조 전달
        return obj;
    }

    private void OnTakeFromPool(SquarePooling obj) => obj.gameObject.SetActive(true);
    private void OnReturnedToPool(SquarePooling obj) => obj.gameObject.SetActive(false);
    private void OnDestroyPoolObject(SquarePooling obj) => Destroy(obj.gameObject);

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var square = _pool.Get();
            
            Vector2 random_pos = Random.insideUnitCircle * 3f;
            square.transform.position = new Vector3(random_pos.x, random_pos.y, 0f);
        }
    }
}