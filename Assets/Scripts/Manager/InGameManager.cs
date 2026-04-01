using UnityEngine;

public class InGameManager : MonoBehaviour
{
    // 다른 스크립트에서 쉽게 접근할 수 있도록 싱글톤(인스턴스) 패턴 사용
    public static InGameManager instance;

    [Header("UI Connect")]
    public GameOverPopUp game_over_ui;
    public LevelUpPopUp level_up_ui;

    [Header("Level Data")]
    public int pending_level_up_count = 0; // 레벨 경험치가 한 번에 들어왔을 때, 팝업을 띄워야 하는 수

    [Header("Test Data")]
    public float exp = 0f;
    public int level = 1;
    public int max_level = 5; // 임시 만렙
    public int gold = 0;
    public int kill = 0;
    public float max_game_time = 300f; // 기본 5분 
    public float game_time = 0f;
    public float health;
    public float max_health;

    void Awake()
    {
        instance = this; // 이 스크립트 InGameManager를 인스턴스에 집어넣어서 찾기 쉽게
    }

    void Start()
    {
        // 플레이어의 체력 이벤트 구독
        Player.instance.health.OnHPChanged += UpdateHealthData;

        // 플레이어의 죽음 이벤트 구독 
        Player.instance.health.OnDead += ShowGameOverUI;

        // 초기 최대 체력, 현재 체력 값 세팅
        max_health = Player.instance.max_hp;
        health = Player.instance.health.CurrentHP;
    }

    // 경험치 획득 시 호출되는 함수
    public void GetExp(float get_exp)
    {
        exp += get_exp;

        // 레벨 업까지 필요한 경험치 가져오기
        int req_exp = ExpDataManager.instance.GetRequiredExp(level);

        // 초과된 경험치가 없을 때까지 반복
        while (exp >= req_exp)
        {
            // 만렙 미만이면 일반 레벨업 보상
            if (level < max_level)
            {
                LevelUp(req_exp);
            }
            // 만렙 이상이면 만렙 이후 레벨업 보상
            else
            {
                MaxLevelUP(req_exp);
            }

            pending_level_up_count++; // 팝업을 띄워야 할 횟수 누적
            req_exp = ExpDataManager.instance.GetRequiredExp(level); //다음 경험치 필요 요구량을 계산하기 위해 한 번 더 정리

            // 만렙 이후 경험치까지 다 소진했다면 탈출
            if (level >= max_level && exp < req_exp) break;
        }
        // 팝업이 아직 안 떠 있다면 첫 번째 팝업 호출
        if (pending_level_up_count > 0 && !level_up_ui.level_up_pop_up.activeSelf)
        {
            level_up_ui.Show();
        }
    }

    // 일반 레벨업 보상
    void LevelUp(int req_exp)
    {
        exp -= req_exp;
        level++;
        Debug.Log($"[System] 레벨업! 현재 레벨: {level}");

        // 스킬 선택 팝업 추가
    }

    // 만렙 이후 경험치 달성 보상
    void MaxLevelUP(int req_exp)
    {
        exp -= req_exp; 
        Debug.Log("[System] 만렙 이후 보상 획득!");

        // 골드 획득 or 체력 회복 팝업 추가
    }

    // 플레이어 체력 이벤트 구독 시 호출되는 함수
    void UpdateHealthData(float hp)
    {
        health = hp;
    }

    // 플레이어 사망 이벤트 구독 시 호출되는 함수
    void ShowGameOverUI()
    {
        if (game_over_ui != null)
        {
            game_over_ui.Show();  // GameOverPopUp.cs에서 팝업 호출
        }
    }

    // 몬스터가 죽을 때마다 호출되는 함수
    public void KillCount()
    {
        kill++;
    }

    void Update()
    {
        // 시간은 자동으로 흐르게
        game_time += Time.deltaTime;
        
    }
}