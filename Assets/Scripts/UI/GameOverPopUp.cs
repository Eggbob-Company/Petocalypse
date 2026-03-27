using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverPopUp : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject game_over_popup; 
    public TMP_Text final_kill_text;   
    public TMP_Text final_gold_text;

    //팝업창을 띄우는 기능
    public void Show()
    {
        final_kill_text.text = string.Format("KILL {0:N0}", InGameManager.instance.kill);
        final_gold_text.text = string.Format("G {0:N0}", InGameManager.instance.gold);

        // 팝업 활성화 및 게임 정지
        game_over_popup.SetActive(true);
        Time.timeScale = 0f; //Unity 엔진에서 시간이 흐르는 배율. (0.5는 절반 속도, 2는 2배속)
    }

    //다시 시작 버튼 (Retry)
    public void OnClickRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 재로드
    }

    //로비로 이동 버튼 (Lobby)
    public void OnClickLobby()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Lobby"); // 로비 씬
    }
}