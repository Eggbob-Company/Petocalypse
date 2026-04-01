using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpPopUp : MonoBehaviour
{
    public GameObject level_up_pop_up; // LevelUpPopUp 오브젝트 자신
    
    [Header("Slots (0~2: Skill / 3~4: FullLevel)")]
    public GameObject[] all_slots; // 인스펙터에서 5개를 순서대로 연결

    public void Show()
    {
        level_up_pop_up.SetActive(true);
        Time.timeScale = 0f; // 게임 일시정지

        // 모든 슬롯 초기화 (비활성화)
        foreach (GameObject slot in all_slots) slot.SetActive(false);

        // 만렙 여부에 따른 슬롯 활성화 분기
        if (InGameManager.instance.level > InGameManager.instance.max_level)
        {
            // 만렙 모드: FullLevel 1, 2번 슬롯만 활성화
            all_slots[3].SetActive(true);
            all_slots[4].SetActive(true);
        }
        else
        {
            // 일반 모드: Skill 1, 2, 3번 슬롯 활성화
            all_slots[0].SetActive(true);
            all_slots[1].SetActive(true);
            all_slots[2].SetActive(true);
        }
    }

    // 모든 버튼의 On Click() 이벤트에 이 함수를 연결
    public void OnSelect()
    {
        // 1. 누적된 팝업 횟수 하나 차감
        InGameManager.instance.pending_level_up_count--;

        // 2. 남은 팝업이 있는지 체크
        if (InGameManager.instance.pending_level_up_count > 0)
        {
            // 아직 남았다면 다시 Show (새로운 랜덤 데이터를 뽑는 로직이 여기 들어갈 예정)
            Show();
        }
        else
        {
            // 다 끝났다면 닫고 게임 재개
            Close();
        }
    }

    public void Close()
    {
        level_up_pop_up.SetActive(false);
        Time.timeScale = 1f; // 게임 재개
    }
}
