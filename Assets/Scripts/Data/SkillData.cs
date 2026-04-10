using UnityEngine;

[System.Serializable] // 이게 있어야 인스펙터 창에서 확인할 수 있고, 데이터 저장 및 불러오는 작업이 가능.
public class SkillData
{
    // CSV의 헤더 이름과 완전히 똑같이 적어야 한다.
    public int id;
    public int level;
    public float damage;
    public float cooldown;
    public int count;
    public float speed;
    public float area;
    public float range;
    public int penetrate;
    public float duration;
    public AttackType attack_type;
    public string prefab_name;
    public string desc_key;

    public string icon_name;
}

public enum AttackType
{
    Forward = 0,      // 정면 발사
    Nearest = 1,      // 가장 가까운 적 추적
    RandomArea = 2,   // 범위 내 랜덤 생성
    Orbit = 3         // 플레이어 주변 회전
}