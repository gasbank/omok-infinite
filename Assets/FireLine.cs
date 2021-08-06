using UnityEngine;

public class FireLine : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineRenderer;

    public void SetPositions(Vector3[] positionList)
    {
        lineRenderer.SetPositions(positionList);
    }

    public void SetPower(float time)
    {
        var copyGradient = lineRenderer.colorGradient;
        var alphaKeysCopy = copyGradient.alphaKeys;
        alphaKeysCopy[1].time = time;
        copyGradient.alphaKeys = alphaKeysCopy;
        lineRenderer.colorGradient = copyGradient;
    }
}
