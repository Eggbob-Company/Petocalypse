using UnityEngine;

[RequireComponent(typeof(Boss))]
public class BossHealth : MonoBehaviour
{
    private Boss _boss;
    
    [Header("--- Status ---")]
    public float current_health;

    private bool _is_dead = false; // 사망 여부 체크 (중복 사망 방지)
    
    void Awake()
    {
        _boss = GetComponent<Boss>();
    }

    // Boss가 (데이터 설정 후) 호출할 초기화 함수
    public void InitHealth()
    {
        current_health = _boss.max_health;
    }

    // 보스가 데미지를 입는 함수
    public void TakeDamage(float damage)
    {
        if (_is_dead) return;

        current_health -= damage;

        if (current_health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 인게임매니저의 승리 함수 호출
        if (InGameManager.instance != null)
        {
            InGameManager.instance.Victory();
        }

        Destroy(gameObject);
    }
}