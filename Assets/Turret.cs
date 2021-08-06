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

    [SerializeField]
    float lerpRatio;

    [SerializeField]
    float rotationExtent = 40;
    
    void Update()
    {
        if (rotationExtent != 0)
        {
            lerpRatio += rotationSpeedDeg / (rotationExtent * 2) * Input.GetAxis("Horizontal") * Time.deltaTime;
            lerpRatio = Mathf.Clamp(lerpRatio, -1, 1);
            fireRotationPivot.localRotation = Quaternion.Lerp(
                Quaternion.Euler(0, 0, rotationExtent),
                Quaternion.Euler(0, 0, -rotationExtent), 
                (lerpRatio + 1) / 2);
        }

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
