
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


// 실제로 플레이어에게 대사를 보여주는 부분
public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

    [Header("연출")]
    [SerializeField] private DialogueDirector director;

    private int currentIndex = 0;
    private bool isPlaying = false;

    // 현재 조사 모드인지
    private bool isInvestigating = false;

    // 현재 조사해야 하는 오브젝트 ID
    private string requiredObjectId = "";

    public void StartDialogue()
    {
        currentIndex = 0;
        isPlaying = true;
        isInvestigating = false;
        requiredObjectId = "";

        dialoguePanel.SetActive(true);

        ShowCurrentDialogue();
    }

    private void ShowCurrentDialogue()
    {
        if (currentIndex >= DialogueDatabase.Instance.Count)
        {
            EndDialogue();
            return;
        }

        DialogueData data =
            DialogueDatabase.Instance.GetDialogue(currentIndex);

        if (data == null)
        {
            EndDialogue();
            return;
        }

        // ==========================================
        // 해금조건이 있다면 조사 모드
        // ==========================================

        if (data.HasUnlockCondition())
        {
            StartInvestigation(data.unlockCondition);
            return;
        }

        // ==========================================
        // 일반 대사
        // ==========================================

        isInvestigating = false;
        requiredObjectId = "";


        // 배경 프리팹 / 연출 적용
        if (director != null)
        {
            director.ApplyDialogue(data);
        }

        // 대사 표시
        dialogueText.text = data.dialogue;
    }

    private void StartInvestigation(string objectId)
    {
        isInvestigating = true;

        // 앞뒤 공백 제거
        requiredObjectId = objectId.Trim();

        Debug.Log(
            "조사 모드 시작 - 필요한 오브젝트: ["
            + requiredObjectId
            + "]"
        );

    }


    public void OnObjectClicked(string objectId)
    {
        if (!isPlaying || !isInvestigating)
            return;

        // 앞뒤 공백 제거
        objectId = objectId.Trim();

        Debug.Log(
            "오브젝트 클릭: ["
            + objectId
            + "] / 필요한 오브젝트: ["
            + requiredObjectId
            + "]"
        );

        // 필요한 오브젝트인지 비교
        if (string.Equals(
            objectId,
            requiredObjectId,
            System.StringComparison.OrdinalIgnoreCase))
        {
            InvestigationComplete();
            return;
        }
    }


    private void InvestigationComplete()
    {
        Debug.Log(
            "조사 완료: " + requiredObjectId
        );

        isInvestigating = false;
        requiredObjectId = "";


        // 현재 조건이 걸려 있던 대사 다시 가져오기
        DialogueData data =
            DialogueDatabase.Instance.GetDialogue(currentIndex);

        if (data == null)
            return;

        // 해금된 대사의 배경 / 캐릭터 적용
        if (director != null)
        {
            director.ApplyDialogue(data);
        }

        // 해금된 대사 표시
        dialogueText.text = data.dialogue;
    }


    public void NextDialogue()
    {
        if (!isPlaying)
            return;

        // 조사 중에는 대사 진행 금지
        if (isInvestigating)
            return;

        currentIndex++;

        ShowCurrentDialogue();
    }



    private void EndDialogue()
    {
        isPlaying = false;
        isInvestigating = false;
        requiredObjectId = "";


        dialoguePanel.SetActive(false);

        Debug.Log("대화 종료");
    }


    private void Update()
    {
        if (!isPlaying)
            return;

        // 조사 중에는 일반 대사 넘기기 불가능
        if (isInvestigating)
            return;

        // 스페이스바
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            NextDialogue();
        }

        // 마우스 왼쪽 클릭
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            NextDialogue();
        }
    }

    public void OnInvestigationButtonClicked(
    string objectId)
    {
        if (!isInvestigating)
            return;

        objectId = objectId.Trim();

        Debug.Log(
            $"필수 오브젝트 확인: [{objectId}] / " +
            $"필요: [{requiredObjectId}]"
        );

        if (string.Equals(
            objectId,
            requiredObjectId,
            System.StringComparison.OrdinalIgnoreCase))
        {
            InvestigationComplete();
        }
    }
}

