using UnityEngine;
using Cinemachine;

public class InGameManager : MonoBehaviour
{
    // 다른 스크립트에서 쉽게 접근할 수 있도록 싱글톤(인스턴스) 패턴 사용
    public static InGameManager instance;
    public EnemyHealth boss_health;

    [Header("UI Connect")]
    public GameOverPopUp game_over_ui;
    public LevelUpPopUp level_up_ui;
    public GameObject boss_hp_bar;

    [Header("Map Settings")]
    public Vector2 stage_map_min;    // 일반 맵 왼쪽 아래
    public Vector2 stage_map_max;    // 일반 맵 오른쪽 위
    public Vector2 boss_map_min;     // 보스 맵 왼쪽 아래
    public Vector2 boss_map_max;     // 보스 맵 오른쪽 위
    public Vector2 boss_spawn_pos;   // 보스 맵 이동 시 좌표

    // 현재 게임에 적용 중인 실시간 범위
    private Vector2 _current_map_min;
    private Vector2 _current_map_max;

    [Header("Game State")]
    public bool is_boss_stage = false; // 보스 맵 진입 여부
    public bool is_game_over = false;  // 게임 종료 여부

    // 다른 스크립트가 참조할 프로퍼티
    public Vector2 MapMin => _current_map_min;
    public Vector2 MapMax => _current_map_max;

    [Header("Level Data")]
    public int pending_level_up_count = 0; // 레벨 경험치가 한 번에 들어왔을 때, 팝업을 띄워야 하는 수
    
    [Header("Max Level Reward Settings")]
    public float health_reward_amount = 30f;
    public int gold_reward_amount = 100;

    // 만렙 보상 여부를 체크하는 깃발
    public bool is_max_reward_trigger = false;

    [Header("Test Data")]
    public float exp = 0f;
    public int level = 1;
    public int max_level = 5; // 임시 만렙
    public int gold = 0;
    public int kill = 0;
    public float max_game_time = 300f; // 기본 5분
    public float boss_limit_time = 180f; // 보스 타이머: 3분
    public float game_time = 0f;
    public float health;
    public float max_health;
    public int boss_id = 2001;

    void Awake()
    {
        instance = this; // 이 스크립트 InGameManager를 인스턴스에 집어넣어서 찾기 쉽게

        _current_map_min = stage_map_min;
        _current_map_max = stage_map_max;
    }

    void Start()
    {
        // 시작하자마자 스킬 1개를 고르고 시작.
        pending_level_up_count = 1;
        LevelUpPopUp.instance.Show();
        
        // 플레이어의 체력 이벤트 구독
        Player.instance.health.OnHPChanged += UpdateHealthData;

        // 플레이어 사망 시 게임 결과 호출 
        Player.instance.health.OnDead += () => ShowGameOverUI(false);

        // 초기 최대 체력, 현재 체력 값 세팅
        max_health = Player.instance.max_hp;
        health = Player.instance.health.CurrentHP;
    }

    // 승리 조건: 보스 처치 시 보스 스크립트에서 호출할 것
    public void Victory()
    {
        ShowGameOverUI(true);
    }

    public void TeleportToBoss() // 이동 버튼용
    {
        is_boss_stage = true; // 보스 스테이지 진입 표시
        ClearMapObjects(); // 맵에 있던 오브젝트 삭제

        // ExpSpawnManager에서 exp 소환 이벤트 구독 해제 함수 호출
        ExpSpawnManager exp_spawn_manager = FindObjectOfType<ExpSpawnManager>();
        if (exp_spawn_manager != null)
        {
            exp_spawn_manager.StopSpawnExp();
        }

        game_time = 0f;
        max_game_time = boss_limit_time;

        GameObject boss_prefab = Resources.Load<GameObject>("Prefabs/Enemy/ZombieKing");
        if (boss_prefab != null)
        {
            GameObject boss_instance = Instantiate(boss_prefab, boss_spawn_pos + new Vector2(0, 5), Quaternion.identity);
            boss_health = boss_instance.GetComponent<EnemyHealth>();
            // 보스 세팅 초기화
            Enemy enemy = boss_instance.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.InitEnemy(boss_id);
            }
            
            Debug.Log("보스 프리팹 소환 완료!");
        }
        else Debug.Log("보스 프리팹을 찾을 수 없습니다.");

        // 보스 HP바 활성화
        if (boss_hp_bar != null) boss_hp_bar.SetActive(true);

        // 현재 맵 범위를 보스용으로 교체
        _current_map_min = boss_map_min;
        _current_map_max = boss_map_max;

        // 플레이어 보스방으로 텔레포트
        if (Player.instance != null)
        {
            Player.instance.transform.position = boss_spawn_pos;
        }

        CinemachineVirtualCamera vcam = FindObjectOfType<CinemachineVirtualCamera>();
        if(vcam != null) vcam.transform.position = boss_spawn_pos;
        
        // 카메라 범위 업데이트
        CameraRange camera_range = FindObjectOfType<CameraRange>();
        if (camera_range != null)
        {
            camera_range.SetRange();
        }

        // 몬스터 소환 범위 업데이트
        EnemySpawnManager enemy_spawn_manager = FindObjectOfType<EnemySpawnManager>();
        if (enemy_spawn_manager != null)
        {
            // 잡몹 몬스터 웨이브 변경
            enemy_spawn_manager.SetMapRange();
        }

    }

    public void ClearMapObjects()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        GameObject[] exps = GameObject.FindGameObjectsWithTag("Exp");
        foreach (GameObject exp in exps)
        {
            Destroy(exp);
        }

        Debug.Log("맵에 남은 오브젝트 제거 완료");
    }
    
    // 경험치 획득 시 호출되는 함수
    public void GetExp(float get_exp)
    {
        exp += get_exp;

        // 레벨 업까지 필요한 경험치 가져오기
        int req_exp = ExpDataManager.instance.GetRequiredExp(level);
        Debug.Log($"현재 경험치: {exp} / 필요 경험치: {req_exp}");

        // 초과된 경험치가 없을 때까지 반복
        while (exp >= req_exp)
        {
            // 만렙 미만이면 일반 레벨업 보상
            if (level < max_level)
            {
                LevelUp(req_exp);
                is_max_reward_trigger = false; // 레벨 6이 되지 않게 하기 위해
            }
            // 만렙 이상이면 만렙 이후 레벨업 보상
            else
            {
                MaxLevelUP(req_exp);
                is_max_reward_trigger = true;
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

    public void ShowGameOverUI(bool isVictory)
    {
        if(is_game_over) return; // 이미 종료되었다면 무시
        is_game_over = true;

        if(game_over_ui != null)
        {
            game_over_ui.Show(isVictory); // GameOverPopUp.cs에서 팝업 호출
        }
    }

    // 몬스터가 죽을 때마다 호출되는 함수
    public void KillCount()
    {
        kill++;
    }

    void Update()
    {
        if(is_game_over) return;

        // 시간은 자동으로 흐르게
        if (game_time < max_game_time)
        {
            game_time += Time.deltaTime;

            if (game_time >= max_game_time)
            {
                game_time = max_game_time;
                ShowGameOverUI(false); // 시간 초과로 패배 처리
            }
        }
        
    }
}