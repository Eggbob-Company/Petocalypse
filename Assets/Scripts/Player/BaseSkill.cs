using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : PoolAble
{
    // protected 생성자는 private와 다른건 동일하지만 이 클래스를 상속받는 자식 클래스에서도 접근할 수 있게 함. (private 기준 변수명 사용)
    protected SkillData _data;
    protected Vector2 _direction;
    protected int _current_penetrate; // 현재 남은 관통 횟수
    protected bool _is_init = false;  // Init이 완료되었는지 확인
    
    // 스킬 오브젝트가 특정 이슈 (구조물 사이에 끼는 등)로 인해 Enemy에게 닿지 않고, 화면 밖으로 나가지도 않을 때를 대비하여 비상용 소멸 타이머
    private float _life_time_timer;
    private const float MAX_LIFE_TIME = 5f; // 5초 지나면 강제 소멸

    public virtual void Init(SkillData data, Vector2 direction)
    {
        _data = data;
        _direction = direction;
        
        // 기본 1마리 + 데이터에 적힌 추가 관통 수
        _current_penetrate = 1 + _data.penetrate;

        // 타이머 리셋
        _life_time_timer = 0f;

        // 크기 설정 적용
        transform.localScale = transform.localScale * _data.area * Player.instance.area;

        // 날아가는 방향에 맞춰 총알 각도 조절
        if (_direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        _is_init = true;
    }

    protected virtual void Update()
    {
        if (!_is_init) return;

        // 이동 (speed 데이터 활용)
        transform.Translate(_direction * _data.speed * Time.deltaTime, Space.World);

        // 비상용 강제 소멸 로직
        _life_time_timer += Time.deltaTime;
        if (_life_time_timer >= MAX_LIFE_TIME)
        {
            Deactivate();
        }
    }

    // 화면 밖으로 나가면 즉시 소멸
    protected virtual void OnBecameInvisible()
    {
        if (!_is_init) return;
        Deactivate();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // 데미지 주기 (damage * Player.might)
            float final_damage = _data.damage * Player.instance.might;

            // 때린 대상이 일반 몬스터일 경우
            EnemyHealth enemy_health = collision.GetComponent<EnemyHealth>();
            if (enemy_health)
            {
                enemy_health.TakeDamage(final_damage); // 대미지 주기
                SpawnDamageText(collision.transform.position, final_damage); // 오브젝트풀에서 대미지 텍스트 꺼내옴 & 대미지 값 인가
            }

            // 때린 대상이 보스일 경우
            BossHealth boss_health = collision.GetComponent<BossHealth>();
            if (boss_health != null)
            {
                boss_health.TakeDamage(final_damage); // 대미지 주기
                SpawnDamageText(collision.transform.position, final_damage); 
            }

            // 관통 횟수 차감
            _current_penetrate--;

            // 0이 되면 소멸 (기본 1회 + 추가 n회 모두 소진 시)
            if (_current_penetrate <= 0)
            {
                Deactivate();
            }
        }
    }

    private void SpawnDamageText(Vector2 position, float damage)
    {
        GameObject text_go = ObjectPoolManager.instance.GetGo("DamageText");

        if (text_go != null)
        {   
            Vector2 spawn_pos = position + Vector2.up * 0.5f;
            text_go.transform.position = spawn_pos;

            // DamageText의 Init 호출
            if (text_go.TryGetComponent<DamageText>(out var damage_text))
            {
                damage_text.Init(damage); // 대미지 값 보내서 텍스트 설정 초기화
            }
        }
    }

    protected virtual void Deactivate()
    {
        if (!gameObject.activeSelf) return; // 비활성화 상태면 실행하지 마라

        _is_init = false;

        ReleaseObject(); // 오브젝트 풀로 반환
    }
}
