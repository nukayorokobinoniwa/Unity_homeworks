using UnityEngine;

public class Children : MonoBehaviour
{
    private Parent myParent;

    void Start(){
        myParent = GetComponentInParent<Parent>();
        
        if(myParent == null){
            Debug.LogError("ChildrenオブジェクトにParentコンポーネントが見つかりません。");
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
                    Vector3 contactPoint = (transform.position + collision.transform.position) / 2f;
                    myParent.PerfomMerge(otherParent.gameObject, contactPoint);
                }
            }
        }
    }

}
