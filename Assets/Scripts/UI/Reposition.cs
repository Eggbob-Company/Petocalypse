using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 센서(Area)랑 충돌해서 벗어났을 때
        if (!collision.CompareTag("Area")) return;

        // 플레이어 위치와 타일 위치의 차이 계산
        Vector3 player_pos = Player.instance.transform.position;
        Vector3 my_pos = transform.position;

        //플레이어 위치랑 타일맵 위치 거리 구하기
        float diff_x = player_pos.x - my_pos.x; // x간의 거리
        float diff_y = player_pos.y - my_pos.y; // y간의 거리

        // 2x2 방식: 진행 방향으로 2칸(tileSize * 2) 점프
        // 절대값이 더 큰 쪽(더 멀리 간 쪽)으로 이동
        if (Mathf.Abs(diff_x) > Mathf.Abs(diff_y))
        {
            float dir_x = diff_x > 0 ? 1 : -1; // 플레이어가 타일보다 왼쪽에 있는지 오른쪽에 있는지 알기 위해서 사용
            transform.Translate(Vector3.right * dir_x * 48);
        }
        else
        {
            float dir_y = diff_y > 0 ? 1 : -1; //플레이어가 타일보다 위쪽에 있는지 아래쪽에 있는지 알기 위해서 사용
            transform.Translate(Vector3.up * dir_y * 48);
        }
    }
}
