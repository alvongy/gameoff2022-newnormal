using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : SerializedMonoSingleton<UI_Manager>
{
    [SerializeField] Image cover;
    [SerializeField] Button playButton;
    [SerializeField] Button exitButton;
    public Transform startGameEntrancePanel;
    public Canvas canvas;

    public UICaptureEquip captureEquipPanel;
    public UIEquip_DragItem dragItem_Equip;

    public UICharacterInfoCard characterInfoCard;

    public UIGameLevel gameLevelPanel;
    public UISelectEvolutionTerm selectEvolutionTerm;
    public UIShopControl shopControlPanel;
    public UISurvivalDifficulty survivalDifficultyUI;

    public Image rollMask_Image;
    public Text roolTimer_Text;

    public void Init(CharacterSO so)
    {
        captureEquipPanel.Init(so);
        characterInfoCard.Init();

        selectEvolutionTerm.Init();
        shopControlPanel.Init();
        survivalDifficultyUI.Init();
    }
    public void OnGameOver()
    {
        selectEvolutionTerm.OnGameOver();
        captureEquipPanel.OnGameOver();
        BackCover();
    }

    protected override void OnAwake()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
    }

    public void OnPlayButtonClick()
    {
        cover.gameObject.SetActive(false);
        //调出Level路线图

        //++++++++++++
        EntranceManager.Instance.Play();
    }

    public void OnExitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void BackCover()
    {
        cover.gameObject.SetActive(true);
    }

}
