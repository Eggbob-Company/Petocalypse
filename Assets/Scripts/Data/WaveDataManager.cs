using System.Collections.Generic;
using UnityEngine;

public class WaveDataManager : MonoBehaviour
{
    // 싱글톤 구현
    public static WaveDataManager instance;

    // Key: 현재 시간, Value: 소환될 enemy 정보
    private List<WaveData> _wave_list = new List<WaveData>();

    void Awake()
    {
        // 싱글톤 초기화 및 중복 방지
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        LoadWaveData();
    }

    void LoadWaveData()
    {
        // csv 파일을 읽어와서 텍스트 덩어리로 만들어줌.
        TextAsset csv_data = Resources.Load<TextAsset>("WaveData");

        if (csv_data == null)
        {
            Debug.LogError("[WaveDataManager] WaveData.csv 파일을 찾을 수 없습니다.");
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
            WaveData data = new WaveData();
            data.time_min = int.Parse(row[0].Trim());
            data.time_sec = int.Parse(row[1].Trim());
            data.enemy_id = int.Parse(row[2].Trim());
            data.amount = int.Parse(row[3].Trim());
            data.interval = float.Parse(row[4].Trim());
            
            _wave_list.Add(data);
        }

        // 혹시 모르니 시간순(SpawnTime)으로 정렬
        _wave_list.Sort((a, b) => a.SpawnTime.CompareTo(b.SpawnTime));

        Debug.Log($"[System] WaveData 로드 완료. 총 데이터 개수: {_wave_list.Count}");
    }

    // 현재 게임 시간 정보를 전달 받은 후 발생해야 할 웨이브 데이터를 반환하는 함수
    public List<WaveData> GetWavesForTime(int currentTime)
    {
        return _wave_list.FindAll(x => x.SpawnTime == currentTime);
    }
}