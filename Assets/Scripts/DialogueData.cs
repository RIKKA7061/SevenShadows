
using System;

// 한 줄의 대사를 저장하는 데이터입니다.
[Serializable]
public class DialogueData
{
    public int order;
    public string backgroundId;
    public string characterId;
    public string dialogue;

    // 해금조건
    public string unlockCondition;

    public DialogueData(
        int order,
        string backgroundId,
        string characterId,
        string dialogue,
        string unlockCondition)
    {
        this.order = order;
        this.backgroundId = backgroundId;
        this.characterId = characterId;
        this.dialogue = dialogue;
        this.unlockCondition = unlockCondition;
    }

    public bool HasCharacter()
    {
        return !string.IsNullOrEmpty(characterId)
               && characterId != "NONE";
    }

    // 해금조건이 있는지 확인
    public bool HasUnlockCondition()
    {
        return !string.IsNullOrEmpty(unlockCondition);
    }
}

