using UnityEngine;

public class BoidWrap : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.forward;
    public float speed = 2f;

    public float wallAvoidanceDistance = 1.0f;
    public float wallAvoidanceStrength = 5f;

    // Define boundary size (half-extent in each direction from center)
    public float boundsWidth = 10f;
    public float boundsHeight = 5f;
    public float boundsDepth = 10f;

    void Update()
    {
        AvoidWalls();

        transform.position += moveDirection.normalized * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(moveDirection.normalized);
    }

    void AvoidWalls()
    {
        Vector3 pos = transform.position;
        Vector3 steer = Vector3.zero;

        float left = -boundsWidth;
        float right = boundsWidth;
        float bottom = -boundsHeight;
        float top = boundsHeight;
        float back = -boundsDepth;
        float front = boundsDepth;

        // Avoid left wall (X-)
        if (pos.x - left < wallAvoidanceDistance)
        {
            steer += Vector3.right * wallAvoidanceStrength * (1f - (pos.x - left) / wallAvoidanceDistance);
        }

        // Avoid right wall (X+)
        if (right - pos.x < wallAvoidanceDistance)
        {
            steer += Vector3.left * wallAvoidanceStrength * (1f - (right - pos.x) / wallAvoidanceDistance);
        }

        // Avoid bottom wall (Y-)
        if (pos.y - bottom < wallAvoidanceDistance)
        {
            steer += Vector3.up * wallAvoidanceStrength * (1f - (pos.y - bottom) / wallAvoidanceDistance);
        }

        // Avoid top wall (Y+)
        if (top - pos.y < wallAvoidanceDistance)
        {
            steer += Vector3.down * wallAvoidanceStrength * (1f - (top - pos.y) / wallAvoidanceDistance);
        }

        // Avoid back wall (Z-)
        if (pos.z - back < wallAvoidanceDistance)
        {
            steer += Vector3.forward * wallAvoidanceStrength * (1f - (pos.z - back) / wallAvoidanceDistance);
        }

        // Avoid front wall (Z+)
        if (front - pos.z < wallAvoidanceDistance)
        {
            steer += Vector3.back * wallAvoidanceStrength * (1f - (front - pos.z) / wallAvoidanceDistance);
        }

        if (steer != Vector3.zero)
        {
            moveDirection += steer * Time.deltaTime;
            moveDirection = moveDirection.normalized;
        }
    }

    // Draw boundary box in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(boundsWidth * 2f, boundsHeight * 2f, boundsDepth * 2f));
    }
}