using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static Action<Vector2> OnEnemyDeath;

    [Header("--- Status ---")]
    // [SerializeField]를 추가하여 private 기능을 사용하며 인스펙터 창에서 해당 변수 값 표시되게끔 함
    [SerializeField] private float _current_hp; // 현재 체력 (내부 계산용)
    [SerializeField] private float _max_hp; // 최대 체력
    [SerializeField] private bool _is_dead = false; // 사망 여부 체크 (중복 사망 방지)

    // 외부에서 읽기용 프로퍼티
    public float CurrentHP => _current_hp;
    public float MaxHP => _max_hp;

    private Enemy _enemy_main;

    void Awake()
    {
        // Enemy.cs 컴포넌트 찾기
        _enemy_main = GetComponent<Enemy>();
    }

    void OnEnable()
    {
        _is_dead = false;
    }

    public void InitHealth(float max)
    {
        _current_hp = max;
        _max_hp = max;
        _is_dead = false;
        Debug.Log($"[EnemyHealth] 체력 초기화 완료");
    }

    // 데미지를 입는 함수 (외부에서 호출 가능)
    public void TakeDamage(float damage)
    {
        // 이미 죽은 상태라면 데미지 로직 무시
        if (_is_dead) return;

        // 체력 감소
        _current_hp -= damage;
        Debug.Log($"[Enemy] 입은 대미지: {damage} | 남은 체력: {_current_hp}");

        // 체력이 0 이하가 되면 사망 처리
        if (_current_hp <= 0)
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

        if(_enemy_main.enemy_type == "Boss")
        {
            InGameManager.instance.Victory();
            Destroy(gameObject);
        }
        else
        {
            OnEnemyDeath?.Invoke(transform.position);

            // kill 카운트 증가
            InGameManager.instance.KillCount();

            // 본체인 Enemy.cs에게 사망 알림
            _enemy_main.OnDeath();
        }
        
    }
}
