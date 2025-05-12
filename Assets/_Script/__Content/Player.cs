using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera mainCamera;
    private Node currentNode;
    private Vector2 initialNodeVector;
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector2 worldMousePoint = mainCamera.ScreenToWorldPoint(mousePosition);

        const float k_radius = 0.05f;

        //single click
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D castCollider = Physics2D.OverlapCircle(worldMousePoint, k_radius);
            Node nodeComponent = null;
            if (castCollider != null)
            {
                bool flagIsNode = castCollider.TryGetComponent(out nodeComponent);
                Debug.Assert(flagIsNode, "node not found");

                nodeComponent.NodeActive(true);
                initialNodeVector = nodeComponent.transform.position;
            }
            currentNode = nodeComponent;
        }

        bool flagIsSelectNode = currentNode != null;
        if (flagIsSelectNode)
        {
            Vector2 direction = worldMousePoint - initialNodeVector;

            bool flagIsOrigin = direction.IsOrigin();
            Vector2Int clampedDirection = direction.GetClampedVectorInt();
            Vector2Int clampedDirectionOpposite = -clampedDirection;

            //up
            if (Input.GetMouseButtonUp(0))
            {
                int tarX = currentNode.X + clampedDirection.x;
                int tarY = currentNode.Y + clampedDirection.y;

                currentNode.NodeActive(false);
                currentNode.MovePosition(clampedDirection.x, clampedDirection.y);

                Node changeTargetNode = NodeManager.GetNodeFromArray(tarX, tarY);
                if (changeTargetNode != null)
                {
                    changeTargetNode.MovePosition(clampedDirectionOpposite.x, clampedDirectionOpposite.y);
                }

                //swap
                NodeManager.SetNode(currentNode.X, currentNode.Y, currentNode);
                NodeManager.SetNode(changeTargetNode.X, changeTargetNode.Y, changeTargetNode);
            }
            //holding
            if (Input.GetMouseButton(0))
            {
                Color color = flagIsOrigin ? Color.red : Color.blue;
                Debug.DrawRay(initialNodeVector, (Vector2)clampedDirection, Color.magenta);
                Debug.DrawLine(initialNodeVector, worldMousePoint, color);
            }
        }
    }
    //private bool TryGetNodeRaycast(Vector2 mousePoint, float radius, out Node node)
    //{
    //    Collider2D collider = Physics2D.OverlapCircle(mousePoint, radius);
    //    bool result = collider != null;
    //    Node tempNode = null;
    //    if (result)
    //    {
    //        result = collider.TryGetComponent(out tempNode);
    //    }
    //    node = tempNode;
    //    return result;
    //}
}
