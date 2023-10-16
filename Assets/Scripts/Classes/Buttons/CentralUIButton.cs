using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CentralUIButton : MonoBehaviour, IPointerClickHandler {

    [Header("Buttons")]
    [SerializeField] GameObject centralUIButton;

    [Header("Events")]
    public GameEvent onSwitchCentralUIState;

    public void OnPointerClick(PointerEventData eventData)
    {
        onSwitchCentralUIState.Raise();
    }
}
