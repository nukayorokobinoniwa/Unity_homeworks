using UnityEngine;

public class Parent : MonoBehaviour
{
    public enum ObjectLevel
    {
        Level0,
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        Level6,
        Level7,
        Level8,
        Level9,
        Level10
    }

    [Header("現在のレベル設定")]
    public ObjectLevel currentLevel;

    [Header("進化先のプレハブ")]
    public GameObject nextLevelGroupPrefab;

    [HideInInspector]
    public bool isMerged = false;

    [Header("合体時のエフェクト")]
    public GameObject mergeEffectPrefab;

    [Header("サウンド設定")]
    public AudioClip mergeSound;


    /// <summary>
    /// 衝突時に他のParentオブジェクトと合体する関数
    /// </summary>
    /// <param name="otherParent">合体する相手のParentオブジェクト</param>
    /// <param name="spawnPos">生成する座標</param>
    public void PerfomMerge(GameObject otherParent, Vector3 spawnPos)
    {
        if (isMerged) return;

        Parent otherParentScript = otherParent.GetComponent<Parent>();
        if (otherParentScript == null || otherParentScript.isMerged) return;

        isMerged = true;
        otherParentScript.isMerged = true;

        if (mergeSound != null){
            AudioSource.PlayClipAtPoint(mergeSound, spawnPos);
        }
        if (mergeEffectPrefab != null){
            Quaternion spawnRotation = Quaternion.Euler(-1 *90f, 0f, 0f);
            Instantiate(mergeEffectPrefab, spawnPos, spawnRotation);
        }
        if (nextLevelGroupPrefab != null){
            Instantiate(nextLevelGroupPrefab, spawnPos, Quaternion.identity);
        }

        Destroy(otherParent);
        Destroy(gameObject);
    }

}
