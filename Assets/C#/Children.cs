using UnityEngine;

public class Children : MonoBehaviour
{
    private Parent myParent;

    [Header("ゲームディレクター")]
    public GameDirector gameDirector;

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start(){
        if (gameDirector == null)
        {
            gameDirector = Object.FindFirstObjectByType<GameDirector>();
        }

        if (gameDirector == null)
        {
            Debug.LogError("ヒエラルキーに GameDirector が見つかりません！");
        }

        myParent = GetComponentInParent<Parent>();
        
        if(myParent == null){
            Debug.LogError("ChildrenオブジェクトにParentコンポーネントが見つかりません。");
        }
    }

    [Header("破壊される高さのしきい値")]
    public float destroyYThreshold = -3.0f;

    /// <summary>
    /// 毎フレーム呼ばれる更新関数.今回はゲームオーバー判定に使用
    /// </summary>
    void Update()
    {
        if (transform.position.y < destroyYThreshold)
        {
                Destroy(myParent.gameObject);
                gameDirector.GameOver();
        }
    }

    /// <summary>
    /// 物理的な衝突時に呼ばれる
    /// </summary>
    /// <param name="collision">衝突した相手の情報</param>

    private void OnCollisionEnter(Collision collision){
        Parent otherParent = collision.gameObject.GetComponentInParent<Parent>();
        if (otherParent != null && myParent != null){
            if (otherParent.currentLevel == myParent.currentLevel){
                if (myParent.GetInstanceID() < otherParent.GetInstanceID()){
                    gameDirector.AddScore(otherParent.currentLevel switch{
                        Parent.ObjectLevel.Level0 => 10,
                        Parent.ObjectLevel.Level1 => 20,
                        Parent.ObjectLevel.Level2 => 40,
                        Parent.ObjectLevel.Level3 => 80,
                        Parent.ObjectLevel.Level4 => 160,
                        Parent.ObjectLevel.Level5 => 320,
                        Parent.ObjectLevel.Level6 => 640,
                        Parent.ObjectLevel.Level7 => 1280,
                        Parent.ObjectLevel.Level8 => 2560,
                        Parent.ObjectLevel.Level9 => 5120,
                        Parent.ObjectLevel.Level10 => 10240,
                        _ => 0,
                    });
                    Vector3 contactPoint = (transform.position + collision.transform.position) / 2f;
                    myParent.PerfomMerge(otherParent.gameObject, contactPoint);
                }
            }
        }
    }

}
