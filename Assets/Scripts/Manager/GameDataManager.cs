using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    // 싱글톤 구현
    public static GameDataManager instance;

    [Header("Resources")]
    public int gold; // 보유 골드

    [Header("Training Levels")]
    // 실제 스탯 수치가 아닌 레벨을 저장
    public int level_hp;        // ID 100
    public int level_speed;     // ID 200
    public int level_recovery;  // ID 300
    public int level_might;     // ID 400

    void Awake()
    {
        // 싱글톤 없으면 생성
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad -> 씬이 변경될 때 오브젝트가 삭제되는 것을 방지
            DontDestroyOnLoad(gameObject);
            LoadData(); // 게임 시작 시 저장된 데이터를 불러옵니다.
        }
        else { Destroy(gameObject); }  // 있으면 제거하여 중복 방지
    }

    // id를 파라미터로 사용하여 각 훈련 항목들의 현재 레벨을 반환
    public int GetLevelByID(int id)
    {
        return id switch
        {
            100 => level_hp,
            200 => level_speed,
            300 => level_recovery,
            400 => level_might,
            _ => 0
        };
    }

    // id와 level을 파라미터로 사용하여 각 훈련 항목들의 레벨을 설정
    public void SetLevelByID(int id, int level)
    {
        if (id == 100) level_hp = level;
        else if (id == 200) level_speed = level;
        else if (id == 300) level_recovery = level;
        else if (id == 400) level_might = level;
    }

    // csv 데이터를 참고하여 보너스 스탯을 계산
    public float GetStatValue(int id)
    {
        var data = TrainingDataManager.instance.GetTrainingData(id);
        if (data == null) return 0f;  // 데이터가 없으면 예외 처리
        
        int current_level = GetLevelByID(id);
        
        // 보너스 스탯 수치 = (훈련 레벨 * 훈련 시 증가 수치)
        return current_level * data.upgrade_value;
    }

    // 데이터를 기기에 저장
    public void SaveData()
    {
        PlayerPrefs.SetInt("TotalGold", gold);
        PlayerPrefs.SetInt("BonusHP", level_hp);
        PlayerPrefs.SetInt("BonusSpeed", level_speed);
        PlayerPrefs.SetInt("BonusRecovery", level_recovery);
        PlayerPrefs.SetInt("BonusMight", level_might);
        PlayerPrefs.Save();
        Debug.Log("데이터 저장 완료!");
    }

    // 데이터를 기기에서 불러오기
    public void LoadData()
    {
        // 저장된 값이 없으면 0을 로드
        gold = PlayerPrefs.GetInt("TotalGold", 0);
        level_hp = PlayerPrefs.GetInt("BonusHP", 0);
        level_speed = PlayerPrefs.GetInt("BonusSpeed", 0);
        level_recovery = PlayerPrefs.GetInt("BonusRecovery", 0);
        level_might = PlayerPrefs.GetInt("BonusMight", 0);
        Debug.Log($"데이터 로드 완료!");
    }

}
