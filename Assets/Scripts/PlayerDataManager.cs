using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    // 싱글톤 구현
    // ObjectPoolManager에 해당 구현이 되어있어서 오브젝트 풀링을 사용하지 않는 데이터에만 추가
    public static PlayerDataManager instance;

    private Dictionary<int, PlayerData> _player_dict = new Dictionary<int, PlayerData>();

    void Awake()
    {
        // 싱글톤 초기화 및 중복 방지
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        LoadPlayerData();
    }

    void LoadPlayerData()
    {
        TextAsset csv_data = Resources.Load<TextAsset>("PlayerData");

        if (csv_data == null)
        {
            Debug.LogError("[System] PlayerData.csv 파일을 찾을 수 없습니다.");
            return;
        }

        // 줄 단위로 나누기
        string[] lines = csv_data.text.Split('\n');

        // 파싱 (i=1부터 시작하여 헤더 스킵)
        for (int i = 1; i < lines.Length; i++)
        {
            // 비어있는 줄 스킵
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            // 쉼표로 칸 나누기
            string[] row = lines[i].Split(',');

            PlayerData data = new PlayerData();
            // 각 칸의 데이터를 타입에 맞게 파싱 (Trim으로 유령 공백 제거)
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

        Debug.Log($"[System] PlayerData 로드 완료. 총 {_player_dict.Count}개의 캐릭터가 등록됨.");
    }

    // 현재 게임 시간 정보를 받아 발생해야 할 웨이브 데이터를 반환하는 함수
    public PlayerData GetPlayerData(int id)
    {
        if (_player_dict.TryGetValue(id, out PlayerData data))
        {
            return data;
        }

        Debug.LogWarning($"[System] {id}번의 플레이어 데이터를 찾을 수 없습니다");
        return null;
    }
}