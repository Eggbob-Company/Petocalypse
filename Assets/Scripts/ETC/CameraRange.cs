using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class CameraRange : MonoBehaviour
{
    void Start()
    {
        SetRange();
    }

    public void SetRange()
    {

        PolygonCollider2D poly = GetComponent<PolygonCollider2D>();

        float min_x = -9f;
        float max_x = 9f;
        float min_y = -12f;
        float max_y = 12f;

        // Polygon Collider 포인트 좌표 설정
        Vector2[] path = new Vector2[4];
        path[0] = new Vector2(min_x, max_y); // 왼쪽 위
        path[1] = new Vector2(max_x, max_y); // 오른쪽 위
        path[2] = new Vector2(max_x, min_y); // 오른쪽 아래
        path[3] = new Vector2(min_x, min_y); // 왼쪽 아래

        poly.SetPath(0, path);
        
        Debug.Log("카메라 제한 범위 설정 완료");
    }
}