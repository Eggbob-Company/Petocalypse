using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // 테스트용 ID, 씬에 올려둔 몬스터가 어떤 데이터를 받아올지 결정.
    // 스포너가 생기면 없어질 예정
    public int test_monster_id = 1001;

    // csv에서 받아올 스탯
    private float _max_health;
    private float _base_damage;
    private float _exp_reward;

    private float _current_health;    // 현재 체력 (내부 계산용)
    private bool _is_dead = false;    // 사망 여부 체크 (중복 사망 방지)

    // start() 부분은 스폰 매니저 코드랑 합치게 되면 없어질 예정.
    // 지금은 테스트를 위해 데이터 매니저를 불러서 데이터를 찾아와야 함.
    void Start()
    {
        // 1. 씬에 있는 매니저 프리팹을 찾아오기.
        MonsterDataManager manager = FindObjectOfType<MonsterDataManager>();
        
        if (manager != null)
        {
            // 2. 내 ID(기본값 1001)에 맞는 데이터를 달라고 요청.
            MonsterData data = manager.GetMonsterData(test_monster_id);
            
            if (data != null)
            {
                // 3. 데이터를 성공적으로 받았으면 초기화 함수에 넣기!
                InitHealth(data);
            }
        }
        
        else
        {
            Debug.LogError("[EnemyHealth] 씬에 MonsterDataManager가 없습니다! 프리팹을 올려주세요.");
        }
    }
    // 데이터 삽입 & 부활 함수
    // 나중에 스포너가 풀에서 몬스터를 꺼낼 때 이 함수를 부르게 된다.
    public void InitHealth(MonsterData data)
    {
        // 받아온 데이터 상자에서 내 스탯으로 값을 복사.
        _max_health = data.max_health;
        _base_damage = data.base_damage;
        _exp_reward = data.exp_reward;

        // 새 몬스터처럼 체력을 꽉 채우고 사망 상태를 해제
        _current_health = _max_health;
        _is_dead = false;
        
        Debug.Log($"[{data.name}] 스폰 완료! 세팅된 최대 체력: {_max_health}, 데미지: {_base_damage}");
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
        _is_dead = true;
        Debug.Log($"[Enemy] 사망, 경험치 {_exp_reward} 드롭 예정");

        // 오브젝트 삭제
        Destroy(gameObject);
        // 지금은 오브젝트 풀링이 적용이 안되어 있어서 제거하지만 나중에 적용되면 Destroy를 지우고
        // gameObject.SetActive(false); 로 변경.
    }

    // 충돌 감지 (테스트용: 플레이어와 부딪히면 즉사) -> 플레이어 말고 공격 매체로 변경 가능
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 부딪힌 대상의 태그가 Player인지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            // 체력보다 많은 데미지를 줘서 즉사시킴
            TakeDamage(100.0f);
        }
    }
}
