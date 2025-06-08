using UnityEngine;

public enum BackgroundType
{
    Blue,
    Brown,
    Gray,
    Green,
    Pink,
    Purple,
    Yellow
}

public class AnimatedBackground : MonoBehaviour
{
    [SerializeField] private Vector2 movementDirection;
    private MeshRenderer mesh => GetComponent<MeshRenderer>();

    [Header("Background Type")]
    [SerializeField] private BackgroundType backgroundType;

    [SerializeField] private Texture2D[] textures;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (mesh != null)
        {
            Vector2 offset = Time.deltaTime * movementDirection;
            mesh.material.mainTextureOffset += offset;
        }
    }

    [ContextMenu("Set Background Type")]
    private void UpdateBackgroundTexture()
    {
        mesh.sharedMaterial.mainTexture = textures[(int)backgroundType];
    }
}
