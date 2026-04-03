using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(PolygonCollider2D))]
public class CameraRange : MonoBehaviour
{
    public void SetRange()
    {
        Vector2 min = TestInGameManager.instance.MapMin;
        Vector2 max = TestInGameManager.instance.MapMax;

        PolygonCollider2D poly = GetComponent<PolygonCollider2D>();

        Vector2[] path = new Vector2[4];
        path[0] = new Vector2(min.x, max.y);
        path[1] = new Vector2(max.x, max.y);
        path[2] = new Vector2(max.x, min.y);
        path[3] = new Vector2(min.x, min.y);
        poly.SetPath(0, path);

        // 카메라가 새로운 범위를 즉시 인식하도록 시네머신(카메라) 캐시 초기화
        CinemachineConfiner2D confiner = FindObjectOfType<CinemachineConfiner2D>();
        if (confiner != null)
        {
            confiner.InvalidateCache();
        }
        
        Debug.Log("카메라 제한 범위 설정 완료");
    }
}