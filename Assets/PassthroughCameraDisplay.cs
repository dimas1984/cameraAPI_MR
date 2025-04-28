using UnityEngine;
using PassthroughCameraSamples;

public class PassthroughCameraDisplay : MonoBehaviour
{
    public WebCamTextureManager WebCamTextureManager;
    public Renderer quadRenderer;
    public string textureName;

    private Texture2D m_texture;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
            }
        }
    }

    public void TakePicture()
    {
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

}
