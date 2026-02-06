using UnityEngine;

public class Injection : MonoBehaviour
{
    [Header("生成するプレハブの設定")]
    public GameObject level0Prefab;
    public GameObject level1Prefab;
    public GameObject level2Prefab;
    public GameObject level3Prefab;

    [Header("移動範囲の設定")]
    public float minX = -2.0f;
    public float maxX = 2.0f;
    public float minZ = -2.0f;
    public float maxZ = 2.0f;
    public float moveSpeed = 5.0f;

    [Header("ゲームディレクター")]
    public GameDirector gameDirector;

    /// <summary>
    /// 更新関数
    /// </summary>
    void Update()
    {
        if (gameDirector != null && gameDirector.isGameOver){
            return; 
        }
        HandleMovement();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnPrefab();
        }
    }

    /// <summary>
    /// 上下左右キーによる移動処理（範囲制限付き）
    /// </summary>
    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        Vector3 newPos = transform.position + new Vector3(moveX, 0, moveZ);

        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.z = Mathf.Clamp(newPos.z, minZ, maxZ);

        transform.position = newPos;
    }

    /// <summary>
    /// 確率に基づいてプレハブを生成する
    /// </summary>
    private void SpawnPrefab()
    {
        int rand = gameDirector.GetNowRand(); 
        GameObject prefabToSpawn = null;

        if (rand < 50) 
        {
            prefabToSpawn = level0Prefab;
        }
        else if (rand < 75) 
        {
            prefabToSpawn = level1Prefab;
        }
        else if (rand < 93) 
        {
            prefabToSpawn = level2Prefab;
        }
        else 
        {
            prefabToSpawn = level3Prefab;
        }

        if (prefabToSpawn != null)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y - 1.0f, transform.position.z);
            
            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        }
        gameDirector.MakeNewRand();
    }
}
