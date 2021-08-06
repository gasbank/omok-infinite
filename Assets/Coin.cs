using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb2D;

    [SerializeField]
    bool isInside;

    [SerializeField]
    float age;
    
    public void SetVelocity(Vector2 v)
    {
        rb2D.velocity = v;
    }

    public void SetInside()
    {
        isInside = true;
    }

    public void SetOutside()
    {
        isInside = false;
    }

    void Update()
    {
        age += Time.deltaTime;
        if (age > 2 && isInside == false)
        {
            Destroy(gameObject);
        }
    }
}
