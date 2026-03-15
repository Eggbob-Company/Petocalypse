using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
// 기존 코드와 달리 해당 스크립트에서 여러 종류의 요소들을 풀에 넣었다 뺐다 할 수 있음
// 이는 딕셔너리 구조로 구현함. 각 요소별로 인자 값을 다르게 전달하고 각자 다른 풀에서 관리함

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    private class ObjectInfo
    {
        // 오브젝트 이름
        public string object_name;
        // 오브젝트 풀에서 관리할 오브젝트
        public GameObject prefab;
        // 몇개를 미리 생성해둘건지
        public int count;
    }


    public static ObjectPoolManager instance;

    // 오브젝트풀 매니저 준비 완료표시
    public bool is_ready { get; private set; }

    [SerializeField]
    private ObjectInfo[] _object_infos = null;

    // 생성할 오브젝트의 key값 지정을 위한 변수
    private string _object_name;

    // 오브젝트풀들을 관리할 딕셔너리
    private Dictionary<string, IObjectPool<GameObject>> _object_pool_dic = new Dictionary<string, IObjectPool<GameObject>>();

    // 오브젝트풀에서 오브젝트를 새로 생성할 때 사용할 딕셔너리
    private Dictionary<string, GameObject> _go_dic = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        Init();
    }


    private void Init()
    {
        is_ready = false;

        for (int idx = 0; idx < _object_infos.Length; idx++)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
            OnDestroyPoolObject, true, _object_infos[idx].count, _object_infos[idx].count);

            if (_go_dic.ContainsKey(_object_infos[idx].object_name))
            {
                Debug.LogFormat("{0} 이미 등록된 오브젝트입니다.", _object_infos[idx].object_name);
                return;
            }

            _go_dic.Add(_object_infos[idx].object_name, _object_infos[idx].prefab);
            _object_pool_dic.Add(_object_infos[idx].object_name, pool);

            // 미리 오브젝트 생성 해놓기
            for (int i = 0; i < _object_infos[idx].count; i++)
            {
                _object_name = _object_infos[idx].object_name;
                PoolAble poolAbleGo = CreatePooledItem().GetComponent<PoolAble>();
                poolAbleGo.Pool.Release(poolAbleGo.gameObject);
            }
        }

        Debug.Log("오브젝트풀링 준비 완료");
        is_ready = true;
    }

    // 생성
    private GameObject CreatePooledItem()
    {
        GameObject poolGo = Instantiate(_go_dic[_object_name]);
        poolGo.GetComponent<PoolAble>().Pool = _object_pool_dic[_object_name];
        return poolGo;
    }

    // 대여
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }

    // 반환
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }

    // 삭제
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }

    public GameObject GetGo(string goName)
    {
        _object_name = goName;

        if (_go_dic.ContainsKey(goName) == false)
        {
            Debug.LogFormat("{0} 오브젝트풀에 등록되지 않은 오브젝트입니다.", goName);
            return null;
        }

        return _object_pool_dic[goName].Get();
    }
}