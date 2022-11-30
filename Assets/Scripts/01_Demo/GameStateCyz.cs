using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateCyz 
{
    public static Transform playerTsf;
    public static int ObsCount = 0;
    public static Transform GetPlayerTsf()
    {
        if (playerTsf == null)
        {
            // playerTsf = GameObject.Find("PlayerX").transform;
            playerTsf = GameObject.FindWithTag("Player").transform;
        }
        return playerTsf;
    }
    public static int GetR()
    {
      
        if (ObsCount < 4)
        {
            return 2;
        }else if (ObsCount < 8)
        {
            return 3;
        }
        else if (ObsCount < 16)
        {
            return 4;
        }
        else if (ObsCount < 22)
        {
            return 5;
        }
        else
        {
            return 6;
        }

    }
}
