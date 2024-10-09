using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DragAndDropManager : MonoBehaviour
{
    public List<CountryObject> draggableObjects;
    public List<ContinentObject> spaceObjects;
    public Text resultText;
    public TextMeshProUGUI timerText;
    public GameObject modalWindow;

    public GameObject[] countryPrefabs; // πινακας με τα πιθανα prefab
    public Canvas canvasParent;

    private float startTime;
    private float elapsedTime;
    private bool isTestCompleted = false;

    void Start()
    {
        startTime = Time.time;
        // εκκαθαριση λιστας
        draggableObjects.Clear();

        // κεντρο του καμβα
        Vector2 canvasCenter = canvasParent.GetComponent<RectTransform>().sizeDelta / 2f;

        // λιστα για την αποθηκευση των ηδη παρουσιασμενων prefab
        List<int> usedPrefabIndices = new List<int>();

        // τυχαια prefabs εμφανιζονται
        for (int i = 0; i < 5; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, countryPrefabs.Length);
            } while (usedPrefabIndices.Contains(randomIndex));

            float yOffset = 280f;

            float prefabWidth = countryPrefabs[randomIndex].GetComponent<RectTransform>().rect.width;

            // εδω υπολογίζετε το που θα τοποθετηθεί, έχει δημιουργηθηει και ενα custom offset για να φερει τα εικονιδια λιγο πιο ψηλα απο το κεντρο
            Vector2 spawnPosition = new Vector2(canvasCenter.x - prefabWidth / 2f + i * (prefabWidth + 45f), canvasCenter.y + yOffset);

            // δημιουργια με τυχαιο prefab
            GameObject countryObject = Instantiate(countryPrefabs[randomIndex], spawnPosition, Quaternion.identity, canvasParent.transform);
            draggableObjects.Add(countryObject.GetComponent<CountryObject>());

            // προσθηκη στην λιστα με τα χρησιμοποιημενα
            usedPrefabIndices.Add(randomIndex);
        }
    }

    void Update()
    {
        if (!isTestCompleted)
        {
            elapsedTime = Time.time - startTime;
            UpdateTimerText();
        }

        if (isTestCompleted && (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Mouse1)))
        {
            ReturnToMainMenu();
        }
    }

    public void SubmitAnswers()
    {
        if (!isTestCompleted)
        {
            int correctCount = 0;
            int wrongCount = 0;
            int unansweredCount = 0;

            foreach (var draggable in draggableObjects)
            {
                if (draggable.IsInCorrectSpace())
                {
                    correctCount++;
                }
                else if (draggable.IsInAnySpace())
                {
                    wrongCount++;
                }
                else
                {
                    unansweredCount++;
                }
            }

            DisplayResults(correctCount, wrongCount, unansweredCount);
            CalculateScore(correctCount);
            isTestCompleted = true;

            ShowModalWindow();
            FreezeTime();
        }
    }

    void UpdateTimerText()
    {
        timerText.text = "Χρόνος: " + Mathf.RoundToInt(elapsedTime) + " δευτερόλεπτα";
    }

    void DisplayResults(int correctCount, int wrongCount, int unansweredCount)
    {
        resultText.text = "Αποτελέσματα:\n" +
                          "Σωστές: " + correctCount + "\n" +
                          "Λάθος: " + wrongCount + "\n" +
                          "Αναπάντητες: " + unansweredCount + "\n\n" +
                          "Πιέστε 'Esc' για να επιστρέψετε στο κύριο μενού";
    }

    void CalculateScore(int correctCount)
    {
        int timeBonus = CalculateTimeBonusInSeconds();
        int correctAnswersScore = correctCount * 10;
        int totalScore = timeBonus + correctAnswersScore;
        int highScore = PlayerPrefs.GetInt("DragContinentsHighScore");
        Debug.Log("Συνολικό Σκορ: " + totalScore);

        if (totalScore > highScore)
        {
            PlayerPrefs.SetInt("DragContinentsHighScore", totalScore);
            PlayerPrefs.Save();
            Debug.Log("Το σκορ νικήθηκε, εγγραφή νέου σκορ!");
        }
    }

    int CalculateTimeBonusInSeconds()
    {
        float maxTime = 60.0f;
        float timePercentage = Mathf.Clamp01(1.0f - (elapsedTime / maxTime));
        int maxTimeBonus = 50;
        int timeBonus = Mathf.RoundToInt(timePercentage * maxTimeBonus);

        return timeBonus;
    }

    void FreezeTime()
    {
        Time.timeScale = 0f;
    }

    void UnfreezeTime()
    {
        Time.timeScale = 1f;
    }

    void ShowModalWindow()
    {
        modalWindow.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void OnDisable()
    {
        UnfreezeTime();
    }
}