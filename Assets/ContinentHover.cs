using UnityEngine;
using TMPro;

//λειτυοργια εμφανισης των λεπτομερειων των ηπειρων οταν γινεται hover
public class ContinentHover : MonoBehaviour
{
    public string continentTitle;
    public TextMeshProUGUI titleText;

    private void Start()
    {
        titleText.gameObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        titleText.text = continentTitle;
        titleText.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        titleText.gameObject.SetActive(false);
    }
}
