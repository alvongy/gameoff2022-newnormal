using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BossSpeakControl : SerializedMonoBehaviour
{
    public Transform TerrainUIParent;
    public GameObject DiaLogObj;
    public Transform TerrainBoss;

    [SerializeField] private VoidEventChannelSO _mapBuffNearBossChannelSO;
    [SerializeField] private VoidEventChannelSO OnMapBuffDetectionChannelSO;

    public Dictionary<int, BossLanguageLibrarySO> BossLanguageLibrary;

    int randomLibraryNum;
    bool IsFirst;

    float speakTimer = 1.5f;
    float speakTime = 1.5f;

    private void Awake()
    {
        IsFirst = YKDataInfoManager.Instance.isFirstTerrain;
        BossSpeakEvent();
    }
    private void Start()
    {
     
    }
    private void OnEnable()
    {
        OnMapBuffDetectionChannelSO.OnEventRaised += BossSpeakEvent;
    }
    private void OnDisable()
    {
        OnMapBuffDetectionChannelSO.OnEventRaised -= BossSpeakEvent;
    }

    private void Update()
    {
        speakTime += Time.deltaTime;
        if (speakTime>= speakTimer)
        {
            speakTime = speakTimer;
        }
    }

    void BossSpeakEvent()
    {
        if (TerrainBoss != null && speakTime >= speakTimer)
        {
            GameObject tempDialog = Instantiate(DiaLogObj, TerrainBoss);
            tempDialog.transform.GetChild(0).GetComponent<Text>().text = BossSpeak();
            Destroy(tempDialog, 1.5f);
            BossSpeakShake();
        }
    }
    void BossSpeakShake()
    {
        speakTime = 0;
        TerrainBoss.DOShakePosition(2, new Vector3(10 ,20, 10));
    }

    private string BossSpeak()
    {
       // randomNum = Random.Range(0,speakList.Count);
        //randomLibraryNum = Random.Range(0, BossLanguageLibrary[0].LanguageStrs.Length);

        //满足某种条件说某句话
        //Plan1:使用YKDataInfoManager时刻监听Boss说话的判定条件，当条件发生变化时，变更Boss的话语库
        //Plan2:

        if (IsFirst)
        {
            randomLibraryNum = Random.Range(0, BossLanguageLibrary[1].LanguageStrs.Length);
            return BossLanguageLibrary[1].LanguageStrs[randomLibraryNum].GetLocalizedString();
        }
        else if (YKDataInfoManager.Instance.desktopCardNum == 0)
        {
            randomLibraryNum = Random.Range(0, BossLanguageLibrary[0].LanguageStrs.Length);
            return BossLanguageLibrary[0].LanguageStrs[randomLibraryNum].GetLocalizedString();
        }
        else if (YKDataInfoManager.Instance.desktopCardNum == 8)
        {
            randomLibraryNum = Random.Range(0, BossLanguageLibrary[2].LanguageStrs.Length);
            return BossLanguageLibrary[2].LanguageStrs[randomLibraryNum].GetLocalizedString();
        }
        else if (YKDataInfoManager.Instance.desktopCardNum == 7)
        {
            randomLibraryNum = Random.Range(0, BossLanguageLibrary[3].LanguageStrs.Length);
            return BossLanguageLibrary[3].LanguageStrs[randomLibraryNum].GetLocalizedString();
        }
        else if (YKDataInfoManager.Instance.desktopCardNum >= 7)
        {
            randomLibraryNum = Random.Range(0, BossLanguageLibrary[7].LanguageStrs.Length);
            return BossLanguageLibrary[7].LanguageStrs[randomLibraryNum].GetLocalizedString();
        }
        else
        {
            randomLibraryNum = Random.Range(0, BossLanguageLibrary[9].LanguageStrs.Length);
            return BossLanguageLibrary[9].LanguageStrs[randomLibraryNum].GetLocalizedString();
        }
    }

}
