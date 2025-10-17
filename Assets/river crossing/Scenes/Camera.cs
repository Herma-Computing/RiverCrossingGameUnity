using UnityEngine;
using System.Collections;

[ExecuteAlways] // works in Editor + Play mode
public class CameraAnchor : MonoBehaviour
{
    public enum AnchorType
    {
        BottomLeft,
        BottomCenter,
        BottomRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        TopLeft,
        TopCenter,
        TopRight,
    };

    [SerializeField] private AnchorType anchorType;
    [SerializeField] private Vector3 anchorOffset;

    private Camera targetCamera;

    private void OnEnable()
    {
        targetCamera = Camera.main;
        UpdateAnchor();
    }

    private void LateUpdate()
    {
        UpdateAnchor();
    }

    private void UpdateAnchor()
    {
        if (targetCamera == null) return;

        Vector3 anchor = GetAnchorWorldPosition(anchorType);
        transform.position = anchor + anchorOffset;
    }

    private Vector3 GetAnchorWorldPosition(AnchorType type)
    {
        switch (type)
        {
            case AnchorType.BottomLeft: return targetCamera.ViewportToWorldPoint(new Vector3(0, 0, targetCamera.nearClipPlane));
            case AnchorType.BottomCenter: return targetCamera.ViewportToWorldPoint(new Vector3(0.5f, 0, targetCamera.nearClipPlane));
            case AnchorType.BottomRight: return targetCamera.ViewportToWorldPoint(new Vector3(1, 0, targetCamera.nearClipPlane));

            case AnchorType.MiddleLeft: return targetCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, targetCamera.nearClipPlane));
            case AnchorType.MiddleCenter: return targetCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, targetCamera.nearClipPlane));
            case AnchorType.MiddleRight: return targetCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, targetCamera.nearClipPlane));

            case AnchorType.TopLeft: return targetCamera.ViewportToWorldPoint(new Vector3(0, 1, targetCamera.nearClipPlane));
            case AnchorType.TopCenter: return targetCamera.ViewportToWorldPoint(new Vector3(0.5f, 1, targetCamera.nearClipPlane));
            case AnchorType.TopRight: return targetCamera.ViewportToWorldPoint(new Vector3(1, 1, targetCamera.nearClipPlane));
        }

        return Vector3.zero;
    }
}
