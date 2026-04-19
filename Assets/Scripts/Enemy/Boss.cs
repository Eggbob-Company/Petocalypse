using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("--- Boss Stats ---")]
    public int enemy_id = 2001; // 보스 데이터 ID
    public string enemy_name;
    public float max_health;
    public float move_speed;
    public float base_damage;
    public float attack_range;
    public float attack_rate;
    public int exp_reward;
    public string enemy_type;

    void Start()
    {
        InitBoss();
    }

    public void InitBoss()
    {
        // 매니저에서 데이터 로드
        if (EnemyDataManager.instance != null)
        {
            EnemyData data = EnemyDataManager.instance.GetEnemyData(enemy_id);
            if (data != null)
            {
                enemy_name = data.name;
                max_health = data.max_health;
                move_speed = data.move_speed;
                base_damage = data.base_damage;
                attack_range = data.attack_range;
                attack_rate = data.attack_rate;
                // exp_reward = data.exp_reward;
                enemy_type = data.enemy_type;
            }
        }

        // BossHealth의 체력 초기화 함수 호출
        BossHealth healthScript = GetComponent<BossHealth>();
        if (healthScript != null)
        {
            healthScript.InitHealth();
        }
    }
}