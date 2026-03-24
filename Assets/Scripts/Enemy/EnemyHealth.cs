using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static Action<Vector2> OnEnemyDeath;

    [Header("--- Status ---")]
    // [SerializeField]를 추가하여 private 기능을 사용하며 인스펙터 창에서 해당 변수 값 표시되게끔 함
    [SerializeField] private float _current_health; // 현재 체력 (내부 계산용)
    [SerializeField] private bool _is_dead = false; // 사망 여부 체크 (중복 사망 방지)

    private Enemy _enemy_main;

    void Awake()
    {
        // Enemy.cs 컴포넌트 찾기
        _enemy_main = GetComponent<Enemy>();
    }

    void OnEnable()
    {
        if (_enemy_main != null)
        {
            // 현재 체력 = 최대 체력으로 설정 / 사망 상태 false 설정
            _current_health = _enemy_main.max_health;
            _is_dead = false;
        }
    }

    // 데미지를 입는 함수 (외부에서 호출 가능)
    public void TakeDamage(float damage)
    {
        // 이미 죽은 상태라면 데미지 로직 무시
        if (_is_dead) return;

        // 체력 감소
        _current_health -= damage;
        Debug.Log($"[Enemy] 남은 체력: {_current_health}");

        // 체력이 0 이하가 되면 사망 처리
        if (_current_health <= 0)
        {
            Die();
        }
    }

    // 사망 처리 함수
    void Die()
    {
        // 이미 죽은 상태라면 데미지 로직 무시
        if (_is_dead) return;
        
        _is_dead = true;

        OnEnemyDeath?.Invoke(transform.position);
        Debug.Log($"{gameObject.name} 사망 신호 발송 및 풀 반환");

        // 경험치 추가
        TestGameManager.instance.KillCount();

        // 본체인 Enemy.cs에게 사망 알림
        _enemy_main.OnDeath();
    }

    // 테스트용으로 이 위치에 "Skill" 태그와 부딪히면 데미지 입게 해놓은 것
    // 추후에는 이 위치에서 삭제하고 스킬 스크립트에 EnemyHealth.TakeDamage(skill_damage)로 호출할 예정
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 부딪힌 대상의 태그가 Skill인지 확인
        if (collision.gameObject.CompareTag("Skill"))
        {
            Debug.Log($"[Enemy] 총알 피격. 사망 상태 : {_is_dead}");
            // 체력보다 많은 데미지를 줘서 즉사시킴
            TakeDamage(100.0f);
        }
    }
}
