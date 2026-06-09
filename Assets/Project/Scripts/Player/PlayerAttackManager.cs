using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    public Collider2D attackForward;
    public Collider2D attackBack;
    public Collider2D attackSide;


    void Start()
    {
        attackForward.enabled = false;
        attackBack.enabled = false;
        attackSide.enabled = false;
    }

    // 当たり判定
    // 前方
    public void EnableHitBoxForward()
    {
        attackForward.enabled = true;
    }
    public void DisableHitBoxForward()
    {
        attackForward.enabled = false;
    }
    // 後方
    public void EnableHitBoxBack()
    {
        Debug.Log("前方攻撃開始");
        attackBack.enabled = true;
    }
    public void DisableHitBoxBack()
    {
        Debug.Log("前方攻撃おわり");
        attackBack.enabled = false;
    }
    // 横
    public void EnableHitBoxSide()
    {
        attackSide.enabled = true;
    }
    public void DisableHitBoxSide()
    {
        attackSide.enabled = false;
    }

}
