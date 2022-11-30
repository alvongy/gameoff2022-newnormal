using DG.Tweening;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YK.Game.Events;

public class CollectableItem_Prop : CollectableItem
{
    public enum ItemBuffTypeEnum
    {
        SPEED,
        HP,
        SHIELD,
        BANGALORE,
        GOLDINCREASE,
        GOLD,
        GameEntrance,
    }

    public ItemBuffTypeEnum itemBuffType;

    [ShowIf("@(this.itemBuffType)==ItemBuffTypeEnum.HP")]
    public int increaseHp = 1;
    [ShowIf("@(this.itemBuffType)==ItemBuffTypeEnum.GOLD")]
    public int increaseGold = 1;
    [ShowIf("@(this.itemBuffType)==ItemBuffTypeEnum.GOLD")]
    public bool isGoldActive = false;
    [ShowIf("@(this.itemBuffType)==ItemBuffTypeEnum.GOLD")]
    public MMFeedbacks goldFeedbacks;

    [ShowIf("@(this.itemBuffType)==ItemBuffTypeEnum.BANGALORE")]
    public float bangaloreTime = 1f;
    [ShowIf("@(this.itemBuffType)==ItemBuffTypeEnum.BANGALORE")]
    public float bangaloreDamageRadiu = 10f;
    [ShowIf("@(this.itemBuffType)==ItemBuffTypeEnum.BANGALORE")]
    public float bangaloreDamage = 40f;
    [ShowIf("@(this.itemBuffType)==ItemBuffTypeEnum.BANGALORE")]
    public SpriteRenderer bangaloreReadySign;
    [ShowIf("@(this.itemBuffType)==ItemBuffTypeEnum.BANGALORE")]
    public SpriteRenderer warningArea;
    [ShowIf("@(this.itemBuffType)==ItemBuffTypeEnum.BANGALORE")]
    public Animator bangaloreAnimator;

    private int finallyGoldNum;

    public override void InitMethod()
    {
        switch (itemBuffType)
        {
            case ItemBuffTypeEnum.SPEED:
                SpeedItem();
                break;
            case ItemBuffTypeEnum.HP:
                HpItem();
                break;
            case ItemBuffTypeEnum.SHIELD:
                ShieldItem();
                break;
            case ItemBuffTypeEnum.GOLD:
                RecoverGold();
                break;          
            case ItemBuffTypeEnum.BANGALORE:
                OnTriggrtBangalore();
                break;
            case ItemBuffTypeEnum.GameEntrance:
                StartGameEntrance();
                break;
            default:
                Debug.Log("Not have this item !");
                break;
        }
    }

    private void Start()
    {
        if (itemBuffType== ItemBuffTypeEnum.GameEntrance)
        {
            //transform.DOShakePosition(11, new Vector3(0, 0, 2), 20);
            transform.DOPunchScale(new Vector3(.6f,.6f,.6f),1f,0,0).SetLoops(-1);
        }
    }

    private void StartGameEntrance()
    {
        UI_Manager.Instance.startGameEntrancePanel.gameObject.SetActive(true);
    }

    void HpItem()
    {
        CharacterManager.Instance.character.Recover(increaseHp);
        UseItemTrigger();
    }

    /// <summary>
    /// Speed Control
    /// </summary>
    /// <param name="tran"></param>
    void SpeedItem()
    {
        //tran.GetComponent<MainCharacter>()._isSpecialState = true;
        //tran.GetComponent<MainCharacter>()._specialTimer = _timer;
        //tran.GetComponent<MainCharacter>().RuntimeAdditionalDic[CharacterDataType.MoveSpeed].Data = increaseSpeed;
    }

    void ShieldItem()
    {
        //tran.GetComponent<MainCharacter>()._InvincibleCD.ForceSetCurrentValue(increaseShild);
    }
 
    private void RecoverGold()
    {
        finallyGoldNum = Mathf.Max(1, (int)(GameLevelManager.Instance.levelGoldMultiplyDic[GameLevelManager.Instance.GetCurrentLevel] * increaseGold * Random.Range(0.95f, 1.05f)));
        goldFeedbacks.PlayFeedbacks();

        //Vector3 worldPos = Camera.main.ScreenToWorldPoint(SceneItemManager.Instance._goldItem.transform.position);
        transform.DOMove(SceneItemManager.Instance._goldItem.transform.position, 0.5f, false).OnComplete(() =>
        {
            ShopManager.Instance.playerGlods.Increace(finallyGoldNum);
            UseItemTrigger();
        });
    }
    public void InitGold(int n)
    {
        increaseGold = n;
    }

    /// <summary>
    /// 触发炸药桶效果
    /// </summary>
    private void OnTriggrtBangalore()
    {
        enemies = new List<Entity>(8);
        arr = new Collider[30];
        StartCoroutine(OnDisplay());
    }
    IEnumerator OnDisplay()
    {
        bangaloreReadySign.enabled = true;
        warningArea.enabled = true;
        bangaloreAnimator.SetTrigger("Blast");

        yield return new WaitForSeconds(bangaloreTime);
        
        if (GetOverlap(bangaloreDamageRadiu))
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (BattleManager.Instance.CheckAttake(enemies[i]))
                {
                    BattleManager.Instance.Hit(bangaloreDamage, enemies[i]);
                }
            }
        }
        bangaloreReadySign.enabled = false;
        warningArea.enabled = false;
        bangaloreAnimator.StopPlayback();

        StopCoroutine(OnDisplay());
        UseItemTrigger();
    }

    Collider[] arr;
    List<Entity> enemies;

    bool GetOverlap(float r)
    {
        enemies.Clear();
        int count = Physics.OverlapSphereNonAlloc(transform.position, r, arr, LayerMask.GetMask("Enemy")| LayerMask.GetMask("Characters"));
        for (int i = 0; i < count; i++)
        {
            Entity enemyCtrl = arr[i].GetComponent<Entity>();
            if (enemyCtrl)
            {
                enemies.Add(enemyCtrl);
            }
        }
        return enemies.Count > 0;
    }

    /// <summary>
    /// 触发完道具效果之后调用
    /// </summary>
    public override void UseItemTrigger()
    {
        base.UseItemTrigger();

    }
}
