using System.Collections.Generic;
using UnityEngine;

public class TextDataManager : MonoBehaviour
{
    // 싱글톤 구현
    public static TextDataManager instance;

    // Key: key(description_key), Value: Header, description
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
        // csv 파일을 읽어와서 텍스트 덩어리로 만들어줌.
        TextAsset csv_data = Resources.Load<TextAsset>("TextData");

        if (csv_data == null)
        {
            Debug.LogError("[TextDataManager] TextData.csv 파일을 찾을 수 없습니다.");
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
            TextData data = new TextData();
            data.key = row[0].Trim();
            data.header = row[1].Trim();
            data.description = row[2].Trim();

            // 메모 데이터가 없을 경우를 대비하여 조건문 추가
            if (row.Length > 3) data.memo = row[3].Trim();

            _text_dict.Add(data.key, data);
        }

        Debug.Log($"[System] TextData 로드 완료. 총 데이터 개수: {_text_dict.Count}");
    }

    // description_key 값을 전달 받은 후 해당 key의 Header와 description 내용을 반환하는 함수
    public TextData GetText(string key)
    {
        if (_text_dict.TryGetValue(key, out TextData data))
        {
            return data;
        }

        Debug.LogError($"[TextDataManager] '{key}'에 해당하는 텍스트를 찾을 수 없습니다.");
        return null;
    }
}