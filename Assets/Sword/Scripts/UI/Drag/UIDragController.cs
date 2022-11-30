using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDragController : MonoBehaviour
{
    public UnityEvent OnDragStart = default;
    public UnityEvent OnDragEnd = default;

    Vector2 mousePoint;
    Vector3 offset = Vector2.zero;
    //Vector2 originalPosition;
    public bool IsDragging => isDragging;
    bool isDragging;
    DragTrigger dragTrigger;
    DragableItem dragableItem;
    DragTarget dragTarget;
    bool canSet;

    private void Awake()
    {

    }

    public void OnDraging(BaseEventData eventData)
    {
        if (isDragging)
        {
            mousePoint = ((PointerEventData)eventData).position;
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = mousePoint;
            GraphicRaycaster gr = UI_Manager.Instance.canvas.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pointerEventData, results);
            List<DragTarget> list = new List<DragTarget>();
            for (int i = 0; i < results.Count; i++)
            {
                DragTarget t = results[i].gameObject.transform.parent.GetComponent<DragTarget>();
                if (t != null)
                {
                    list.Add(t);
                }
            }
            DragTarget result = dragableItem.SearchTarget(list);
            if (result)
            {
                if (dragTarget && result != dragTarget)
                {
                    dragTarget.OnDragExit();
                }
                dragTarget = result;
                dragTarget.OnDragEnter();
                canSet = true;
            }
            else
            {
                if (dragTarget)
                {
                    dragTarget.OnDragExit();
                }
                dragTarget = null;
                canSet = false;
                //ReplaceTrigger(results);
            }
            dragableItem.Dragging(canSet, mousePoint);
        }
    }

    public void ReplaceTrigger(List<RaycastResult> list)
    {
        float sqrM = float.MaxValue;
        DragTrigger result = null;
        for (int i = 0; i < list.Count; i++)
        {
            DragTrigger temp = list[i].gameObject.transform.parent.GetComponent<DragTrigger>();
            if (temp != null)
            {
                Vector2 v2 = (Vector2)temp.transform.position - mousePoint;
                if (v2.sqrMagnitude < sqrM)
                {
                    result = temp;
                }
            }
        }
        if (result && result != dragTrigger)
        {
            dragableItem.DragEnd();
            dragableItem = result.DragStart();
            dragableItem.transform.position = mousePoint;
            dragableItem.DragStart(result);
            canSet = false;
            dragTrigger = result;
        }
    }

    public void OnDown(BaseEventData eventData)
    {
        if (!isDragging)
        {
            mousePoint = ((PointerEventData)eventData).position;
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = mousePoint;
            GraphicRaycaster gr = UI_Manager.Instance.canvas.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pointerEventData, results);
            for (int i = 0; i < results.Count; i++)
            {
                DragTrigger result = results[i].gameObject.transform.parent.GetComponent<DragTrigger>();
                if (result)
                {
                    dragableItem = result.DragStart();
                    dragableItem.transform.position = mousePoint;
                    dragableItem.DragStart(result);
                    isDragging = true;
                    canSet = false;
                    dragTrigger = result;
                    break;
                }
            }
        }
    }

    public void OnUp(BaseEventData eventData)
    {
        if (isDragging)
        {
            if (canSet)
            {
                dragTarget.OnArrival(dragableItem);
                dragableItem.OnDragCompelete(mousePoint);
            }
            else
            {
                dragableItem.DragEnd();
            }
            dragableItem = null;
            dragTarget = null;
            isDragging = false;
            OnDragEnd.Invoke();
        }
    }

    public void Interrupt()
    {
        if (isDragging)
        {
            canSet = false;
            dragableItem.DragEnd();
            dragableItem = null;
            dragTarget = null;
            isDragging = false;
            OnDragEnd.Invoke();
        }
    }
}
