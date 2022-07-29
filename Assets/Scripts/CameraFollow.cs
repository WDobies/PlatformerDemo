using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float speed;
    public float maxDistance;
    public float xMinPos;
    public float xMaxPos;
    public Vector2 offset;
    public Transform player;

    private void FixedUpdate()
    {
        Vector2 camPosition = transform.position;
        if (player)
        {
            float distance = Vector2.Distance(camPosition, player.position);

            if (distance > maxDistance)
            {
                Vector2 targetPos = (Vector2)player.position + offset;

                //clamp camera x postion between min and max positions
                camPosition.x = Mathf.Clamp(transform.position.x, xMinPos, xMaxPos);

                Vector2 smoothPos = Vector2.Lerp(camPosition, targetPos, distance * speed * Time.deltaTime);
                transform.position = new Vector3(smoothPos.x, smoothPos.y, transform.position.z);
            }
        }
    }
}
