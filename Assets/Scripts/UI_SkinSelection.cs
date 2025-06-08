using UnityEngine;

public class UI_SkinSelection : MonoBehaviour
{
    [SerializeField] private int currentIndex;
    [SerializeField] private int maxIndex;
    [SerializeField] private Animator skinDisplay;

    public void NextSkin()
    {
        currentIndex++;
        if (currentIndex >= maxIndex)
        {
            currentIndex = 0;
        }

        UpdateSkinDisplay();
    }

    public void PreviousSkin()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = maxIndex-1;
        }

        UpdateSkinDisplay();
    }

    private void UpdateSkinDisplay()
    {
        for(int i = 0; i < skinDisplay.layerCount; i++)
        {
            skinDisplay.SetLayerWeight(i, 0);
        }

        skinDisplay.SetLayerWeight(currentIndex, 1);
    }

    public void SelectSkin()
    {
        SkinManager.Instance.SetSkin(currentIndex);
    }
}
