using UnityEngine;

[CreateAssetMenu(fileName = "New Capital Data", menuName = "Custom/Capital Data")]
public class CapitalDataScriptableObject : ScriptableObject
{
    public CapitalData[] capitalDataArray;
}

[System.Serializable]
public class CapitalData
{
    public string countryName;
    public string capital;
    public string population;
}
