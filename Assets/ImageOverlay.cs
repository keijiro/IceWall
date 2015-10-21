using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ImageOverlay : MonoBehaviour
{
    [SerializeField] Texture _overlayTexture;

    [SerializeField] Shader _shader;

    Material _material;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_material == null)
        {
            _material = new Material(_shader);
            _material.hideFlags = HideFlags.DontSave;
        }
        _material.SetTexture("_OverlayTex", _overlayTexture);
        Graphics.Blit(source, destination, _material, 0);
    }
}
