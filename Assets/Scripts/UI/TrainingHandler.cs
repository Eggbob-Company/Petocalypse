using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // 씬 이동을 위해 추가

public class TrainingHandler : MonoBehaviour
{
    [Header("UI References")]
    // 보유 골드
    public TMP_Text total_gold_text;
    // 훈련 슬롯 생성
    public Transform slot_parent;   // 슬롯들이 생성될 부모 오브젝트
    public GameObject slot_prefab;  // TrainingSlot 프리팹
    
    [Header("Detail Panel")]
    public TMP_Text detail_name_text;
    public TMP_Text detail_desc_text;
    public TMP_Text detail_cost_text;
    public TMP_Text detail_value_text;
    public Image detail_icon_image;
    public Button upgrade_button;

    private int _selected_id = -1;

    void Start()
    {
        CreateSlots(); // 강화에 필요한 비용 호출
        UpdateTotalUI(); // 시작하자마자 UI 갱신
    }

    // 로비로 이동
    public void OnClickLobby()
    {
        // 실제 넘어가고 싶은 씬의 이름
        SceneManager.LoadScene("Lobby");
    }

    // csv 데이터를 기반으로 강화 슬롯들을 생성
    public void CreateSlots()
    {
        // 기존 슬롯 제거
        foreach (Transform child in slot_parent) Destroy(child.gameObject);

        // TrainingDataManager에서 모든 데이터를 가져와 슬롯 생성
        var all_data = TrainingDataManager.instance.GetAllTrainingData();

        // 첫 번째로 생성된 슬롯 확인용 불 변수
        bool is_first = true;

        foreach (var data in all_data)
        {
            GameObject go = Instantiate(slot_prefab, slot_parent);
            // 생성된 슬롯의 Init 함수 호출
            go.GetComponent<TrainingSlot>().Init(data.id);

            if (is_first)
            {
                OnSelectSlot(data.id);
                is_first = false;
            }
        }
    }

    // 특정 슬롯이 클릭됐을 때 호출
    public void OnSelectSlot(int id)
    {
        _selected_id = id;
        RefreshDetailPanel();
    }

    // 디테일 패널 정보 갱신
    public void RefreshDetailPanel()
    {
        if (_selected_id == -1) return;

        var data = TrainingDataManager.instance.GetTrainingData(_selected_id);
        int current_level = GameDataManager.instance.GetLevelByID(_selected_id);

        detail_name_text.text = data.name;
        detail_icon_image.sprite = Resources.Load<Sprite>($"Icons/{data.name}");

        float training_value = GameDataManager.instance.GetStatValue(_selected_id);

        switch (_selected_id)
        {
            case 100: // 최대 체력
                detail_value_text.text = $"최대 체력 {training_value} 증가";
                detail_desc_text.text = "훈련 진행 시 최대 체력 10 증가\n(최대 50)";
                break;
            case 200: // 이동속도
                detail_value_text.text = $"이동속도 {training_value} 증가";
                detail_desc_text.text = "훈련 진행 시 이동속도 1 증가\n(최대 3)";
                break;
            case 300: // 체력 재생
                detail_value_text.text = $"초당 체력 {training_value} 재생";
                detail_desc_text.text = "훈련 진행 시 초댕 체력 0.2 회복\n(최대 초당 1)";
                break;
            case 400: // 공격력
                detail_value_text.text = $"공격력 {training_value * 100}% 증가";
                detail_desc_text.text = "훈련 진행 시 공격력 20% 증가\n(최대 100%)";
                break;
        }
        
        // 최대 강화 확인
        if (current_level >= data.max_level)
        {
            detail_cost_text.text = "MAX";
            upgrade_button.interactable = false;
        }
        else
        {
            int cost = data.base_cost + (current_level * data.cost_mult);
            detail_cost_text.text = cost.ToString();
            upgrade_button.interactable = (GameDataManager.instance.gold >= cost);
        }
    }

    // 훈련 버튼 클릭 시 실행
    public void OnClickTraining()
    {
        if (_selected_id == -1) return;

        var data = TrainingDataManager.instance.GetTrainingData(_selected_id);
        int current_level = GameDataManager.instance.GetLevelByID(_selected_id);
        int cost = data.base_cost + (current_level * data.cost_mult);

        if (GameDataManager.instance.gold >= cost)
        {
            GameDataManager.instance.gold -= cost;
            GameDataManager.instance.SetLevelByID(_selected_id, current_level + 1);
            GameDataManager.instance.SaveData();

            UpdateTotalUI();
            RefreshDetailPanel();

            var all_slots = slot_parent.GetComponentsInChildren<TrainingSlot>();
            foreach (var slot in all_slots)
            {
                // 현재 선택된 슬롯의 체크박스 탐색 후 해당 슬롯만 새로고침
                if (slot.GetID() == _selected_id) 
                {
                    slot.UpdateUI();
                    break;
                }
            }
        }
    }

    // 보유 골드 새로고침
    public void UpdateTotalUI()
    {
        total_gold_text.text = $"{GameDataManager.instance.gold}";
    }

    // 테스트용: 골드 획득
    public void AddGold()
    {
        GameDataManager.instance.gold += 100;
        GameDataManager.instance.SaveData();
        UpdateTotalUI();
        RefreshDetailPanel();
        Debug.Log("골드 100 획득!");
    }

    // 테스트용: 증가된 스탯들과 Gold 초기화
    public void ResetData()
    {
        GameDataManager.instance.level_hp = 0;
        GameDataManager.instance.level_speed = 0;
        GameDataManager.instance.level_recovery = 0;
        GameDataManager.instance.level_might = 0;
        GameDataManager.instance.SaveData();
        UpdateTotalUI();
        RefreshDetailPanel();
        CreateSlots();
        Debug.Log("데이터 초기화");
    }

}
