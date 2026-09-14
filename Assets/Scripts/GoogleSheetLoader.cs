using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Google Sheets에서 TSV를 받아오는 역할입니다.
public class GoogleSheetLoader : MonoBehaviour
{
    [Header("Google Sheets TSV URL")]
    [SerializeField]
    private string csvUrl;

    public void Load(Action<List<DialogueData>> onComplete)
    {
        StartCoroutine(LoadCoroutine(onComplete));
    }

    private IEnumerator LoadCoroutine(
        Action<List<DialogueData>> onComplete)
    {
        using (UnityWebRequest request =
               UnityWebRequest.Get(csvUrl))
        {
            yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (request.result != UnityWebRequest.Result.Success)
#else
            if (request.isNetworkError ||
                request.isHttpError)
#endif
            {
                Debug.LogError(
                    "Google Sheets 불러오기 실패: "
                    + request.error);

                yield break;
            }

            string tsv = request.downloadHandler.text;

            List<DialogueData> data =
                ParseDialogueData(tsv);

            Debug.Log(
                $"대사 {data.Count}개 로드 완료");

            onComplete?.Invoke(data);
        }
    }

    private List<DialogueData> ParseDialogueData(string tsv)
    {
        List<DialogueData> result =
            new List<DialogueData>();

        string[] lines = tsv.Split(
            new[] { '\r', '\n' },
            StringSplitOptions.RemoveEmptyEntries
        );

        // 첫 번째 줄 = 헤더
        for (int i = 1; i < lines.Length; i++)
        {
            string[] columns = lines[i].Split('\t');

            // 순서 / 배경ID / 캐릭터ID / 대사 / 해금조건
            if (columns.Length < 5)
            {
                Debug.LogWarning(
                    $"잘못된 데이터 행: {lines[i]}"
                );

                continue;
            }

            // ==========================================
            // 엑셀의 각 열
            // ==========================================

            int order = 0;

            int.TryParse(
                columns[0].Trim(),
                out order
            );

            string backgroundId =
                columns[1].Trim();

            string characterId =
                columns[2].Trim();

            string dialogue =
                columns[3].Trim();

            string unlockCondition =
                columns[4].Trim();

            // 대사가 비어있으면 무시
            if (string.IsNullOrEmpty(dialogue))
                continue;

            // ==========================================
            // DialogueData 생성
            // ==========================================

            result.Add(
                new DialogueData(
                    order,
                    backgroundId,
                    characterId,
                    dialogue,
                    unlockCondition
                )
            );
        }

        // 순서대로 정렬
        result.Sort(
            (a, b) => a.order.CompareTo(b.order)
        );

        return result;
    }
}

