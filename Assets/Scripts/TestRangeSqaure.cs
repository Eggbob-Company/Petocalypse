using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Player 태그 오브젝트를 따라가는 스크립트
public class TestRangeSquare : MonoBehaviour
{
    private Transform _player_transform;

    private void Start()
    {
        FindPlayer();
    }

    private void LateUpdate()
    {
        if (_player_transform == null)
        {
            FindPlayer();
            return;
        }

        FollowPlayer();
    }

    private void FindPlayer()
    {
        GameObject player_go = GameObject.FindWithTag("Player");
        if (player_go != null)
        {
            _player_transform = player_go.transform;
        }
    }

    private void FollowPlayer()
    {
        Vector2 player_pos = (Vector2)_player_transform.position;
        transform.position = new Vector2(player_pos.x, player_pos.y);
    }
}