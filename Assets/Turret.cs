using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    bool interactive;

    [SerializeField]
    Transform fireRotationPivot;

    [SerializeField]
    float rotationSpeedDeg = 60;

    [SerializeField]
    GameObject coinPrefab;

    float FireSpeed => fireSpeedDelta * fireSpeedStep;

    [SerializeField]
    float fireSpeedDelta = 5;

    [SerializeField]
    int fireSpeedStep = 6;

    [SerializeField]
    float lerpRatio;

    [SerializeField]
    float rotationExtent = 40;

    [SerializeField]
    LayerMask raycastLayerMask;

    [SerializeField]
    FireLine fireLine;

    [SerializeField]
    TextMeshProUGUI powerText;

    [SerializeField]
    LayerMask coinAlignmentLayerMask;

    [SerializeField]
    LayerMask coinLayerMask;

    [SerializeField]
    readonly List<Coin> coinList = new List<Coin>();

    readonly RaycastHit2D[] raycastResultList = new RaycastHit2D[4];

    readonly RaycastHit2D[] coinResultList = new RaycastHit2D[100];

    static bool Verbose => false;

    [SerializeField]
    bool aligned;

    [SerializeField]
    Material matchMaterial;

    [SerializeField]
    Material coinMaterial;

    float horizontalInput;

    [SerializeField]
    Turret otherTurret;

    [SerializeField]
    ResultGroup resultGroup;

    void Start()
    {
        ClampAndUpdateFireSpeedStep();
    }

    void Update()
    {
        if (interactive)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
        }

        if (rotationExtent != 0)
        {
            lerpRatio += rotationSpeedDeg / (rotationExtent * 2) * horizontalInput * Time.deltaTime;
            lerpRatio = Mathf.Clamp(lerpRatio, -1, 1);
            fireRotationPivot.localRotation = Quaternion.Lerp(
                Quaternion.Euler(0, 0, rotationExtent),
                Quaternion.Euler(0, 0, -rotationExtent),
                (lerpRatio + 1) / 2);
        }

        if (interactive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireCoin();

                otherTurret.FireCoin();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                fireSpeedStep++;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                fireSpeedStep--;
            }

            ClampAndUpdateFireSpeedStep();

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (coinList.Count > 0)
                {
                    var lastCoin = coinList.Last();
                    if (lastCoin != null)
                    {
                        lastCoin.TrySetFrozen();
                    }
                }
            }
            
            var rayStartPos = fireRotationPivot.position;
            var rayDir = (Vector2) fireRotationPivot.up;
            Transform exceptTransform = null;

            var positionList = new List<Vector3> {rayStartPos};

            for (var i = 0; i < 5; i++)
            {
                if (CastRay(rayStartPos, rayDir, out var hit, i, exceptTransform))
                {
                    positionList.Add(hit.centroid);

                    rayStartPos = hit.centroid;
                    rayDir = Vector2.Reflect(rayDir, hit.normal);
                    exceptTransform = hit.transform;
                }
            }

            fireLine.SetPositions(positionList.ToArray());
        }

        CheckAlignment();
    }

    void CheckAlignment()
    {
        if (aligned) return;

        var validCoinList = coinList.Where(e => e != null && e.gameObject != null && e.IsInside).ToList();
        for (var i = 0; i < validCoinList.Count; i++)
        {
            var ca = validCoinList[i];
            var caPos = ca.transform.position;
            for (var j = i + 1; j < validCoinList.Count; j++)
            {
                var cb = validCoinList[j];
                var cbPos = cb.transform.position;

                var direction = cbPos - caPos;
                var dist = direction.magnitude;

                var resultCount = Physics2D.RaycastNonAlloc(caPos, direction, coinResultList,
                    dist, coinAlignmentLayerMask);

                if (Verbose)
                {
                    Debug.Log($"Match Result Count = {resultCount}");
                    Debug.Log($"  Start = {ca}", ca);
                    Debug.Log($"  End = {cb}", cb);
                }

                for (var k = 0; k < resultCount; k++)
                {
                    var result = coinResultList[k];
                    if (Verbose)
                    {
                        Debug.Log($"  {result}", result.transform);
                    }
                }

                if (resultCount >= 5 && dist < resultCount - 0.5f && coinResultList.Take(resultCount)
                        .All(e => e.transform.GetComponent<Coin>().IsInside))
                {
                    StopAllCoin();
                    otherTurret.StopAllCoin();

                    if (interactive)
                    {
                        resultGroup.ShowVictory();
                    }
                    else
                    {
                        resultGroup.ShowDefeat();
                    }

                    for (var k = 0; k < resultCount; k++)
                    {
                        var result = coinResultList[k];
                        result.transform.GetComponent<Coin>().SetMaterial(matchMaterial);
                    }

                    return;
                }
            }
        }
    }

    void StopAllCoin()
    {
        aligned = true;

        foreach (var coin in coinList)
        {
            if (coin != null)
            {
                coin.Stop();
            }
        }
    }

    void ClampAndUpdateFireSpeedStep()
    {
        fireSpeedStep = Mathf.Clamp(fireSpeedStep, 1, 6 + 1);

        if (interactive)
        {
            powerText.text = $"Power: {FireSpeed}";

            fireLine.SetPower(fireSpeedStep / 6.0f);
        }
    }

    bool CastRay(Vector2 rayStartPos, Vector2 rayDir, out RaycastHit2D hit, int c, Transform exceptTransform)
    {
        hit = new RaycastHit2D();

        var resultCount =
            Physics2D.CircleCastNonAlloc(rayStartPos, 0.5f, rayDir, raycastResultList, 1000.0f, raycastLayerMask);

        for (var i = 0; i < resultCount; i++)
        {
            var result = raycastResultList[i];


            var reverse = false;
            if (result.collider != null)
            {
                if (Vector3.Dot(result.transform.up, result.normal) < 0)
                {
                    reverse = true;
                }
            }

            var resultName = result.collider != null ? result.collider.name : "NONE";
            //Debug.Log($"name = {resultName} / normal = {result.normal} / reverse = {reverse}");

            if (result.collider != null && reverse == false &&
                (exceptTransform == null || exceptTransform != result.transform))
            {
                if (Verbose)
                {
                    Debug.Log(
                        $"[{c}] rayPos = {rayStartPos}, rayDir = {rayDir} : name = {resultName} / normal = {result.normal} / point = {result.point} / centroid = {result.centroid}");
                }

                hit = result;
                return true;
            }
        }

        return false;
    }

    public void FireCoin()
    {
        if (aligned) return;

        var coin = Instantiate(coinPrefab).GetComponent<Coin>();
        coin.SetLayer(coinLayerMask, coinAlignmentLayerMask);
        coin.transform.position = fireRotationPivot.position;
        coin.SetVelocity(fireRotationPivot.up * FireSpeed);
        coin.SetMaterial(coinMaterial);
        coinList.Add(coin);
    }

    public void SetHorizontalInput(float v)
    {
        horizontalInput = v;
    }
}