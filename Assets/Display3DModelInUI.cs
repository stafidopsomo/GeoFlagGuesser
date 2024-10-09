using UnityEngine;
using UnityEngine.UI;

public class Display3DModelInUI : MonoBehaviour
{
    public Camera modelCamera;
    public RenderTexture renderTexture;
    public RawImage rawImage;
    public float rotationSpeed = 10f;
    void Start()
    {
        modelCamera.targetTexture = renderTexture;
        rawImage.texture = renderTexture;
    }

    void Update()
    {
        // περιστροφη της υδρογειου μονο στον οριζοντιο αξονα
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}