using System.Collections.Generic;
using UnityEngine;

public class SkillDataManager : MonoBehaviour
{
    // 싱글톤 구현
    public static SkillDataManager instance;

    // Key: id_level, Value: 해당 skill data
    private Dictionary<string, SkillData> _skill_dict = new Dictionary<string, SkillData>();

    void Awake()
    {
        // 싱글톤 초기화 및 중복 방지
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        LoadSkillData();
    }

    void LoadSkillData()
    {
        // csv 파일을 읽어와서 텍스트 덩어리로 만들어줌.
        TextAsset csv_data = Resources.Load<TextAsset>("SkillData");

        if (csv_data == null)
        {
            Debug.LogError("[SkillDataManager] SkillData.csv 파일을 찾을 수 없습니다.");
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
            SkillData data = new SkillData();
            data.id          = int.Parse(row[0].Trim());
            data.level       = int.Parse(row[1].Trim());
            data.damage      = float.Parse(row[2].Trim());
            data.cooldown    = float.Parse(row[3].Trim());
            data.count       = int.Parse(row[4].Trim());
            data.speed       = float.Parse(row[5].Trim());
            data.area        = float.Parse(row[6].Trim());
            data.range       = float.Parse(row[7].Trim());
            data.penetrate   = int.Parse(row[8].Trim());
            data.duration    = float.Parse(row[9].Trim());
            data.attack_type = (AttackType)int.Parse(row[10].Trim());
            data.prefab_name = row[11].Trim();
            data.desc_key    = row[12].Trim();
            data.icon_name    = row[13].Trim();

            // "ID_레벨" 형태의 키 생성 (예: "100_1")
            string key = $"{data.id}_{data.level}";

            _skill_dict.Add(key, data);
        }
        
        Debug.Log($"[System] SkillData 로드 완료. 총 데이터 개수: {_skill_dict.Count}");
    }

    // id와 level 정보를 받은 후 해당 skill 데이터를 반환하는 함수
    public SkillData GetSkillData(int id, int level)
    {
        string key = $"{id}_{level}";
        if (_skill_dict.TryGetValue(key, out SkillData data))
        {
            return data;
        }

        Debug.LogError($"[SkillDataManager] {id}번 Skill의 {level}레벨 데이터를 찾을 수 없습니다.");
        return null;
    }

    // 모든 고유 스킬 ID 리스트를 반환하는 함수
    public List<int> GetAllSkillIds()
    {
        HashSet<int> unique_ids = new HashSet<int>();

        // 딕셔너리의 모든 값(SkillData)을 돌면서 ID만 수집
        foreach (var data in _skill_dict.Values)
        {
            unique_ids.Add(data.id);
        }

        // HashSet을 List로 변환해서 반환 (HashSet은 자동으로 중복을 제거)
        return new List<int>(unique_ids);
    }
}