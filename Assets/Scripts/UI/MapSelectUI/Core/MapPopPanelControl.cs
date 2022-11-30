using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPopPanelControl : MonoBehaviour
{
    public PrayDescription _prayDescription;
    public AchievementDescription _AchievementDescription;
    public LearnAbilityDescription _LearnDescription;
    public DiePunishmentPanelDescription _PunishmentDescription;
    public TerrainRawardPanelDescription _TerrainRawardDescription;
    public PopCurrentSkillDescription _CurrentSkillDescription;
    public GameObject _GameOverPanel;
    public GameObject _AttackBossPanel;

    public Button _closePanel;
    public Button _GameOverBtn;

    private void Awake()
    {
        _closePanel.onClick.AddListener(ClosePopPanel);
        _GameOverBtn.onClick.AddListener(GameOver);
    }

    public void ClosePopPanel()
    {
        _prayDescription.gameObject.SetActive(false);
        _AchievementDescription.gameObject.SetActive(false);
        _LearnDescription.gameObject.SetActive(false);
        _PunishmentDescription.gameObject.SetActive(false);
        _AttackBossPanel.SetActive(false);
        _TerrainRawardDescription.gameObject.SetActive(false);
        _CurrentSkillDescription.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// ��ʯ����
    /// </summary>
    /// <param name="item"></param>
    public void InitializePrayData(ItemSO item)
    {
        gameObject.SetActive(true);
        _prayDescription.gameObject.SetActive(true);
        _prayDescription.Initialize(item);
    }
    /// <summary>
    /// �ɾ͵���
    /// </summary>
    /// <param name="item"></param>
    public void InitializeAchievementData(ItemSO item)
    {
        gameObject.SetActive(true);
        _AchievementDescription.gameObject.SetActive(true);
        _AchievementDescription.Initialize(item);
    } 
    /// <summary>
    /// ��ѧ���ܵ���
    /// </summary>
    public void InitializeLearnAbilityData()
    {
        gameObject.SetActive(true);
        _LearnDescription.gameObject.SetActive(true);
        _LearnDescription.ReadLearnAbility();
    }
    /// <summary>
    /// ��������ͷ�����
    /// </summary>
    public void InitializePlayerDiedData(TerrainSO terrain)
    {
        gameObject.SetActive(true);
        _PunishmentDescription.gameObject.SetActive(true);
        _PunishmentDescription.Initialize(terrain);
    }
    /// <summary>
    /// ��Ϸ��������
    /// </summary>
    public void InitializeGameOver()
    {
        gameObject.SetActive(true);
        _GameOverPanel.SetActive(true);
    }
    void GameOver()
    {
        YKDataInfoManager.Instance.OnPlayerDie();
        MapSelectUIManager.Instance.LoadMenuScene.Load();
    }
    /// <summary>
    /// ����Boss����
    /// </summary>
    public void InitializeAttackBoss()
    {
        gameObject.SetActive(true);
        _AttackBossPanel.SetActive(true);
    }

    /// <summary>
    /// ��ͼ����������
    /// </summary>
    public void InitializeTerrainRaward(YKRewardSO reward)
    {
        gameObject.SetActive(true);
        _TerrainRawardDescription.gameObject.SetActive(true);
        _TerrainRawardDescription.Initialize(reward);
    }
    /// <summary>
    /// ��ʾ��ǰ����ļ�������
    /// </summary>
    public void InitializeCurrentSkill(MFAbility ability)
    {
        gameObject.SetActive(true);
        _CurrentSkillDescription.gameObject.SetActive(true);
        _CurrentSkillDescription.Initialize(ability);
    }
}
