using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Lean.Gui;
using UnityEngine.SceneManagement;
using System;

public class FlagsQuizGameManagerHard : MonoBehaviour
{
    public Image flagImage;
    public TextMeshProUGUI[] optionTexts;
    public LeanButton[] optionButtons;
    public TextMeshProUGUI timerText;
    public Text modalWindowText;
    public Image modalIcon;
    public TextMeshProUGUI scoreText;
    public TextAsset countriesJson; // αναφορα στο JSON αρχειο που εχει τα ονοματα χωρων και τα ISO

    private List<Sprite> flagSprites = new List<Sprite>();
    private List<string> countryNames = new List<string>();
    private List<string> remainingCountries = new List<string>();
    private string correctCountry;
    private int correctOptionIndex;
    private int correctAnswers;
    private int totalFlags;
    private float startTime;
    private bool isGameOver = false;
    private Dictionary<string, string> countryDictionaryHard = new Dictionary<string, string>();
    private int flagsGuessed = 0;
    private bool isModalShown = false;

    private void Start()
    {
        startTime = Time.time;
        LoadFlagSprites();
        LoadCountryNames();
        totalFlags = flagSprites.Count;
        UpdateScoreText();
        LoadNextFlag();
    }

    private void Update()
    {
        if (!isGameOver)
        {
            float elapsedTime = Time.time - startTime;
            UpdateTimerText(elapsedTime);
        }
        else if (isGameOver && (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0)))
        {
            GoToMainMenu();
        }
    }

    private void LoadFlagSprites()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Flags");
        flagSprites.AddRange(sprites);
    }

    private void LoadCountryNames()
    {
        string jsonText = countriesJson.text;
        CountriesData countriesData = JsonUtility.FromJson<CountriesData>(jsonText);
        foreach (CountryData country in countriesData.flags)
        {
            countryNames.Add(country.countryName);
            countryDictionaryHard.Add(country.countryCode, country.countryName); // σε ενα dictionary αποθκευουμε κωδικο και ονομα χωρας
        }
    }

    private void LoadNextFlag()
    {
        if (flagsGuessed < 20)
        {
            bool validCountryFound = false;

            while (!validCountryFound)
            {
                int randomIndex = UnityEngine.Random.Range(0, flagSprites.Count);
                Sprite randomFlag = flagSprites[randomIndex];
                correctCountry = GetCountryName(randomFlag.name);

                // ελεγχος εαν το ISO υπαρχει στο json
                if (!string.IsNullOrEmpty(correctCountry))
                {
                    validCountryFound = true;

                    flagImage.sprite = randomFlag;
                    remainingCountries.Clear();
                    remainingCountries.AddRange(countryNames);

                    List<string> options = new List<string>();

                    options.Add(correctCountry);

                    // αφαιρουμε την χωρα απο τα remaining countries για να μην εμφανιστει δευτερη φορα
                    remainingCountries.Remove(correctCountry);

                    // εδω γεμιζουμε τις υπολοιπες επιλογες με τυχαιες λανθασμενες ωστε να εχουμε συνολο 4 επιλογες με μια να ειναι σωστη
                    for (int i = 1; i < optionTexts.Length; i++)
                    {
                        int randomCountryIndex = UnityEngine.Random.Range(0, remainingCountries.Count);
                        options.Add(remainingCountries[randomCountryIndex]);
                        remainingCountries.RemoveAt(randomCountryIndex);
                    }

                    // τυχαιο ανακατεμα
                    for (int i = 0; i < options.Count; i++)
                    {
                        string temp = options[i];
                        randomIndex = UnityEngine.Random.Range(i, options.Count);
                        options[i] = options[randomIndex];
                        options[randomIndex] = temp;
                    }

                    // ορισμος των τυχαιων επιλογων στη λιστα
                    for (int i = 0; i < optionTexts.Length; i++)
                    {
                        optionTexts[i].text = options[i];

                        // ευρεση της σωστης απαντησης
                        if (options[i] == correctCountry)
                        {
                            correctOptionIndex = i;
                        }
                    }
                }
            }
        }
        else
        {
            EndGame();
        }
    }

    private string GetCountryName(string isoCode)
    {
        Debug.Log("ISO Code: " + isoCode);

        if (countryDictionaryHard.ContainsKey(isoCode.ToUpper()))
        {
            string countryName = countryDictionaryHard[isoCode.ToUpper()];
            Debug.Log("Country Name: " + countryName);
            return countryName; // επιστρεφουμε το ονομα χωρας βαση του κωδικου
        }
        return "";
    }

    public void OnOptionSelected(int selectedOptionIndex)
    {
        if (!isGameOver)
        {
            flagsGuessed++;

            if (selectedOptionIndex == correctOptionIndex)
            {
                modalWindowText.text = "<size=60>Σωστό!</size>\r\n\r\nΗ σωστή απάντηση είναι: \r\n" + correctCountry;
                modalIcon.color = Color.green;
                correctAnswers++;
            }
            else
            {
                modalWindowText.text = "<size=60>Λάθος!</size>\r\n\r\nΗ σωστή απάντηση είναι: \r\n" + correctCountry;
                modalIcon.color = Color.red;
            }

            UpdateScoreText();

            if (flagsGuessed < 20)
            {
                LoadNextFlag();
            }
            else
            {
                EndGame();
            }
        }
    }

    private void UpdateScoreText()
    {
        int questionsPresented = Mathf.Min(flagsGuessed, totalFlags);
        float correctRatio = (float)correctAnswers / questionsPresented;
        scoreText.text = correctAnswers + " / " + questionsPresented + " Σωστές απαντήσεις (" + (correctRatio * 100f).ToString("F2") + "%)";
    }

    private void UpdateTimerText(float elapsedTime)
    {
        timerText.text = "Χρόνος: " + elapsedTime.ToString("F0") + "'";
    }

    private void EndGame()
    {
        isModalShown = true;
        int questionsPresented = Mathf.Min(flagsGuessed, totalFlags);
        isGameOver = true;
        float elapsedTime = Time.time - startTime;
        int timeBonus = CalculateTimeBonus(elapsedTime);
        int correctAnswersScore = correctAnswers * 10;
        int totalScore = timeBonus + correctAnswersScore;

        // αποθηκευση σκορ εαν ειανι μ,εγαλυτερο του αποθηκευμενου
        int previousScore = PlayerPrefs.GetInt("QuizFlagHardScore", 0);
        if (totalScore > previousScore)
        {
            PlayerPrefs.SetInt("QuizFlagHardScore", totalScore);
            PlayerPrefs.Save();
        }

        scoreText.text = correctAnswers + " / " + questionsPresented + "Σωστές απαντήσεις";
        modalWindowText.text = "<size=30>Ολοκλήρωση κουιζ!</size>\r\n\r\n\r\n"
            + "Με " + correctAnswers + " σωστές απαντήσεις\r\n"
            + "Συνολικός χρόνος: " + elapsedTime.ToString("F2") + " δευτερόλεπτα\r\n"
            + "Το σκορ σου είναι: " + totalScore.ToString("F2") + ". Συγχαρητήρια!\r\n"
            + "Πίεσε 'Esc' για να επιστρέψεις στο αρχικό μένού";
        modalIcon.color = Color.blue;
    }

    private int CalculateTimeBonus(float elapsedTime)
    {
        float maxTime = 60.0f;
        float timePercentage = Mathf.Clamp01(1.0f - (elapsedTime / maxTime));
        int maxTimeBonus = 50;
        int timeBonus = Mathf.RoundToInt(timePercentage * maxTimeBonus);
        return timeBonus;
    }

    [System.Serializable]
    private class CountryData
    {
        public string countryName;
        public string countryCode;
    }

    [System.Serializable]
    private class CountriesData
    {
        public List<CountryData> flags;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}