using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillSlot
{
    public int skill_id;
    public int level;
    public float attack_timer; // 스킬마다 개별적으로 흐르는 쿨타임 타이머

    public SkillSlot(int id, int lv)
    {
        skill_id = id;
        level = lv;
        attack_timer = 0f;
    }
}

public class PlayerAttack : MonoBehaviour
{
    // 내가 현재 보유한 스킬들의 리스트
    public List<SkillSlot> mySkills = new List<SkillSlot>();

    private PlayerMove _player_move;

    void Awake()
    {
        // PlayerMove.cs에서 프로퍼티를 가져오기 위해 PlayerMove.cs 컴포넌트 가져오기
        _player_move = GetComponent<PlayerMove>();

        // 테스트용으로 시작할 때 100번 스킬 3레벨을 하나 추가
        // 나중에 레벨업 시스템이 완성되면 거기서 추가
        mySkills.Add(new SkillSlot(100, 3));
    }

    void Update()
    {
        foreach (SkillSlot slot in mySkills)
        {
            // 스킬 데이터 가져오기
            SkillData data = SkillDataManager.instance.GetSkillData(slot.skill_id, slot.level);
            if (data == null) continue;

            // 시간을 측정해서
            slot.attack_timer += Time.deltaTime;
            
            // 내가 설정한 공격 시간 이상이 되면 발사
            if (slot.attack_timer >= data.cooldown)
            {
                // 코루틴을 사용하여 연사 시작 (count만큼)
                StartCoroutine(AttackRoutine(data));
                slot.attack_timer = 0f; // 해당 스킬 타이머만 리셋
            }
        }
    }

    private IEnumerator AttackRoutine(SkillData data)
    {
        for (int i = 0; i < data.count; i++)
        {
            SpawnBullet(data);

            // 한 발 쏘고 아주 잠깐(0.1초) 쉬어서 연사 느낌 내기
            if (data.count > 1) yield return new WaitForSeconds(0.1f);
        }
    }

    private void SpawnBullet(SkillData data)
    {
        // 프리팹 로드 (prefab_name 컬럼 활용)
        GameObject prefab = Resources.Load<GameObject>($"Prefabs/Skill/{data.prefab_name}");
        if (prefab == null){
            Debug.LogError($"[System] Prefabs/{data.prefab_name} 경로에서 프리팹을 찾을 수 없습니다!");
            return;
        }

        // 방향 결정
        Vector2 dir = GetDirection(data);
        if (dir == Vector2.zero) return;

        // 생성 및 데이터 설정
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
        obj.GetComponent<BaseSkill>().Init(data, dir);
    }

    // 해당 스킬의 attack_type을 확인하여 공격 방향 설정 함수
    private Vector2 GetDirection(SkillData data)
    {
        // 일단 공격 타입 중 Forward와 Nearest만 구현
        if (data.attack_type == AttackType.Forward)
            return _player_move.LastMoveVector;
            
        if (data.attack_type == AttackType.Nearest)
            return GetDirectionToNearestEnemy(data.range);

        return _player_move.LastMoveVector;
    }

    // 가장 가까운 적 방향 찾는 함수 (range 매개변수 추가)
    private Vector2 GetDirectionToNearestEnemy(float range)
    {
        // 플레이어 위치를 중심으로 사거리(attack_range) 내의 Enemy 태그를 가진 오브젝트 검출
        int enemy_layer_mask = 1 << LayerMask.NameToLayer("Enemy"); 
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemy_layer_mask);

        // 없으면 리턴
        if (enemies.Length == 0)
            return Vector2.zero;

        Collider2D nearest_enemy = null;
        float min_dist = float.MaxValue;
        Vector2 my_pos = transform.position;

        // 검출된 Enemy 돌면서 플레이어와 거리 비교
        foreach (Collider2D enemy in enemies)
        {
            float dist = Vector2.Distance(my_pos, enemy.transform.position);

            // 현재 적보다 가까우면 갱신
            if (dist < min_dist)
            {
                min_dist = dist;
                nearest_enemy = enemy;
            }
        }

        // 모든 적이 죽어있는 상태일 수도 있으므로 방어 코드 추가
        if (nearest_enemy == null) return Vector2.zero;

        // 가장 가까운 적의 방향 벡터 계산 후 반환
        return ((Vector2)nearest_enemy.transform.position - my_pos).normalized;
    }
}
