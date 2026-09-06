using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrainingSlot : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text training_name_text; // TrainingName 연결용
    public Image training_icon_image;   // TrainingIcon 연결용
    public Transform level_group;  // 체크박스가 담길 부모 컴포넌트

    [Header("Prefabs & Sprites")]  
    public GameObject dot_prefab;    // 체크박스 낱개 프리팹
    public Sprite empty_box;         // 빈 체크박스 이미지
    public Sprite checked_box;       // 체크된 체크박스 이미지

    private int _id;

    // 슬롯 초기화
    public void Init(int id)
    {
        _id = id;
        UpdateUI();
    }

    // 강화 상태 반영
    public void UpdateUI()
    {
        var data = TrainingDataManager.instance.GetTrainingData(_id);
        int current_level = GameDataManager.instance.GetLevelByID(_id);

        if (data == null) return;

        
        training_name_text.text = data.name;  // 이름 반영
        training_icon_image.sprite = Resources.Load<Sprite>($"Icons/{data.name}");  // 아이콘 반영
        
        // 체크박스 반영
        foreach (Transform child in level_group) Destroy(child.gameObject);

        for (int i = 0; i < data.max_level; i++)
        {
            GameObject dot = Instantiate(dot_prefab, level_group);
            Image dot_img = dot.GetComponent<Image>();
            
            // 현재 레벨만큼 체크된 체크박스 이미지, 나머진 빈 체크박스 이미지 부여
            dot_img.sprite = (i < current_level) ? checked_box : empty_box;
        }
    }

    // 슬롯 클릭 시 호출
    public void OnClickSlot()
    {
        // TrainingHandler한테 클릭된 슬롯의 id를 알림
        SendMessageUpwards("OnSelectSlot", _id);
    }

    // 본인의 id를 반환하는 함수
    public int GetID()
    {
        return _id; 
    }
}
