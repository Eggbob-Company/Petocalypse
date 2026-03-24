using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;

public class DamageText : PoolAble
{
    private TextMeshPro _damage_text;
    private float _move_speed = 0.15f;
    private float _alpha_speed = 1.5f;
    private float _destroy_time = 0.4f;
    private float _timer;
    private Color _text_color;

    private void Awake()
    {
        _damage_text = GetComponent<TextMeshPro>();
    }

    public void Init(float damage)
    {
        _damage_text.text = Mathf.RoundToInt(damage).ToString(); // 대미지 값 지정

        // 투명도 값 초기화
        _text_color = _damage_text.color;
        _text_color.a = 1f;
        _damage_text.color = _text_color;

        // 타이머 초기화
        _timer = 0f;
    }

    private void Update()
    {
        transform.Translate(Vector3.up * _move_speed * Time.deltaTime); // 텍스트를 위로 둥둥 뜨게 함

        _timer += Time.deltaTime; // 동작 타이머 업데이트

        if(_timer >= _destroy_time * 0.5f) // (소멸 시간 절반 남았을 때부터) 점점 투명해지게 함
        {
            _text_color.a -= _alpha_speed * Time.deltaTime;
            _damage_text.color = _text_color;
        }

        if(_timer >= _destroy_time) // 시간 다 되면 풀로 반납
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        ReleaseObject();
    }
}
