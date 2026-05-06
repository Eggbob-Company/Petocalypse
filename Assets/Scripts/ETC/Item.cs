using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Magnet, Heal, Gold }
    public ItemType item_type;

    
    [Header("--- Drop Effect Setting ---")]
    public int max_bounce = 3; // 최대 반동 횟수
    public float x_force = 3f; // x축으로 튀는 힘
    public float y_force = 15f; // y축으로 튀는 힘
    public float gravity = 25f;

    private Vector2 _direction; // 아이템이 튀어오르는 방향과 속도를 저장하는 벡터
    private int _current_bounce = 0; // 현재까지 반동 횟수
    private bool _is_grounded = true; // 땅에 닿았는가

    private float _max_height;
    private float _current_height;

    public Transform sprite;
    public Transform shadow;

    void Start()
    {
        _current_height = Random.Range(y_force - 1, y_force); // 처음 튀어오를 높이 랜덤 설정
        _max_height = _current_height;
        Initialize(new Vector2(Random.Range(-x_force, x_force), Random.Range(-x_force, x_force)));
    }

    void Update()
    {
        if(!_is_grounded)
        {
            _current_height += -gravity * Time.deltaTime;
            sprite.position += new Vector3(0, _current_height, 0) * Time.deltaTime;
            transform.position += (Vector3)_direction * Time.deltaTime;

            float totalVelocity = Mathf.Abs(_current_height) + Mathf.Abs(_max_height);
            float scaleXY = Mathf.Abs(_current_height) / totalVelocity;
            shadow.localScale = Vector2.one * Mathf.Clamp(scaleXY, 0.5f, 1.0f);

            CheckGroundHit();
        }
    }

    void Initialize(Vector2 direction) // 초기값 설정
    {
        _is_grounded = false;
        _max_height /= 1.5f;
        _direction = direction;
        _current_height = _max_height;
        _current_bounce++;
    }

    void CheckGroundHit() // 아이템이 땅에 닿았는지 확인
    {
        if (sprite.position.y < shadow.position.y)
        {
            sprite.position = shadow.position;
            shadow.localScale = Vector2.one;

            if(_current_bounce < max_bounce)
            {
                Initialize(_direction / 1.5f);
            }
            else
            {
                _is_grounded = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(!_is_grounded) return; // 땅에 닿기 전에는 획득 불가

            ApplyEffect(); // 효과를 부여하고
            Destroy(gameObject); // 아이템 오브젝트 소멸
        }
    }

    void ApplyEffect()
    {
        switch (item_type)
        {
            case ItemType.Magnet:
                InGameManager.instance.ActivateMagnet(3.0f);
                Debug.Log("아이템 : 자석 효과");
                break;
            case ItemType.Heal:
                Player.instance.health.MaxLevelUpHeal(InGameManager.instance.health_reward_amount);
                Debug.Log("아이템 : 체력 회복");
                break;
            case ItemType.Gold:
                InGameManager.instance.gold += 10;
                Debug.Log("아이템 : 골드 획득");
                break;
        }
    }
}
