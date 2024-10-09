using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CapitalPrefabController : MonoBehaviour
{
    public TextMeshProUGUI countryText;
    public TextMeshProUGUI capitalText;
    public TextMeshProUGUI populationText;

    public void SetCapitalData(CapitalData capitalData)
    {
        countryText = transform.Find("CountryName").GetComponent<TextMeshProUGUI>();
        capitalText = transform.Find("CapitalName").GetComponent<TextMeshProUGUI>();
        populationText = transform.Find("Population").GetComponent<TextMeshProUGUI>();

        countryText.text = capitalData.countryName;
        capitalText.text = capitalData.capital;
        populationText.text = capitalData.population;

    }
}
