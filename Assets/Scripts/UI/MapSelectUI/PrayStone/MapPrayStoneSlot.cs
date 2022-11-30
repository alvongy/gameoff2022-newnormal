using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPrayStoneSlot : MonoBehaviour
{
    public int _slotID;
    public PrayStoneType _prayStonen_Type;
    public Material _materail;
    [SerializeField] public PrayStoneSO _prayStoneSO;
    public VoidEventChannelSO MapPrayStoneChangeChannel;

    private List<Vector3> rangePos;

    private void OnEnable()
    {
        MapPrayStoneChangeChannel.OnEventRaised += RefreshPrayStoneData;
    }
    private void OnDisable()
    {
        MapPrayStoneChangeChannel.OnEventRaised -= RefreshPrayStoneData;
    }
    private void Start()
    {
        RefreshPrayStoneData();
    }

    private void RefreshPrayStoneData()
    {
        rangePos = new List<Vector3>
        {
            { new Vector3(-3f,1.5f,-3f) },{ new Vector3(-3f,1.5f,0f) },{ new Vector3(-3f,1.5f,3f) },
            { new Vector3(3f,1.5f,-3f) },{ new Vector3(3f,1.5f,0f) },{ new Vector3(3f,1.5f,3f) },
            { new Vector3(0f,1.5f,-3f) },{ new Vector3(0f,1.5f,0f) },{ new Vector3(0f,1.5f,3f) },
        };

        for (int i=0;i<transform.childCount;i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        if (YKDataInfoManager.Instance.CurrentPrayStoneRecord.ContainsKey(_prayStonen_Type))
        {
            for (int i = 0; i < YKDataInfoManager.Instance.CurrentPrayStoneRecord[_prayStonen_Type]; i++)
            {
                GameObject prayGO = Instantiate(MapSelectUIManager.Instance.PrayStonePrefab, transform);
                prayGO.GetComponent<MapPrayStoneEneity>().PrayStoneInit(_prayStoneSO);
                prayGO.GetComponent<MeshRenderer>().material = _materail;

                int index = Random.Range(0, rangePos.Count);
                prayGO.transform.localPosition = rangePos[index];
                rangePos.RemoveAt(index);
            }
        }
    }
}
