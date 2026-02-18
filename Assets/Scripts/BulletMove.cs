using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    private float _speed;
    private Vector2 _direction;

    // 총알 초기화 함수
    public void Init(float speed, Vector2 dir)
    {
        _speed = speed;
        _direction = dir.normalized;
        
        // 날아가는 방향에 맞춰 총알 각도 조절
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Update()
    {
        // 매 프레임마다 이동
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
    }

    void OnBecameInvisible()
    {
        // 화면 밖으로 나가면 비활성화 (오브젝트 풀링용)
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("총알 맞았어요");
    }
}
