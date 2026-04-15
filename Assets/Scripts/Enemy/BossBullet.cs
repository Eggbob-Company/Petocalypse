using System.Collections;
using UnityEngine;

// MonoBehaviour 대신 PoolAble 상속
public class BossBullet : PoolAble 
{
    [Header("--- Bullet Settings ---")]
    public float speed = 5f;        
    public float lifeTime = 3f;     
    public float damage = 10f;      

    private Vector2 _direction;     

    public void SetDirection(Vector2 dir)
    {
        _direction = dir;
    }

    void OnEnable()
    {
        StartCoroutine(AutoReleaseRoutine());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    // 일정 시간이 지나면 자동으로 풀에 반환
    IEnumerator AutoReleaseRoutine()
    {
        yield return new WaitForSeconds(lifeTime); // 유지 시간 지난 후에
        ReleaseObject();
    }

    void Update()
    {
        transform.Translate(_direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player.instance.health.TakeDamage(damage);
            // 플레이어를 맞추면 반환
            ReleaseObject(); 
        }
    }
}