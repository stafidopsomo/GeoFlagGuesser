using UnityEngine;

[CreateAssetMenu(fileName = "New Flag Data", menuName = "Custom/Flag Data")]
public class FlagDataScriptableObject : ScriptableObject
{
    public FlagData[] flagDataArray;
}

[System.Serializable]
public class FlagData
{
    public Sprite flagIcon;
    public string countryName;
}
