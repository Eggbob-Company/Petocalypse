using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpPopUp : MonoBehaviour
{
    public static LevelUpPopUp instance;
    public GameObject level_up_pop_up; 
    
    [Header("Slots (0~2: Skill / 3~4: FullLevel)")]
    public GameObject[] all_slots; 

    // 가중치 계산을 위한 구조체
    private struct SkillWeightInfo
    {
        public SkillData data;
        public float weight;
        public SkillWeightInfo(SkillData data, float weight) { this.data = data; this.weight = weight; }
    }

    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void Show()
    {
        level_up_pop_up.SetActive(true);
        Time.timeScale = 0f; 

        foreach (GameObject slot in all_slots) slot.SetActive(false);

        if (InGameManager.instance.is_max_reward_trigger)
        {
            // 만렙 모드: 3, 4번 슬롯만 활성화
            all_slots[3].SetActive(true);
            all_slots[4].SetActive(true);

            all_slots[3].GetComponent<SkillUIItem>().InitMaxLevel(0); // 골드 슬롯
            all_slots[4].GetComponent<SkillUIItem>().InitMaxLevel(1); // 체력 슬롯

        }
        else
        {
            // 일반 스킬을 골라야 할 때는 가중치를 반영해 랜덤 추출된 스킬 정보를 각 슬롯에 삽입
            List<SkillData> selected_skills = GetWeightedRandomSkills(3);

            for (int i = 0; i < selected_skills.Count; i++)
            {
                all_slots[i].SetActive(true);
                // 각 슬롯에 붙어있는 SkillUIItem 컴포넌트를 가져와서 초기화
                SkillUIItem item = all_slots[i].GetComponent<SkillUIItem>();
                if (item != null)
                {
                    item.Init(selected_skills[i]);
                }
            }
        }
    }

    // 가중치 기반 랜덤 스킬 리스트 반환
    private List<SkillData> GetWeightedRandomSkills(int count)
    {
        List<SkillData> result_list = new List<SkillData>();
        List<SkillWeightInfo> candidates = new List<SkillWeightInfo>();

        // 모든 스킬 후보군 조사
        foreach (int id in SkillDataManager.instance.GetAllSkillIds())
        {
            int current_lv = GetCurrentSkillLevel(id);
            int next_lv = current_lv + 1;

            // 다음 레벨 데이터 가져오기 (없으면 만렙이므로 패스)
            SkillData data = SkillDataManager.instance.GetSkillData(id, next_lv);
            if (data == null) continue;

            // 가중치 설정 (없던 거면 100, 있던 거면 50)
            float weight = (current_lv == 0) ? 100f : 50f;
            candidates.Add(new SkillWeightInfo(data, weight));
        }

        // 룰렛 휠 방식으로 중복 없이 count만큼 뽑기
        for (int i = 0; i < count && candidates.Count > 0; i++)
        {
            float total_weight = 0;
            foreach (var c in candidates) total_weight += c.weight;

            float pivot = Random.Range(0f, total_weight);
            float current_sum = 0;

            for (int j = 0; j < candidates.Count; j++)
            {
                current_sum += candidates[j].weight;
                if (pivot <= current_sum)
                {
                    result_list.Add(candidates[j].data);
                    candidates.RemoveAt(j); // 중복 방지 RemoveAt()은 C#에서 제공하는 함수, 특정값을 지워라
                    break;
                }
            }
        }
        return result_list;
    }

    // 플레이어가 현재 해당 스킬을 몇 레벨 가지고 있는지 확인
    private int GetCurrentSkillLevel(int id)
    {
        PlayerAttack player_attack = Player.instance.GetComponent<PlayerAttack>();
        if (player_attack == null) return 0;

        foreach (var slot in player_attack.mySkills)
        {
            if (slot.skill_id == id) return slot.level;
        }
        return 0;
    }

    public void OnSelect()
    {
        InGameManager.instance.pending_level_up_count--;

        if (InGameManager.instance.pending_level_up_count > 0)
        {
            Show(); // 다시 호출하여 새로운 랜덤 스킬 세팅
        }
        else
        {
            Close();
        }
    }

    public void Close()
    {
        level_up_pop_up.SetActive(false);
        Time.timeScale = 1f;
    }
}