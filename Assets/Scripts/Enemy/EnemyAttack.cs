using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Enemy _enemy_main;
    private float _attack_timer;

    void Awake()
    {
        // 본체인 Enemy.cs 참조 (base_damage, attack_rate 등을 가져오기 위함)
        _enemy_main = GetComponent<Enemy>();
    }

    void Update()
    {
        // 공격 쿨타임 계산
        if (_attack_timer > 0)
        {
            _attack_timer -= Time.deltaTime;
        }
    }

    // 충돌 유지 중일 때 호출 (뱀서류의 전형적인 몸빵 공격 방식)
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 1충돌 대상이 플레이어인지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            // 공격 쿨타임이 다 찼는지 확인
            if (_attack_timer <= 0)
            {
                Attack(collision.gameObject);
            }
        }
    }

    // 실제 공격 로직 (나중에 원거리 공격 몬스터 생기면 attack_range도 사용해서 코드 변경해야 함)
    public void Attack(GameObject target)
    {
        PlayerHealth player_health = target.GetComponent<PlayerHealth>();
        if (player_health != null)
        {
            player_health.TakeDamage(_enemy_main.base_damage);
        }
        
        Debug.Log($"{_enemy_main.enemy_name}가 플레이어를 공격! " + $"데미지: {_enemy_main.base_damage}");

        // 공격 후 쿨타임 리셋 (CSV 데이터 기반)
        _attack_timer = _enemy_main.attack_rate;
    }
}
