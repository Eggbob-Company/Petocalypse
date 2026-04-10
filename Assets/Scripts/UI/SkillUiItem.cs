using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUiItem : MonoBehaviour
{
    [Header("UI Components")]
    public Image image_icon;
    public TextMeshProUGUI text_name;
    public TextMeshProUGUI text_level;
    public TextMeshProUGUI text_desc;

    [Header("Badges")]
    public GameObject new_badge;
    public GameObject level_up_badge;

    private SkillData _skill_data;

    private PlayerAttack _player_attack; // 참조를 저장할 변수

    // Start는 게임 시작 시 딱 한 번 실행됩니다.
    void Start()
    {
        // 여기서 미리 찾아둡니다 (배선을 미리 연결해두는 과정)
        if (Player.instance != null)
        {
            _player_attack = Player.instance.GetComponent<PlayerAttack>();
        }
    }

    // 팝업창에서 데이터를 넣어줄 때 호출
    public void Init(SkillData skill_data)
    {
        _skill_data = skill_data;

        // 텍스트 데이터
        text_name.text = _skill_data.prefab_name; // SkillData.csv의 prefab_name 사용
        text_level.text = "Lv." + _skill_data.level;
        text_desc.text = _skill_data.desc_key;

        // 아이콘 이미지 로드 (Resources/Icons/)
        Sprite icon_sprite = Resources.Load<Sprite>("Icons/" + _skill_data.icon_name);
        if (icon_sprite != null)
        {
            image_icon.sprite = icon_sprite;
        }

        // 배지 상태 업데이트
        UpdateBadgeStatus(_skill_data.id);
    }

    private void UpdateBadgeStatus(int skill_id)
    {
        
        bool is_owned = false;

        if (_player_attack != null)
        {
            foreach (var slot in _player_attack.mySkills) //스킬창에 뜬 스킬과 내가 갖고 있는 스킬을 비교해서 배지 활성화
            {
                if (slot.skill_id == skill_id)
                {
                    is_owned = true;
                    break;
                }
            }
        }

        // 처음 고르는 거면 New 활성화, 이미 있으면 LevelUp 활성화
        if (new_badge != null) new_badge.SetActive(!is_owned);
        if (level_up_badge != null) level_up_badge.SetActive(is_owned);
    }

    // 버튼 클릭 이벤트
    public void OnClickSkill()
    {
        
        if (_player_attack != null)
        {
            // PlayerAttack 리스트에 스킬 추가 또는 레벨업 수행
            _player_attack.AddOrUpgradeSkill(_skill_data.id);
            
            // 선택이 끝났으니 LevelUpPopUp.cs에 OnSelect를 호출해서 남은 로직 수행
            LevelUpPopUp.instance.OnSelect();
        }
    }
}