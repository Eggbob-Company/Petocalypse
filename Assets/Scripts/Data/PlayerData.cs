using UnityEngine;

[System.Serializable] // 이게 있어야 인스펙터 창에서 확인할 수 있고, 데이터 저장 및 불러오는 작업이 가능.
public class PlayerData
{
    // CSV의 헤더 이름과 완전히 똑같이 적어야 한다.
    public int id;
    public float max_hp;
    public float recovery;
    public float move_speed;
    public float might;
    public float area;
    public float projectile_speed;
    public float duration;
    public float magnet_range;
    public float luck;
    public string desc_key;
}