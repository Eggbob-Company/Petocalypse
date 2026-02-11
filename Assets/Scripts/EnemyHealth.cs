using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("체력 설정")]
    public float max_health = 10.0f;  // 몬스터의 최대 체력

    private float _current_health;    // 현재 체력 (내부 계산용)
    private bool _is_dead = false;    // 사망 여부 체크 (중복 사망 방지)

    void Start()
    {
        // 게임 시작 시 현재 체력을 최대 체력으로 초기화
        _current_health = max_health;
        _is_dead = false;
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
        Debug.Log("[Enemy] 사망");

        // 오브젝트 삭제
        Destroy(gameObject);
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

        //     // 플레이어랑 몬스터랑 닿으면 플레이어에게 데미지를 준다. -> 원래 정상적인 게임이라면 이 코드를 사용. 
        // if (collision.gameObject.CompareTag("Player"))
        // {
        //     // 플레이어 스크립트를 찾아옴
        //     PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            
        //     // 플레이어에게 데미지를 줌 (내 공격력만큼)
        //     if (player != null)
        //     {
        //         player.TakeDamage(10.0f); // 플레이어에게 데미지를 준다. 괄호 안에는 몬스터의 공격력 변수가 들어가면 됨.
        //     }
        // }
    }
}
