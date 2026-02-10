using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("플레이어 조작")]
    public Joystick joystick;  // 조이스틱 연결
    public float move_speed = 3f;  // 플레이어 이동 속도

    private Rigidbody2D _rb;  // 물리 엔진을 사용하기 위한 컴포넌트
    private Vector2 _move_vector;  // 조이스틱 입력값을 담을 벡터
    private Animator _anim;  // 애니메이터 컴포넌트
    private Vector2 _last_move_vector = Vector2.down;  // 초기 상태를 아래를 보고 있게 설정

    // 다른 스크립트에서 읽을 수 있게 프로퍼티 제공
    public Vector2 LastMoveVector => _last_move_vector;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // 조이스틱에서 x축과 y축의 입력 값 받아오기
        _move_vector.x = joystick.Horizontal;
        _move_vector.y = joystick.Vertical;

        // 애니메이션 함수 호출
        Animate();
    }

    void FixedUpdate()
    {
        // 물리 엔진을 이용해 플레이어 위치 이동
        // 현재 위치 + (방향 * 속도 * 시간 보정)
        Vector2 current_pos = _rb.position;
        Vector2 move_amount = _move_vector * move_speed * Time.fixedDeltaTime;
        _rb.MovePosition(current_pos + move_amount);
    }

    void Animate()
    {
        // 0.1보다 크면 움직이는 것으로 판단
        bool is_moving = _move_vector.magnitude > 0.1f;
        _anim.SetBool("is_moving", is_moving);

        if (is_moving)
        {
            // 움직일 때 애니메이션 속도를 1로 설정
            _anim.SetFloat("anim_speed", 1f);
            
            // 블렌드 트리의 input_x, input_y 파라미터에 조이스틱 값 전달
            _anim.SetFloat("input_x", _move_vector.x);
            _anim.SetFloat("input_y", _move_vector.y);

            // 마지막으로 바라보던 방향 기억 (정지 시 해당 방향을 보기 위함)
            _last_move_vector = _move_vector;
        }
        else
        {
            // 멈췄을 때 애니메이션 속도를 0로 설정
            _anim.SetFloat("anim_speed", 0f);
            
            // 멈췄을 때 마지막으로 바라보던 방향 유지
            _anim.SetFloat("input_x", _last_move_vector.x);
            _anim.SetFloat("input_y", _last_move_vector.y);
        }
    }
}
