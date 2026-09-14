using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 대사에 맞춰 배경 프리팹과 캐릭터를 변경합니다.
public class DialogueDirector : MonoBehaviour
{
    [System.Serializable]
    public class StageEntry
    {
        public string id;
        public GameObject prefab;
    }

    [System.Serializable]
    public class CharacterEntry
    {
        public string id;
        public Sprite sprite;
    }

    [Header("스테이지")]
    [SerializeField]
    private List<StageEntry> stages;

    [Header("스테이지 생성 위치")]
    [SerializeField]
    private Transform stageParent;

    [Header("캐릭터 이미지")]
    [SerializeField]
    private Image characterImage;

    [Header("캐릭터")]
    [SerializeField]
    private List<CharacterEntry> characters;

    private GameObject currentStage;

    public void ApplyDialogue(DialogueData data)
    {
        // 배경 프리팹 변경
        ChangeStage(data.backgroundId);

        // 캐릭터 얼굴 변경
        ChangeCharacter(data.characterId);
    }

    private void ChangeStage(string id)
    {
        if (string.IsNullOrEmpty(id))
            return;

        // 같은 프리팹이면 다시 생성하지 않음
        if (currentStage != null &&
            currentStage.name == id)
        {
            return;
        }

        foreach (StageEntry entry in stages)
        {
            if (entry.id == id)
            {
                if (currentStage != null)
                {
                    Destroy(currentStage);
                }

                currentStage =
                    Instantiate(
                        entry.prefab,
                        stageParent
                    );

                currentStage.name = id;

                Debug.Log(
                    "스테이지 변경: " + id
                );

                return;
            }
        }

        Debug.LogWarning(
            "스테이지 프리팹을 찾을 수 없습니다: "
            + id
        );
    }

    private void ChangeCharacter(string id)
    {
        // 캐릭터 없음
        if (string.IsNullOrEmpty(id) ||
            id == "NONE")
        {
            if (characterImage != null)
            {
                characterImage.gameObject.SetActive(false);
            }

            return;
        }

        foreach (CharacterEntry entry in characters)
        {
            if (entry.id == id)
            {
                if (characterImage != null)
                {
                    characterImage.gameObject.SetActive(true);

                    characterImage.sprite =
                        entry.sprite;
                }

                Debug.Log(
                    "캐릭터 변경: " + id
                );

                return;
            }
        }

        Debug.LogWarning(
            "캐릭터를 찾을 수 없습니다: " + id
        );
    }
}