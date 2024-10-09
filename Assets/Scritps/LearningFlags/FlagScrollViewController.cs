using UnityEngine;

public class FlagScrollViewController : MonoBehaviour
{
    public Transform contentPanel;
    public GameObject flagPrefab;
    public FlagDataScriptableObject flagDataScriptableObject;

    void Start()
    {
        PopulateScrollView();
    }

    void PopulateScrollView()
    {
        float initialYPosition = 0f;

        foreach (var flagData in flagDataScriptableObject.flagDataArray)
        {
            GameObject newFlag = Instantiate(flagPrefab, contentPanel);

            RectTransform flagRectTransform = newFlag.GetComponent<RectTransform>();

            flagRectTransform.anchoredPosition = new Vector2(0f, initialYPosition);

            initialYPosition -= flagRectTransform.rect.height;

            FlagPrefabController flagPrefabController = newFlag.GetComponent<FlagPrefabController>();
            flagPrefabController.SetFlagData(flagData);
        }
    }
}
