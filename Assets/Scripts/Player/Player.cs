using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 다른 스크립트(Enemy 등)에서 접근할 수 있는 싱글톤
    public static Player instance;

    [Header("--- Player Stats ---")]
    public int id = 101;
    public float max_hp;
    public float recovery;
    public float move_speed;
    public float might;
    public float area;
    public float projectile;
    public float duration;
    public float magnet_range;
    public float luck;
    public string desc_key;

    public PlayerHealth health;

    void Awake()
    {
        instance = this;

        health = GetComponent<PlayerHealth>();
    }

    void Start()
    {
        InitPlayer();
    }

    // Update is called once per frame
    public void InitPlayer()
    {
        // PlayerDataManager를 통해 ID 101번(이미지 기준) 데이터를 가져옵니다.
        var data = PlayerDataManager.instance.GetPlayerData(id);
        
        if (data != null)
        {
            // CSV 컬럼명과 1:1 매칭
            max_hp = data.max_hp + GameDataManager.instance.bonus_max_hp;
            recovery = data.recovery;
            move_speed = data.move_speed + GameDataManager.instance.bonus_speed;
            might = data.might;
            area = data.area;
            projectile = data.projectile_speed;
            duration = data.duration;
            magnet_range = data.magnet_range;
            luck = data.luck;
            desc_key = data.desc_key;

            Debug.Log($"플레이어 데이터 로드 완료 (ID: {id})");
        }

        GetComponent<PlayerHealth>().InitHealth();
    }
}
