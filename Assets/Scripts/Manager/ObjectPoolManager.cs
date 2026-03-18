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
        GameObject poolGo = Instantiate(_prefab_dic[_object_name]);
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
        // 오브젝트 풀 생성이 되지 않은 이름이라면
        if(!_object_pool_dic.ContainsKey(goName))
        {
            if(_prefab_dic.ContainsKey(goName)) // 딕셔너리 이름과 일치
            {
                Debug.Log($"{goName} 풀이 없어 새로 생성합니다.");
                RegisterNewPool(goName, _prefab_dic[goName], 20);
            }
            else // 딕셔너리 이름과 불일치
            {
                Debug.Log($"{goName} 딕셔너리에 등록되지 않은 이름입니다.");
                return null;
            }
        }

        _object_name = goName; // CreatePooledItem에서 사용할 변수 업데이트
        return _object_pool_dic[goName].Get();
    }

    public void RegisterNewPool(string goName, GameObject prefab, int count)
    {
        if(_object_pool_dic.ContainsKey(goName)) return;

        // 풀 생성을 Init이 아니라 여기서 처리
        IObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            CreatePooledItem,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            true,
            count,
            1000
        );

        _object_pool_dic.Add(goName, pool);
        _object_name = goName;
    
        for(int i = 0; i < count; i++)
        {
            PoolAble poolAbleGo = CreatePooledItem().GetComponent<PoolAble>();
            poolAbleGo.Pool.Release(poolAbleGo.gameObject);
        }
    }
}