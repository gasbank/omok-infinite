using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    EdgeCollider2D edgeCollider2D;

    public void SetColliderSize(float w)
    {
        edgeCollider2D.points[0] = new Vector2( w / 2, 0);
        edgeCollider2D.points[1] = new Vector2( -w / 2, 0);
    }

    public void SetWorldPosition(Vector2 p)
    {
        transform.position = p;
    }
}
