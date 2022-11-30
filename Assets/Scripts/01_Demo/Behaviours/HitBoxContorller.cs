using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxContorller : MonoBehaviour
{
    [SerializeField] List<GameObject> HitBoxObject;
    public void Play(int index,Vector2 range)
    {
        StartCoroutine(ActiveHitBox(index, range));
    }
    IEnumerator ActiveHitBox(int index, Vector2 range)
    {
        yield return new WaitForSeconds(range.x);
        HitBoxObject[index].SetActive(true);
        yield return new WaitForSeconds(range.y);
        HitBoxObject[index].SetActive(false);

   
    }
}
