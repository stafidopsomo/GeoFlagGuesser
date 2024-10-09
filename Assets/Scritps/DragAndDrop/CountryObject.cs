using UnityEngine;

public class CountryObject : MonoBehaviour
{
    public string countryName;
    public string continentName;
    private DragAndDropManager gameManager;
    private string currentSpaceName;

    private void Start()
    {
        gameManager = FindObjectOfType<DragAndDropManager>();
        //gameManager.AddCountry(this);
    }

    public bool IsInCorrectSpace()
    {
        return currentSpaceName == continentName;
    }

    public bool IsInAnySpace()
    {
        return !string.IsNullOrEmpty(currentSpaceName);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("spaceObject"))
        {
            currentSpaceName = other.GetComponent<ContinentObject>().continentName;

            if (currentSpaceName == continentName)
            {
                Debug.Log(countryName + " είναι στην λάθος ήπειρο: " + continentName);
            }
            else
            {
                Debug.Log(countryName + " είναι στην σωστή ήπειρο: " + currentSpaceName);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("spaceObject"))
        {
            Debug.Log(countryName + " βγήκε από " + other.GetComponent<ContinentObject>().continentName);
            currentSpaceName = null;
        }
    }
}
