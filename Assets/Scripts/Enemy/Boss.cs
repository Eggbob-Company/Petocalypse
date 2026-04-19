using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("--- Boss Stats ---")]
    public int monster_id = 2001; // 보스 데이터 ID
    public string monster_name;
    public float max_health;
    public float move_speed;
    public float base_damage;
    public float attack_range;
    public float attack_rate;
    public int exp_reward;
    public string monster_type;

    void Start()
    {
        InitBoss();
    }

    public void InitBoss()
    {
        // 매니저에서 데이터 로드
        if (MonsterDataManager.instance != null)
        {
            MonsterData data = MonsterDataManager.instance.GetMonsterData(monster_id);
            if (data != null)
            {
                monster_name = data.name;
                max_health = data.max_health;
                move_speed = data.move_speed;
                base_damage = data.base_damage;
                attack_range = data.attack_range;
                attack_rate = data.attack_rate;
                // exp_reward = data.exp_reward;
                monster_type = data.monster_type;
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