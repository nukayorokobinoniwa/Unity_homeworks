using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public static int score = 0;

    public static int now_rand = 0;
    public static int next_rand = 0;

    /// <summary>
    /// スコアを加算する関数
    /// </summary>
    /// <param name="points">加算するスコアの値</param>
    public void AddScore(int points){
        score += points;
        Debug.Log("Current Score: " + score);
    }

    /// <summary>
    /// 新しい乱数を生成して更新する関数
    /// </summary>
    public void MakeNewRand(){
        now_rand = next_rand;
        next_rand = Random.Range(0, 100);
    }

    /// <summary>
    /// ゲームオーバー処理を行う関数
    /// </summary>
    public void GameOver(){
        Debug.Log("Game Over! Final Score: " + score);
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


}
