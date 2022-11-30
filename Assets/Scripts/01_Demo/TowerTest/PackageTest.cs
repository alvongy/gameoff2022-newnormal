using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageTest : MonoBehaviour
{
    [SerializeField]  GameObject PackageUI;
    [SerializeField] TransformAnchor anchor;
    [SerializeField] List<GameObject> builds;

    public void OpenPackage()
    {
        PackageUI.SetActive(true);
    }
    public void ClosePackage()
    {
        PackageUI.SetActive(false);
    }
    public void Build(int i)
    {
        GameObject.Instantiate(builds[i], anchor.Value.position,Quaternion.identity);
    }

}
