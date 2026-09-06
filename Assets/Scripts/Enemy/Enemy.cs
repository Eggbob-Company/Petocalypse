using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PoolAble
{
    public static Transform target; // 모든 Enemy가 추적할 target

    [Header("--- Enemy Stats ---")]
    public int enemy_id;
    public string enemy_name;
    public float max_hp;
    public float move_speed;
    public float base_damage;
    public float attack_range;
    public float attack_rate;
    public int exp_reward;
    public string enemy_type;

    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable() // 오브젝트 풀링을 고려해 OnEnable에서 초기화
    {
        if (Player.instance != null) target = Player.instance.transform;
    }

    public void InitEnemy(int id) // 외부에서 호출하는 초기화 함수
    {
        enemy_id = id;
        InitEnemy();
    }

    public void InitEnemy() // InitEnemy(id)가 호출하는 함수 : 오버로딩
    {
        // EnemyDataManager를 통해 데이터 로드
        EnemyData data = EnemyDataManager.instance.GetEnemyData(enemy_id);
        if (data == null) return;

        enemy_name = data.name;
        max_hp = data.max_hp;
        move_speed = data.move_speed;
        base_damage = data.base_damage;
        attack_range = data.attack_range;
        attack_rate = data.attack_rate;
        exp_reward = data.exp_reward;
        enemy_type = data.enemy_type;

        EnemyHealth enemy_health = GetComponent<EnemyHealth>();
        if (enemy_health != null)
        {
            enemy_health.InitHealth(max_hp);
        }
    }

    public void OnDeath()
    {
        // EnemyHealth.cs에서 Enemy 죽으면 Enemy.OnDeath()실행해서 풀로 복귀시킬 것임
        ReleaseObject();
    }

    void FixedUpdate()
    {
        // 정적 타겟이 없으면 이동하지 않음
        if (target == null) return;

        // 플레이어 방향 벡터 계산
        Vector2 dir_vec = (Vector2)target.transform.position - _rb.position;
        Vector2 next_vec = dir_vec.normalized * move_speed * Time.fixedDeltaTime;
        
        // 물리 엔진을 이용한 위치 이동
        _rb.MovePosition(_rb.position + next_vec);
    }
}
