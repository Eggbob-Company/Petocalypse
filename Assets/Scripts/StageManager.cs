using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static StageManager instance; // 싱글톤 선언

    public bool is_gameover = false; // 게임 오버 상태
    public Text score_text; // 점수 텍스트
    public GameObject game_over_ui; // 게임 오버 시 활성화되는 팝업

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

}
