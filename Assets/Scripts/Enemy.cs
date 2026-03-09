using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PoolAble
{
    public static Transform target; // 모든 Enemy가 추적할 target

    [Header("--- Monster Stats ---")]
    public int monster_id = 1001; // 스포너에서 설정 지금은 임시로 1001 넣음
    public string monster_name;
    public float max_health;
    public float move_speed;
    public float base_damage;
    public float attack_range;
    public float attack_rate;
    public int exp_reward;
    public string monster_type;

    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable() // 오브젝트 풀링을 고려해 OnEnable에서 초기화
    {
        // 현재 Player.cs를 생성하지 않았기 때문에 테스트를 위해 임시로 "Player" 태그를 가진 오브젝트를 찾아 타겟으로 설정
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) target = player.transform;
        }

        // // Player를 타겟으로 등록
        // if (Player.instance != null) target = Player.instance;
        InitMonster();
    }

    public void InitMonster()
    {
        // MonsterDataManager를 통해 데이터 로드
        MonsterData data = MonsterDataManager.instance.GetMonsterData(monster_id);
        if (data == null) return;

        monster_name = data.name;
        max_health = data.max_health;
        move_speed = data.move_speed;
        base_damage = data.base_damage;
        attack_range = data.attack_range;
        attack_rate = data.attack_rate;
        exp_reward = data.exp_reward;
        monster_type = data.monster_type;
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
        Vector2 dirVec = (Vector2)target.transform.position - _rb.position;
        Vector2 nextVec = dirVec.normalized * move_speed * Time.fixedDeltaTime;
        
        // 물리 엔진을 이용한 위치 이동
        _rb.MovePosition(_rb.position + nextVec);
    }
}
