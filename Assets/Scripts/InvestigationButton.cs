
using TMPro;
using UnityEngine;

public class InvestigationButton : MonoBehaviour
{
    [Header("오브젝트 ID")]
    [SerializeField]
    private string objectId;

    [Header("조사 대사")]
    [TextArea(2, 5)]
    [SerializeField]
    private string investigationDialogue;

    [Header("대사 UI")]
    [SerializeField]
    private TMP_Text dialogueText;

    private DialogueManager dialogueManager;

    private void Awake()
    {
        dialogueText = FindFirstObjectByType<TMP_Text>();

        if (dialogueText == null)
        {
            Debug.LogError(
                "씬에서 TMP_Text를 찾을 수 없습니다."
            );
        }

        dialogueManager =
            FindFirstObjectByType<DialogueManager>();
    }

    public void OnClick()
    {
        Debug.Log(
            $"조사 버튼 클릭: {objectId}"
        );

        // 조사 대사 표시
        if (dialogueText != null)
        {
            dialogueText.text =
                investigationDialogue;
        }

        // DialogueManager에 클릭한 오브젝트 ID 전달
        if (dialogueManager != null)
        {
            dialogueManager.OnInvestigationButtonClicked(
                objectId.Trim()
            );
        }
        else
        {
            Debug.LogWarning(
                "DialogueManager를 찾을 수 없습니다."
            );
        }
    }
}
