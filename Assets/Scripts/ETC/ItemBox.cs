using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    [Header("--- Drop Items ---")]
    public GameObject[] item_prefabs;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) // 충돌 대상이 플레이어일 시
        {   
            DropRandomItem(); // 랜덤 아이템 토출
            Destroy(gameObject); // 상제 삭제
        }
    }

    void DropRandomItem()
    {
        if(item_prefabs.Length == 0) return;

        int random_num = Random.Range(0, item_prefabs.Length);
        
        // 상자 위치에 아이템 생성
        Instantiate(item_prefabs[random_num], transform.position, Quaternion.identity);
        Debug.Log($"상자에서 {item_prefabs[random_num].name} 아이템 발견!");
    }
}
