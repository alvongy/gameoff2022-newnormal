using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SerializedMonoSingleton<DataManager>
{
    public CharacterDatabase characterDatabase;
    public EquipDatabase equipDatabase;
    public CaptureDataSO captureData;
}
