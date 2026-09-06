using System.Collections.Generic;
using UnityEngine;

public class TrainingDataManager : MonoBehaviour
{
    // 싱글톤 구현
    public static TrainingDataManager instance;

    // Key: id, Value: 해당 훈련 항목의 data
    private Dictionary<int, TrainingData> _training_dict = new Dictionary<int, TrainingData>();


    void Awake()
    {
        // 싱글톤 없으면 생성
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad -> 씬이 변경될 때 오브젝트가 삭제되는 것을 방지
            DontDestroyOnLoad(gameObject);
            LoadTrainingData(); // 게임 시작 시 저장된 데이터를 불러옵니다.
        }
        else { Destroy(gameObject); }  // 있으면 제거하여 중복 방지
    }

    void LoadTrainingData()
    {
        // csv 파일을 읽어와서 텍스트 덩어리로 만들어줌.
        TextAsset csv_data = Resources.Load<TextAsset>("TrainingData");

        if (csv_data == null)
        {
            Debug.LogError("[TrainingDataManager] TrainingData.csv 파일을 찾을 수 없습니다.");
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
            TrainingData data = new TrainingData();
            data.id = int.Parse(row[0]);
            data.name = row[1];
            data.upgrade_value = float.Parse(row[2]);
            data.base_cost = int.Parse(row[3]);
            data.cost_mult = int.Parse(row[4]);
            data.max_level = int.Parse(row[5]);
            data.desc_key = row[6].Trim();

            _training_dict.Add(data.id, data);
        }
        
        Debug.Log($"[System] TrainingData 로드 완료. 총 데이터 개수: {_training_dict.Count}");
    }

    // 특정 ID 값을 전달 받은 후 해당 ID의 훈련 데이터를 반환하는 함수
    public TrainingData GetTrainingData(int id)
    {
        if (_training_dict.TryGetValue(id, out TrainingData data))
        {
            return data;
        }

        Debug.LogError($"[TrainingDataManager] {id}번의 강화 데이터를 찾을 수 없습니다.");
        return null;
    }

    // 모든 훈련 데이터를 가져올 때 사용하는 함수
    public List<TrainingData> GetAllTrainingData()
    {
        return new List<TrainingData>(_training_dict.Values);
    }
}