using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("--- Status ---")]
    // [SerializeField]를 추가하여 private 기능을 사용하며 인스펙터 창에서 해당 변수 값 표시되게끔 함
    [SerializeField] private float _current_hp;
    [SerializeField] private bool _is_dead = false;

    // 외부에서 읽기용 프로퍼티
    public float CurrentHP => _current_hp;

    // 현재 체력을 전달하는 이벤트 생성
    public Action<float> OnHPChanged;

    // 플레이어가 죽었을 때 발생하는 이벤트 생성
    public Action OnDead;

    public void InitHealth()
    {
        // Player.cs에 로드된 데이터를 기반으로 변수 초기화
        // 현재 체력 = 최대 체력으로 설정 / 사망 상태 false 설정
        _current_hp = Player.instance.max_hp;
        _is_dead = false;

        // 초기화 시 이벤트 발생
        OnHPChanged?.Invoke(_current_hp);

        // 초당 체력 회복(recovery) 루틴 시작
        if (Player.instance.recovery > 0)
        {
            // Coroutine(코루틴)을 사용하여 체력 자연 회복 기능 구현
            // 일반 함수를 사용하면 1초마다 체력 회복을 하려면 1초를 기다리는 동안 게임 전체가 멈춰야함
            // 코루틴을 사용하면 다른 기능은 다 그대로 돌아가면서 코루틴 시킨 함수만 혼자 1초 기다리고 다시 실행됨
            StartCoroutine(RecoveryRoutine());
        }
    }

    // 데미지를 입는 함수 (외부에서 호출 가능)
    public void TakeDamage(float damage)
    {
        // 이미 죽은 상태라면 데미지 로직 무시
        if (_is_dead) return;

        // 체력 감소
        _current_hp -= damage;
        Debug.Log($"플레이어 피격! 남은 체력: {_current_hp}");

        // 체력 감소 시 이벤트 발생
        OnHPChanged?.Invoke(_current_hp);

        // 체력이 0 이하가 되면 사망 처리
        if (_current_hp <= 0)
        {
            // 체력을 표기해아하는 상황을 위하여 현재 체력을 0으로 설정 (음수로 들어가있으면 이상하니까)
            _current_hp = 0;
            Die();
        }
    }

    public void MaxLevelUpHeal(float amount)
    {
        if (_is_dead) return;

        _current_hp += amount;

        // 최대 체력 초과 방지 (InGameManager의 player_max_hp 변수 활용)
        if (_current_hp > InGameManager.instance.player_max_hp)
        {
            _current_hp = InGameManager.instance.player_max_hp;
        }

        // 체력 변경 이벤트 호출 (UI 업데이트)
        OnHPChanged?.Invoke(_current_hp);
    }

    private void Die()
    {
        if (_is_dead) return;

        _is_dead = true;

        Debug.Log("플레이어 사망!");

        // 사망 신호 보내기
        OnDead?.Invoke();
    }

    // recovery 데이터를 활용한 체력 회복 코루틴
    private IEnumerator RecoveryRoutine()
    {
        while (!_is_dead)
        {
            // 여기서 일단 멈추고 정해진 시간 동안 기다려라 (게임은 멈추지 않고 이 함수에 담긴 내용만)
            yield return new WaitForSeconds(1f); // 1초가 지나면 아래 코드가 실행

            // 플레이어가 풀피가 아니라면
            if (_current_hp < Player.instance.max_hp)
            {
                // csv 데이터에 있는 recovery 값 만큼 체력 회복
                _current_hp += Player.instance.recovery;

                // 체력 회복 시 이벤트 발생
                OnHPChanged?.Invoke(_current_hp);
                
                // 최대 체력 초과 방지
                if (_current_hp > Player.instance.max_hp)
                {
                    _current_hp = Player.instance.max_hp;
                }
            }
        }
    }
}
