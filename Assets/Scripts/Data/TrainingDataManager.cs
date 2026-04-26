using System.Collections.Generic;
using UnityEngine;

public class TrainingDataManager : MonoBehaviour
{
    // 싱글톤 구현
    public static TrainingDataManager instance;

   // 파싱된 데이터를 담아둘 딕셔너리 (key 값은 id)
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
        // Resources/TrainingData.csv 파일을 읽어옴
        TextAsset csv_data = Resources.Load<TextAsset>("TrainingData");

        if (csv_data == null)
        {
            Debug.LogError("[System] TrainingData.csv 파일을 찾을 수 없습니다.");
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

            TrainingData data = new TrainingData();
            // 각 칸의 데이터를 타입에 맞게 파싱 (Trim으로 유령 공백 제거)
            data.id = int.Parse(row[0]);
            data.name = row[1];
            data.upgrade_value = float.Parse(row[2]);
            data.base_cost = int.Parse(row[3]);
            data.cost_mult = int.Parse(row[4]);
            data.max_level = int.Parse(row[5]);
            data.desc_key = row[6].Trim();

            _training_dict.Add(data.id, data);
        }
        
        Debug.Log($"[System] TrainingData 로드 완료. 총 {_training_dict.Count}개의 데이터가 저장되었습니다.");
    }

    // 데이터를 가져올 때 사용하는 함수
    public TrainingData GetTrainingData(int id)
    {
        if (_training_dict.TryGetValue(id, out TrainingData data))
        {
            return data;
        }

        Debug.LogWarning($"[System] {id}번의 강화 데이터를 찾을 수 없습니다.");
        return null;
    }

    // 모든 데이터를 가져올 때 사용하는 함수
    public List<TrainingData> GetAllTrainingData()
    {
        return new List<TrainingData>(_training_dict.Values);
    }
}