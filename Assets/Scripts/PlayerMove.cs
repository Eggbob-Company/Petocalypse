using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("플레이어 조작")]
    public Joystick joystick;  // 조이스틱 연결
    public float move_speed = 3f;  // 플레이어 이동 속도

    private Rigidbody2D rb;  // 물리 엔진을 사용하기 위한 컴포넌트
    private Vector2 move_vector;  // 조이스틱 입력값을 담을 벡터

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 조이스틱에서 x축과 y축의 입력값 받아오기
        move_vector.x = joystick.Horizontal;
        move_vector.y = joystick.Vertical;
    }

    void FixedUpdate()
    {
        // 물리 엔진을 이용해 플레이어 위치 이동
        // 현재 위치 + (방향 * 속도 * 시간 보정)
        Vector2 current_pos = rb.position;
        Vector2 move_amount = move_vector * move_speed * Time.fixedDeltaTime;
        rb.MovePosition(current_pos + move_amount);
    }
}
