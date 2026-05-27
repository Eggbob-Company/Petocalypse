using System.Collections.Generic;
using UnityEngine;

public class ExpDataManager : MonoBehaviour
{
    // 싱글톤 구현
    public static ExpDataManager instance;

    // Key: 현재 레벨, Value: 다음 레벨의 요구 경험치
    private Dictionary<int, int> _exp_dict = new Dictionary<int, int>();

    void Awake()
    {
        // 싱글톤 초기화 및 중복 방지
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        LoadExpData();
    }

    void LoadExpData()
    {
        // csv 파일을 읽어와서 텍스트 덩어리로 만들어줌.
        TextAsset csv_data = Resources.Load<TextAsset>("ExpData");

        if (csv_data == null)
        {
            Debug.LogError("[ExpDataManager] ExpData.csv 파일을 찾을 수 없습니다.");
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
            int level = int.Parse(row[0].Trim());
            int reqExp = int.Parse(row[1].Trim());
            
            _exp_dict.Add(level, reqExp);
        }

        Debug.Log($"[System] ExpData 로드 완료. 총 데이터 개수: {_exp_dict.Count}");
    }

    // 현재 레벨 정보를 전달 받은 후 다음 레벨까지 필요한 경험치를 반환하는 함수
    public int GetRequiredExp(int currentLevel)
    {
        if (_exp_dict.TryGetValue(currentLevel, out int exp))
        {
            return exp;
        }

        Debug.LogError($"[ExpDataManager] {currentLevel}레벨에서 레벨 업까지 필요한 경험치 데이터를 찾을 수 없습니다");
        return int.MaxValue;
    }
}