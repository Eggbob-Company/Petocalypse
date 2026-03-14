using System.Collections.Generic;
using UnityEngine;

public class TextDataManager : MonoBehaviour
{
    // 싱글톤 구현
    // ObjectPoolManager에 해당 구현이 되어있어서 오브젝트 풀링을 사용하지 않는 데이터에만 추가
    public static TextDataManager instance;

    // 키(string)를 통해 TextData 객체 전체를 관리
    private Dictionary<string, TextData> _text_dict = new Dictionary<string, TextData>();

    void Awake()
    {
        // 싱글톤 초기화 및 중복 방지
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        LoadTextData();
    }

    void LoadTextData()
    {
        TextAsset csv_data = Resources.Load<TextAsset>("TextData");

        if (csv_data == null)
        {
            Debug.LogError("[System] TextData.csv 파일을 찾을 수 없습니다.");
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

            TextData data = new TextData();
            // 각 칸의 데이터를 타입에 맞게 파싱 (Trim으로 유령 공백 제거)
            data.key = row[0].Trim();
            data.header = row[1].Trim();
            data.description = row[2].Trim();

            // 메모 데이터가 없을 경우를 대비한 처리
            if (row.Length > 3) data.memo = row[3].Trim();

            _text_dict.Add(data.key, data);
        }

        Debug.Log($"[System] TextData 로드 완료. 총 {_text_dict.Count}개의 문구 저장됨.");
    }

    // 현재 게임 시간 정보를 받아 발생해야 할 웨이브 데이터를 반환하는 함수
    public TextData GetText(string key)
    {
        if (_text_dict.TryGetValue(key, out TextData data))
        {
            return data;
        }

        Debug.LogWarning($"[System] '{key}' 에 해당하는 텍스트를 찾을 수 없습니다.");
        return null;
    }
}