using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager instance; // 싱글톤 선언
    public static Action<bool> OnGameOverEvent; // 게임 종료 이벤트

    public bool is_gameover = false; // 게임 오버 상태
    public bool is_paused = false; // 게임 정지 상태

    [Header("UI Popups")]
    public GameObject gameover_popup;
    public GameObject pause_popup;
    public Text score_text;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        Time.timeScale = 1f; // 게임 시작 시 시간 초기화
    }

    void OnEnable() // 게임 종료 확인 이벤트 구독
    {
        OnGameOverEvent += GameOver;
    }

    void OnDisable() // 게임 종료 확인 이벤트 구독 해제
    {
        OnGameOverEvent -= GameOver;
    }

    public void GamePause()
    {
        if (is_gameover)
            return;

        is_paused = true;
        Time.timeScale = 0f; // 시간 정지
        
        if (pause_popup != null) 
            pause_popup.SetActive(true);
            
        Debug.Log("게임 일시 정지");
    }

    public void GameResume()
    {
        if (is_gameover)
            return;

        is_paused = false;
        Time.timeScale = 1f;
        
        if (pause_popup != null) 
            pause_popup.SetActive(false);
            
        Debug.Log("게임 계속하기");
    }

    public void GameRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Debug.Log("게임 재시작");
    }

    public void GameOver(bool isWin)
    {
        if (Time.timeScale == 0)
            return;

        is_gameover = true;
        Time.timeScale = 0f; // 게임 정지

        if (gameover_popup != null)
            gameover_popup.SetActive(true);
        
        Debug.Log(isWin ? "승리" : "패배");
    }

}
