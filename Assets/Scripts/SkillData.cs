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
    public int penetrate;
    public string desc_key;
}