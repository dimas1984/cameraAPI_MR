using UnityEngine;
using PassthroughCameraSamples;

public class PassthroughCameraDisplay : MonoBehaviour
{
    public WebCamTextureManager WebCamTextureManager;
    public Renderer quadRenderer;
    public string textureName;
    public float quadDistance = 1;

    private Texture2D m_texture;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        quadRenderer.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (WebCamTextureManager != null)
        {
            //quadRenderer.material.mainTexture = WebCamTextureManager.WebCamTexture;
            if(OVRInput.GetDown(OVRInput.Button.One) && WebCamTextureManager.WebCamTexture != null)
            {
                TakePicture();
                PlaceQuad();
            }
        }
    }

    public void TakePicture()
    {
        quadRenderer.gameObject.SetActive(true);
        int widhth = WebCamTextureManager.WebCamTexture.width;
        int height = WebCamTextureManager.WebCamTexture.height;

        if(m_texture == null)
        {
            m_texture = new Texture2D(widhth, height);
        }
        Color32[] pixels = new Color32[widhth * height];
        WebCamTextureManager.WebCamTexture.GetPixels32(pixels);

        m_texture.SetPixels32(pixels);
        m_texture.Apply();

        //quadRenderer.material.mainTexture = m_texture;
        quadRenderer.material.SetTexture(textureName, m_texture);
    }

    public void PlaceQuad()
    {
        Transform quadTransform = quadRenderer.transform;

        Pose cameraPose = PassthroughCameraUtils.GetCameraPoseInWorld(PassthroughCameraEye.Left);
        
        Vector2Int  resolution = PassthroughCameraUtils.GetCameraIntrinsics(PassthroughCameraEye.Left).Resolution;

        quadTransform.position = cameraPose.position + cameraPose.forward * quadDistance;
        quadTransform.rotation = cameraPose.rotation;

        Ray leftSide = PassthroughCameraUtils.ScreenPointToRayInCamera(PassthroughCameraEye.Left, new Vector2Int(0, resolution.y/2));
        Ray rightSide = PassthroughCameraUtils.ScreenPointToRayInCamera(PassthroughCameraEye.Left, new Vector2Int(resolution.x, resolution.y / 2));


        float horizontalFov =Vector3.Angle(leftSide.direction, rightSide.direction);
        float quadScale = 2 * quadDistance*Mathf.Tan(horizontalFov * Mathf.Deg2Rad)/2;

        float ratio = (float)m_texture.height / m_texture.width;

        quadTransform.localScale = new Vector3(quadScale,quadScale*ratio,1);
    }

}
