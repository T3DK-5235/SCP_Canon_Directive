using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
//using UnityEngine.UIElements;

using TMPro;
using System;

public class UIHandler : MonoBehaviour
{
    [SerializeField] HiddenGameVariables hiddenGameVariables;

    [SerializeField] AchievementsList achievementsList;

    private TextMeshProUGUI currentMonthText;

    [SerializeField] GameObject O5Elements;

    [SerializeField] GameObject achievementMask;

    [SerializeField] GameObject newMonthBlackout;

    [SerializeField] GameObject MainRoomLights;
    [SerializeField] GameObject HallLights;
    [SerializeField] GameObject DeskLights;
    [SerializeField] GameObject TopStatLight;
    [SerializeField] GameObject ambientWindowLight;
    [SerializeField] GameObject creditUILight;
    [SerializeField] GameObject achieveUILight;

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

    [Header("Tablet")]
    private bool tabletOn = false;
    [SerializeField] GameObject initialTabletScreen;
    [SerializeField] GameObject scorpLogo;
    [SerializeField] GameObject foundationStatScreen;
    [SerializeField] GameObject GoIStatScreen;

    [Header("Central UI")]

    private bool centralUIOpen = false;
    [SerializeField] GameObject SCPCredit;
    [SerializeField] GameObject SCPAchieve;
    [SerializeField] GameObject CentralUI;
    private RectTransform SCPCreditRT;
    private RectTransform SCPAchieveRT;

    private RectTransform CentralUIRT;

    [SerializeField] GameObject achievementPrefab;
    [SerializeField] GameObject achievementContainer;


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

    [Header("Details UI")]
    [SerializeField] DetailsList detailsList;
    [SerializeField] GameObject detailsPrefab;
    [SerializeField] GameObject detailsContainer;
    List<GameObject> detailsPrefabList;

    [Header("Events")]
    public GameEvent DecideNextAction;
    void Awake() {
        // get the text from the proposal UI object (And cache it to prevent unneeded GetComponent calls)
        proposalTitle = proposalClipboard.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        proposalDesc = proposalClipboard.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();

        SCPCreditRT = SCPCredit.GetComponent<RectTransform>();
        SCPAchieveRT = SCPAchieve.GetComponent<RectTransform>();
        CentralUIRT = CentralUI.GetComponent<RectTransform>();

        currentMonthText = currentMonthTextObj.transform.GetComponent<TextMeshProUGUI>();

        personnelPrefabList = new List<GameObject>();
        detailsPrefabList = new List<GameObject>();

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
        //Empty list from last usage
        for(int i = 0; i < personnelPrefabList.Count; i++) {
            Destroy(personnelPrefabList[i]);
        }

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

                //Set the first prefab to be active
                personnelPrefabList[currentPrefabNum].SetActive(true);
            }
        } else {
            genericInfoContainer.SetActive(true);
            personnelContainer.SetActive(false);

            extraInfoTitle.text = hiddenGameVariables._currentExtraInfo.getInfoTitle();
            //If it isn't Employees there will only be one element in the list at position 0
            extraInfoDesc.text = hiddenGameVariables._currentExtraInfo.getInfoDescription()[0];
        }
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

        SwitchTabletState(null, null);

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

            if ((i - 1) == 0) {
                TopStatLight.SetActive(false);
            }

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(2f);

        for(int i = 0; i < HallLightCount; i++) {
            HallLights.transform.GetChild(i).gameObject.SetActive(true);

            if (i == 0) {
                TopStatLight.SetActive(true);
            }

            yield return new WaitForSeconds(0.5f);
        }
        for(int i = 0; i < MainRoomLightCount; i++) {
            MainRoomLights.transform.GetChild(i).gameObject.SetActive(true);

            if (i == 0) {
                DeskLights.transform.GetChild(1).gameObject.SetActive(true);
            } else if (i == 1) {
                DeskLights.transform.GetChild(0).gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(0.5f);
        }

        SwitchTabletState(null, null);

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

    //====================================================================
    //                     TABLET STATUS SECTION                    |
    //====================================================================
    public void SwitchTabletState(Component sender, object data) {
        if(tabletOn == false) {
            StartCoroutine(ITabletOn());
            StartCoroutine(IDisplayLogo());
            tabletOn = true;
        } else if (tabletOn == true) {
            StartCoroutine(ITabletOff());
            tabletOn = false;
        }
    }

    IEnumerator ITabletOn()
    {
        initialTabletScreen.SetActive(true);

        Image initialTabletImage = initialTabletScreen.GetComponent<Image>();
        Color temp = initialTabletImage.color;

        float elapsedTime = 0;
        float duration = 0.1f;
        while (initialTabletImage.color.a < 0.95f){//elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(0.1f);

            temp = initialTabletImage.color;
            temp.a = Mathf.Lerp(temp.a, 1, elapsedTime / duration); //Time.deltaTime
            initialTabletImage.color = temp;
        }

        //Syncs this co-routine with the other co-routine below
        yield return new WaitForSeconds(1.5f);

        temp = initialTabletImage.color;
        temp.a = 0f; //Time.deltaTime
        initialTabletImage.color = temp;
        initialTabletScreen.SetActive(false);
    }

    IEnumerator ITabletOff()
    {
        initialTabletScreen.SetActive(true);
        scorpLogo.SetActive(true);

        Image scorpImage = scorpLogo.GetComponent<Image>();
        Color temp = scorpImage.color;

        float elapsedTime = 0;
        float duration = 1f;
        Debug.Log("scorpImage.color.a: " + scorpImage.color.a);
        while (scorpImage.color.a < 0.95f){//elapsedTime < duration) {
            Debug.Log("scorpImage.color.a: " + scorpImage.color.a);
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(0.1f);

            temp = scorpImage.color;
            temp.a = Mathf.Lerp(temp.a, 1, elapsedTime / duration); //Time.deltaTime
            scorpImage.color = temp;
        }

        yield return new WaitForSeconds(0.2f);

        foundationStatScreen.SetActive(false);
        GoIStatScreen.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        //Resets the image back to being invisible
        temp = scorpImage.color;
        temp.a = 0f; //Time.deltaTime
        scorpImage.color = temp;
        scorpLogo.SetActive(false);
    }

    IEnumerator IDisplayLogo()
    {
        yield return new WaitForSeconds(0.5f);
        scorpLogo.SetActive(true);

        Image scorpImage = scorpLogo.GetComponent<Image>();
        Color temp = scorpImage.color;

        float elapsedTime = 0;
        float duration = 0.1f;

        while (scorpImage.color.a < 0.95f){//elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(0.1f);

            temp = scorpImage.color;
            temp.a = Mathf.Lerp(temp.a, 1, elapsedTime / duration); //Time.deltaTime
            scorpImage.color = temp;
        }
        
        yield return new WaitForSeconds(0.5f);

        foundationStatScreen.SetActive(true);
        GoIStatScreen.SetActive(false);

        //Resets the image back to being invisible
        temp = scorpImage.color;
        temp.a = 0f; //Time.deltaTime
        scorpImage.color = temp;
        scorpLogo.SetActive(false);
    }

    //====================================================================
    //                      UPDATING STAT UI SECTION                     |
    //====================================================================

    public void SwitchCentralUIState(Component sender, object data) {
        if(centralUIOpen == false) {
            StartCoroutine(IOpenCentralUI());
            centralUIOpen = true;
        } else if (centralUIOpen == true) {
            StartCoroutine(ICloseCentralUI());
            centralUIOpen = false;
        }
    }

    IEnumerator IOpenCentralUI()
    {
        //Activate SCP section of details list
        SwitchDetails(null, "SCPs");

        achievementMask.GetComponent<Mask>().enabled = false;

        //Show first 4 achievements
        for(int i = 0; i < 4; i++) {
            //Instantiate new achievementPrefab with achievementContainer as the parent
            GameObject achievementInstance = Instantiate(achievementPrefab, achievementContainer.transform) as GameObject;
            achievementsList._displayedAchievements.Add(achievementInstance);
            //TODO populate achievement with info

            Image prefabIcon = achievementInstance.transform.GetChild(0).GetComponent<Image>();
            TextMeshProUGUI prefabText = achievementInstance.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            //TODO remove repeated calls to achievement list byy caching "achievementsList._achievements[i]"
            if (achievementsList._achievements[i].getAchievementCompletion() == true) {
                prefabText.text = achievementsList._achievements[i].getAchievementName() + achievementsList._achievements[i].getAchievementDescription();
            } else {
                prefabText.text = achievementsList._achievements[i].getAchievementHint();
            }

            achievementInstance.SetActive(true);
            
            //TODO add icon logic (later)
        }
        
        yield return new WaitForSeconds(0.5f);

        creditUILight.SetActive(true);
        achieveUILight.SetActive(true);

        LeanTween.moveY(CentralUIRT, 66.55f, 1f).setEase(LeanTweenType.easeInOutQuad).setDelay(1f);

        yield return new WaitForSeconds(2f);

        O5Elements.SetActive(false);
        achievementMask.GetComponent<Mask>().enabled = true;

        LeanTween.moveY(SCPAchieveRT, 10007f, 0.75f).setEase(LeanTweenType.easeOutBounce).setDelay(0.5f);

    }

    //TODO depending on how far the scrollbar moves, instantiate new achievement prefabs

    IEnumerator ICloseCentralUI()
    {
        yield return new WaitForSeconds(0.5f);

        LeanTween.moveY(SCPAchieveRT, 3450f, 0.75f).setEase(LeanTweenType.easeOutQuad).setDelay(0.5f);

        yield return new WaitForSeconds(1.5f);

        achievementMask.GetComponent<Mask>().enabled = false;
        O5Elements.SetActive(true);
        
        LeanTween.moveY(CentralUIRT, -17.5f, 1f).setEase(LeanTweenType.easeInOutQuad).setDelay(1f);

        //Lets animation finish before removing lights
        yield return new WaitForSeconds(2f);

        creditUILight.SetActive(false);
        achieveUILight.SetActive(false);

        for (int i = 0; i < 3; i++) {
            GameObject.Destroy(achievementsList._displayedAchievements[i]);
        }

        //TODO check if this is needed
        achievementsList._displayedAchievements.Clear();
    }

    //====================================================================
    //                 SWITCHING SHOWN DETAILS SECTION                   |
    //====================================================================

    public void SwitchDetails(Component sender, object data) {
        List<int> infoToDisplay = new List<int>();

        for(int i = 0; i < detailsPrefabList.Count; i++) {
            Destroy(detailsPrefabList[i]);
        }
        detailsPrefabList.Clear();

        if (data == "SCPs") {
            infoToDisplay = detailsList._discoveredSCPs;
        } else if (data == "Tales") {
            infoToDisplay = detailsList._discoveredTales;
        } else if (data == "Canons") {
            infoToDisplay = detailsList._discoveredCanons;
        } else if (data == "Series") {
            infoToDisplay = detailsList._discoveredSeries;
        } else if (data == "Groups") {
            infoToDisplay = detailsList._discoveredGroups;
        }

        for(int i = 0; i < infoToDisplay.Count; i++) {
            //Get the actual Details object from the list
            GenericDetails detailsToDisplay = detailsList._details[infoToDisplay[i]];  
            //Instantiate a Details prefab
            GameObject detailsPrefabInstance = Instantiate(detailsPrefab, detailsContainer.transform) as GameObject;
            
            TextMeshProUGUI prefabTitle = detailsPrefabInstance.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI prefabDescription = detailsPrefabInstance.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI prefabAuthors = detailsPrefabInstance.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            prefabTitle.text = detailsToDisplay.getArticleName();
            prefabDescription.text = detailsToDisplay.getArticleDescription();
            prefabAuthors.text = detailsToDisplay.getArticleAuthors();

            detailsPrefabList.Add(detailsPrefabInstance);

            detailsPrefabInstance.transform.GetChild(0).GetChild(0).transform.localScale = new Vector3(1, 1, 1);

            detailsPrefabInstance.SetActive(true);
        }
    }

}
