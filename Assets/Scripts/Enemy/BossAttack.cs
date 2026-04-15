using UnityEngine;

[RequireComponent(typeof(Boss))]
public class BossAttack : MonoBehaviour
{
    [Header("--- Attack Settings ---")]
    public Transform fire_point;         

    [Header("--- Spread Attack ---")]
    public int projectile_count = 8; // 발사할 총알 개수

    private Boss _boss;
    private Transform _target;
    private float _attack_timer = 0f;

    void Awake()
    {
        _boss = GetComponent<Boss>();
    }

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) _target = player.transform;

        if (fire_point == null) fire_point = transform; 
    }

    void Update()
    {
        if (_target == null || _boss == null) return;

        float distance = Vector2.Distance(transform.position, _target.position);

        if (distance <= _boss.attack_range)
        {
            _attack_timer += Time.deltaTime;
            
            if (_attack_timer >= _boss.attack_rate)
            {
                ShootSpread(); // 8방향 발사 함수 호출
                _attack_timer = 0f; 
            }
        }
        else
        {
            _attack_timer = 0f; 
        }
    }

    void ShootSpread()
    {
        float angleStep = 360f / projectile_count;

        for (int i = 0; i < projectile_count; i++)
        {
            float currentAngle = i * angleStep;

            Vector2 fireDirection = new Vector2(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad), 
                Mathf.Sin(currentAngle * Mathf.Deg2Rad)
            ).normalized;

            GameObject bullet = ObjectPoolManager.instance.GetGo("BossBullet"); // 풀 호출
            
            // 위치 초기화
            bullet.transform.position = fire_point.position;
            bullet.transform.rotation = Quaternion.identity;

            // 방향 설정
            BossBullet bulletScript = bullet.GetComponent<BossBullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(fireDirection);
            }
        }
    }
}