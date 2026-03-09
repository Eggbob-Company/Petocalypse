using UnityEngine;

[System.Serializable] // 이게 있어야 인스펙터 창에서 확인할 수 있고, 데이터 저장 및 불러오는 작업이 가능.
public class MonsterData
{
    // CSV의 헤더 이름과 완전히 똑같이 적어야 한다.
    public int id;
    public string name;
    public float max_health;
    public float move_speed;
    public float base_damage;
    public float attack_range;
    public float attack_rate;
    public int exp_reward;
    public string monster_type;
}