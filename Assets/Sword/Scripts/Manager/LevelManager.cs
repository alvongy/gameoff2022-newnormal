using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    

    public void EnterLevel()
    {
        EntranceManager.Instance.Play();
    }
}
