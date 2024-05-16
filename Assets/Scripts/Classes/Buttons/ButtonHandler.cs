using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CentralUIButton : MonoBehaviour, IPointerClickHandler {

    [Header("Buttons")]

    //Buttons for choosing a proposal decision
    [SerializeField] GameObject decisionButton;
    [SerializeField] GameObject signatureButton;

    //Button to open Central UI
    [SerializeField] GameObject centralUIButton;

    //Buttons for switching details panel of central UI
    [SerializeField] GameObject SCPsButton;
    [SerializeField] GameObject TalesButton;
    [SerializeField] GameObject CanonsButton;
    [SerializeField] GameObject SeriesButton;
    [SerializeField] GameObject GroupsButton;

    //Button for switching displayed Extra Info page
    [SerializeField] GameObject nextPrefabButton;

    //Buttons for swapping the displayed stat pane
    [SerializeField] GameObject foundationStatButton;
    [SerializeField] GameObject goiStatButton;

    //Exit button
    [SerializeField] GameObject exitButton;

    [Header("Events")]
    public GameEvent onSwitchCentralUIState;

    [SerializeField] Texture2D objectCursor;

    //Dictionary to change
    //IDictionary<gameObject, Action()> buttonHoveredAction = new Dictionary<gameObject, Action()>();
    //Dictionary to call certain functions depending on the pressed button
    IDictionary<gameObject, Action()> buttonPressedAction = new Dictionary<gameObject, Action()>();
    void Awake()
    {
        
        buttonPressedAction.Add(decisionButton, );
    }

    public void OnMouseEnter() {
        // Debug.Log(this.gameObject.name);
        // if (this.gameObject == centralUIButton) {
        //     Debug.Log("this.gameobject does return same as gameobject in field");
        // }

        //Loop through specific buttons as some just change cursor, some start animations etc
        switch(this.gameObject) 
        {
            case centralUIButton:
                //TODO change this so that it changes the curstor based on the if statement it goes through
                objectCursor = objectCursor;
                break;
            
        }
        Cursor.SetCursor(objectCursor, new Vector2(12, 10), CursorMode.Auto);
    }

    public void OnMouseExit() {
        Cursor.SetCursor(null, new Vector2(12, 10), CursorMode.Auto);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onSwitchCentralUIState.Raise();
    }
}
