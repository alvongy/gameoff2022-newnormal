using System.Collections.Generic;
using UnityEngine;
using static TerrainSO;

public static class MapAdditionDeckUtility
{
    private static int greenOnBlueCenter;//围绕蓝色牌的绿色牌数量
    private static int redOnBlueCenter;//围绕蓝色牌的红色牌数量
    private static int redOnGreenCenter;//围绕绿色牌的红色牌数量
    private static int greenOnRedCenter;//围绕红色牌的绿色牌数量
    private static int nearBossNum;//围绕Boss牌的其他牌数量

    private static bool isOnBlueCenter;//围绕蓝色牌的牌是否有蓝色的
    private static bool isOnGreenCenter;//围绕绿色牌的牌是否有绿色的
    private static bool isOnRedCenter;//围绕红色牌的牌是否有红色的
    private static bool isOnNearBoss;//有没有其他牌在Boss周围


    private static int isTotalRedNum; //桌面所有红色牌数量
    private static int isTotalGreenNum; //桌面所有绿色牌数量
    private static int isTotalBlueNum; //桌面所有蓝色牌数量

    private static int isOnFirstRowRed; //第一行红色牌数量
    private static int isOnSecondRowRed;//第二行红色牌数量
    private static int isOnThirdRowRed; //第三行红色牌数量

    private static int isOnFirstRowGreen; //第一行绿色牌数量
    private static int isOnSecondRowGreen;//第二行绿色牌数量
    private static int isOnThirdRowGreen; //第三行绿色牌数量

    private static int isOnFirstRowBlue; //第一行蓝色牌数量
    private static int isOnSecondRowBlue;//第二行蓝色牌数量
    private static int isOnThirdRowBlue; //第三行蓝色牌数量

    private static int isOnFirstRankRed; //第一列红色牌数量
    private static int isOnSecondRankRed;//第二列红色牌数量
    private static int isOnThirdRankRed; //第三列红色牌数量

    private static int isOnFirstRankGreen; //第一列绿色牌数量
    private static int isOnSecondRankGreen;//第二列绿色牌数量
    private static int isOnThirdRankGreen; //第三列绿色牌数量

    private static int isOnFirstRankBlue; //第一列蓝色牌数量
    private static int isOnSecondRankBlue;//第二列蓝色牌数量
    private static int isOnThirdRankBlue; //第三列蓝色牌数量

    private static MapBuffControl mapBuff;

    private static int LineNumRed3; //满足 3红 规则的数量
    private static int LineNumGreen3; //满足 3绿 规则的数量
    private static int LineNumBlue3; //满足 3蓝 规则的数量
    private static int LineNumRed2Green1; //满足 2红1绿 规则的数量
    private static int LineNumRed2Blue1; //满足 2红1蓝 规则的数量
    private static int LineNumGreen2Red1; //满足 2绿1红 规则的数量
    private static int LineNumGreen2Blue1; //满足 2绿1蓝 规则的数量
    private static int LineNumBlue2Red1; //满足 2蓝1红 规则的数量
    private static int LineNumBlue2Green1; //满足 2蓝1绿 规则的数量
    private static int LineNumRed1Blue1Green1; //满足 1红1蓝1绿 规则的数量

    private static int ObliqueRedNum; //满足 红斜对面 规则的数量
    private static int ObliqueGreenNum; //满足 绿斜对面 规则的数量
    private static int ObliqueBlueNum; //满足 蓝斜对面 规则的数量
    private static int ObliqueMixNum; //满足 混合斜对面 规则的数量

    private static int SquaresRedNum; //满足 红方阵 规则的数量
    private static int SquaresGreenNum; //满足 绿方阵 规则的数量
    private static int SquaresBlueNum; //满足 蓝方阵 规则的数量


    static Vector2 first = new Vector2(0, 0);
    static Vector2 second = new Vector2(0, 1);
    static Vector2 third = new Vector2(0, 2);
    static Vector2 fourth = new Vector2(1, 0);
    static Vector2 fifth = new Vector2(1, 1);
    static Vector2 sixth = new Vector2(1, 2);
    static Vector2 seventh = new Vector2(2, 0);
    static Vector2 eighth = new Vector2(2, 1);
    static Vector2 ninth = new Vector2(2, 2);
    public static void CheckCondition(Dictionary<Vector2, MapTerrainUISlot> mapTerrainUISlotDic, MapBuffControl mapBuffControl)
    {
        InitRule(mapBuffControl);
        OnRowRankExamine(mapTerrainUISlotDic);
        ObliqueExamine(mapTerrainUISlotDic);
        //OnFinallyExamine();
        Calculate();
        SquaresExamine(mapTerrainUISlotDic);
    }

    static void InitRule(MapBuffControl mB)
    {
        mapBuff = mB;
        greenOnBlueCenter = 0;
        redOnBlueCenter = 0;
        redOnGreenCenter = 0;
        greenOnRedCenter = 0;
        nearBossNum = 0;

        isOnBlueCenter = false;
        isOnGreenCenter = false;
        isOnRedCenter = false;

        mapBuff.GetMapBuffOnAloneRed1(false);
        mapBuff.GetMapBuffOnAloneGreen1(false);
        mapBuff.GetMapBuffOnAloneBlue1(false);
        mapBuff.GetMapBuffOnLineRuleRed3(false);
        mapBuff.GetMapBuffOnLineRuleGreen3(false);
        mapBuff.GetMapBuffOnLineRuleBlue3(false);
        mapBuff.GetMapBuffOnLineRuleRed2Green1(false);
        mapBuff.GetMapBuffOnLineRuleRed2Blue1(false);
        mapBuff.GetMapBuffOnLineRuleGreen2Red1(false);
        mapBuff.GetMapBuffOnLineRuleGreen2Blue1(false);
        mapBuff.GetMapBuffOnLineRuleBlue2Red1(false);
        mapBuff.GetMapBuffOnLineRuleBlue2Green1(false);
        mapBuff.GetMapBuffOnLineRuleRed1Green1Blue1(false);
        mapBuff.GetMapBuffOnObliqueRuleRed3(false);
        mapBuff.GetMapBuffOnObliqueRuleGreen3(false);
        mapBuff.GetMapBuffOnObliqueRuleBlue3(false);
        mapBuff.GetMapBuffOnObliqueRuleRed1Green1Blue1(false);

        mapBuff.GetMapBuffOnSquaresRed(false);
        mapBuff.GetMapBuffOnSquaresGreen(false);
        mapBuff.GetMapBuffOnSquaresBlue(false);

        mapBuff.GetMapBuffOnObliqueRed(false);
        mapBuff.GetMapBuffOnObliqueGreen(false);
        mapBuff.GetMapBuffOnObliqueBlue(false);
        mapBuff.GetMapBuffOnObliqueMix(false);

        mapBuff.GetMapBuffOnHomochromaticRed(false);
        mapBuff.GetMapBuffOnHomochromaticGreen(false);
        mapBuff.GetMapBuffOnHomochromaticBlue(false);

        isOnFirstRowRed = 0;
        isOnSecondRowRed = 0;
        isOnThirdRowRed = 0;

        isOnFirstRowGreen = 0;
        isOnSecondRowGreen = 0;
        isOnThirdRowGreen = 0;

        isOnFirstRowBlue = 0;
        isOnSecondRowBlue = 0;
        isOnThirdRowBlue = 0;

        isOnFirstRankRed = 0;
        isOnSecondRankRed = 0;
        isOnThirdRankRed = 0;

        isOnFirstRankGreen = 0;
        isOnSecondRankGreen = 0;
        isOnThirdRankGreen = 0;

        isOnFirstRankBlue = 0;
        isOnSecondRankBlue = 0;
        isOnThirdRankBlue = 0;

        LineNumRed3 = 0;
        LineNumGreen3 = 0;
        LineNumBlue3 = 0;
        LineNumRed2Green1 = 0;
        LineNumRed2Blue1 = 0;
        LineNumGreen2Red1 = 0;
        LineNumGreen2Blue1 = 0;
        LineNumBlue2Red1 = 0;
        LineNumBlue2Green1 = 0;
        LineNumRed1Blue1Green1 = 0;

        ObliqueRedNum = 0;
        ObliqueGreenNum = 0;
        ObliqueBlueNum = 0;
        ObliqueMixNum = 0;
        SquaresRedNum = 0;
        SquaresGreenNum = 0;
        SquaresBlueNum = 0;

        Vector2 first = new Vector2(0, 0);
        Vector2 third = new Vector2(0, 2);
        Vector2 fifth = new Vector2(1, 1);
        Vector2 seventh = new Vector2(2, 0);
        Vector2 ninth = new Vector2(2, 2);
    }

    /*
    /// <summary>
    /// 围绕蓝色牌在中心展开
    /// </summary>
    /// <param name="mapTerrainUISlotDic"></param>
    /// <param name="ter"></param>
    static void OnBlueCenter(Dictionary<Vector2, MapTerrainUISlot> mapTerrainUISlotDic,KeyValuePair<Vector2, MapTerrainUISlot> ter)
    {
        if (ter.Value._currentTerrainUI != null && ter.Value._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
        {
            mapBuff.GetMapBuff8(true);
            if (ter.Key.x == 1 && ter.Key.y == 1)//中心点
            {
                //3.7 当蓝色卡牌处于中心位置时 true SO
                mapBuff.GetMapBuff7(true);
                mapBuff.GetMapBuff8(false);

            }

            if (ter.Key.x - 1 >= 0)//蓝色牌上方是否在桌面上
            {
                Vector2 v2 = new Vector2(ter.Key.x - 1, ter.Key.y);
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    greenOnBlueCenter += 1;
                }
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    redOnBlueCenter += 1;
                }
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
                {
                    isOnBlueCenter = true;
                }
            }
            if (ter.Key.x + 1 <= 2)//蓝色牌下方是否在桌面上
            {
                Vector2 v2 = new Vector2(ter.Key.x + 1, ter.Key.y);
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    greenOnBlueCenter += 1;
                }
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    redOnBlueCenter += 1;
                }
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
                {
                    isOnBlueCenter = true;
                }
            }
            if (ter.Key.y - 1 >= 0)//蓝色牌左方是否在桌面上
            {
                Vector2 v2 = new Vector2(ter.Key.x, ter.Key.y - 1);
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    greenOnBlueCenter += 1;
                }
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    greenOnBlueCenter += 1;
                }
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
                {
                    isOnBlueCenter = true;
                }
            }
            if (ter.Key.y + 1 <= 2)//蓝色牌右方是否在桌面上
            {
                Vector2 v2 = new Vector2(ter.Key.x, ter.Key.y + 1);
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    greenOnBlueCenter += 1;
                }
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    redOnBlueCenter += 1;
                }
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
                {
                    isOnBlueCenter = true;
                }
            }

            if (greenOnBlueCenter != 0)
            {
                if (greenOnBlueCenter >= 2) { mapBuff.GetMapBuff11(true); }//   3.11 当与任一蓝色卡牌相邻的绿色卡牌数量>2  SO

                if (greenOnBlueCenter == 4 || redOnBlueCenter == 4) { }//   3.12 当与任一蓝色卡牌相邻的所有卡牌颜色一致时 需修改

            }
            else { mapBuff.GetMapBuff3(true); }//   3.3 没有绿色卡牌与蓝色卡牌相邻  SO

            if (redOnBlueCenter != 0)
            {
                if (redOnBlueCenter >= 2) { mapBuff.GetMapBuff10(true); }//3.10 当与任一蓝色卡牌相邻的红色卡牌数量>2  SO
            }
            else { mapBuff.GetMapBuff6(true); }// 3.6 没有红色卡牌与蓝色卡牌相邻  SO 
        }
    }

    /// <summary>
    /// 围绕绿色牌在中心展开
    /// </summary>
    /// <param name="mapTerrainUISlotDic"></param>
    /// <param name="ter"></param>
    static void OnGreenCenter(Dictionary<Vector2, MapTerrainUISlot> mapTerrainUISlotDic, KeyValuePair<Vector2, MapTerrainUISlot> ter)
    {
        if (ter.Value._currentTerrainUI != null && ter.Value._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
        {
            if (ter.Key.x == 1 && ter.Key.y == 1)//中心点
            {
                //3.1 当绿色卡牌处于中心位置时 true SO
                mapBuff.GetMapBuff1(true);
            }

            if (ter.Key.x - 1 >= 0)//绿色牌上方是否在桌面上
            {
                Vector2 v2 = new Vector2(ter.Key.x - 1, ter.Key.y);

                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    redOnBlueCenter += 1;
                }
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    isOnGreenCenter = true;
                }
            }
            if (ter.Key.x + 1 <= 2)//绿色牌下方是否在桌面上
            {
                Vector2 v2 = new Vector2(ter.Key.x + 1, ter.Key.y);
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    redOnBlueCenter += 1;
                }
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    isOnGreenCenter = true;
                }
            }
            if (ter.Key.y - 1 >= 0)//绿色牌左方是否在桌面上
            {
                Vector2 v2 = new Vector2(ter.Key.x, ter.Key.y - 1);

                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    greenOnBlueCenter += 1;
                }
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    isOnGreenCenter = true;
                }
            }
            if (ter.Key.y + 1 <= 2)//绿色牌右方是否在桌面上
            {
                Vector2 v2 = new Vector2(ter.Key.x, ter.Key.y + 1);
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    redOnBlueCenter += 1;
                }
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    isOnGreenCenter = true;
                }
            }
            else//绿色牌在最右方
            {
                mapBuff.GetMapBuffGreenCardFarRight(true);
            }
        }

        if (redOnGreenCenter < 4)
        {
            //   3.2 当所有与绿色卡牌临近（上下左右）的红色卡牌数量<4  SO
            mapBuff.GetMapBuff2(true);
        }

        //3.9 所有卡牌相邻的卡牌均与其颜色不同 绿色牌
    }

    /// <summary>
    /// 围绕红色牌在中心展开
    /// </summary>
    /// <param name="mapTerrainUISlotDic"></param>
    /// <param name="ter"></param>
    static void OnRedCenter(Dictionary<Vector2, MapTerrainUISlot> mapTerrainUISlotDic, KeyValuePair<Vector2, MapTerrainUISlot> ter)
    {
        if (ter.Value._currentTerrainUI != null && ter.Value._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
        {
            if (ter.Key.x == 1 && ter.Key.y == 1)//中心点
            {
                //3.4 当红色卡牌处于中心位置时 true SO
                mapBuff.GetMapBuff4(true);
            }

            if (ter.Key.x - 1 >= 0)//红色牌上方进行检查
            {
                Vector2 v2 = new Vector2(ter.Key.x - 1, ter.Key.y);

                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    greenOnRedCenter += 1;
                }
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    isOnRedCenter = true;
                }
            }
            if (ter.Key.x + 1 <= 2)//红色牌下方是否在桌面上
            {
                Vector2 v2 = new Vector2(ter.Key.x + 1, ter.Key.y);
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    greenOnRedCenter += 1;
                }
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    isOnRedCenter = true;
                }
            }
            if (ter.Key.y - 1 >= 0)//红色牌左方是否在桌面上
            {
                Vector2 v2 = new Vector2(ter.Key.x, ter.Key.y - 1);

                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    greenOnRedCenter += 1;
                }
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    isOnRedCenter = true;
                }
            }
            else//红色牌在最左方
            {
                mapBuff.GetMapBuffRedCardFarLeft(true);
            }
            if (ter.Key.y + 1 <= 2)//红色牌右方是否在桌面上
            {
                Vector2 v2 = new Vector2(ter.Key.x, ter.Key.y + 1);
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    greenOnRedCenter += 1;
                }
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    isOnRedCenter = true;
                }
            }
            if (greenOnRedCenter < 4)
            {
                //  3.5 当所有与红色卡牌临近（上下左右）的红色卡牌数量<4  SO
                mapBuff.GetMapBuff5(true);
            }
        }
    }

    /// <summary>
    /// 围绕Boss牌展开
    /// </summary>
    /// <param name="mapTerrainUISlotDic"></param>
    /// <param name="ter"></param>
    static void OnBossCenter(Dictionary<Vector2, MapTerrainUISlot> mapTerrainUISlotDic, KeyValuePair<Vector2, MapTerrainUISlot> ter)
    {
        if (ter.Value._currentTerrainUI != null && ter.Value._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Purple)
        {
            mapBuff.GetMapBuff8(true);
            if (ter.Key.x == 1 && ter.Key.y == 1)//中心点
            {
                //3.7 当蓝色卡牌处于中心位置时 true SO
                //mapBuff.GetMapBuff7(true);
                mapBuff.GetMapBuff8(false);
            }

            if (ter.Key.x - 1 >= 0)//Boss牌上方是否在桌面上
            {
                Vector2 v2 = new Vector2(ter.Key.x - 1, ter.Key.y);
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    isOnNearBoss = true;
                    nearBossNum += 1;
                }
                else if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    nearBossNum += 1;
                }
                else if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
                {
                    nearBossNum += 1;
                }
            }
            if (ter.Key.x + 1 <= 2)//Boss牌下方是否在桌面上
            {
                Vector2 v2 = new Vector2(ter.Key.x + 1, ter.Key.y);
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    nearBossNum += 1;
                }
                else if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    nearBossNum += 1;
                }
                else if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
                {
                    nearBossNum += 1;
                }
            }
            if (ter.Key.y - 1 >= 0)//Boss牌左方是否在桌面上
            {
                Vector2 v2 = new Vector2(ter.Key.x, ter.Key.y - 1);
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    nearBossNum += 1;
                }
                else if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    nearBossNum += 1;
                }
                else if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
                {
                    nearBossNum += 1;
                }
            }
            if (ter.Key.y + 1 <= 2)//Boss牌右方是否在桌面上
            {
                Vector2 v2 = new Vector2(ter.Key.x, ter.Key.y + 1);
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    nearBossNum += 1;
                }
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    mapBuff.GetMapBuffRedCardOnBossRight(true);
                    nearBossNum += 1;
                }
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
                {
                    nearBossNum += 1;
                }
            }
            mapBuff.GetMapNearBoss(nearBossNum);
        }
    }


    static void OnRedExamine(Dictionary<Vector2, MapTerrainUISlot> mapTerrainUISlotDic, KeyValuePair<Vector2, MapTerrainUISlot> ter)
    {
        if (ter.Value._currentTerrainUI != null && ter.Value._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
        {
            if (ter.Key.x == 1 && ter.Key.y == 1)//中心点
            {
                //3.4 当红色卡牌处于中心位置时 true SO
                mapBuff.GetMapBuff4(true);

                Vector2 rowLeft = new Vector2(ter.Key.x, ter.Key.y-1);
                Vector2 rowRight = new Vector2(ter.Key.x, ter.Key.y+1);
            }

            if (ter.Key.x - 1 >= 0)//红色牌上方进行检查
            {
                Vector2 v2 = new Vector2(ter.Key.x - 1, ter.Key.y);

                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    greenOnRedCenter += 1;
                }
                if (mapTerrainUISlotDic[v2]._currentTerrainUI != null && mapTerrainUISlotDic[v2]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    isOnRedCenter = true;
                }
            }
        }
    }
    */

    /// <summary>
    /// 查询行列的排列的规则
    /// </summary>
    /// <param name="mapTerrainUISlotDic"></param>
    static void OnRowRankExamine(Dictionary<Vector2, MapTerrainUISlot> mapTerrainUISlotDic)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Vector2 rowFirst = new Vector2(i, j);

                if (mapTerrainUISlotDic[rowFirst]._currentDesktipCard == null) { continue; }
                if (mapTerrainUISlotDic[rowFirst]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Red)
                {
                    if (i == 0) { isOnFirstRowRed++; }
                    else if (i == 1) { isOnSecondRowRed++; }
                    else { isOnThirdRowRed++; }

                    if (j == 0) { isOnFirstRankRed++; }
                    else if (j == 1) { isOnSecondRankRed++; }
                    else { isOnThirdRankRed++; }
                }
                else if (mapTerrainUISlotDic[rowFirst]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Green)
                {
                    if (i == 0) { isOnFirstRowGreen++; }
                    else if (i == 1) { isOnSecondRowGreen++; }
                    else { isOnThirdRowGreen++; }

                    if (j == 0) { isOnFirstRankGreen++; }
                    else if (j == 1) { isOnSecondRankGreen++; }
                    else { isOnThirdRankGreen++; }
                }
                else if (mapTerrainUISlotDic[rowFirst]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Blue)
                {
                    if (i == 0) { isOnFirstRowBlue++; }
                    else if (i == 1) { isOnSecondRowBlue++; }
                    else { isOnThirdRowBlue++; }

                    if (j == 0) { isOnFirstRankBlue++; }
                    else if (j == 1) { isOnSecondRankBlue++; }
                    else { isOnThirdRankBlue++; }
                }
            }
        }
    }
    #region 对每行每列进行检查
    /*
    static void OnFirstRow(Dictionary<Vector2, MapTerrainUISlot> mapTerrainUISlotDic)
    {
        //检查第一行
        for (int i = 0; i < 3; i++)
        {
            Vector2 rowFirst = new Vector2(0, i);

            if (mapTerrainUISlotDic[rowFirst]._currentTerrainUI == null) { break; }
            if (mapTerrainUISlotDic[rowFirst]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
            {
                isOnFirstRowRed++;
            }
            else if (mapTerrainUISlotDic[rowFirst]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
            {
                isOnFirstRowGreen++;
            }
            else if (mapTerrainUISlotDic[rowFirst]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
            {
                isOnFirstRowBlue++;
            }
        }

        //检查第二行
        for (int i = 0; i < 3; i++)
        {
            Vector2 rowSecond = new Vector2(1, i);

            if (mapTerrainUISlotDic[rowSecond]._currentTerrainUI == null) { break; }
            if (mapTerrainUISlotDic[rowSecond]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
            {
                isOnSecondRowRed++;
            }
            else if (mapTerrainUISlotDic[rowSecond]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
            {
                isOnSecondRowGreen++;
            }
            else if (mapTerrainUISlotDic[rowSecond]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
            {
                isOnSecondRowBlue++;
            }
        }

        //检查第三行
        for (int i = 0; i < 3; i++)
        {
            Vector2 rowThird = new Vector2(2, i);

            if (mapTerrainUISlotDic[rowThird]._currentTerrainUI == null) { break; }
            if (mapTerrainUISlotDic[rowThird]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
            {
                isOnThirdRowRed++;
            }
            else if (mapTerrainUISlotDic[rowThird]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
            {
                isOnThirdRowGreen++;
            }
            else if (mapTerrainUISlotDic[rowThird]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
            {
                isOnThirdRowBlue++;
            }
        }
    }
    static void OnFirstRank(Dictionary<Vector2, MapTerrainUISlot> mapTerrainUISlotDic)
    {
        //检查第一列
        for (int i = 0; i < 3; i++)
        {
            Vector2 rowFirst = new Vector2(i, 0);

            if (mapTerrainUISlotDic[rowFirst]._currentTerrainUI == null) { break; }
            if (mapTerrainUISlotDic[rowFirst]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
            {
                isOnFirstRankRed++;
            }
            else if (mapTerrainUISlotDic[rowFirst]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
            {
                isOnFirstRankGreen++;
            }
            else if (mapTerrainUISlotDic[rowFirst]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
            {
                isOnFirstRankBlue++;
            }
        }

        //检查第二列
        for (int i = 0; i < 3; i++)
        {
            Vector2 rowSecond = new Vector2(i, 1);

            if (mapTerrainUISlotDic[rowSecond]._currentTerrainUI == null) { break; }
            if (mapTerrainUISlotDic[rowSecond]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
            {
                isOnSecondRankRed++;
            }
            else if (mapTerrainUISlotDic[rowSecond]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
            {
                isOnSecondRankGreen++;
            }
            else if (mapTerrainUISlotDic[rowSecond]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
            {
                isOnSecondRankBlue++;
            }
        }

        //检查第三列
        for (int i = 0; i < 3; i++)
        {
            Vector2 rowThird = new Vector2(i, 2);

            if (mapTerrainUISlotDic[rowThird]._currentTerrainUI == null) { break; }
            if (mapTerrainUISlotDic[rowThird]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
            {
                isOnThirdRankRed++;
            }
            else if (mapTerrainUISlotDic[rowThird]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
            {
                isOnThirdRankGreen++;
            }
            else if (mapTerrainUISlotDic[rowThird]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
            {
                isOnThirdRankBlue++;
            }
        }
    }
    */
    #endregion


    /// <summary>
    /// 查询斜对面的排列规则
    /// </summary>
    /// <param name="mapTerrainUISlotDic"></param>
    static void ObliqueExamine(Dictionary<Vector2, MapTerrainUISlot> mapTerrainUISlotDic)
    {
        if (mapTerrainUISlotDic[fifth]._currentDesktipCard == null) { return; }
        if (mapTerrainUISlotDic[first]._currentDesktipCard != null && mapTerrainUISlotDic[ninth]._currentDesktipCard != null)
        {
            if (mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == mapTerrainUISlotDic[first]._currentDesktipCard._prayStoneSO.PrayStonen_Type &&
                mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == mapTerrainUISlotDic[ninth]._currentDesktipCard._prayStoneSO.PrayStonen_Type)
            {
                OnObliqueExamine(mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type);
            }
            else if (mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type != mapTerrainUISlotDic[first]._currentDesktipCard._prayStoneSO.PrayStonen_Type &&
                mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type != mapTerrainUISlotDic[ninth]._currentDesktipCard._prayStoneSO.PrayStonen_Type &&
                mapTerrainUISlotDic[first]._currentDesktipCard._prayStoneSO.PrayStonen_Type != mapTerrainUISlotDic[ninth]._currentDesktipCard._prayStoneSO.PrayStonen_Type)
            {
                PrayStoneType type = PrayStoneType.Mix;
                OnObliqueExamine(type);
            }
        }

        if (mapTerrainUISlotDic[third]._currentDesktipCard != null && mapTerrainUISlotDic[seventh]._currentDesktipCard != null)
        {
            if (mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == mapTerrainUISlotDic[third]._currentDesktipCard._prayStoneSO.PrayStonen_Type &&
                mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == mapTerrainUISlotDic[seventh]._currentDesktipCard._prayStoneSO.PrayStonen_Type)
            {
                OnObliqueExamine(mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type);
            }
            else if (mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type != mapTerrainUISlotDic[third]._currentDesktipCard._prayStoneSO.PrayStonen_Type &&
                mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type != mapTerrainUISlotDic[seventh]._currentDesktipCard._prayStoneSO.PrayStonen_Type &&
                mapTerrainUISlotDic[third]._currentDesktipCard._prayStoneSO.PrayStonen_Type != mapTerrainUISlotDic[seventh]._currentDesktipCard._prayStoneSO.PrayStonen_Type)
            {
                PrayStoneType type = PrayStoneType.Mix;
                OnObliqueExamine(type);
            }
        }

        if (ObliqueRedNum >= 1) { mapBuff.GetMapBuffOnObliqueRed(true, ObliqueRedNum); }// 红斜
        if (ObliqueGreenNum >= 1) { mapBuff.GetMapBuffOnObliqueGreen(true, ObliqueGreenNum); }// 绿斜
        if (ObliqueBlueNum >= 1) { mapBuff.GetMapBuffOnObliqueBlue(true, ObliqueBlueNum); }// 蓝斜
        if (ObliqueMixNum >= 1) { mapBuff.GetMapBuffOnObliqueMix(true, ObliqueMixNum); }// 混合斜


        #region test
        /*
        if (mapTerrainUISlotDic[first]._currentTerrainUI != null && mapTerrainUISlotDic[ninth]._currentTerrainUI != null)
        {
            switch (mapTerrainUISlotDic[first]._currentTerrainUI._terrainSO.Terrain_Type)
            {
                case TerrainType.Red:
                    if (mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Red)
                    {
                        if (mapTerrainUISlotDic[ninth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Red)
                        {
                            mapBuff.GetMapBuffOnObliqueRuleRed3(true);
                            //Debug.Log("//斜杠 红*3");
                        }
                    }
                    else if (mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Green)
                    {
                        if (mapTerrainUISlotDic[ninth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Blue)
                        {
                            //Debug.Log("//斜杠 红1 绿1 蓝1");
                            mapBuff.GetMapBuffOnObliqueRuleRed1Green1Blue1(true);
                        }
                    }
                    else if (mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Blue)
                    {
                        if (mapTerrainUISlotDic[ninth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Green)
                        {
                            //Debug.Log("//斜杠 红1 绿1 蓝1");
                            mapBuff.GetMapBuffOnObliqueRuleRed1Green1Blue1(true);
                        }
                    }
                    break;
                case TerrainType.Green:
                    if (mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Green)
                    {
                        if (mapTerrainUISlotDic[ninth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Green)
                        {
                            //Debug.Log("//斜杠 绿*3");
                            mapBuff.GetMapBuffOnObliqueRuleGreen3(true);
                        }
                    }
                    else if (mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Red)
                    {
                        if (mapTerrainUISlotDic[ninth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Blue)
                        {
                            //Debug.Log("//斜杠 红1 绿1 蓝1");
                            mapBuff.GetMapBuffOnObliqueRuleRed1Green1Blue1(true);
                        }
                    }
                    else if (mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Blue)
                    {
                        if (mapTerrainUISlotDic[ninth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Green)
                        {
                            //Debug.Log("//斜杠 红1 绿1 蓝1");
                            mapBuff.GetMapBuffOnObliqueRuleRed1Green1Blue1(true);
                        }
                    }
                    break;
                case TerrainType.Blue:
                    if (mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Blue)
                    {
                        if (mapTerrainUISlotDic[ninth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Blue)
                        {
                            //Debug.Log("//斜杠 蓝*3");
                            mapBuff.GetMapBuffOnObliqueRuleBlue3(true);
                        }
                    }
                    else if (mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Green)
                    {
                        if (mapTerrainUISlotDic[ninth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Red)
                        {
                            // Debug.Log("//斜杠 红1 绿1 蓝1");
                            mapBuff.GetMapBuffOnObliqueRuleRed1Green1Blue1(true);
                        }
                    }
                    else if (mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Red)
                    {
                        if (mapTerrainUISlotDic[ninth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == PrayStoneType.Green)
                        {
                            //Debug.Log("//斜杠 红1 绿1 蓝1");
                            mapBuff.GetMapBuffOnObliqueRuleRed1Green1Blue1(true);
                        }
                    }
                    break;
            }
        }
        if (mapTerrainUISlotDic[third]._currentTerrainUI == null || mapTerrainUISlotDic[seventh]._currentTerrainUI == null) { return; }


        switch (mapTerrainUISlotDic[third]._currentTerrainUI._terrainSO.Terrain_Type)
        {
            case TerrainType.Red:
                if (mapTerrainUISlotDic[fifth]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red && mapTerrainUISlotDic[seventh]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    //Debug.Log("//斜杠 红*3");
                    mapBuff.GetMapBuffOnObliqueRuleRed3(true);
                }
                else if (mapTerrainUISlotDic[fifth]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    if (mapTerrainUISlotDic[seventh]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
                    {
                        //Debug.Log("//斜杠 红1 绿1 蓝1");
                        mapBuff.GetMapBuffOnObliqueRuleRed1Green1Blue1(true);
                    }
                }
                else if (mapTerrainUISlotDic[fifth]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
                {
                    if (mapTerrainUISlotDic[seventh]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                    {
                        //Debug.Log("//斜杠 红1 绿1 蓝1");
                        mapBuff.GetMapBuffOnObliqueRuleRed1Green1Blue1(true);
                    }
                }
                break;
            case TerrainType.Green:
                if (mapTerrainUISlotDic[fifth]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green && mapTerrainUISlotDic[seventh]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    //Debug.Log("//斜杠 绿*3");
                    mapBuff.GetMapBuffOnObliqueRuleGreen3(true);
                }
                else if (mapTerrainUISlotDic[fifth]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    if (mapTerrainUISlotDic[seventh]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
                    {
                        //Debug.Log("//斜杠 红1 绿1 蓝1");
                        mapBuff.GetMapBuffOnObliqueRuleRed1Green1Blue1(true);
                    }
                }
                else if (mapTerrainUISlotDic[fifth]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
                {
                    if (mapTerrainUISlotDic[seventh]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                    {
                        //Debug.Log("//斜杠 红1 绿1 蓝1");
                        mapBuff.GetMapBuffOnObliqueRuleRed1Green1Blue1(true);
                    }
                }
                break;
            case TerrainType.Blue:
                if (mapTerrainUISlotDic[fifth]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
                {
                    if (mapTerrainUISlotDic[seventh]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
                    {
                        //Debug.Log("//斜杠 蓝*3");
                        mapBuff.GetMapBuffOnObliqueRuleBlue3(true);
                    }
                }
                else if (mapTerrainUISlotDic[fifth]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                {
                    if (mapTerrainUISlotDic[seventh]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Blue)
                    {
                        //Debug.Log("//斜杠 红1 绿1 蓝1");
                        mapBuff.GetMapBuffOnObliqueRuleRed1Green1Blue1(true);
                    }
                }
                else if (mapTerrainUISlotDic[fifth]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Red)
                {
                    if (mapTerrainUISlotDic[seventh]._currentTerrainUI._terrainSO.Terrain_Type == TerrainType.Green)
                    {
                        //Debug.Log("//斜杠 红1 绿1 蓝1");
                        mapBuff.GetMapBuffOnObliqueRuleRed1Green1Blue1(true);
                    }
                }
                break;
        }
        */
        #endregion
    }

    static void OnObliqueExamine(PrayStoneType stoneType)
    {
        switch (stoneType)
        {
            case PrayStoneType.Red:
                ObliqueRedNum++;
                break;
            case PrayStoneType.Green:
                ObliqueGreenNum++;
                break;
            case PrayStoneType.Blue:
                ObliqueBlueNum++;
                break;
            case PrayStoneType.Mix:
                ObliqueMixNum++;
                break;
        }
    }

    /// <summary>
    /// 查询正方形方阵的排列规则
    /// </summary>
    /// <param name="mapTerrainUISlotDic"></param>
    static void SquaresExamine(Dictionary<Vector2, MapTerrainUISlot> mapTerrainUISlotDic)
    {
        if (mapTerrainUISlotDic[fifth]._currentDesktipCard == null || (isOnSecondRankRed < 2 && isOnSecondRankGreen < 2 && isOnSecondRankBlue < 2)) { return; }//若中心点为空 或 第二行有相同颜色的牌数量< 2 则直接跳过

        if (mapTerrainUISlotDic[first]._currentDesktipCard != null)
        {
            if (mapTerrainUISlotDic[second]._currentDesktipCard != null && mapTerrainUISlotDic[fourth]._currentDesktipCard != null && mapTerrainUISlotDic[fifth]._currentDesktipCard != null)
            {
                if (mapTerrainUISlotDic[second]._currentDesktipCard._prayStoneSO.PrayStonen_Type == mapTerrainUISlotDic[first]._currentDesktipCard._prayStoneSO.PrayStonen_Type &&
                    mapTerrainUISlotDic[fourth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == mapTerrainUISlotDic[first]._currentDesktipCard._prayStoneSO.PrayStonen_Type &&
                    mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == mapTerrainUISlotDic[first]._currentDesktipCard._prayStoneSO.PrayStonen_Type)
                {
                    OnSquaresExamine(mapTerrainUISlotDic[first]._currentDesktipCard._prayStoneSO.PrayStonen_Type);
                }
            }
        }
        if (mapTerrainUISlotDic[second]._currentDesktipCard != null)
        {
            if (mapTerrainUISlotDic[third]._currentDesktipCard != null && mapTerrainUISlotDic[fifth]._currentDesktipCard != null && mapTerrainUISlotDic[sixth]._currentDesktipCard != null)
            {
                if (mapTerrainUISlotDic[third]._currentDesktipCard._prayStoneSO.PrayStonen_Type == mapTerrainUISlotDic[third]._currentDesktipCard._prayStoneSO.PrayStonen_Type &&
                    mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == mapTerrainUISlotDic[third]._currentDesktipCard._prayStoneSO.PrayStonen_Type &&
                    mapTerrainUISlotDic[sixth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == mapTerrainUISlotDic[third]._currentDesktipCard._prayStoneSO.PrayStonen_Type)
                {
                    OnSquaresExamine(mapTerrainUISlotDic[second]._currentDesktipCard._prayStoneSO.PrayStonen_Type);
                }
            }
        }
        if (mapTerrainUISlotDic[fourth]._currentDesktipCard != null)
        {
            if (mapTerrainUISlotDic[fifth]._currentDesktipCard != null && mapTerrainUISlotDic[seventh]._currentDesktipCard != null && mapTerrainUISlotDic[eighth]._currentDesktipCard != null)
            {
                if (mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == mapTerrainUISlotDic[fourth]._currentDesktipCard._prayStoneSO.PrayStonen_Type &&
                    mapTerrainUISlotDic[seventh]._currentDesktipCard._prayStoneSO.PrayStonen_Type == mapTerrainUISlotDic[fourth]._currentDesktipCard._prayStoneSO.PrayStonen_Type &&
                    mapTerrainUISlotDic[eighth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == mapTerrainUISlotDic[fourth]._currentDesktipCard._prayStoneSO.PrayStonen_Type)
                {
                    OnSquaresExamine(mapTerrainUISlotDic[fourth]._currentDesktipCard._prayStoneSO.PrayStonen_Type);
                }
            }
        }
        if (mapTerrainUISlotDic[fifth]._currentDesktipCard != null)
        {
            if (mapTerrainUISlotDic[sixth]._currentDesktipCard != null && mapTerrainUISlotDic[eighth]._currentDesktipCard != null && mapTerrainUISlotDic[ninth]._currentDesktipCard != null)
            {
                if (mapTerrainUISlotDic[sixth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type &&
                    mapTerrainUISlotDic[eighth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type &&
                    mapTerrainUISlotDic[ninth]._currentDesktipCard._prayStoneSO.PrayStonen_Type == mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type)
                {
                    OnSquaresExamine(mapTerrainUISlotDic[fifth]._currentDesktipCard._prayStoneSO.PrayStonen_Type);
                }
            }
        }

        if (SquaresRedNum >= 1) { mapBuff.GetMapBuffOnSquaresRed(true, SquaresRedNum); }// 红方阵
        if (SquaresGreenNum >= 1) { mapBuff.GetMapBuffOnSquaresGreen(true, SquaresGreenNum); }// 绿方阵
        if (SquaresBlueNum >= 1) { mapBuff.GetMapBuffOnSquaresBlue(true, SquaresBlueNum); }// 蓝方阵
    }

    static void OnSquaresExamine(PrayStoneType stoneType)
    {
        switch (stoneType)
        {
            case PrayStoneType.Red:
                SquaresRedNum++;
                break;
            case PrayStoneType.Green:
                SquaresGreenNum++;
                break;
            case PrayStoneType.Blue:
                SquaresBlueNum++;
                break;
        }
    }

    #region 计算满足某条规则的数量
    /// <summary>
    /// 计算满足某条规则的数量
    /// </summary>
    public static void Calculate()
    {
        if (isOnFirstRankRed == 3) { LineNumRed3++; }
        if (isOnSecondRankRed == 3) { LineNumRed3++; }
        if (isOnThirdRankRed == 3) { LineNumRed3++; }
        if (isOnFirstRowRed == 3) { LineNumRed3++; }
        if (isOnSecondRowRed == 3) { LineNumRed3++; }
        if (isOnThirdRowRed == 3) { LineNumRed3++; }
        if (LineNumRed3 >= 1) { mapBuff.GetMapBuffOnLineRuleRed3(true, LineNumRed3); }// 红3

        if (isOnFirstRankGreen == 3) { LineNumGreen3++; }
        if (isOnSecondRankGreen == 3) { LineNumGreen3++; }
        if (isOnThirdRankGreen == 3) { LineNumGreen3++; }
        if (isOnFirstRowGreen == 3) { LineNumGreen3++; }
        if (isOnSecondRowGreen == 3) { LineNumGreen3++; }
        if (isOnThirdRowGreen == 3) { LineNumGreen3++; }
        if (LineNumGreen3 >= 1) { mapBuff.GetMapBuffOnLineRuleGreen3(true, LineNumGreen3); }//绿3

        if (isOnFirstRankBlue == 3) { LineNumBlue3++; }
        if (isOnSecondRankBlue == 3) { LineNumBlue3++; }
        if (isOnThirdRankBlue == 3) { LineNumBlue3++; }
        if (isOnFirstRowBlue == 3) { LineNumBlue3++; }
        if (isOnSecondRowBlue == 3) { LineNumBlue3++; }
        if (isOnThirdRowBlue == 3) { LineNumBlue3++; }
        if (LineNumBlue3 >= 1) { mapBuff.GetMapBuffOnLineRuleBlue3(true, LineNumBlue3); }//蓝3

        if (isOnFirstRowRed == 2 && isOnFirstRowGreen == 1) { LineNumRed2Green1++; }
        if (isOnSecondRowRed == 2 && isOnSecondRowGreen == 1) { LineNumRed2Green1++; }
        if (isOnThirdRowRed == 2 && isOnThirdRowGreen == 1) { LineNumRed2Green1++; }
        if (isOnFirstRankRed == 2 && isOnFirstRankGreen == 1) { LineNumRed2Green1++; }
        if (isOnSecondRankRed == 2 && isOnSecondRankGreen == 1) { LineNumRed2Green1++; }
        if (isOnThirdRankRed == 2 && isOnThirdRankGreen == 1) { LineNumRed2Green1++; }
        if (LineNumRed2Green1 >= 1) { mapBuff.GetMapBuffOnLineRuleRed2Green1(true, LineNumRed2Green1); }// 红2绿1


        if (isOnFirstRowRed == 2 && isOnFirstRowBlue == 1) { LineNumRed2Blue1++; }
        if (isOnSecondRowRed == 2 && isOnSecondRowBlue == 1) { LineNumRed2Blue1++; }
        if (isOnThirdRowRed == 2 && isOnThirdRowBlue == 1) { LineNumRed2Blue1++; }
        if (isOnFirstRankRed == 2 && isOnFirstRankBlue == 1) { LineNumRed2Blue1++; }
        if (isOnSecondRankRed == 2 && isOnSecondRankBlue == 1) { LineNumRed2Blue1++; }
        if (isOnThirdRankRed == 2 && isOnThirdRankBlue == 1) { LineNumRed2Blue1++; }
        if (LineNumRed2Blue1 >= 1) { mapBuff.GetMapBuffOnLineRuleRed2Blue1(true, LineNumRed2Blue1); }//红2蓝1

        if (isOnFirstRowGreen == 2 && isOnFirstRowRed == 1) { LineNumGreen2Red1++; }
        if (isOnSecondRowGreen == 2 && isOnSecondRowRed == 1) { LineNumGreen2Red1++; }
        if (isOnThirdRowGreen == 2 && isOnThirdRowRed == 1) { LineNumGreen2Red1++; }
        if (isOnFirstRankGreen == 2 && isOnFirstRankRed == 1) { LineNumGreen2Red1++; }
        if (isOnSecondRankGreen == 2 && isOnSecondRankRed == 1) { LineNumGreen2Red1++; }
        if (isOnThirdRankGreen == 2 && isOnThirdRankRed == 1) { LineNumGreen2Red1++; }
        if (LineNumGreen2Red1 >= 1) { mapBuff.GetMapBuffOnLineRuleGreen2Red1(true, LineNumGreen2Red1); }//绿2红1

        if (isOnFirstRowGreen == 2 && isOnFirstRowBlue == 1) { LineNumGreen2Blue1++; }
        if (isOnSecondRowGreen == 2 && isOnSecondRowBlue == 1) { LineNumGreen2Blue1++; }
        if (isOnThirdRowGreen == 2 && isOnThirdRowBlue == 1) { LineNumGreen2Blue1++; }
        if (isOnFirstRankGreen == 2 && isOnFirstRankBlue == 1) { LineNumGreen2Blue1++; }
        if (isOnSecondRankGreen == 2 && isOnSecondRankBlue == 1) { LineNumGreen2Blue1++; }
        if (isOnThirdRankGreen == 2 && isOnThirdRankBlue == 1) { LineNumGreen2Blue1++; }
        if (LineNumGreen2Blue1 >= 1) { mapBuff.GetMapBuffOnLineRuleGreen2Blue1(true, LineNumGreen2Blue1); }//绿2蓝1

        if (isOnFirstRowBlue == 2 && isOnFirstRowRed == 1) { LineNumBlue2Red1++; }
        if (isOnSecondRowBlue == 2 && isOnSecondRowRed == 1) { LineNumBlue2Red1++; }
        if (isOnThirdRowBlue == 2 && isOnThirdRowRed == 1) { LineNumBlue2Red1++; }
        if (isOnFirstRankBlue == 2 && isOnFirstRankRed == 1) { LineNumBlue2Red1++; }
        if (isOnSecondRankBlue == 2 && isOnSecondRankRed == 1) { LineNumBlue2Red1++; }
        if (isOnThirdRankBlue == 2 && isOnThirdRankRed == 1) { LineNumBlue2Red1++; }
        if (LineNumBlue2Red1 >= 1) { mapBuff.GetMapBuffOnLineRuleBlue2Red1(true, LineNumBlue2Red1); }//蓝2红1

        if (isOnFirstRowBlue == 2 && isOnFirstRowGreen == 1) { LineNumBlue2Green1++; }
        if (isOnSecondRowBlue == 2 && isOnSecondRowGreen == 1) { LineNumBlue2Green1++; }
        if (isOnThirdRowBlue == 2 && isOnThirdRowGreen == 1) { LineNumBlue2Green1++; }
        if (isOnFirstRankBlue == 2 && isOnFirstRankGreen == 1) { LineNumBlue2Green1++; }
        if (isOnSecondRankBlue == 2 && isOnSecondRankGreen == 1) { LineNumBlue2Green1++; }
        if (isOnThirdRankBlue == 2 && isOnThirdRankGreen == 1) { LineNumBlue2Green1++; }
        if (LineNumBlue2Green1 >= 1) { mapBuff.GetMapBuffOnLineRuleBlue2Green1(true, LineNumBlue2Green1); }//蓝2绿1

        if (isOnFirstRowRed == 1 && isOnFirstRowGreen == 1 && isOnFirstRowBlue == 1) { LineNumRed1Blue1Green1++; }
        if (isOnSecondRowRed == 1 && isOnSecondRowGreen == 1 && isOnSecondRowBlue == 1) { LineNumRed1Blue1Green1++; }
        if (isOnThirdRowRed == 1 && isOnThirdRowGreen == 1 && isOnThirdRowBlue == 1) { LineNumRed1Blue1Green1++; }
        if (isOnFirstRankRed == 1 && isOnFirstRankGreen == 1 && isOnFirstRankBlue == 1) { LineNumRed1Blue1Green1++; }
        if (isOnSecondRankRed == 1 && isOnSecondRankGreen == 1 && isOnSecondRankBlue == 1) { LineNumRed1Blue1Green1++; }
        if (isOnThirdRankRed == 1 && isOnThirdRankGreen == 1 && isOnThirdRankBlue == 1) { LineNumRed1Blue1Green1++; }
        if (LineNumRed1Blue1Green1 >= 1) { mapBuff.GetMapBuffOnLineRuleRed1Green1Blue1(true, LineNumRed1Blue1Green1); }//红1绿1蓝1


        isTotalRedNum = isOnFirstRowRed + isOnSecondRowRed + isOnThirdRowRed;
        isTotalGreenNum = isOnFirstRowGreen + isOnSecondRowGreen + isOnThirdRowGreen;
        isTotalBlueNum = isOnFirstRowBlue + isOnSecondRowBlue + isOnThirdRowBlue;

        if (isTotalRedNum >= 1) //桌面红色牌数量
        { 
            mapBuff.GetMapBuffOnAloneRed1(true, isTotalRedNum);
            if (isTotalRedNum >=9 ) { mapBuff.GetMapBuffOnHomochromaticRed(true); }
        }
        if (isTotalGreenNum >= 1) //桌面绿色牌数量
        { 
            mapBuff.GetMapBuffOnAloneGreen1(true, isTotalGreenNum);
            if (isTotalGreenNum >= 9) { mapBuff.GetMapBuffOnHomochromaticGreen(true); }
        }
        if (isTotalBlueNum >= 1) //桌面蓝色牌数量
        { 
            mapBuff.GetMapBuffOnAloneBlue1(true, isTotalBlueNum);
            if (isTotalBlueNum >= 9) { mapBuff.GetMapBuffOnHomochromaticBlue(true); }
        }

    }

    #endregion
    /// <summary>
    /// 根据当前排列效果发送加成SO信息
    /// </summary>
    static void OnFinallyExamine()
    {
        /*
        //Debug.Log("地图排列。红色数量：isOnFirstRankRed："+ isOnFirstRankRed+ "，isOnSecondRankRed："+ isOnSecondRankRed+ "，isOnThirdRankRed:"+ isOnThirdRankRed+
        //"，第一行："+isOnFirstRowRed+"，第二行："+isOnSecondRowRed+"，第三行："+isOnThirdRowRed);

        if (isOnFirstRankRed == 3 || isOnSecondRankRed == 3 || isOnThirdRankRed == 3 || isOnFirstRowRed == 3 || isOnSecondRowRed == 3 || isOnThirdRowRed == 3)
        {
            mapBuff.GetMapBuffOnLineRuleRed3(true);
        }
        if (isOnFirstRankGreen == 3 || isOnSecondRankGreen == 3 || isOnThirdRankGreen == 3 || isOnFirstRowGreen == 3 || isOnSecondRowGreen == 3 || isOnThirdRowGreen == 3)
        {
            mapBuff.GetMapBuffOnLineRuleGreen3(true);
        }
        if (isOnFirstRankBlue == 3 || isOnSecondRankBlue == 3 || isOnThirdRankBlue == 3 || isOnFirstRowBlue == 3 || isOnSecondRowBlue == 3 || isOnThirdRowBlue == 3)
        {
            mapBuff.GetMapBuffOnLineRuleBlue3(true);
        }
        if (isOnFirstRowRed == 2 && isOnFirstRowGreen == 1 || isOnSecondRowRed == 2 && isOnSecondRowGreen == 1 || isOnThirdRowRed == 2 && isOnThirdRowGreen == 1 
            || isOnFirstRankRed == 2 && isOnFirstRankGreen == 1 || isOnSecondRankRed == 2 && isOnSecondRankGreen == 1 || isOnThirdRankRed == 2 && isOnThirdRankGreen == 1)
        {
            mapBuff.GetMapBuffOnLineRuleRed2Green1(true);
        }
        if (isOnFirstRowRed == 2 && isOnFirstRowBlue == 1 || isOnSecondRowRed == 2 && isOnSecondRowBlue == 1 || isOnThirdRowRed == 2 && isOnThirdRowBlue == 1
            || isOnFirstRankRed == 2 && isOnFirstRankBlue == 1 || isOnSecondRankRed == 2 && isOnSecondRankBlue == 1 || isOnThirdRankRed == 2 && isOnThirdRankBlue == 1)
        {
            mapBuff.GetMapBuffOnLineRuleRed2Blue1(true);
        }
        if (isOnFirstRowGreen == 2 && isOnFirstRowRed == 1 || isOnSecondRowGreen == 2 && isOnSecondRowRed == 1 || isOnThirdRowGreen == 2 && isOnThirdRowRed == 1 
            || isOnFirstRankGreen == 2 && isOnFirstRankRed == 1 || isOnSecondRankGreen == 2 && isOnSecondRankRed == 1 || isOnThirdRankGreen == 2 && isOnThirdRankRed == 1)
        {
            mapBuff.GetMapBuffOnLineRuleGreen2Red1(true);
        }
        if (isOnFirstRowGreen == 2 && isOnFirstRowBlue == 1 || isOnSecondRowGreen == 2 && isOnSecondRowBlue == 1 || isOnThirdRowGreen == 2 && isOnThirdRowBlue == 1
            || isOnFirstRankGreen == 2 && isOnFirstRankBlue == 1 || isOnSecondRankGreen == 2 && isOnSecondRankBlue == 1 || isOnThirdRankGreen == 2 && isOnThirdRankBlue == 1)
        {
            mapBuff.GetMapBuffOnLineRuleGreen2Blue1(true);
        }
        if (isOnFirstRowBlue == 2 && isOnFirstRowRed == 1 || isOnSecondRowBlue == 2 && isOnSecondRowRed == 1 || isOnThirdRowBlue == 2 && isOnThirdRowRed == 1
            || isOnFirstRankBlue == 2 && isOnFirstRankRed == 1 || isOnSecondRankBlue == 2 && isOnSecondRankRed == 1 || isOnThirdRankBlue == 2 && isOnThirdRankRed == 1)
        {
            mapBuff.GetMapBuffOnLineRuleBlue2Red1(true);
        }
        if (isOnFirstRowBlue == 2 && isOnFirstRowGreen == 1 || isOnSecondRowBlue == 2 && isOnSecondRowGreen == 1 || isOnThirdRowBlue == 2 && isOnThirdRowGreen == 1
            || isOnFirstRankBlue == 2 && isOnFirstRankGreen == 1 || isOnSecondRankBlue == 2 && isOnSecondRankGreen == 1 || isOnThirdRankBlue == 2 && isOnThirdRankGreen == 1)
        {
            mapBuff.GetMapBuffOnLineRuleBlue2Green1(true);
        }
        if (isOnFirstRowRed == 1 && isOnFirstRowGreen == 1 && isOnFirstRowBlue == 1 || isOnSecondRowRed == 1 && isOnSecondRowGreen == 1 && isOnSecondRowBlue == 1 ||
            isOnThirdRowRed == 1 && isOnThirdRowGreen == 1 && isOnThirdRowBlue == 1|| isOnFirstRankRed == 1 && isOnFirstRankGreen == 1 && isOnFirstRankBlue == 1 || 
            isOnSecondRankRed == 1 && isOnSecondRankGreen == 1 && isOnSecondRankBlue == 1 || 
            isOnThirdRankRed == 1 && isOnThirdRankGreen == 1 && isOnThirdRankBlue == 1)
        {
            mapBuff.GetMapBuffOnLineRuleRed1Green1Blue1(true);
        }
        */
    }

}
