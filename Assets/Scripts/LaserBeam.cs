using System.Text;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    LineRenderer lineRenderer;

    [Header("Beam Settings")]
    [SerializeField] int maxBeamReflections = 3;
    [SerializeField] float laserWidth = 0.05f;
    [SerializeField] float maxDistance;
    [SerializeField] bool LaserActive = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Laser();
    }

    

    void Laser()
    {
        if (!LaserActive) return;

        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;

        Vector3 laserStart = transform.position;
        Vector3 laserDir = transform.forward;

        lineRenderer.positionCount = maxBeamReflections + 2;
        lineRenderer.SetPosition(0, laserStart);

        int currentIndex = 1;

        for (int i = 0; i <= maxBeamReflections; i++)
        {
            Ray ray = new Ray(laserStart, laserDir);

            if (Physics.Raycast(laserStart, laserDir, out RaycastHit hitInfo))
            {
                Vector3 hitPos = hitInfo.point;
                Vector3 hitNormal = hitInfo.normal.normalized;

                // Set line point to hit position
                lineRenderer.SetPosition(currentIndex, hitPos);
                currentIndex++;

                // Reflection math
                float ReflectDot = Vector3.Dot(laserDir, hitNormal);
                Vector3 ReflectedVector = laserDir - 2f * ReflectDot * hitNormal;

                // Offset to prevent hitting same surface again
                laserStart = hitPos + ReflectedVector * 0.01f;
                laserDir = ReflectedVector;
            }
            else
            {
                // No hit, extend laser forward
                lineRenderer.SetPosition(currentIndex, laserStart + laserDir * maxDistance);
                currentIndex++;

                break;
            }
        }
        // Trim unused positions
        lineRenderer.positionCount = currentIndex;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            LaserActive = true;
        }
    }
}
