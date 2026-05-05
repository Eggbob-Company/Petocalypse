using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Magnet, Heal, Gold }
    public ItemType item_type;

    
    [Header("--- Drop Effect Setting ---")]
    public int max_bounce = 3;
    public float x_force = 3f;
    public float y_force = 15f;
    public float gravity = 25f;

    private Vector2 _direction;
    private int _current_bounce = 0;
    private bool _is_grounded = true;

    private float max_height;
    private float current_height;

    public Transform sprite;
    public Transform shadow;

    void Start()
    {
        current_height = Random.Range(y_force - 1, y_force);
        max_height = current_height;
        Initialize(new Vector2(Random.Range(-x_force, x_force), Random.Range(-x_force, x_force)));
    }

    void Update()
    {
        if(!_is_grounded)
        {
            current_height += -gravity * Time.deltaTime;
            sprite.position += new Vector3(0, current_height, 0) * Time.deltaTime;
            transform.position += (Vector3)_direction * Time.deltaTime;

            float totalVelocity = Mathf.Abs(current_height) + Mathf.Abs(max_height);
            float scaleXY = Mathf.Abs(current_height) / totalVelocity;
            shadow.localScale = Vector2.one * Mathf.Clamp(scaleXY, 0.5f, 1.0f);

            CheckGroundHit();
        }
    }

    void Initialize(Vector2 direction)
    {
        _is_grounded = false;
        max_height /= 1.5f;
        _direction = direction;
        current_height = max_height;
        _current_bounce++;
    }

    void CheckGroundHit()
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
        // 추후 효과 구현
        switch (item_type)
        {
            case ItemType.Magnet:
                Debug.Log("아이템 : 자석 효과");
                break;
            case ItemType.Heal:
                Debug.Log("아이템 : 체력 회복");
                break;
            case ItemType.Gold:
                Debug.Log("아이템 : 골드 획득");
                break;
        }
    }
}
