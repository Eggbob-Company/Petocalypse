using UnityEngine;

[System.Serializable]
public class MonsterData
{
    // CSV의 헤더 이름과 완전히 똑같이 적어야 한다.
    public int id;
    public string name;
    public float max_health;
    public float move_speed;
    public float base_damage;
    public int exp_reward;
}