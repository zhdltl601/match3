using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;

    }
    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        Vector3 worldPoint = mainCamera.ScreenToWorldPoint(mousePosition);

        const float radius = 0.05f;

        Collider2D castCollider = Physics2D.OverlapCircle(worldPoint, radius);
        Debug.DrawRay(worldPoint, Vector3.left * radius, Color.red, 0.1f);
        Debug.DrawRay(worldPoint, Vector3.right * radius, Color.magenta, 0.1f);
        if (castCollider != null)
        {
            bool nodeFlag = castCollider.TryGetComponent(out Node node);
            Debug.Assert(nodeFlag, "node not found");

            if (nodeFlag)
            {
                //node.
            }

            //castCollider.tg
        }
    }
}
