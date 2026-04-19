using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    // 싱글톤 구현
    public static GameDataManager instance;

    [Header("--- Meta Data ---")]
    public int gold = 0; // 보유 골드
    public int bonus_max_hp = 0; // 강화된 HP
    public int bonus_speed = 0; // 강화된 Speed

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

    // 데이터를 기기에 저장
    public void SaveData()
    {
        PlayerPrefs.SetInt("TotalGold", gold);
        PlayerPrefs.SetInt("BonusHP", bonus_max_hp);
        PlayerPrefs.SetInt("BonusSpeed", bonus_speed);
        PlayerPrefs.Save();
        Debug.Log("데이터 저장 완료!");
    }

    // 데이터를 기기에서 불러오기
    public void LoadData()
    {
        // 저장된 값이 없으면 0을 로드
        gold = PlayerPrefs.GetInt("TotalGold", 0);
        bonus_max_hp = PlayerPrefs.GetInt("BonusHP", 0);
        bonus_speed = PlayerPrefs.GetInt("BonusSpeed", 0);
        Debug.Log($"데이터 로드 완료!");
    }

}
