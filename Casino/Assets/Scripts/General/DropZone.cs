using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class DropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] int betValue; // REPLACE THIS W/ chip value
    [SerializeField] PlayerInfo playerInfo;
    [SerializeField] bool destroyOnDrop = false;

    public void OnDrop(PointerEventData eventData)
    {
        DragNDrop chip = eventData.pointerDrag.GetComponent<DragNDrop>();

        if (destroyOnDrop)
        {
            Destroy(eventData.pointerDrag);
        }
        playerInfo.chipBalance += chip.betValue; // REPLACE WITH CHIP VALUE CURRENTLY INCORRECT

        Debug.Log(playerInfo.chipBalance);
    }
}
