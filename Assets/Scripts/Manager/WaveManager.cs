using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private EnemySpawnManager _spawn_manager;
    private int _last_checked_time = -1; // 중복 실행 방지를 위한 변수

    void Awake()
    {
        _spawn_manager = GetComponent<EnemySpawnManager>();
    }

    void Update()
    {
        // InGameManager에서 현재 게임 시간을 정수 단위(= 초 단위)로 가져옴
        int current_time = Mathf.FloorToInt(InGameManager.instance.game_time);

        // 같은 초에 여러 번 실행하지 않기 위해 중복 체크
        if (current_time != _last_checked_time)
        {
            _last_checked_time = current_time;
            // 소환해야하는 몹이 있는지 확인
            CheckAndSpawnWave(current_time);
        }
    }

    private void CheckAndSpawnWave(int time)
    {
        // WaveDataManager에서 지금 이 시간에 소환해야 하는 몬스터가 있는지 확인
        List<WaveData> current_waves = WaveDataManager.instance.GetWavesForTime(time);

        if (current_waves != null && current_waves.Count > 0)
        {
            foreach (WaveData wave in current_waves)
            {
                // 웨이브 데이터를 코루틴으로 실행
                StartCoroutine(CoSpawnWave(wave));
            }
        }
    }

    private IEnumerator CoSpawnWave(WaveData data)
    {
        // 몬스터 id로 EnemyData csv에 name 반환
        string enemy_name = EnemyDataManager.instance.GetEnemyData(data.enemy_id).name;

        for (int i = 0; i < data.amount; i++)
        {
            // EnemySpawnManager의 랜덤 좌표 소환 함수 호출
            Vector2 spawn_pos = _spawn_manager.GetRandomSpawnPosition();

            if (spawn_pos != Vector2.zero)
            {
                _spawn_manager.SpawnAtPosition(enemy_name, data.enemy_id, spawn_pos);
            }

            // csv에 설정된 interval만큼 대기 후 소환
            yield return new WaitForSeconds(data.interval);
        }
    }
}
