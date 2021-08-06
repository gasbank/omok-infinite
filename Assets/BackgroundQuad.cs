using UnityEngine;

public class BackgroundQuad : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        var coin = other.GetComponent<Coin>();
        if (coin != null)
        {
            coin.SetInside();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var coin = other.GetComponent<Coin>();
        if (coin != null)
        {
            coin.SetOutside();
        }
    }
}
