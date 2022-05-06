using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DragNDrop : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private Vector2 dragOffset;

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragOffset = eventData.position - (Vector2)transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position - dragOffset;
    }
}