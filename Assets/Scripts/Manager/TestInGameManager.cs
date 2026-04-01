using UnityEngine;
using Cinemachine;

public class TestInGameManager : MonoBehaviour
{
    // 다른 스크립트에서 쉽게 접근할 수 있도록 싱글톤(인스턴스) 패턴 사용
    public static TestInGameManager instance;

    [Header("UI Connect")]
    public GameOverPopUp game_over_ui;

    [Header("Map Settings")]
    public Vector2 stage_map_min;    // 일반 맵 왼쪽 아래
    public Vector2 stage_map_max;    // 일반 맵 오른쪽 위
    public Vector2 boss_map_min;     // 보스 맵 왼쪽 아래
    public Vector2 boss_map_max;     // 보스 맵 오른쪽 위
    public Vector2 boss_spawn_pos;   // 보스 맵 이동 시 좌표

    // 현재 게임에 적용 중인 실시간 범위
    private Vector2 _current_map_min;
    private Vector2 _current_map_max;

    // 다른 스크립트가 참조할 프로퍼티
    public Vector2 MapMin => _current_map_min;
    public Vector2 MapMax => _current_map_max;

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

        _current_map_min = stage_map_min;
        _current_map_max = stage_map_max;
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

    public void TeleportToBoss() // 이동 버튼용
    {
        ClearMapObjects(); // 맵에 있던 오브젝트 삭제

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
        CameraRange cameraRange = FindObjectOfType<CameraRange>();
        if (cameraRange != null)
        {
            cameraRange.SetRange();
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