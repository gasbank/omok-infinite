using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    Transform fireRotationPivot;

    [SerializeField]
    float rotationSpeedDeg = 60;

    [SerializeField]
    GameObject coinPrefab;

    [SerializeField]
    float fireSpeed = 5;
    
    void Update()
    {
        fireRotationPivot.Rotate(rotationSpeedDeg * Input.GetAxis("Horizontal") * Time.deltaTime * Vector3.back);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireCoin();
        }
    }

    void FireCoin()
    {
        var coin = Instantiate(coinPrefab).GetComponent<Coin>();
        coin.transform.position = fireRotationPivot.position;
        coin.SetVelocity(fireRotationPivot.transform.up * fireSpeed);
    }
}
