using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class FlagDataPopulator : MonoBehaviour
{
    //αυτο το σκιρπτ μας βοηθαει να γεμισουμε ολα τα δεδομενα απο τις σημαιες (εικονιδια, ονοματα κτλπ) αυτοματα απο μια επιλογη μενου
    [MenuItem("Tools/Populate Flag Data")]
    public static void PopulateFlagData()
    {
        string assetPath = "Assets/FlagData.asset";
        FlagDataScriptableObject flagDataScriptableObject = AssetDatabase.LoadAssetAtPath<FlagDataScriptableObject>(assetPath);

        if (flagDataScriptableObject == null)
        {
            Debug.LogError("Αποτυχία φόρτωσης του FlagDataScriptableObject. Πρέπει να βρίσκεται στο path: " + assetPath);
            return;
        }

        // φορτωση εικονδιων
        Sprite[] flagIcons = LoadFlagIconsFromFolder("Assets/Resources/Flags");

        // φορτωση ονοματων χωρων απο json
        string jsonFilePath = "Assets/Resources/countries.json";
        FlagInfo[] flagInfos = LoadFlagInfoFromJSON(jsonFilePath);

        // εικονιδια και χωρες πρεπει να εχουν το ιδιο ονομα και να εχουν το ιδιο πληθος για να ειμαστε σιγουροι
        // οτι το οι λιστες θα ειναι συμβατες (προυποθετει οτι οι λιστες εχουν την ιδια σειρα)
        if (flagIcons != null && flagInfos != null && flagIcons.Length == flagInfos.Length)
        {
            // αρχικοποιηση
            flagDataScriptableObject.flagDataArray = new FlagData[flagIcons.Length];

            // γεμισμα των δεδομενων
            for (int i = 0; i < flagIcons.Length; i++)
            {
                flagDataScriptableObject.flagDataArray[i] = new FlagData();
                flagDataScriptableObject.flagDataArray[i].flagIcon = flagIcons[i];
                flagDataScriptableObject.flagDataArray[i].countryName = flagInfos[i].countryName;
            }

            // αποθηκευση
            EditorUtility.SetDirty(flagDataScriptableObject);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Τα δεδομένα σημαίων έχουν αποθηκευτεί επιτυχώς!");
        }
        else
        {
            Debug.LogError("Αποτυχία διαδικασίας.");
        }
    }

    private static Sprite[] LoadFlagIconsFromFolder(string folderPath)
    {
        //Φορτωση των εικονιδιων απο τον φακελο resources που τα εχουμε
        return Resources.LoadAll<Sprite>(folderPath.Replace("Assets/Resources/", ""));
    }

    private static FlagInfo[] LoadFlagInfoFromJSON(string filePath)
    {
        string json = File.ReadAllText(filePath);
        // κανουμε deserialize το json στην δικη μας FlagInfoContainer μορφη που δηλωσαμε παρακατω
        FlagInfoContainer container = JsonUtility.FromJson<FlagInfoContainer>(json);
        return container.flags;
    }

    [System.Serializable]
    public class FlagInfoContainer
    {
        public FlagInfo[] flags;
    }
}

[System.Serializable]
public class FlagInfo
{
    public string countryName;
    public string countryCode;
}
