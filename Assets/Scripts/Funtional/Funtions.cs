using UnityEngine;

static public class Funtions
{
    static public void DrawBoxCasting(Vector3 box, Vector3 center)
    {
        // 박스의 모서리를 계산
        Vector3[] corners = new Vector3[8];
        Vector3 halfBoxSize = box / 2;

        // 박스의 8개의 코너 계산
        corners[0] = center + new Vector3(-halfBoxSize.x, -halfBoxSize.y, -halfBoxSize.z); // 왼쪽 아래 앞
        corners[1] = center + new Vector3(halfBoxSize.x, -halfBoxSize.y, -halfBoxSize.z); // 오른쪽 아래 앞
        corners[2] = center + new Vector3(-halfBoxSize.x, -halfBoxSize.y, halfBoxSize.z); // 왼쪽 아래 뒤
        corners[3] = center + new Vector3(halfBoxSize.x, -halfBoxSize.y, halfBoxSize.z); // 오른쪽 아래 뒤
        corners[4] = center + new Vector3(-halfBoxSize.x, halfBoxSize.y, -halfBoxSize.z); // 왼쪽 위 앞
        corners[5] = center + new Vector3(halfBoxSize.x, halfBoxSize.y, -halfBoxSize.z); // 오른쪽 위 앞
        corners[6] = center + new Vector3(-halfBoxSize.x, halfBoxSize.y, halfBoxSize.z); // 왼쪽 위 뒤
        corners[7] = center + new Vector3(halfBoxSize.x, halfBoxSize.y, halfBoxSize.z); // 오른쪽 위 뒤

        // 박스의 모서리 연결
        Debug.DrawLine(corners[0], corners[1], Color.red); // 아래 앞 변
        Debug.DrawLine(corners[1], corners[3], Color.red); // 아래 오른쪽 변
        Debug.DrawLine(corners[3], corners[2], Color.red); // 아래 뒤 변
        Debug.DrawLine(corners[2], corners[0], Color.red); // 아래 왼쪽 변
        Debug.DrawLine(corners[4], corners[5], Color.red); // 위 앞 변
        Debug.DrawLine(corners[5], corners[7], Color.red); // 위 오른쪽 변
        Debug.DrawLine(corners[7], corners[6], Color.red); // 위 뒤 변
        Debug.DrawLine(corners[6], corners[4], Color.red); // 위 왼쪽 변

        // 아래 위 연결
        Debug.DrawLine(corners[0], corners[4], Color.red); // 왼쪽 아래 -> 왼쪽 위
        Debug.DrawLine(corners[1], corners[5], Color.red); // 오른쪽 아래 -> 오른쪽 위
        Debug.DrawLine(corners[2], corners[6], Color.red); // 왼쪽 뒤 -> 왼쪽 위
        Debug.DrawLine(corners[3], corners[7], Color.red); // 오른쪽 뒤 -> 오른쪽 위
    }
}
    

