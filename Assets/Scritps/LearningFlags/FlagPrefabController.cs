using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlagPrefabController : MonoBehaviour
{
    public Image flagSprite;
    public TextMeshProUGUI countryText;

    public void SetFlagData(FlagData flagData)
    {
        flagSprite = transform.Find("FlagIcon").GetComponent<Image>();

        countryText = transform.Find("FlagName").GetComponent<TextMeshProUGUI>();

        flagSprite.sprite = flagData.flagIcon;
        countryText.text = flagData.countryName;
    }
}
