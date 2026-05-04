using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Magnet, Heal, Gold }
    public ItemType item_type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
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
