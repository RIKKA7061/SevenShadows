using System.Collections.Generic;
using UnityEngine;

// 게임에서 대사 데이터를 관리합니다.

public class DialogueDatabase : MonoBehaviour
{
    public static DialogueDatabase Instance { get; private set; }

    private List<DialogueData> dialogues =
        new List<DialogueData>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetData(List<DialogueData> data)
    {
        dialogues = data;

        Debug.Log(
            $"DialogueDatabase 저장 완료: {dialogues.Count}개");
    }

    public DialogueData GetDialogue(int index)
    {
        if (index < 0 || index >= dialogues.Count)
            return null;

        return dialogues[index];
    }

    public int Count
    {
        get { return dialogues.Count; }
    }
}