using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxSpawnManager : MonoBehaviour
{
    public GameObject item_box_prefab; // 소환할 상자 프리팹
    public float spawn_interval = 30f; // 소환 쿨타임

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawn_interval);
            SpawnBox();
        }
    }

    void SpawnBox()
    {
        if(InGameManager.instance == null) return;

        // InGameManager에 있는 맵 범위를 가져와서 랜덤 좌표 생성
        float x = Random.Range(InGameManager.instance.MapMin.x + 1f, InGameManager.instance.MapMax.x - 1f);
        float y = Random.Range(InGameManager.instance.MapMin.y + 1f, InGameManager.instance.MapMax.y - 1f);
        Vector2 spawn_position = new Vector2(x, y);

        Instantiate(item_box_prefab, spawn_position, Quaternion.identity);
        Debug.Log($"상자 소환 완료. 좌표 : {spawn_position}");
    }
}
