using UnityEngine;

public class FireLine : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineRenderer;

    public void SetPositions(Vector3[] positionList)
    {
        lineRenderer.SetPositions(positionList);
    }
}
