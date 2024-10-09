using UnityEngine;

public class CapitalScrollViewController : MonoBehaviour
{
    public Transform contentPanel;
    public GameObject capitalPrefab;
    public CapitalDataScriptableObject capitalDataScriptableObject;

    void Start()
    {
        PopulateScrollView();
    }

    void PopulateScrollView()
    {
        float initialYPosition = 0f;

        foreach (var capitalData in capitalDataScriptableObject.capitalDataArray)
        {
            GameObject newCapital = Instantiate(capitalPrefab, contentPanel);

            RectTransform capitalRectTransform = newCapital.GetComponent<RectTransform>();

            capitalRectTransform.anchoredPosition = new Vector2(0f, initialYPosition);

            initialYPosition -= capitalRectTransform.rect.height;

            CapitalPrefabController capitalPrefabController = newCapital.GetComponent<CapitalPrefabController>();
            capitalPrefabController.SetCapitalData(capitalData);
        }
    }
}
