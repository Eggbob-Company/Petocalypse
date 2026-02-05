using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCtrl : MonoBehaviour
{

    public Transform target_player;   // 추적할 대상 (플레이어)
    public float move_speed = 1.0f; // 이동 속도

    void Start()
    {
        // "Player"라는 태그가 달린 녀석을 찾아서 몬스터의 타겟으로 삼겠다
        GameObject player = GameObject.FindWithTag("Player");

        // 만약 플레이어를 찾았다면 플레이어를 타겟으로 삼는다.
        if (player != null)
        {
            target_player = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어가 죽어서 화면에서 사라졌을 때를 대비해 에러방지 목적으로 넣어줌.
        if (target_player == null)
            return;

        // 방향 구하기 (목적지 - 내 위치)
        // .normalized를 해야 거리에 상관없이 방향(길이 1)만 구해짐
        // .normalized가 없으면 거리가 멀면 값이 엄청 커져서 멀리 있을수록 빠르게 다가감. 그래서 모두 1로 잘라버리고 방향만 알수있게끔 하는 속성.
        // 우리는 이 방향 값에 speed를 곱해서 일정한 속도로 플레이어에게 다가가게 할 것.
        Vector3 dir = (target_player.position - transform.position).normalized;

        // 이동하기 (방향 * 속도 * 시간) => 거리는 속력 곱하기 시간
        // Time.deltaTime은 "지난 프레임부터 지금 프레임까지 걸린 시간(초)"을 담고 있다. (약 0.016초) -> 시간 보정을 해준다.
        transform.position += dir * move_speed * Time.deltaTime;

        // 3. (선택) 시선 처리: 왼쪽/오른쪽 바라보기
        // 이동 방향(x)이 음수면 왼쪽, 양수면 오른쪽
        if (dir.x < 0)
            transform.localScale = new Vector3(-0.5f, 0.5f, 1); // 왼쪽 (좌우 반전)
        else
            transform.localScale = new Vector3(0.5f, 0.5f, 1);  // 오른쪽 (원래대로) 
    }
}