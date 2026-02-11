using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(CoSpawnObject1());
    }

    // Object1을 1초마다 자동 생성하는 코루틴
    private IEnumerator CoSpawnObject1()
    {
        yield return new WaitUntil(() => ObjectPoolManager.instance != null); // ObjectPoolManager가 준비될 때까지 대기

        while (true)
        {
            Vector2 random_one = Random.insideUnitCircle * 3f;
            SpawnAtPosition("Object1", new Vector3(random_one.x, random_one.y, 0f));

            yield return new WaitForSeconds(1f); // 1초 대기
        }
    }

    public void SpawnAtPosition(string objectName, Vector3 position)
    {
        if (ObjectPoolManager.instance == null) return;

        GameObject obj = ObjectPoolManager.instance.GetGo(objectName);
        if (obj != null)
        {
            obj.transform.position = position;
        }
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Alpha2)) // 키보드 2 키를 누를 때 오브젝트 랜덤 생성
        // {
        //     Vector2 random_two = Random.insideUnitCircle * 3f;
        //     SpawnAtPosition("Object2", new Vector3(random_two.x, random_two.y, 0f));
        // }
    }
}