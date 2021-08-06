using UnityEngine;

public class TurretEnemy : MonoBehaviour
{
    [SerializeField]
    Turret turret;

    void Start()
    {
        //InvokeRepeating(nameof(FireRegularly), 5, 1.0f);
    }

    void Update()
    {
        turret.SetHorizontalInput(Mathf.Sin(Time.time * 2));
    }

    void FireRegularly()
    {
        turret.FireCoin();
    }
}
