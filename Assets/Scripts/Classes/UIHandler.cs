using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
//using UnityEngine.UIElements;

using TMPro;

public class UIHandler : MonoBehaviour
{
    [SerializeField] HiddenGameVariables hiddenGameVariables;

    //Maybe instead try make a UI scriptable object
    
    [SerializeField] GameObject currentMonthTextObj;

    [SerializeField] GameObject proposalClipboard;

    [SerializeField] GameObject extraInfoClipboard;
    
    private TextMeshProUGUI proposalTitle;
    private TextMeshProUGUI proposalDesc;
    private TextMeshProUGUI currentMonthText;

    [SerializeField] TextMeshProUGUI extraInfoTitle;
    [SerializeField] TextMeshProUGUI extraInfoDesc;

    [SerializeField] GameObject genericInfoContainer;
    [SerializeField] GameObject personnelContainer;
    [SerializeField] GameObject personnelPrefab;
    
    List<GameObject> personnelPrefabList;

    [Header("Foundation UI Stat Bars")]

    [SerializeField] Slider totalMtfBar;
    [SerializeField] Slider availableMtfBar;

    [SerializeField] Slider totalResearcherBar;
    [SerializeField] Slider availableResearcherBar;

    [SerializeField] Slider totalDClassBar;
    [SerializeField] Slider availableDClassBar;

    [SerializeField] Slider totalMoraleBar;
    [SerializeField] Slider currentMoraleBar;

    [Header("GOI UI Stat Bars")]

    [SerializeField] Slider gocBar;
    [SerializeField] Slider nalkaBar;
    [SerializeField] Slider mekaniteBar;
    [SerializeField] Slider serpentsHandBar;
    [SerializeField] Slider factoryBar;
    [SerializeField] Slider andersonBar;

    private int currentPrefabNum = 0;

    private bool startUIFlashing;

    private bool flashStatBar = false;

    private IEnumerator activeBlinkTimer;

    [Header("Events")]
    public GameEvent onExtraInfoDisplayed;

    //TODO utilize events to fire the below actions instead of update?
    //TODO update background of window 
    //TODO add screen overlays

    void Start() {
        // Maybe put in try catch
        // get the text from the proposal UI object (And cache it to prevent unneeded GetComponent calls)
        proposalTitle = proposalClipboard.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        proposalDesc = proposalClipboard.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();

        // and set it to the current proposal's description
        proposalTitle.text = hiddenGameVariables._currentProposal.getProposalTitle();
        proposalDesc.text = hiddenGameVariables._currentProposal.getProposalDescription();

        currentMonthText = currentMonthTextObj.transform.GetComponent<TextMeshProUGUI>();
        currentMonthText.text = "Current Month:  " + hiddenGameVariables._currentMonth;

        personnelPrefabList = new List<GameObject>();

        //TODO set sliders to HiddenGameVariables values
    }
        //Is called after decision is made to update proposal
    public void updateProposal(Component sender, object data) {
        //TODO Add animation or movement here of the proposal
        proposalTitle.text = hiddenGameVariables._currentProposal.getProposalTitle();
        proposalDesc.text = hiddenGameVariables._currentProposal.getProposalDescription();
    }

    public void updateExtraInfo(Component sender, object data) {

        //Sets the number of player checked prefabs back to 0
        currentPrefabNum = 0;

        extraInfoClipboard.SetActive(true);
        // StartCoroutine(ExtraInfoClipboard());

        // string extraInfoType = hiddenGameVariables._currentExtraInfo.getExtraInfoType();
        // if (extraInfoType = "PotentialEmployees") {
        //     //TODO figure out how to deal with prefab stuff
        // }

        //TODO Add animation or movement here of the proposal

        string extraInfoType = hiddenGameVariables._currentExtraInfo.getExtraInfoType();
        int extraInfoNum = hiddenGameVariables._currentExtraInfo.getInfoDescription().Count;

        if(extraInfoType == "PotentialEmployees") {
            personnelContainer.SetActive(true);
            genericInfoContainer.SetActive(false);

            for(int i = 0; i < extraInfoNum; i++) {
                GameObject personnelPrefabInstance = Instantiate(personnelPrefab, personnelContainer.transform) as GameObject;
                
                TextMeshProUGUI prefabTitle = personnelPrefabInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI prefabDescription = personnelPrefabInstance.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

                prefabTitle.text = hiddenGameVariables._currentExtraInfo.getInfoTitle();
                prefabDescription.text = hiddenGameVariables._currentExtraInfo.getInfoDescription()[i];

                personnelPrefabList.Add(personnelPrefabInstance);

                personnelPrefabInstance.SetActive(false);

                //TODO delete all clones after use
            }
        } else {
            genericInfoContainer.SetActive(true);
            personnelContainer.SetActive(false);

            extraInfoTitle.text = hiddenGameVariables._currentExtraInfo.getInfoTitle();
            //If it isn't Employees there will only be one element in the list at position 0
            extraInfoDesc.text = hiddenGameVariables._currentExtraInfo.getInfoDescription()[0];
        }

        //Set the first prefab to be active
        personnelPrefabList[currentPrefabNum].SetActive(true);

        // Debug.Log("Raising extra info display event");
        // onExtraInfoDisplayed.Raise();
    }

    public void getNextPrefab(Component sender, object data) { 
        int prevPrefabNum = 0;

        // if the current prefab number is less than the possible total prefab number
        // as currentPrefabNum starts at 0, rather than 1, personnelPrefabList.Count has to be decreased as well
        if (currentPrefabNum < personnelPrefabList.Count - 1) {
            // Debug.Log("Going up!" + currentPrefabNum);
            //Set prev prefab to inactive
            prevPrefabNum = currentPrefabNum;
            //Increase the current prefab num
            currentPrefabNum++;
            //Set next prefab to active (currentPrefabNum starts at 1, whilst lists start at 0)
            personnelPrefabList[currentPrefabNum].SetActive(true);
        } else {
            // Debug.Log("Back to start");
            //reset it back to the start prefab
            currentPrefabNum = 0;
            personnelPrefabList[currentPrefabNum].SetActive(true);
            //Set prev prefab to inactive
            prevPrefabNum = personnelPrefabList.Count - 1;
        }

        personnelPrefabList[prevPrefabNum].SetActive(false);
    }

    //TODO need to call this from proposal manager when stats change during a proposal
    public void updateFlashingStatUI(Component sender, object data) 
    {
        //Stop any running versions of the blink timer first
        //check if it is running already before trying to stop it
        if (activeBlinkTimer != null) {
            StopCoroutine(activeBlinkTimer);
        }

        //UI told to flash
        startUIFlashing = true;
        
        activeBlinkTimer = BlinkTimer();
        StartCoroutine(activeBlinkTimer);
    }

    //called from proposal manager when stats change at the end of a proposal
    public void updateStatUI(Component sender, object data) 
    {
        //For every stat changed
        if(hiddenGameVariables._myStatCopy.__statsChanged.Count != 0) {
            for (int i = 0; i < hiddenGameVariables._myStatCopy.__statsChanged.Count; i++) {
                switch (hiddenGameVariables._myStatCopy.__statsChanged[i]) 
                {
                    case 0:
                        availableMtfBar.value = hiddenGameVariables._availableMTF;
                        break;
                    case 1:
                        totalMtfBar.value = hiddenGameVariables._totalMTF;
                        break;
                    case 2:
                        availableResearcherBar.value = hiddenGameVariables._availableResearchers;
                        break;
                    case 3:
                        totalResearcherBar.value = hiddenGameVariables._totalResearchers;
                        break;
                    case 4:
                        availableDClassBar.value = hiddenGameVariables._availableDClass;
                        break;
                    case 5:
                        totalDClassBar.value = hiddenGameVariables._totalDClass;
                        break;
                    case 6:
                        currentMoraleBar.value = hiddenGameVariables._currentMorale;
                        break;
                    case 7:
                        totalMoraleBar.value = hiddenGameVariables._totalMorale;
                        break;
                    case 8:
                        gocBar.value = hiddenGameVariables._favourGOC;
                        break;
                    case 9:
                        nalkaBar.value = hiddenGameVariables._favourNalka;
                        break;
                    case 10:
                        mekaniteBar.value = hiddenGameVariables._favourMekanite;
                        break;
                    case 11:
                        serpentsHandBar.value = hiddenGameVariables._favourSerpentsHand;
                        break;
                    case 12:
                        factoryBar.value = hiddenGameVariables._favourFactory;
                        break;
                    case 13:
                        andersonBar.value = hiddenGameVariables._favourAnderson;
                        break;
                }
            }
        }
        
        startUIFlashing = false;
    }    

    IEnumerator BlinkTimer()
    {

        //TODO fix this so not all stats flash, just the needed ones
        //TODO may need to use dictionary for this to make it easier
        //Used to flash between new and old value
        while (startUIFlashing) {
            if (flashStatBar == false) {
                availableMtfBar.value = hiddenGameVariables._myStatCopy.__availableMTF;
                totalMtfBar.value = hiddenGameVariables._myStatCopy.__totalMTF;

                availableResearcherBar.value = hiddenGameVariables._myStatCopy.__availableResearchers;
                totalResearcherBar.value = hiddenGameVariables._myStatCopy.__totalResearchers;

                availableDClassBar.value = hiddenGameVariables._myStatCopy.__availableDClass;
                totalDClassBar.value = hiddenGameVariables._myStatCopy.__totalDClass;

                currentMoraleBar.value = hiddenGameVariables._myStatCopy.__currentMorale;
                totalMoraleBar.value = hiddenGameVariables._myStatCopy.__totalMorale;

                gocBar.value = hiddenGameVariables._myStatCopy.__favourGOC;
                nalkaBar.value = hiddenGameVariables._myStatCopy.__favourNalka;
                mekaniteBar.value = hiddenGameVariables._myStatCopy.__favourMekanite;
                serpentsHandBar.value = hiddenGameVariables._myStatCopy.__favourSerpentsHand;
                factoryBar.value = hiddenGameVariables._myStatCopy.__favourFactory;
                andersonBar.value = hiddenGameVariables._myStatCopy.__favourAnderson;


                yield return new WaitForSeconds(0.1f);
                flashStatBar = true;
            } else if (flashStatBar == true) {
                availableMtfBar.value = hiddenGameVariables._availableMTF;
                totalMtfBar.value = hiddenGameVariables._totalMTF;

                availableResearcherBar.value = hiddenGameVariables._availableResearchers;
                totalResearcherBar.value = hiddenGameVariables._totalResearchers;

                availableDClassBar.value = hiddenGameVariables._availableDClass;
                totalDClassBar.value = hiddenGameVariables._totalDClass;

                currentMoraleBar.value = hiddenGameVariables._currentMorale;
                totalMoraleBar.value = hiddenGameVariables._totalMorale;

                gocBar.value = hiddenGameVariables._favourGOC;
                nalkaBar.value = hiddenGameVariables._favourNalka;
                mekaniteBar.value = hiddenGameVariables._favourMekanite;
                serpentsHandBar.value = hiddenGameVariables._favourSerpentsHand;
                factoryBar.value = hiddenGameVariables._favourFactory;
                andersonBar.value = hiddenGameVariables._favourAnderson;

                yield return new WaitForSeconds(0.1f);
                flashStatBar = false;
            }

            //Delay between flashes
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void updateNewMonthUI(Component sender, object data) 
    {
        //Set all finished changed values in hiddenGameVariables
        availableMtfBar.value = hiddenGameVariables._availableMTF;
        totalMtfBar.value = hiddenGameVariables._totalMTF;

        availableResearcherBar.value = hiddenGameVariables._availableResearchers;
        totalResearcherBar.value = hiddenGameVariables._totalResearchers;

        availableDClassBar.value = hiddenGameVariables._availableDClass;
        totalDClassBar.value = hiddenGameVariables._totalDClass;

        currentMoraleBar.value = hiddenGameVariables._currentMorale;
        totalMoraleBar.value = hiddenGameVariables._totalMorale;

        gocBar.value = hiddenGameVariables._favourGOC;
        nalkaBar.value = hiddenGameVariables._favourNalka;
        mekaniteBar.value = hiddenGameVariables._favourMekanite;
        serpentsHandBar.value = hiddenGameVariables._favourSerpentsHand;
        factoryBar.value = hiddenGameVariables._favourFactory;
        andersonBar.value = hiddenGameVariables._favourAnderson;

        //Sets the new month UI
        currentMonthText.text = "Current Month:  " + hiddenGameVariables._currentMonth;
    }
}
