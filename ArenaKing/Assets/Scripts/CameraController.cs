using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public float minDistance = 5f;
    public float maxDistance = 15f;
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        float distance = Vector3.Distance(player1.position, player2.position);
        float desiredSize = Mathf.Clamp(distance, minDistance, maxDistance);
        Vector3 middlePoint = (player1.position + player2.position) / 2;

        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, desiredSize, smoothSpeed * Time.deltaTime);
        Camera.main.transform.position = new Vector3(middlePoint.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
    }
}
