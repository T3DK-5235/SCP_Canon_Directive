using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
//using UnityEngine.UIElements;

using TMPro;

public class UIHandler : MonoBehaviour
{
    [SerializeField] HiddenGameVariables hiddenGameVariables;

    private TextMeshProUGUI currentMonthText;

    [SerializeField] GameObject newMonthBlackout;

    [SerializeField] GameObject MainRoomLights;
    [SerializeField] GameObject HallLights;
    [SerializeField] GameObject DeskLights;

    [SerializeField] GameObject currentMonthTextObj;

    [Header("Proposal UI")]
    private TextMeshProUGUI proposalTitle;
    private TextMeshProUGUI proposalDesc;

    [SerializeField] GameObject proposalClipboard;
    
    [Header("Extra Info UI")]
    
    [SerializeField] GameObject extraInfoClipboard;

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
    public GameEvent DecideNextAction;
    void Awake() {
        // get the text from the proposal UI object (And cache it to prevent unneeded GetComponent calls)
        proposalTitle = proposalClipboard.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        proposalDesc = proposalClipboard.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();

        currentMonthText = currentMonthTextObj.transform.GetComponent<TextMeshProUGUI>();

        personnelPrefabList = new List<GameObject>();

        UpdateProposalUI(null, null);
        UpdateMonthUI();
    }

    //====================================================================
    //                        PROPOSAL UI SECTION                        |
    //====================================================================

    public void UpdateProposalUI(Component sender, object data) {
        proposalTitle.text = hiddenGameVariables._currentProposal.getProposalTitle();
        proposalDesc.text = hiddenGameVariables._currentProposal.getProposalDescription();
    }
    //====================================================================
    //                         EXTRA INFO SECTION                        |
    //====================================================================

    public void UpdateExtraInfo(Component sender, object data) {
        //Sets the number of player checked prefabs back to 0
        currentPrefabNum = 0;
        extraInfoClipboard.SetActive(true);

        //TODO figure out how to deal with prefab stuff

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
    }

    //Called from ClipButton
    public void GetNextPrefab(Component sender, object data) { 
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


    //====================================================================
    //                      UPDATING STAT UI SECTION                     |
    //====================================================================

    //Called by GameManager
    public void UpdateFlashingStatUI(Component sender, object data) 
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

    
    public void UpdateStatUI(Component sender, object data) 
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

    public void UpdateMonthUI() {
        currentMonthText.text = "Current Month:  " + hiddenGameVariables._currentMonth;
    }

    //====================================================================
    //                     NEW MONTH CHECKING SECTION                    |
    //====================================================================

    public void CheckNextAnim(Component sender, object data) {
        //If the month should end and the game isnt in the tutorial section (proposals 0-6)
        if (data == "newMonth") {
            UpdateMonthUI();
            StartCoroutine(INewMonth());
        } else if (data == "newProposal") {
            //Animation of person walking over and giving files? Or just other stuff
            StartCoroutine(INewProposal());
        }
    }

    IEnumerator INewMonth() {
        // newMonthBlackout.SetActive(false);

        // yield return new WaitForSeconds(1);

        // newMonthBlackout.SetActive(true);

        //TODO figure out if this is better defined at the start
        int MainRoomLightCount = MainRoomLights.transform.childCount;
        int HallLightCount = HallLights.transform.childCount;

        for(int i = MainRoomLightCount; i > 0; i--) {
            MainRoomLights.transform.GetChild(i-1).gameObject.SetActive(false);

            if ((i - 1) == 4) {
                DeskLights.transform.GetChild(0).gameObject.SetActive(false);
            } else if ((i - 1) == 3) {
                DeskLights.transform.GetChild(1).gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(0.5f);
        }
        for(int i = HallLightCount; i > 0; i--) {
            HallLights.transform.GetChild(i-1).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(2f);

        for(int i = 0; i < HallLightCount; i++) {
            HallLights.transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
        for(int i = 0; i < MainRoomLightCount; i++) {
            MainRoomLights.transform.GetChild(i).gameObject.SetActive(true);

            if ((i - 1) == 1) {
                DeskLights.transform.GetChild(1).gameObject.SetActive(true);
            } else if ((i - 1) == 2) {
                DeskLights.transform.GetChild(0).gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(0.5f);
        }

        LoadNextProposal();
    }

    IEnumerator INewProposal() {
        yield return new WaitForSeconds(0.1f);

        LoadNextProposal();
    }

    private void LoadNextProposal() {
        hiddenGameVariables._currentGameState = GameStateEnum.PROPOSAL_LOADING;
        DecideNextAction.Raise();
    }


}
