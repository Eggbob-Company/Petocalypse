using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro를 쓰려면 사용해야 함.

public class Hud : MonoBehaviour
{
    public enum InfoType { Exp, Gold, Level, Kill, Time, Health, BossHP } // 열거형으로 변수를 만들면 읽고 사용하기 편함.
    public InfoType type;

    TMP_Text my_text;
    Slider my_slider;

    void Awake()
    {
        my_text = GetComponent<TMP_Text>();
        my_slider = GetComponent<Slider>();
        EnemyHealth enemy_health = GetComponent<EnemyHealth>();
    }
    // 데이터가 Update()에서 갱신이 될 때마다, 그 연산이 다 끝날 때쯤 갱신을 해야 하니까 LateUpdate()를 사용.
    void LateUpdate()
    {
        // 테스트 매니저가 씬에 없으면 에러가 나지 않도록 방어 코드 추가
        if (InGameManager.instance == null) return;

        switch (type)
        {
            case InfoType.Exp:
                // 보스맵으로 이동하면 exp 바가 사라지도록
                if (InGameManager.instance.is_boss_stage)
                {
                    if(my_slider.gameObject.activeSelf)
                    {
                        my_slider.gameObject.SetActive(false);
                    }
                    return;
                }
                float current_exp = InGameManager.instance.exp;
                float max_exp = ExpDataManager.instance.GetRequiredExp(InGameManager.instance.level);
                my_slider.value = current_exp / max_exp;
                break;
            
            case InfoType.Gold:
                my_text.text = string.Format("G {0:N0}", InGameManager.instance.gold);
                break;
                // 포맷 방식에서 N: 천단위 콤마 자동 추가/ 0: 소수점 없음
            
            case InfoType.Level:
                my_text.text = string.Format("Lv.{0:F0}", InGameManager.instance.level);
                break;
                // text에는 string으로 형변환을 해줘야 함. Format은 각 숫자 인자값을 지정된 형태의 문자열로 만들어주는 함수.
                // 두 가지 이상 매개변수가 들어감.첫 번째는 포맷을 쓸 타입, 두 번째는 그 포맷에 적용되는 데이터
                // 인자값의 문자열이 들어갈 자리를 {순번} 형태로 작성
                // F0, F1, F2... 소수점 자릿수를 표현 (F0은 소수점 아래 필요 없다는 뜻)
            
            case InfoType.Kill:
                my_text.text = string.Format("Kill {0:F0}", InGameManager.instance.kill);
                break;
            
            case InfoType.Time:
                float remain_time = InGameManager.instance.max_game_time - InGameManager.instance.game_time;
                int min = Mathf.FloorToInt(remain_time / 60); // Mathf 함수에서 소수점을 버리는 함수를 사용해서 분을 표시
                int sec = Mathf.FloorToInt(remain_time % 60); // Mathf 함수에서 소수점을 버리는 함수를 사용해서 초를 표시
                my_text.text = string.Format("{0:D2}:{1:D2}", min, sec); // 항상 00 : 00 처럼 두자리로 보이게 하고 싶으니까 D를 써서 자릿수 고정
                break;
                // 스테이지 클리어 타임에서 현재 진행시간을 빼서 타이머에 보이게 하기.
            
            case InfoType.Health:
                float current_health = InGameManager.instance.health;
                float max_health = InGameManager.instance.max_health;
                my_slider.value = current_health / max_health;
                break;

            case InfoType.BossHP:
                if(InGameManager.instance.boss_health){
                    float current_boss_health = InGameManager.instance.boss_health.CurrentHP; // 현재 보스 체력 가져오기
                    float max_boss_health = InGameManager.instance.boss_health.MaxHP; // 최대 보스 체력 가져오기
                    my_slider.value = current_boss_health / max_boss_health;

                    my_slider.value = (max_boss_health > 0) ? current_boss_health / max_boss_health : 0;
                }
                else
                {
                    // 보스가 없으면 HP바를 0으로 만듦
                    my_slider.value = 0;
                }
                break;
        }
    }
}
