using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb2D;

    public void SetVelocity(Vector2 v)
    {
        rb2D.velocity = v;
    }
}
