using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class ScoreData {
    public string name;
    public int score;
}

[System.Serializable]
public class RankingList {
    public List<ScoreData> items;
}

public class RankingManager : MonoBehaviour {
    [SerializeField]
    private string gasUrl = "https://script.google.com/macros/s/AKfycbxLyauD0WVqEdturtplvtqUdL2NtanAO6S8El1VfMOzBi2pecMJH5G15WyeKH1RVj6SRQ/exec";

    [Header("UI参照")]
    public TMP_InputField nameInputField;
    public TextMeshProUGUI rankingDisplayText;

    private void Start() {
        StartCoroutine(GetRankingOnlyRoutine());
    }

    /// <summary>
    /// 起動時などにランキングを取得するためだけの関数
    /// </summary>
    private IEnumerator GetRankingOnlyRoutine() {
        if (rankingDisplayText != null) rankingDisplayText.text = "Loading Ranking...";

        ScoreData data = new ScoreData { name = "READ_ONLY", score = -1 };
        yield return StartCoroutine(PostScoreRoutine(data.name, data.score));
    }

    /// <summary>
    /// ゲームオーバー時にGameDirectorから呼ばれる自動送信関数
    /// 名前未入力時は"player"として送信
    /// </summary>
    public void AutoSendScore(int finalScore) {
        string pName = nameInputField.text;
        if (string.IsNullOrWhiteSpace(pName)) {
            pName = "player";
        }
        
        StartCoroutine(PostScoreRoutine(pName, finalScore));
    }

    /// <summary>
    /// 実際にGASと通信を行うコア部分
    /// </summary>
    private IEnumerator PostScoreRoutine(string userName, int score) {  
        ScoreData data = new ScoreData { name = userName, score = score };
        string json = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(gasUrl, "POST");// POSTメソッドを指定
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);// JSONデータをバイト配列に変換
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);// アップロードハンドラにバイト配列を設定
        request.downloadHandler = new DownloadHandlerBuffer();// ダウンロードハンドラを設定
        request.SetRequestHeader("Content-Type", "application/json");// ヘッダーにコンテンツタイプを設定

        yield return request.SendWebRequest();// リクエスト送信とレスポンス待機

        // 通信結果の確認
        if (request.result == UnityWebRequest.Result.Success) {
            Debug.Log("通信成功: " + request.downloadHandler.text);
            DisplayRanking(request.downloadHandler.text);
        } else {
            Debug.LogError("通信エラー: " + request.error);
            if (rankingDisplayText != null) rankingDisplayText.text = "Failed to load ranking.";
        }
    }

    /// <summary>
    /// 取得したJSONを画面に反映させる
    /// </summary>
    private void DisplayRanking(string jsonResponse) {
        // GASからの配列JSONを、JsonUtilityで扱えるオブジェクト形式にラップ
        string processedJson = "{ \"items\": " + jsonResponse + " }";
        RankingList ranking = JsonUtility.FromJson<RankingList>(processedJson);

        string displayStr = "<size=120%>--- TOP 10 RANKING ---</size>\n";
        
        if (ranking.items == null || ranking.items.Count == 0) {
            displayStr += "No Data Yet";
        } else {
            int count = Mathf.Min(ranking.items.Count, 10);
            for (int i = 0; i < count; i++) {
                displayStr += $"{i + 1}. {ranking.items[i].name} : {ranking.items[i].score}\n";
            }
        }
        
        if (rankingDisplayText != null) {
            rankingDisplayText.text = displayStr;
        }
    }
}
