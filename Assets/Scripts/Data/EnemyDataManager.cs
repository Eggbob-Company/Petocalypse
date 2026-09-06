using UnityEngine;
using System.Collections.Generic;

public class EnemyDataManager : MonoBehaviour
{
    // EnemySpawnManager에서 싱글톤 관리를 하긴 하지만 EnemyDataManager 자체에서도 싱글톤이 있는 것이 유리하다고 함.
    // EnemySpawnManager가 아닌 다른 스크립트에서 Enemy 데이터가 필요할 경우가 있기 때문.
    public static EnemyDataManager instance;

    // Key: id, Value: 해당 id의 enemy data
    private Dictionary<int, EnemyData> _enemy_data_dict = new Dictionary<int, EnemyData>();

    void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
        LoadEnemyData();
    }

    public void LoadEnemyData()
    {
        // csv 파일을 읽어와서 텍스트 덩어리로 만들어줌.
        TextAsset csv_data = Resources.Load<TextAsset>("EnemyData");

        if (csv_data == null)
        {
            Debug.LogError("[EnemyDataManager] EnemyData.csv 파일을 찾을 수 없습니다.");
            return;
        }

        // 텍스트를 줄바꿈(엔터) 기준으로 쪼개기.
        string[] lines = csv_data.text.Split('\n');

        // 첫 번째 줄(0번 인덱스)은 id, name과 같은 헤더니까 1번부터 시작.
        for (int i = 1; i < lines.Length; i++)
        {
            // 비어있는 줄 스킵
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            // 한 줄을 쉼표(,) 기준으로 구분
            string[] row = lines[i].Split(',');

            // 각 칸의 데이터를 타입에 맞게 파싱 (Trim으로 유령 공백 제거)
            EnemyData data = new EnemyData();
            data.id = int.Parse(row[0].Trim());
            data.name = row[1].Trim();
            data.max_hp = float.Parse(row[2].Trim());
            data.move_speed = float.Parse(row[3].Trim());
            data.base_damage = float.Parse(row[4].Trim());
            data.attack_range = float.Parse(row[5].Trim());
            data.attack_rate = float.Parse(row[6].Trim());
            data.exp_reward = int.Parse(row[7].Trim());
            data.enemy_type = row[8].Trim();

            _enemy_data_dict.Add(data.id, data);
        }

        Debug.Log($"[System] EnemyData 로드 완료. 총 데이터 개수: {_enemy_data_dict.Count}");
    }

    // 특정 ID 값을 전달 받은 후 해당 ID의 enemy 데이터를 반환하는 함수
    public EnemyData GetEnemyData(int enemy_id)
    {
        if (_enemy_data_dict.TryGetValue(enemy_id, out EnemyData data))
        {
            return data;
        }

        Debug.LogError($"[EnemyDataManager] {enemy_id}번 Enemy 데이터를 찾을 수 없습니다");
        return null;
    }
}