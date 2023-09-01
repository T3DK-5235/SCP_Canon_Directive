using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExtraInfoButton : MonoBehaviour, IPointerClickHandler {

    [Header("Buttons")]
    [SerializeField] GameObject nextPrefabButton;

    [Header("Events")]
    public GameEvent onGetNextPrefab;

    public void OnPointerClick(PointerEventData eventData)
    {
        onGetNextPrefab.Raise(eventData);
        //StartCoroutine(AnimationCoroutine(canvasGroup));
    }

    // IEnumerator AnimationCoroutine(CanvasGroup canvasGroup)
    // {
    //     yield return new WaitForSeconds(3);
    // }
}
