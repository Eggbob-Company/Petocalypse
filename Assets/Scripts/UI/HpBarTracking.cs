using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarTracking : MonoBehaviour
{
    [SerializeField] private Transform _target; // 추적할 대상 (Player)

    private RectTransform _rect_transform;

    void Awake()
    {
        _rect_transform = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        if (_target == null) return;

        // 플레이어의 월드 좌표를 스크린 좌표로 변환
        Vector3 screen_pos = Camera.main.WorldToScreenPoint(_target.position);
        
        // 플레이어 중심에서 y 방향을 30만큼 낮춤 (hp 바 위치가 플레이어 하단에 위치하도록)
        screen_pos.y -= 30f;

        // UI의 위치를 해당 좌표로 설정
        _rect_transform.position = screen_pos;
    }
}
