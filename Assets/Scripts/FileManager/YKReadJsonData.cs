using Cysharp.Threading.Tasks;
using FullSerializer;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static MFAbility;

public class RuntimeAbilitiesData
{
    public List<int> RuntimeAbilities = new List<int>();
    public Dictionary<int, Dictionary<AbilityDataUpgrade.UpgradeType, AbilityData>> PlayerRuntimeDataPackS = new Dictionary<int, Dictionary<AbilityDataUpgrade.UpgradeType, AbilityData>>();
}

public class YKReadJsonData : MonoBehaviour
{
    public static YKReadJsonData instance;

    //读取数据文件
    public static bool LoadFinish;
    string fileContents;

    [SerializeField] private TransformAnchor _playerTransformAnchor = default;
    public string AbilityJsonItemPath;
    //public RuntimeAbilitiesData runtimeAbilitiesData = new RuntimeAbilitiesData();
    //public RuntimeAbilitiesData runtimeAbilitiesDataTemp = new RuntimeAbilitiesData();//读取技能json后的数据

    private void Awake()
    {
        instance = this;
        AbilityJsonItemPath = Application.persistentDataPath + "/YKRuntimeAbilitiesData.json";
    }

    public void SetSerializerData()
    {
        fsSerializer serializer = new fsSerializer();
        fsData Data;
        serializer.TrySerialize(YKDataInfoManager.Instance.runtimeAbilitiesData, out Data);

#if UNITY_EDITOR_WIN || UNITY_EDITOR
        SaveAsJson(Data, AbilityJsonItemPath);
#endif

#if UNITY_WEBGL|| UNITY_ANDROID || UNITY_IOS
        WebGLSaveAsJson(Data);
#endif
    }
    public void SaveAsJson(fsData Data, string path)
    {
        //var uri = new System.Uri(Path.Combine(Application.persistentDataPath, "YKRuntimeAbilitiesData.json"));
        var savepath = path;
        var file = new StreamWriter(savepath);
        var json = fsJsonPrinter.PrettyJson(Data);
        file.WriteLine(json);
        file.Close();
    }

    /// <summary>
    /// 读数据
    /// </summary>
    public async UniTask GetSerializerDataAsync()
    {
        await LoadYKRuntimeAbilitys();
    }
    public async UniTask LoadYKRuntimeAbilitys()
    {
#if UNITY_EDITOR_WIN || UNITY_EDITOR
        await ReadData();
#endif

#if UNITY_WEBGL|| UNITY_ANDROID || UNITY_IOS
        await WebGLGetData();
#endif
        if (!string.IsNullOrEmpty(fileContents))
        {
            var data = fsJsonParser.Parse(fileContents);
            await ReadRuntimeAbilitys(data);
        }
    }
    async UniTask<TextAsset> LoadResourceTextfile(string path)
    {
        string filePath = "Json/" + path.Replace(".json", "");
        var targetFile = await Resources.LoadAsync<TextAsset>(filePath); ;
        return (targetFile as TextAsset);
    }
    private IEnumerator ReadRuntimeAbilitys(fsData data)
    {
        fsSerializer serializer = new fsSerializer();
        object deserialized = null;
        var re = serializer.TryDeserialize(data, typeof(RuntimeAbilitiesData), ref deserialized);
        while (!re.Succeeded)
        {
            yield return null;
        }
        RuntimeAbilitiesData runtimeAbilities = deserialized as RuntimeAbilitiesData;
        YKDataInfoManager.Instance.runtimeAbilitiesDataTemp = runtimeAbilities;

        LoadFinish = true;

        while (_playerTransformAnchor.Value == null)
        {
            yield return null;
        }
        //_playerTransformAnchor.Value.GetComponent<MFAbilityHandler>().GetAbilityData();
        _playerTransformAnchor.Value.GetComponent<MFAbilityHandler>().GetAbilityData();
    }
    private IEnumerator ReadData()
    {
        string readData;
        string fileUrl = Application.persistentDataPath + "\\YKRuntimeAbilitiesData.json";
        using (StreamReader sr = File.OpenText(fileUrl))
        {
            readData = sr.ReadToEnd();
            sr.Close();
        }
        fileContents = readData;
        yield return null;
    }
    public void WebGLSaveAsJson(fsData Data)
    {
        var json = fsJsonPrinter.PrettyJson(Data);
        PlayerPrefs.SetString("YKRuntimeAbilitiesData", json);
    }

    IEnumerator WebGLGetData()
    {
        fileContents = PlayerPrefs.GetString("YKRuntimeAbilitiesData");
        PlayerPrefs.DeleteKey("YKRuntimeAbilitiesData");
        yield return null;
    }

    public void ClearData()
    {
        YKDataInfoManager.Instance.runtimeAbilitiesDataTemp.RuntimeAbilities.Clear();
        YKDataInfoManager.Instance.runtimeAbilitiesDataTemp.PlayerRuntimeDataPackS.Clear();

        YKDataInfoManager.Instance.runtimeAbilitiesData.RuntimeAbilities.Clear();
        YKDataInfoManager.Instance.runtimeAbilitiesData.PlayerRuntimeDataPackS.Clear();
    }
}
