using System.Collections.Generic;
using UnityEngine;

public class SkillDataManager : MonoBehaviour
{
    // 싱글톤 구현
    // ObjectPoolManager에 해당 구현이 되어있어서 오브젝트 풀링을 사용하지 않는 데이터에만 추가
    public static SkillDataManager instance;

    // 키 값으로 데이터 저장
    // 키 값은 "ID_Level" 형태로 저장 (예: "100_1")
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
        // Resources/SkillData.csv 파일을 읽어옴
        TextAsset csv_data = Resources.Load<TextAsset>("SkillData");

        if (csv_data == null)
        {
            Debug.LogError("[System] SkillData.csv 파일을 찾을 수 없습니다.");
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

            SkillData data = new SkillData();
            // 각 칸의 데이터를 타입에 맞게 파싱 (Trim으로 유령 공백 제거)
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

            // "ID_레벨" 형태의 키 생성 (예: "100_1")
            string key = $"{data.id}_{data.level}";
            _skill_dict.Add(key, data);
        }
        
        Debug.Log($"[System] SkillData 로드 완료. 총 {_skill_dict.Count}개의 데이터가 저장되었습니다.");
    }

    // 데이터를 가져올 때 사용하는 함수
    public SkillData GetSkillData(int id, int level)
    {
        string key = $"{id}_{level}";
        if (_skill_dict.TryGetValue(key, out SkillData data))
        {
            return data;
        }

        Debug.LogWarning($"[System] {id}번의 {level}레벨 스킬 데이터를 찾을 수 없습니다.");
        return null;
    }
}