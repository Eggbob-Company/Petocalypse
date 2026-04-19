using System.Collections.Generic;
using UnityEngine;

public class WaveDataManager : MonoBehaviour
{
    // 싱글톤 구현
    // ObjectPoolManager에 해당 구현이 되어있어서 오브젝트 풀링을 사용하지 않는 데이터에만 추가
    public static WaveDataManager instance;

    // 시간순으로 정렬하여 저장할 리스트
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
        TextAsset csv_data = Resources.Load<TextAsset>("WaveData");

        if (csv_data == null)
        {
            Debug.LogError("[System] WaveData.csv 파일을 찾을 수 없습니다.");
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

            WaveData data = new WaveData();
            // 각 칸의 데이터를 타입에 맞게 파싱 (Trim으로 유령 공백 제거)
            data.time_min = int.Parse(row[0].Trim());
            data.time_sec = int.Parse(row[1].Trim());
            data.enemy_id = int.Parse(row[2].Trim());
            data.amount = int.Parse(row[3].Trim());
            data.interval = float.Parse(row[4].Trim());
            
            _wave_list.Add(data);
        }

        // 혹시 모르니 시간순(SpawnTime)으로 정렬
        _wave_list.Sort((a, b) => a.SpawnTime.CompareTo(b.SpawnTime));

        Debug.Log($"[System] WaveData 로드 완료. 총 {_wave_list.Count}개의 웨이브가 등록됨.");
    }

    // 현재 게임 시간 정보를 받아 발생해야 할 웨이브 데이터를 반환하는 함수
    public List<WaveData> GetWavesForTime(int currentTime)
    {
        // 실제 스포너에서 사용할 로직 (해당 시간에 맞는 데이터를 리턴)
        return _wave_list.FindAll(x => x.SpawnTime == currentTime);
    }
}