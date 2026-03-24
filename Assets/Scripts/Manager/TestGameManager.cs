using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    // 다른 스크립트에서 쉽게 접근할 수 있도록 싱글톤(인스턴스) 패턴 사용
    public static TestGameManager instance;

    [Header("UI Connect")]
    public GameOverPopUp game_over_ui;

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
        instance = this; // 이 스크립트 TestGameManager를 인스턴스에 집어넣어서 찾기 쉽게
    }

    void Start()
    {
        // 플레이어의 체력 이벤트 구독
        Player.instance.helath.OnHPChanged += UpdateHealthData;

        // 플레이어의 죽음 이벤트 구독 
        Player.instance.helath.OnDead += ShowGameOverUI;

        // 초기 최대 체력, 현재 체력 값 세팅
        max_health = Player.instance.max_hp;
        health = Player.instance.helath.CurrentHP;
    }

    // 경험치 획득 시 호출되는 함수
    public void GetExp(float get_exp)
    {
        exp += get_exp;

        // 레벨 업까지 필요한 경험치 가져오기
        int req_exp = ExpDataManager.instance.GetRequiredExp(level);

        // 경험치가 가득 찼다면 레벨업 실행
        if (exp >= req_exp)
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

    // 몬스터가 죽을 때 마다 호출되는 함수
    public void KillCount()
    {
        kill++;
    }

    void Update()
    {
        // 시간은 자동으로 흐르게
        game_time += Time.deltaTime;

        // 스페이스바를 누르면 몬스터를 한 마리 잡았다고 가정하고 수치를 올리기
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // exp += 5f;        // 경험치 5 획득
            gold += 150;      // 골드 150 획득
            kill += 1;        // 1킬 추가
            // health -= 10f;    // 몬스터 잡다가 체력 10 깎임

            // if (health <= 0)
            // {
            //     game_over_ui.Show();// GameOverPopUp.cs에서 팝업 호출
            // }

            // // 만약 경험치가 다음 레벨업 목표치를 넘었다면 레벨업
            // if (level < nextExp.Length && exp >= nextExp[level])
            // {
            //     exp -= nextExp[level]; // 쓴 경험치 빼고
            //     level++;               // 레벨 1 증가
            // }

        }
    }
}