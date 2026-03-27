using System.Collections.Generic;
using UnityEngine;

public class ExpDataManager : MonoBehaviour
{
    // 싱글톤 구현
    // ObjectPoolManager에 해당 구현이 되어있어서 오브젝트 풀링을 사용하지 않는 데이터에만 추가
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
        TextAsset csv_data = Resources.Load<TextAsset>("ExpData");

        if (csv_data == null)
        {
            Debug.LogError("[System] ExpData.csv 파일을 찾을 수 없습니다.");
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

            // 각 칸의 데이터를 타입에 맞게 파싱 (Trim으로 유령 공백 제거)
            int level = int.Parse(row[0].Trim());
            int reqExp = int.Parse(row[1].Trim());
            
            _exp_dict.Add(level, reqExp);
        }

        Debug.Log($"[System] ExpData 로드 완료. 최대 레벨: {_exp_dict.Count}");
    }

    // 현재 게임 시간 정보를 받아 발생해야 할 웨이브 데이터를 반환하는 함수
    public int GetRequiredExp(int currentLevel)
    {
        if (_exp_dict.TryGetValue(currentLevel, out int exp))
        {
            return exp;
        }

        // 데이터가 없다면 만렙으로 간주하거나 아주 큰 값 반환
        return int.MaxValue;
    }
}