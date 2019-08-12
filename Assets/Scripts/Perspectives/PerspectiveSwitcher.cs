using UnityEngine;
using System.Collections;

public class PerspectiveSwitcher : MonoBehaviour
{
    private Matrix4x4 ortho,
                        perspective;
    public float fov = 60f,
                        near = .3f,
                        far = 1000f,
                        orthographicSize = 50f;
    private float aspect;
    private MatrixBlender blender;
    private bool orthoOn;
    Camera m_camera;

    [SerializeField] private float toOrthoDuration;
    [SerializeField] private float toPerspecDuration;
    [SerializeField] private float toOrthoEase;
    [SerializeField] private float toPerspecEase;

    public bool ChangeProjection = false;

    void Start()
    {
        aspect = (float)Screen.width / (float)Screen.height;
        ortho = Matrix4x4.Ortho(-orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far);
        perspective = Matrix4x4.Perspective(fov, aspect, near, far);
        m_camera = GetComponent<Camera>();
        m_camera.projectionMatrix = ortho;
        orthoOn = true;
        blender = (MatrixBlender)GetComponent(typeof(MatrixBlender));
    }

    void Update()
    {
        if (ChangeProjection)
        {
            orthoOn = !orthoOn;
            if (orthoOn)
                blender.BlendToMatrix(ortho, toOrthoDuration, toOrthoEase, true);
            else
                blender.BlendToMatrix(perspective, toPerspecDuration, toPerspecEase, false);
        }
        ChangeProjection = false;
    }
}