using UnityEngine;

[ExecuteAlways]
public class WallGroup : MonoBehaviour
{
    [SerializeField]
    BoxCollider2D boxCollider2D;

    [SerializeField]
    Wall[] wallList;

    [SerializeField]
    Transform bgQuad;

    void Update()
    {
        var s = boxCollider2D.size;
        var b = boxCollider2D.bounds;
        
        wallList[0].SetColliderSize(b.size.y);
        wallList[1].SetColliderSize(b.size.y);
        wallList[2].SetColliderSize(b.size.x);
        wallList[3].SetColliderSize(b.size.x);
        
        wallList[0].SetWorldPosition(new Vector2(b.center.x, b.center.y) + Vector2.right * (b.size.x / 2));
        wallList[1].SetWorldPosition(new Vector2(b.center.x, b.center.y) - Vector2.right * (b.size.x / 2));
        wallList[2].SetWorldPosition(new Vector2(b.center.x, b.center.y) - Vector2.up * (b.size.y / 2));
        wallList[3].SetWorldPosition(new Vector2(b.center.x, b.center.y) + Vector2.up * (b.size.y / 2));
        
        bgQuad.localScale = new Vector3(b.size.x, b.size.y, 1);
    }
}