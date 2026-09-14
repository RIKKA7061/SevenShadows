using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GoogleSheetLoader sheetLoader;

    [SerializeField]
    private DialogueManager dialogueManager;

    private void Start()
    {
        sheetLoader.Load(OnSheetLoaded);
    }

    private void OnSheetLoaded(
        System.Collections.Generic.List<DialogueData> data)
    {
        DialogueDatabase.Instance.SetData(data);

        dialogueManager.StartDialogue();
    }
}