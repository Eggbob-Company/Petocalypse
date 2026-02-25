using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StageTimer : MonoBehaviour
{
    public static StageTimer instance;

    [SerializeField] private Text _time_text;
    [SerializeField] private GameObject _game_over_popup;

    private float _current_time;
    public float current_time => _current_time; // 외부에서 읽기 전용으로 접근

private bool _is_timer_running = true;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else {
            Destroy(this.gameObject);
            return;
        }

        if (_game_over_popup != null) _game_over_popup.SetActive(false);
    }

    private void UpdateTimerUI() // 타이머 UI 띄우는 함수. 분/초로 나눠줌
    {
        int minutes = Mathf.FloorToInt(_current_time / 60f);
        int seconds = Mathf.FloorToInt(_current_time % 60f);
        _time_text.text = $"{minutes:D2} : {seconds:D2}";
    }

    void Update()
    {
        if (!_is_timer_running)
            return;

        _current_time += Time.deltaTime; // 매 프레임마다 시간 업데이트
        UpdateTimerUI();

         // 게임 오버 시간을 초과하면 매니저에게 알림
        if (_current_time >= 30f) // 현재는 30초로 설정해둠
        {
            _is_timer_running = false;
            
            /*
                여기에 스테이지 관리자 호출하는 함수 삽입
            */
        }
    }

}