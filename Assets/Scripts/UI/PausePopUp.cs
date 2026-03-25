using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePopUp : MonoBehaviour
{
    public GameObject pause_pop_up; 
    private bool _is_paused = false;

    void Update()
    {
        // ESC 키나 뒤로가기 버튼 지원
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // HUD의 PauseBtn 클릭 시 호출
    public void TogglePause()
    {
        Debug.Log("일시정지 버튼 눌림! 현재 _is_paused 상태: " + _is_paused);

        _is_paused = !_is_paused; // bool 변수인 _is_paused의 현재 상태를 바꾸는 것. false -> true, true -> false

        if (_is_paused)
        {
            pause_pop_up.SetActive(true);
            Time.timeScale = 0f; 
        }
        else
        {
            ResumeGame();
        }
    }

    // 팝업 내 'Resume' 버튼 클릭 시 호출
    public void ResumeGame()
    {
        _is_paused = false;
        pause_pop_up.SetActive(false);
        Time.timeScale = 1f; 
    }

    //'Retry' 버튼 클릭 시 호출
    public void OnClickRetry()
    {
        Time.timeScale = 1f;
        pause_pop_up.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //'Lobby' 버튼 클릭 시 호출
    public void OnClickLobby()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Lobby"); 
    }
}