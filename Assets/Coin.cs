using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb2D;

    [SerializeField]
    bool isInside;

    [SerializeField]
    float age;

    [SerializeField]
    MeshRenderer meshRenderer;

    public bool IsInside => isInside;

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

    public void SetMatched(Material matchMaterial)
    {
        meshRenderer.material = matchMaterial;
    }

    public void Stop()
    {
        rb2D.velocity = Vector2.zero;
        rb2D.angularVelocity = 0;
    }
}
