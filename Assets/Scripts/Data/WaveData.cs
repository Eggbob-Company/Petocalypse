using UnityEngine;

[System.Serializable] // 이게 있어야 인스펙터 창에서 확인할 수 있고, 데이터 저장 및 불러오는 작업이 가능.
public class WaveData
{
    // CSV의 헤더 이름과 완전히 똑같이 적어야 한다.
    public int time_min;
    public int time_sec;
    public int monster_id;
    public int amount;
    public float interval;

    // 계산 편의를 위해 초 단위로 합산된 시간을 반환하는 프로퍼티
    public int SpawnTime => (time_min * 60) + time_sec;
}