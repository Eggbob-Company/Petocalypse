using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // 씬 이동을 위해 추가

public class ReinforceHandler : MonoBehaviour
{
    [Header("UI References")]
    // 보유 골드
    public TMP_Text gold_text;
    // hp 강화 관련
    public TMP_Text upgrade_hp_text;
    public TMP_Text hp_need_gold_text;
    public Button upgrade_hp_button;
    // speed 강화 관련
    public TMP_Text upgrade_speed_text;
    public TMP_Text speed_need_gold_text;
    public Button upgrade_speed_button;
    
    [Header("Settings")]
    public int hp_upgrade_cost;
    public int speed_upgrade_cost;

    void Start()
    {
        UpdateCost(); // 강화에 필요한 비용 호출
        UpdateUI(); // 시작하자마자 UI 갱신
    }

    // 로비로 이동
    public void OnClickLobby()
    {
        // 실제 넘어가고 싶은 씬의 이름
        SceneManager.LoadScene("Lobby");
    }

    // 강화에 필요한 비용을 계산하는 함수 (강화할 때 마다 50원씩 증가)
    void UpdateCost()
    {
        // hp 강화 비용 계산
        if (GameDataManager.instance != null) hp_upgrade_cost = 100 + (GameDataManager.instance.bonus_max_hp * 5);
        // speed 강화 비용 계산
        if (GameDataManager.instance != null) speed_upgrade_cost = 100 + (GameDataManager.instance.bonus_speed * 50);

    }

    // HP 강화 로직
    public void OnClickUpgradeHP()
    {
        // 보유 골드량이 강화에 필요한 돈보다 많은지 확인
        // 아래 골드가 부족하면 버튼 비활성화를 해놨기에 굳이 필요하진 않지만 혹시모를 상황을 대비하기 위해 추가함
        if (GameDataManager.instance.gold >= hp_upgrade_cost)
        {
            // 강화에 소모된 골드만큼 보유 골드량 감소 및 체력 증가량 10 증가
            GameDataManager.instance.gold -= hp_upgrade_cost;
            GameDataManager.instance.bonus_max_hp += 10;
            
            // 변경사항 저장
            GameDataManager.instance.SaveData();

            // 강화 비용 및 화면 갱신
            UpdateCost();
            UpdateUI();
            Debug.Log("강화 성공!");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    // Speed 강화 로직
    public void OnClickUpgradeSpeed()
    {
        // 보유 골드량이 강화에 필요한 돈보다 많은지 확인
        // 아래 골드가 부족하면 버튼 비활성화를 해놨기에 굳이 필요하진 않지만 혹시모를 상황을 대비하기 위해 추가함
        if (GameDataManager.instance.gold >= speed_upgrade_cost)
        {
            // 강화에 소모된 골드만큼 보유 골드량 감소 및 이동 속도 1 증가
            GameDataManager.instance.gold -= speed_upgrade_cost;
            GameDataManager.instance.bonus_speed += 1;
            
            // 변경사항 저장
            GameDataManager.instance.SaveData();

            // 강화 비용 및 화면 갱신
            UpdateCost();
            UpdateUI();
            Debug.Log("강화 성공!");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    // 화면의 텍스트들 새로고침
    public void UpdateUI()
    {
        gold_text.text = $"보유 골드: {GameDataManager.instance.gold}G";

        upgrade_hp_text.text = $"추가 HP: +{GameDataManager.instance.bonus_max_hp}";
        hp_need_gold_text.text = $"필요 골드: {hp_upgrade_cost}G";

        upgrade_speed_text.text = $"추가 Speed: +{GameDataManager.instance.bonus_speed}";
        speed_need_gold_text.text = $"필요 골드: {speed_upgrade_cost}G";
        
        // (보너스) 돈 없으면 버튼 비활성화 시키기
        upgrade_hp_button.interactable = (GameDataManager.instance.gold >= hp_upgrade_cost);
        upgrade_speed_button.interactable = (GameDataManager.instance.gold >= speed_upgrade_cost);
    }

    // 테스트용: 골드 획득
    public void AddGold()
    {
        GameDataManager.instance.gold += 100;
        GameDataManager.instance.SaveData();
        UpdateUI();
        Debug.Log("골드 100 획득!");
    }

    // 테스트용: 증가된 스탯들과 Gold 초기화
    public void ResetData()
    {
        GameDataManager.instance.gold = 0;
        GameDataManager.instance.bonus_max_hp = 0;
        GameDataManager.instance.bonus_speed = 0;
        GameDataManager.instance.SaveData();
        UpdateCost();
        UpdateUI();
        Debug.Log("데이터 초기화");
    }

}
