using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공격 방식 설정 (정면 발사, 가장 가까운 적에게 발사)
public enum AttackType
{
    Forward,
    NearestEnemy
}

public class PlayerAttack : MonoBehaviour
{
    [Header("공격 설정")]
    public GameObject attack_prefab;  // 공격할 오브젝트 선택
    public float attack_rate = 1.0f;  // 몇 초마다 발사할지
    public float attack_speed = 7.0f;  // 공격 속도

    [Header("공격 방식")]
    public AttackType attack_type;  // 공격 방식 선택

    [Header("공격 범위 설정")]
    public float attack_range = 5.0f; // 일단 하드코딩으로 설정한 사거리 나중에 스킬 사거리 불러오면 될 듯

    private float _attack_timer;
    private PlayerMove _player_move;

    void Start()
    {
        // PlayerMove.cs에서 프로퍼티를 가져오기 위해 PlayerMove.cs 컴포넌트 가져오기
        _player_move = GetComponent<PlayerMove>();
    }

    void Update()
    {
        // 시간을 측정해서
        _attack_timer += Time.deltaTime;

        // 내가 설정한 공격 시간 이상이 되면 발사
        if (_attack_timer >= attack_rate)
        {
            Attack();
            _attack_timer = 0f;
        }
    }

    void Attack()
    {
        
        Vector2 dir = Vector2.zero;

        // 공격 방식 파악
        switch (attack_type)
        {
            case AttackType.Forward:
                dir = _player_move.LastMoveVector;
                break;

            case AttackType.NearestEnemy:
                dir = GetDirectionToNearestEnemy();
                break;
        }

        // 공격할 방향이 없으면 리턴
        if (dir == Vector2.zero)
            return;

        // 공격 오브젝트 파악
        GameObject obj = Instantiate(
            attack_prefab,
            transform.position,
            Quaternion.identity
        );

        // BulletMove.cs 호출
        BulletMove attack  = obj.GetComponent<BulletMove>();
        attack.Init(attack_speed, dir);
    }

    Vector2 GetDirectionToNearestEnemy()
    {
        // 플레이어 위치를 중심으로 사거리(attack_range) 내의 Enemy 태그를 가진 오브젝트 검출
        int enemy_layer_mask = 1 << LayerMask.NameToLayer("Enemy"); 
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attack_range, enemy_layer_mask);

        // 없으면 리턴
        if (enemies.Length == 0)
            return Vector2.zero;

        Collider2D nearest_enemy = null;
        float min_dist = float.MaxValue;
        Vector2 my_pos = transform.position;

        // 검출된 Enemy 돌면서 플레이어와 거리 비교
        foreach (Collider2D enemy in enemies)
        {
            float dist = Vector2.Distance(my_pos, enemy.transform.position);

            // 현재 적보다 가까우면 갱신
            if (dist < min_dist)
            {
                min_dist = dist;
                nearest_enemy = enemy;
            }
        }

        // 가장 가까운 적의 방향 벡터 계산 후 반환
        return ((Vector2)nearest_enemy.transform.position - my_pos).normalized;
    }
}
