using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 이동을 위해 추가

public class LobbyManager : MonoBehaviour
{
    // Start is called before the first frame update
    // 버튼이 클릭되었을 때 실행될 함수
    public void OnClickStart()
    {
        // 실제 넘어가고 싶은 씬의 이름
        SceneManager.LoadScene("InGame");
    }

    public void OnClickReinforce()
    {
        // 실제 넘어가고 싶은 씬의 이름
        SceneManager.LoadScene("Reinforce");
    }
}
