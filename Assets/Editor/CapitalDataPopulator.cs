using UnityEngine;
using UnityEditor;
using System.IO;

public class CapitalDataPopulator
{
    [MenuItem("Tools/Populate Capital Data")]
    public static void PopulateCapitalData()
    {
        //το ιδιο σκριπτ και για το γεμισμα των πρωτευουσων
        string assetPath = "Assets/CapitalData.asset";
        CapitalDataScriptableObject capitalDataScriptableObject = AssetDatabase.LoadAssetAtPath<CapitalDataScriptableObject>(assetPath);

        if (capitalDataScriptableObject == null)
        {
            Debug.LogError("Αποτυχία φόρτωσης του FlagDataScriptableObject. Πρέπει να βρίσκεται στο path: " + assetPath);
            return;
        }

        // φορτωση δεδομενων απο το json
        string jsonFilePath = "Assets/Resources/capitals.json";
        CapitalInfo[] capitalInfos = LoadCapitalInfoFromJSON(jsonFilePath);

        if (capitalInfos != null)
        {
            // αρχικοποιηση του πινακα
            capitalDataScriptableObject.capitalDataArray = new CapitalData[capitalInfos.Length];

            // γεμισμα με τα δεδομενα απο το json
            for (int i = 0; i < capitalInfos.Length; i++)
            {
                capitalDataScriptableObject.capitalDataArray[i] = new CapitalData();
                capitalDataScriptableObject.capitalDataArray[i].countryName = capitalInfos[i].countryName;
                capitalDataScriptableObject.capitalDataArray[i].capital = capitalInfos[i].capital;
                capitalDataScriptableObject.capitalDataArray[i].population = capitalInfos[i].population;
            }

            // αποθηκευση
            EditorUtility.SetDirty(capitalDataScriptableObject);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Τα δεδομένα σημαίων έχουν αποθηκευτεί επιτυχώς!");
        }
        else
        {
            Debug.LogError("Αποτυχία διαδικασίας.");
        }
    }

    private static CapitalInfo[] LoadCapitalInfoFromJSON(string filePath)
    {
        string json = File.ReadAllText(filePath);
        // κανουμε deserialize το json στην δικη μας CapitalInfoContainer μορφη που δηλωσαμε παρακατω οπως καναμε και για τις σημαιες
        CapitalInfoContainer container = JsonUtility.FromJson<CapitalInfoContainer>(json);
        return container.capitals;
    }

    [System.Serializable]
    public class CapitalInfoContainer
    {
        public CapitalInfo[] capitals;
    }

    [System.Serializable]
    public class CapitalInfo
    {
        public string countryName;
        public string capital;
        public string population;
    }
}
