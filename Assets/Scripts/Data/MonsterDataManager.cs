using UnityEngine;
using System.Collections.Generic;

public class MonsterDataManager : MonoBehaviour
{
    // EnemySpawnManager에서 싱글톤 관리를 하긴 하지만 MonsterDataManager 자체에서도 싱글톤이 있는 것이 유리하다고 함.
    // EnemySpawnManager가 아닌 다른 스크립트에서 몬스터 데이터가 필요할 경우가 있기 때문.
    public static MonsterDataManager instance;
    private Dictionary<int, MonsterData> _monster_data_dict = new Dictionary<int, MonsterData>();

    void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
        LoadMonsterData();
    }

    public void LoadMonsterData()
    {
        // Resources 폴더에서 MonsterData.csv 파일을 불러온다. (확장자는 빼고 이름만 적기)
        // csv 파일을 읽어와서 텍스트 덩어리로 만들어줌.
        TextAsset csv_data = Resources.Load<TextAsset>("MonsterData");

        if (csv_data == null)
        {
            Debug.LogError("[System] MonsterData.csv 파일을 찾을 수 없음. Resources 폴더를 확인해주세요.");
            return;
        }

        // 텍스트를 줄바꿈(엔터) 기준으로 쪼개기.
        string[] lines = csv_data.text.Split('\n');

        // 첫 번째 줄(0번 인덱스)은 id, name과 같은 헤더니까 1번부터 시작.
        for (int i = 1; i < lines.Length; i++)
        {
            // 빈 줄이 있으면 건너뜀.
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            // 한 줄을 쉼표(,) 기준으로 다시 쪼개기.
            string[] row = lines[i].Split(',');

            // 그릇에 데이터를 담기.
            MonsterData data = new MonsterData();
            data.id = int.Parse(row[0].Trim());
            data.name = row[1].Trim();
            data.max_health = float.Parse(row[2].Trim());
            data.move_speed = float.Parse(row[3].Trim());
            data.base_damage = float.Parse(row[4].Trim());
            data.attack_range = float.Parse(row[5].Trim());
            data.attack_rate = float.Parse(row[6].Trim());
            data.exp_reward = int.Parse(row[7].Trim());
            data.monster_type = row[8].Trim();

            // 딕셔너리에 저장. (나중에 ID로 빠르게 찾기 위해)
            _monster_data_dict.Add(data.id, data);
        }

        Debug.Log("[System] MonsterData.csv 로드 완료. 총 데이터 개수: " + _monster_data_dict.Count);
    }

    // 나중에 스포너가 몬스터를 생성할 때 특정 ID의 데이터를 달라고 요청하는 함수
    public MonsterData GetMonsterData(int monster_id)
    {
        if (_monster_data_dict.TryGetValue(monster_id, out MonsterData data))
        {
            return data;
        }

        Debug.LogError($"[System] {monster_id}번 몬스터 데이터를 찾을 수 없습니다");
        return null;
    }
}