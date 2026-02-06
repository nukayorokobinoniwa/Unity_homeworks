using UnityEngine;
using TMPro;
using System.Collections;

public class GameDirector : MonoBehaviour
{
    public static int score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI nowRandText;
    public TextMeshProUGUI nextRandText;

    public static int now_rand = 0;
    public static int next_rand = 0;

    [Header("UI設定")]
    public GameObject gameOverPanel;     
    public TextMeshProUGUI finalScoreText;

    public bool isGameOver { get; private set; } = false;

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start(){
        now_rand = Random.Range(0, 100);
        next_rand = Random.Range(0, 100);
        scoreText.text = "Score: 0";
        nowRandText.text = "出てくるレベル: " + GetLevelName(now_rand);
        nextRandText.text = "次に出るレベル: " + GetLevelName(next_rand);

        if (gameOverPanel != null){
            gameOverPanel.SetActive(false);
        }
    }

    /// <summary>
    /// スコアを加算する関数
    /// </summary>
    /// <param name="points">加算するスコアの値</param>
    public void AddScore(int points){
        score += points;
        Debug.Log("Current Score: " + score);
        scoreText.text = "Score: " + score.ToString();

    }

    /// <summary>
    /// 新しい乱数を生成して更新する関数
    /// </summary>
    public void MakeNewRand(){
        now_rand = next_rand;
        next_rand = Random.Range(0, 100);
        nowRandText.text = "出てくるレベル: " + GetLevelName(now_rand);
        nextRandText.text = "次に出るレベル: " + GetLevelName(next_rand);
    }

    /// <summary>
    /// ゲームオーバー処理を行う関数
    /// </summary>
    public void GameOver()
    {
        if (gameOverPanel.activeSelf) return;

        StartCoroutine(GameOverRoutine());
    }
    
    /// <summary>
    /// ゲームオーバー時のルーチン
    /// </summary>
    private IEnumerator GameOverRoutine()
    {
        Debug.Log("Game Over! Final Score: " + score);

        if (isGameOver) yield break;
        isGameOver = true;
        
        if (gameOverPanel != null){
            gameOverPanel.SetActive(true);
        }
        if (finalScoreText != null){
            finalScoreText.text = "Final Score: " + score;
        }

        yield return new WaitForSeconds(10.0f);

        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    /// <summary>
    /// 現在の乱数を取得する関数
    /// </summary>
    public int GetNowRand(){
        return now_rand;
    }

    /// <summary>
    /// 数字に基づいて "Level0" ～ "Level3" の文字列を返す
    /// </summary>
    /// <returns>レベル名の文字列</returns>
    string GetLevelName(int rand)
    {

        if (rand < 50) 
        {
            return "Level0";
        }
        else if (rand < 75) 
        {
            return "Level1";
        }
        else if (rand < 93) 
        {
            return "Level2";
        }
        else 
        {
            return "Level3";
        }
    }


}
