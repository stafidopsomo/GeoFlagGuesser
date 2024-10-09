using UnityEngine;
using UnityEngine.UI;

public class SubmitButton : MonoBehaviour
{
    private DragAndDropManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<DragAndDropManager>();
        GetComponent<Button>().onClick.AddListener(Submit);
    }

    private void Submit()
    {
        gameManager.SubmitAnswers();
    }
}
