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
    private Dictionary<string, GameObject> _prefab_dic = new Dictionary<string, GameObject>();

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
            // 모든 프리팹 정보를 딕셔너리에 저장
            if(!_prefab_dic.ContainsKey(_object_infos[idx].object_name))
            {
                _prefab_dic.Add(_object_infos[idx].object_name, _object_infos[idx].prefab);
            }

            if(_object_infos[idx].count > 0)
            {
                RegisterNewPool(_object_infos[idx].object_name, _object_infos[idx].prefab, _object_infos[idx].count);
            }

        }

        Debug.Log("오브젝트풀링 준비 완료");
        is_ready = true;
    }

    // 생성
    private GameObject CreatePooledItem()
    {
        GameObject go = Instantiate(_prefab_dic[_object_name]);
        go.GetComponent<PoolAble>().Pool = _object_pool_dic[_object_name];
        return go;
    }

    // 대여
    private void OnTakeFromPool(GameObject go)
    {
        go.SetActive(true);
    }

    // 반환
    private void OnReturnedToPool(GameObject go)
    {
        go.SetActive(false);
    }

    // 삭제
    private void OnDestroyPoolObject(GameObject go)
    {
        Destroy(go);
    }

    public GameObject GetGo(string go_name)
    {
        // 오브젝트 풀 생성이 되지 않은 이름이라면
        if(!_object_pool_dic.ContainsKey(go_name))
        {
            if(_prefab_dic.ContainsKey(go_name)) // 딕셔너리 이름과 일치
            {
                Debug.Log($"{go_name} 풀이 없어 새로 생성합니다.");
                RegisterNewPool(go_name, _prefab_dic[go_name], 20);
            }
            else // 딕셔너리 이름과 불일치
            {
                Debug.Log($"{go_name} 딕셔너리에 등록되지 않은 이름입니다.");
                return null;
            }
        }

        _object_name = go_name; // CreatePooledItem에서 사용할 변수 업데이트
        return _object_pool_dic[go_name].Get();
    }

    public void RegisterNewPool(string go_name, GameObject prefab, int count)
    {
        if(_object_pool_dic.ContainsKey(go_name)) return;

        // 풀 생성을 Init이 아니라 여기서 처리
        IObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: CreatePooledItem,
            actionOnGet: OnTakeFromPool,
            actionOnRelease: OnReturnedToPool,
            actionOnDestroy: OnDestroyPoolObject,
            collectionCheck: true,
            defaultCapacity: count,
            maxSize: 1000
        );

        _object_pool_dic.Add(go_name, pool);
        _object_name = go_name;
    
        for(int i = 0; i < count; i++)
        {
            PoolAble pool_able_go = CreatePooledItem().GetComponent<PoolAble>();
            pool_able_go.Pool.Release(pool_able_go.gameObject);
        }
    }
}