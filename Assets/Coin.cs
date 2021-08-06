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

    [SerializeField]
    bool isFrozen;

    [SerializeField]
    GameObject alignment;

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

    public void SetMaterial(Material mat)
    {
        meshRenderer.material = mat;
    }

    public void Stop()
    {
        rb2D.velocity = Vector2.zero;
        rb2D.angularVelocity = 0;
    }

    public void TrySetFrozen()
    {
        if (isFrozen) return;

        isFrozen = true;
        Stop();
    }

    public void SetLayer(LayerMask coinLayer, LayerMask alignmentLayer)
    {
        gameObject.layer = (int) Mathf.Log(coinLayer.value, 2);
        alignment.layer = (int) Mathf.Log(alignmentLayer.value, 2);
    }
}