using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    // 싱글톤 구현
    public static PlayerDataManager instance;

    // Key: id, Value: 해당 id의 player data
    private Dictionary<int, PlayerData> _player_dict = new Dictionary<int, PlayerData>();

    void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
        LoadPlayerData();
    }

    void LoadPlayerData()
    {
        // csv 파일을 읽어와서 텍스트 덩어리로 만들어줌.
        TextAsset csv_data = Resources.Load<TextAsset>("PlayerData");

        if (csv_data == null)
        {
            Debug.LogError("[PlayerDataManager] PlayerData.csv 파일을 찾을 수 없습니다.");
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
            PlayerData data = new PlayerData();
            data.id = int.Parse(row[0].Trim());
            data.max_hp = float.Parse(row[1].Trim());
            data.recovery = float.Parse(row[2].Trim());
            data.move_speed = float.Parse(row[3].Trim());
            data.might = float.Parse(row[4].Trim());
            data.area = float.Parse(row[5].Trim());
            data.projectile_speed = float.Parse(row[6].Trim());
            data.duration = float.Parse(row[7].Trim());
            data.magnet_range = float.Parse(row[8].Trim());
            data.luck = float.Parse(row[9].Trim());
            data.desc_key = row[10].Trim();
            
            _player_dict.Add(data.id, data);
        }

        Debug.Log($"[System] PlayerData 로드 완료. 총 데이터 개수: {_player_dict.Count}");
    }

    // 특정 ID 값을 전달 받은 후 해당 ID의 player 데이터를 반환하는 함수
    public PlayerData GetPlayerData(int id)
    {
        if (_player_dict.TryGetValue(id, out PlayerData data))
        {
            return data;
        }

        Debug.LogError($"[PlayerDataManager] {id}번의 Player 데이터를 찾을 수 없습니다");
        return null;
    }
}