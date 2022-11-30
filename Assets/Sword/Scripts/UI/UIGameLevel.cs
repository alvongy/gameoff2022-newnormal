using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameLevel : MonoBehaviour
{
    [SerializeField] private Text currentLevelEnemy1;
    [SerializeField] private Text currentLevelEnemy2;
    [SerializeField] private Text currentLevelTime;
    [SerializeField] private Text Level_Text;
    [SerializeField] private Button closePopBtn;
    [SerializeField] private Button closePopBtnS;
    [SerializeField] private Transform UIGameOverPop;
    [SerializeField] private Transform UIGameVictoryPop;

    float TIME;
    public void Init()
    {
        Level_Text.text = "Lv:" + GameLevelManager.Instance.GetCurrentLevel.ToString();
    }

    private void Awake()
    {
        closePopBtn.onClick.AddListener(HideGameOverPopUI);
        closePopBtnS.onClick.AddListener(HideGameOverPopUI);
    }
    private void Update()
    {
        //当前关卡结束后更新事件内容
        GameLevelUI();
        TIME += Time.deltaTime;
    }
    public void GameLevelUI()
    {
       // if (!GameLevelManager.Instance.IsGamePlay) { return; }
        currentLevelEnemy1.text = SpawnManager.Instance.counter.ToString();
        currentLevelEnemy2.text = MonsterManager.Instance.Cleaned.ToString();
        currentLevelTime.text = TIME.ToTimeFormat();
        Level_Text.text = "Lv:" + GameLevelManager.Instance.GetCurrentLevel.ToString();
    }

    private void HideGameOverPopUI()
    {
        UIGameOverPop.gameObject.SetActive(false);
        UIGameVictoryPop.gameObject.SetActive(false);
        EntranceManager.Instance.StartGame();
        //EntranceManager.Instance.GameOver();
    }
    public void ShowGameOverPopUI()
    {
        UIGameOverPop.gameObject.SetActive(true);
    }
    public void ShowGameVictoryrPopUI()
    {
        UIGameVictoryPop.gameObject.SetActive(true);
    }
}

public static class FloatExtension
{
    public static string ToTimeFormat(this float time)
    {
        int seconds = (int)time;
        int hour = seconds / 3600;
        int minute = seconds % 3600 / 60;
        seconds = seconds % 3600 % 60;
        //返回00:00:00时间格式
        return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, seconds);
    }
}