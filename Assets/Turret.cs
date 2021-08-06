using System.Collections.Generic;
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

    [SerializeField]
    LayerMask raycastLayerMask;

    [SerializeField]
    FireLine fireLine;

    readonly RaycastHit2D[] raycastResultList = new RaycastHit2D[4];
    
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

        var rayStartPos = fireRotationPivot.position;
        var rayDir = (Vector2)fireRotationPivot.up;

        var positionList = new List<Vector3> {rayStartPos};

        if (CastRay(rayStartPos, rayDir, out var hit, 0, null))
        {
            positionList.Add(hit.centroid);

            var rayDir2 = Vector2.Reflect(rayDir, hit.normal);

            if (CastRay(hit.centroid, rayDir2, out var hit2, 1, hit.transform))
            {
                positionList.Add(hit2.centroid);
                
                var rayDir3 = Vector2.Reflect(rayDir2, hit2.normal);

                if (CastRay(hit2.centroid, rayDir3, out var hit3, 1, hit2.transform))
                {
                    positionList.Add(hit3.centroid);
                }
            }
        }

        fireLine.SetPositions(positionList.ToArray());
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

            if (result.collider != null && reverse == false && (exceptTransform == null || exceptTransform != result.transform))
            {
                Debug.Log(
                    $"[{c}] rayPos = {rayStartPos}, rayDir = {rayDir} : name = {resultName} / normal = {result.normal} / point = {result.point} / centroid = {result.centroid}");

                hit = result;
                return true;
            }
        }

        return false;
    }

    void FireCoin()
    {
        var coin = Instantiate(coinPrefab).GetComponent<Coin>();
        coin.transform.position = fireRotationPivot.position;
        coin.SetVelocity(fireRotationPivot.up * fireSpeed);
    }
}
