using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;

    public int choosenSkinId = 0; // Default skin ID

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSkin(int skinId)
    {
        choosenSkinId = skinId;
    }

    public int GetSkin()
    {
        return choosenSkinId;
    }
}
