using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] private Vector2 _map_min; // 맵 왼쪽 아래 좌표
    [SerializeField] private Vector2 _map_max; // 맵 오른쪽 위 좌표

    [Header("Spawn Settings")] // 스폰 제외할 범위(플레이어 기준 사각형)
    [SerializeField] private float _spawn_except_width = 5f;  // 플레이어 기준 가로 반경(너비의 절반)
    [SerializeField] private float _spawn_except_height = 5f; // 플레이어 기준 세로 반경(높이의 절반)

    private Transform _player_transform;

    private void Start()
    {
        // 태그를 이용해 플레이어 좌표 참조
        GameObject player_go = GameObject.FindWithTag("Player");
        if(player_go != null)
        {
            _player_transform = player_go.transform;
        }

        StartCoroutine(CoSpawnZombie()); // 코루틴 시작
    }

    // 좀비를 0.1초마다 자동 생성하는 코루틴
    private IEnumerator CoSpawnZombie()
    {
        yield return new WaitUntil(() => ObjectPoolManager.instance != null); // ObjectPoolManager가 준비될 때까지 대기

        while (true)
        {
            Vector2 spawn_pos = GetRandomSpawnPosition();

            // 좌표가 유효하다면
            if(spawn_pos != Vector2.zero)
            {
                SpawnAtPosition("Zombie", spawn_pos);
            }

            yield return new WaitForSeconds(0.1f); // 0.1초 대기
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        if(_player_transform == null) return Vector2.zero;

        Vector2 player_pos = (Vector2)_player_transform.position;

        // 스폰 제외 경계 지정(플레이어 주변 사각형)
        float safe_left = player_pos.x - _spawn_except_width;
        float safe_right = player_pos.x + _spawn_except_width;
        float safe_top = player_pos.y + _spawn_except_height;
        float safe_bottom = player_pos.y - _spawn_except_height;

        Rect[] spawn_zones = new Rect[4];
        
        // 왼쪽 영역 (맵 왼쪽 끝 ~ 플레이어 왼쪽)
        spawn_zones[0] = Rect.MinMaxRect(_map_min.x, _map_min.y, safe_left, _map_max.y);
        // 오른쪽 영역 (플레이어 오른쪽 ~ 맵 오른쪽 끝)
        spawn_zones[1] = Rect.MinMaxRect(safe_right, _map_min.y, _map_max.x, _map_max.y);
        // 위쪽 영역 (플레이어 가로 폭 사이의 위쪽)
        spawn_zones[2] = Rect.MinMaxRect(Mathf.Max(_map_min.x, safe_left), safe_top, Mathf.Min(_map_max.x, safe_right), _map_max.y);
        // 아래쪽 영역 (플레이어 가로 폭 사이의 아래쪽)
        spawn_zones[3] = Rect.MinMaxRect(Mathf.Max(_map_min.x, safe_left), _map_min.y, Mathf.Min(_map_max.x, safe_right), safe_bottom);

        // 유효한 영역만 필터링
        List<int> valid_zones = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            if (spawn_zones[i].width > 0 && spawn_zones[i].height > 0) // 영역이 존재할 경우
            {
                valid_zones.Add(i);
            }
        }

        if (valid_zones.Count == 0) return Vector2.zero; // 예외 처리: 범위가 존재하지 않음

        // 랜덤 영역 선택 + 좌표 추출
        int random_idx = valid_zones[Random.Range(0, valid_zones.Count)];
        Rect selected_rect = spawn_zones[random_idx];

        float random_x = Random.Range(selected_rect.xMin, selected_rect.xMax);
        float random_y = Random.Range(selected_rect.yMin, selected_rect.yMax);
        return new Vector2(random_x, random_y); // 좌표 추출, spawn_pos에 저장됨
    }

    public void SpawnAtPosition(string object_name, Vector2 spawn_position) // 좌표 위치에서 소환
    {
        if (ObjectPoolManager.instance == null) return;

        GameObject obj = ObjectPoolManager.instance.GetGo(object_name);
        if (obj != null)
        {
            obj.transform.position = spawn_position;
        }
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Alpha2)) // 키보드 2 키를 누를 때 오브젝트 랜덤 생성
        // {
        //     Vector2 random_two = Random.insideUnitCircle * 3f;
        //     SpawnAtPosition("Object2", new Vector3(random_two.x, random_two.y, 0f));
        // }
    }
}