using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    // 다른 스크립트에서 쉽게 접근할 수 있도록 싱글톤(인스턴스) 패턴 사용
    public static TestGameManager instance;

    [Header("테스트 데이터")]
    public float exp = 0f;
    public int level = 0;
    public int gold = 0;
    public int kill = 0;
    public float max_game_time = 300f; // 기본 5분 
    public float game_time = 0f;
    public float health = 100f;
    public float max_health = 100f;

    // 레벨 0부터 4까지 필요한 경험치 통 (레벨이 오를수록 필요 경험치가 늘어남)
    public float[] nextExp = { 10f, 30f, 60f, 100f, 150f }; 

    void Awake()
    {
        instance = this; // 이 스크립트 TestGameManager를 인스턴스에 집어넣어서 찾기 쉽게
    }

    void Update()
    {
        // 시간은 자동으로 흐르게
        game_time += Time.deltaTime;

        // 스페이스바를 누르면 몬스터를 한 마리 잡았다고 가정하고 수치를 올리기
        if (Input.GetKeyDown(KeyCode.Space))
        {
            exp += 5f;        // 경험치 5 획득
            gold += 150;      // 골드 150 획득
            kill += 1;        // 1킬 추가
            health -= 10f;    // 몬스터 잡다가 체력 10 깎임

            // 만약 경험치가 다음 레벨업 목표치를 넘었다면 레벨업
            if (level < nextExp.Length && exp >= nextExp[level])
            {
                exp -= nextExp[level]; // 쓴 경험치 빼고
                level++;               // 레벨 1 증가
            }

        }
    }
}