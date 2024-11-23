using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public Renderer backgroundRenderer;
    public float scrollSpeed = 0.1f;

    public void UpdateBackground()
    {
        backgroundRenderer.material.mainTextureOffset += new Vector2(0f, (scrollSpeed * Time.deltaTime) % 1f);
    }
}
