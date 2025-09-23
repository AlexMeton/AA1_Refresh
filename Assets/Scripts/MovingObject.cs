using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public Transform[] waypoints; // lista de puntos a seguir
    public float speed = 3f;
    private int currentIndex = 0;

    private void Update()
    {
        if (waypoints.Length == 0) return;

        Transform targetPoint = waypoints[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
        }
    }
}
