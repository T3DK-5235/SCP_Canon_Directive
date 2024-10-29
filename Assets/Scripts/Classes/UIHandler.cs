using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using UnityEngine.UI;
//using UnityEngine.UIElements;

using TMPro;
using System;
using Unity.VisualScripting;

public class UIHandler : MonoBehaviour
{
    [SerializeField] HiddenGameVariables hiddenGameVariables;
    [SerializeField] ProposalsList proposalsList;
    [SerializeField] AchievementsList achievementsList;

    [SerializeField] Canvas gameCanvas;
    
    [SerializeField] GameObject bottomBackgroundUI;
    private Canvas bottomUICanvas;

    private TextMeshProUGUI currentMonthText;
    private TextMeshProUGUI uncheckedDetailsText;

    [SerializeField] GameObject O5Elements;
    [SerializeField] GameObject achievementMask;
    [SerializeField] GameObject MainRoomLights;
    [SerializeField] GameObject HallLights;
    [SerializeField] GameObject DeskLights;
    [SerializeField] GameObject TopStatLight;
    [SerializeField] GameObject ambientWindowLight;
    [SerializeField] GameObject creditUILight;
    [SerializeField] GameObject achieveUILight;

    [SerializeField] GameObject currentMonthTextObj;
    [SerializeField] GameObject uncheckedDetailsObj;
    [SerializeField] GameObject alertLight;

    [Header("Proposal UI")]
    private TextMeshProUGUI proposalTitle;
    private TextMeshProUGUI proposalDesc;

    private RectTransform proposalTitlePos;
    private RectTransform proposalDescPos;

    [SerializeField] GameObject proposalClipboard;
    
    [Header("Extra Info UI")]
    
    [SerializeField] GameObject extraInfoClipboard;

    [SerializeField] TextMeshProUGUI extraInfoTitle;
    [SerializeField] TextMeshProUGUI extraInfoDesc;

    [SerializeField] GameObject genericInfoContainer;
    [SerializeField] GameObject personnelContainer;
    [SerializeField] GameObject personnelPrefab;
    
    private List<GameObject> personnelPrefabList;
    private BoxCollider2D extraInfoClipCollider;
    private Image extraInfoClipImageCollider;

    [Header("Tablet")]
    private bool tabletOn = false;
    [SerializeField] GameObject initialTabletScreen;
    [SerializeField] GameObject scorpLogo;
    [SerializeField] GameObject foundationStatScreen;
    [SerializeField] GameObject GoIStatScreen;

    [Header("Central UI")]
    [SerializeField] GameObject CentralUI;

    [Header("Central Top")]
    private bool centralUITopOpen = false;
    [SerializeField] GameObject SCPCredit;
    [SerializeField] GameObject SCPAchieve;
    private GameObject CentralUITop;
    private RectTransform SCPCreditRT;
    private RectTransform SCPAchieveRT;
    private RectTransform CentralUITopRT;
    private Canvas CentralUITopCanvas;

    [SerializeField] GameObject CentralUIButton;
    private BoxCollider2D centralUIButtonCollider;
    private Image centralUIButtonImageCollider;

    [SerializeField] GameObject achievementPrefab;
    [SerializeField] GameObject achievementContainer;

    [Header("Central Bottom")]
    private bool centralUIBottomOpen = false;
    private GameObject CentralUIBottom;
    [SerializeField] GameObject SiteMapUI;
    [SerializeField] GameObject SiteMapExtraUI;
    private RectTransform CentralUIBottomRT;
    private RectTransform SiteMapRT;
    private RectTransform SiteMapExtraRT;
    private Canvas CentralUIBottomCanvas;

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
    private List<GameObject> detailsPrefabList;
    private string activeDetailsContainer;

    [Header("Follow Up Info UI")]
    [SerializeField] FollowUpInfoList followUpInfoList;
    [SerializeField] GameObject followUpInfoPrefab;
    [SerializeField] GameObject followUpInfoContainer;
    private List<GameObject> followUpInfoPrefabList;
    [SerializeField] GameObject followUpInfoClipboard;
    
    [Header("Events")]
    public GameEvent DecideNextAction;

    public void InitUI(Component sender, object data) {
        // get the text from the proposal UI object (And cache it to prevent unneeded GetComponent calls)
        proposalTitle = proposalClipboard.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        proposalDesc = proposalClipboard.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();

        proposalTitlePos = proposalClipboard.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        proposalDescPos = proposalClipboard.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();

        bottomUICanvas = bottomBackgroundUI.GetComponent<Canvas>();
        // bottomUICanvas.sortingOrder = -5;

        SCPCreditRT = SCPCredit.GetComponent<RectTransform>();
        SCPAchieveRT = SCPAchieve.GetComponent<RectTransform>();
        CentralUITopRT = CentralUI.transform.GetChild(0).GetComponent<RectTransform>();
        CentralUITopCanvas = CentralUI.transform.GetChild(0).GetComponent<Canvas>();

        SiteMapRT = SiteMapUI.GetComponent<RectTransform>();
        SiteMapExtraRT = SiteMapExtraUI.GetComponent<RectTransform>();
        CentralUIBottomRT = CentralUI.transform.GetChild(1).GetComponent<RectTransform>();
        CentralUIBottomCanvas = CentralUI.transform.GetChild(1).GetComponent<Canvas>();

        currentMonthText = currentMonthTextObj.transform.GetComponent<TextMeshProUGUI>();
        uncheckedDetailsText = uncheckedDetailsObj.transform.GetComponent<TextMeshProUGUI>();
        //TODO once saving is implemented then remove this line as the save file will have it in
        detailsList._newlyDiscoveredDetails = 0;

        personnelPrefabList = new List<GameObject>();
        detailsPrefabList = new List<GameObject>();
        followUpInfoPrefabList = new List<GameObject>();
        // UpdateProposalUI(null, null);

        //TODO change this to be default of proposal 0 (unless save data exists)
        proposalTitle.text = proposalsList._proposals[0].getProposalTitle();
        proposalDesc.text = proposalsList._proposals[0].getProposalDescription();
        //TODO fix other related bugs like extra info appearing when it shouldnt(probably all of Current Proposal Handling section of HiddenGameVariables)

        extraInfoClipCollider = extraInfoClipboard.transform.GetChild(0).GetChild(2).GetComponent<BoxCollider2D>();
        extraInfoClipImageCollider = extraInfoClipboard.transform.GetChild(0).GetChild(2).GetComponent<Image>();

        centralUIButtonCollider = CentralUIButton.GetComponent<BoxCollider2D>();
        centralUIButtonImageCollider = CentralUIButton.GetComponent<Image>();

        InitAchievements();

        UpdateMonthUI();
    }

    private void InitAchievements() {
        for(int i = 0; i < achievementsList._achievements.Count; i++) {
            //Instantiate new achievementPrefab with achievementContainer as the parent
            GameObject achievementInstance = Instantiate(achievementPrefab, achievementContainer.transform) as GameObject;
            achievementsList._displayedAchievements.Add(achievementInstance);

            Image prefabIcon = achievementInstance.transform.GetChild(0).GetComponent<Image>();
            TextMeshProUGUI prefabText = achievementInstance.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            GenericAchievement achievement = achievementsList._achievements[i];

            if (achievement.getAchievementCompletion() == true) {
                prefabText.text = achievement.getAchievementName() + achievement.getAchievementDescription();
            } else {
                prefabText.text = achievement.getAchievementHint();
            }

            achievementInstance.SetActive(false);
            
            //TODO add icon logic
            //TODO depending on how far the scrollbar moves, instantiate new achievement prefabs
        }
    }

    //====================================================================
    //                        PROPOSAL UI SECTION                        |
    //====================================================================

    public void UpdateProposalUI(Component sender, object data) {
        //TODO change this to be default of proposal 0 (unless save data exists)
        proposalTitle.text = hiddenGameVariables._currentProposal.getProposalTitle();
        proposalDesc.text = hiddenGameVariables._currentProposal.getProposalDescription();

        if(hiddenGameVariables._currentProposal.getExtraInfo() != -1) {
            UpdateExtraInfo();
        }

        StartCoroutine(ResizeProposalUI());
    }

    IEnumerator ResizeProposalUI()
    {
        //Need to wait a second for the box to finish resizing when the text is entered above
        yield return new WaitForSeconds(0.01f);
        //Debug.Log("Height? " + proposalTitlePos.sizeDelta.y);
        //Will only equal 0 on startup
        if (proposalTitlePos.sizeDelta.y > 8) {
            proposalDescPos.anchoredPosition = new Vector3(47, -39, 0);
        } else {
            proposalDescPos.anchoredPosition = new Vector3(47, -32, 0);
        }
    }


    //====================================================================
    //                         EXTRA INFO SECTION                        |
    //====================================================================

    public void UpdateExtraInfo() {
        //Sets the number of player checked prefabs back to 0
        currentPrefabNum = 0;
        extraInfoClipboard.SetActive(true);
        //Empty list from last usage
        for(int i = 0; i < personnelPrefabList.Count; i++) {
            Destroy(personnelPrefabList[i]);
        }
        personnelPrefabList.Clear();

        string extraInfoType = hiddenGameVariables._currentExtraInfo.getExtraInfoType();
        int extraInfoNum = hiddenGameVariables._currentExtraInfo.getInfoDescription().Count;

        //TODO rewrite this section to allow any info type to have multiple prefabs

        if(extraInfoType == "PotentialEmployees") {
            personnelContainer.SetActive(true);
            genericInfoContainer.SetActive(false);

            for(int i = 0; i < extraInfoNum; i++) {
                GameObject personnelPrefabInstance = Instantiate(personnelPrefab, personnelContainer.transform) as GameObject;
                
                TextMeshProUGUI prefabTitle = personnelPrefabInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI prefabDescription = personnelPrefabInstance.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

                prefabTitle.text = hiddenGameVariables._currentExtraInfo.getInfoTitle();
                prefabDescription.text = hiddenGameVariables._currentExtraInfo.getInfoDescription()[i];

                //TODO change this list to be general not just for personnel prefabs
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

        //If these is more than 1 prefab, enable visual indicators of a user being able to switch prefabs
        if (personnelPrefabList.Count > 1) {
            extraInfoClipCollider.enabled = true;
            extraInfoClipImageCollider.enabled = true;
        } else {
            extraInfoClipCollider.enabled = false;
            extraInfoClipImageCollider.enabled = false;
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
        // Debug.Log("Updating Stats");
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
        //Debug.Log("check which anim to use (true = new month): " + (bool)data);
        //if ((bool)data == true) {
        //    UpdateMonthUI();
        //    StartCoroutine(IEndMonth());
        //} else {
        //    //Animation of person walking over and giving files? Or just other stuff
        //StartCoroutine(INewProposal());
        //}
        //TODO revamp this to slide proposal offscreen (After being notified by GFE_2)
    }

    public void EndMonthAnim(Component sender, object data) {
        StartCoroutine(IEndMonth());
    }

    //TODO seperate this out so the initial lights are done, then events can be checked, then the lights come back on
    IEnumerator IEndMonth() {
        //TODO figure out if this is better defined at the start
        // int MainRoomLightCount = MainRoomLights.transform.childCount;
        // int HallLightCount = HallLights.transform.childCount;

        gameCanvas.transform.GetComponent<GraphicRaycaster>().blockingObjects = GraphicRaycaster.BlockingObjects.All;

        //Turn off tablet
        if (tabletOn == true) {
            SwitchTabletState(null, null);
        }
        //Close central UI

        if(centralUITopOpen == true) {
            SwitchCentralUIState(null, "top");
        }
        if(centralUIBottomOpen == true) {
            SwitchCentralUIState(null, "bottom");
        }

        creditUILight.SetActive(false);
        achieveUILight.SetActive(false);

        yield return new WaitForSeconds(1f);
        MainRoomLights.SetActive(false);
        DeskLights.SetActive(false);
        yield return new WaitForSeconds(1f);
        HallLights.SetActive(false);
        TopStatLight.SetActive(false);
        yield return new WaitForSeconds(1f);

        RemoveOldFollowUpInfo();

        hiddenGameVariables._gameFlowEventBus.Dequeue();
    }

    public void StartMonthAnim(Component sender, object data) {
        StartCoroutine(IStartMonth());
    }

    IEnumerator IStartMonth() {
        // int MainRoomLightCount = MainRoomLights.transform.childCount;
        // int HallLightCount = HallLights.transform.childCount;

        // for(int i = 0; i < HallLightCount; i++) {
        //     HallLights.transform.GetChild(i).gameObject.SetActive(true);

        //     if (i == 0) {
        //         TopStatLight.SetActive(true);
        //     }

        //     yield return new WaitForSeconds(0.35f);
        // }
        // for(int i = 0; i < MainRoomLightCount; i++) {
        //     MainRoomLights.transform.GetChild(i).gameObject.SetActive(true);

        //     if (i == 2) {
        //         DeskLights.transform.GetChild(1).gameObject.SetActive(true);
        //     } else if (i == 3) {
        //         DeskLights.transform.GetChild(0).gameObject.SetActive(true);
        //     }

        //     yield return new WaitForSeconds(0.35f);
        // }

        yield return new WaitForSeconds(1f);
        HallLights.SetActive(true);
        TopStatLight.SetActive(true);
        yield return new WaitForSeconds(1f);
        MainRoomLights.SetActive(true);
        DeskLights.SetActive(true);
        yield return new WaitForSeconds(1f);

        UpdateStatUI(null, null);
        SwitchTabletState(null, null);

        creditUILight.SetActive(true);
        achieveUILight.SetActive(true);

        hiddenGameVariables._gameFlowEventBus.Dequeue();
        
        ShowNewFollowUpInfo();

        gameCanvas.transform.GetComponent<GraphicRaycaster>().blockingObjects = GraphicRaycaster.BlockingObjects.None;
    }

    // IEnumerator INewProposal() {
    //     //fully implement proposal coming offscroon
    //     //check if extra info board is active and slide off too
    //     // LeanTween.moveY(proposalClipboard, -160f, 30f).setEase(LeanTweenType.easeInOutQuad).setDelay(1f);
    //     // yield return new WaitForSeconds(30f);
    //     yield return new WaitForSeconds(0.1f);
    //     //hiddenGameVariables._gameFlowEventBus.Dequeue();
    // }

    private void RemoveOldFollowUpInfo() {
        followUpInfoClipboard.SetActive(false);
        for(int i = 0; i < followUpInfoPrefabList.Count; i++) {
            Destroy(followUpInfoPrefabList[i]);
        }
    }

    private void ShowNewFollowUpInfo(){
        for(int i = 0; i < followUpInfoList._currentFollowUpInfo.Count; i++) {
            GameObject followUpInfoInstance = Instantiate(followUpInfoPrefab, followUpInfoContainer.transform) as GameObject;
            followUpInfoPrefabList.Add(followUpInfoInstance);

            TextMeshProUGUI prefabText = followUpInfoInstance.transform.GetComponent<TextMeshProUGUI>();

            GenericFollowUpInfo followUpInfo = followUpInfoList._followUpInfo[followUpInfoList._currentFollowUpInfo[i]];

            prefabText.text = followUpInfo.getInfo();
            
            followUpInfoInstance.SetActive(true);
        }
        
        if(followUpInfoList._currentFollowUpInfo.Count > 0) {
            followUpInfoClipboard.SetActive(true);
        }
        
        followUpInfoList._currentFollowUpInfo.Clear();
    }

    //====================================================================
    //                     TABLET STATUS SECTION                    |
    //====================================================================
    public void SwitchTabletState(Component sender, object data) {
        if(tabletOn == false) {
            StartCoroutine(ITabletOn());
            StartCoroutine(IDisplayLogo());
            tabletOn = true;
        } else if (tabletOn == true && (string)data != "Initial Activation") {
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
        while (scorpImage.color.a < 0.95f){//elapsedTime < duration) {
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
        if (data == "top") {
            if(centralUITopOpen == false) {
                StartCoroutine(IOpenTopCentralUI());
                centralUITopOpen = true;
            } else if (centralUITopOpen == true) {
                StartCoroutine(ICloseTopCentralUI());
                centralUITopOpen = false;
            }
        //}
        } else if (data == "bottom") {
            if(centralUIBottomOpen == false) {
                StartCoroutine(IOpenBottomCentralUI());
                centralUIBottomOpen = true;
            } else if (centralUIBottomOpen == true) {
                StartCoroutine(ICloseBottomCentralUI());
                centralUIBottomOpen = false;
            }
        }
    }

    IEnumerator IOpenTopCentralUI()
    {
        //Activate SCP section of details list
        SwitchDetails(null, "SCPs");

        achievementMask.GetComponent<Mask>().enabled = false;

        centralUIButtonCollider.enabled = false;
        centralUIButtonImageCollider.enabled = false;

        for (int i = 0; i < achievementsList._displayedAchievements.Count; i++) {
            achievementsList._displayedAchievements[i].SetActive(true);
        }
        
        yield return new WaitForSeconds(0.5f);

        creditUILight.SetActive(true);
        achieveUILight.SetActive(true);

        //LeanTween.moveY(CentralUIRT, 66.55f, 0.75f).setEase(LeanTweenType.easeInOutQuad);
        // As canvas scale is 0.00625 all actual movement values have to be multiplied by the scale for DOTween
        CentralUITopRT.DOMoveY(0.4159f, 0.75f).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(0.25f);

        //O5Elements.SetActive(false);
        achievementMask.GetComponent<Mask>().enabled = true;

        //LeanTween.moveY(SCPAchieveRT, 10007f, 0.75f).setEase(LeanTweenType.easeOutBounce).setDelay(0.5f);
        SCPAchieveRT.DOMoveY(0.807f, 0.75f).SetEase(Ease.OutBounce).SetDelay(0.5f);

        //Reset the total number of discovered details that the user hasnt checked out
        detailsList._newlyDiscoveredDetails = 0;
        uncheckedDetailsText.text = "";
        alertLight.SetActive(false);

        centralUIButtonCollider.enabled = true;
        centralUIButtonImageCollider.enabled = true;

    }

    public void UpdateAchievements(Component sender, object data) {
        int unlockedAchievement = (int)data;

        GameObject achievementPrefabToUpdate = achievementsList._displayedAchievements[unlockedAchievement];
        TextMeshProUGUI achievementText = achievementPrefabToUpdate.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        GenericAchievement achievementToUpdate = achievementsList._achievements[unlockedAchievement];

        achievementText.text = achievementToUpdate.getAchievementName() + achievementToUpdate.getAchievementDescription();
    }

    IEnumerator ICloseTopCentralUI()
    {

        centralUIButtonCollider.enabled = false;
        centralUIButtonImageCollider.enabled = false;

        //LeanTween.moveY(SCPAchieveRT, 3450f, 0.75f).setEase(LeanTweenType.easeOutQuad).setDelay(0.5f);
        SCPAchieveRT.DOMoveY(0.55f, 0.75f).SetEase(Ease.OutQuad);

        //test pos
        achievementMask.GetComponent<Mask>().enabled = false;

        yield return new WaitForSeconds(1f);

        //LeanTween.moveY(CentralUIRT, -17.5f, 1f).setEase(LeanTweenType.easeInOutQuad).setDelay(0.5f);
        CentralUITopRT.DOMoveY(-0.109375f, 0.75f).SetEase(Ease.InOutQuad);

        //yield return new WaitForSeconds(0.3f);

        // og pos achievementMask.GetComponent<Mask>().enabled = false;
        //O5Elements.SetActive(true);

        //Lets animation finish before removing lights
        yield return new WaitForSeconds(2f);

        creditUILight.SetActive(false);
        achieveUILight.SetActive(false);

        for (int i = 0; i < achievementsList._displayedAchievements.Count; i++) {
            achievementsList._displayedAchievements[i].SetActive(false);
        }

        centralUIButtonCollider.enabled = true;
        centralUIButtonImageCollider.enabled = true;
    }

    //====================================================================
    //              SITE MAP CONTROL AND COMMAND SECTION                 |
    //====================================================================

    IEnumerator IOpenBottomCentralUI()
    {
        Debug.Log("got here");
        //Can't change the sorting order of the bottom canvas (currently -1) as it will override the top canvas
        //This means the actual bottom UI has to be at -2, with the topcentralui hidden behind it at -3
        bottomUICanvas.sortingOrder = -2;
        CentralUITopCanvas.sortingOrder = -3;

        CentralUIBottomRT.DOMoveY(-0.109375f, 0.75f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.25f);

        // achievementMask.GetComponent<Mask>().enabled = true;

        SiteMapExtraRT.DOMoveX(-1.275f, 0.75f).SetEase(Ease.InCirc).SetDelay(0.5f);

        yield return new WaitForSeconds(1.5f);

        // After covering up the bottom desk, the order is reajusted so that a player can access the other central UI
        // If these values weren't changed, then the top central ui would be at -1, and be stuck behind the main top UI when opened.
        //TODO disable clicking the open central UI button whilst this happens, and make it obvious to the player that it's disabled.
        CentralUIBottomCanvas.sortingOrder = 3;
        CentralUITopCanvas.sortingOrder = 2;
    }

    IEnumerator ICloseBottomCentralUI()
    {
        yield return new WaitForSeconds(0.25f);

        //Set back to normal values. This allows the top menu to work properly.


        //TODO reset to normal positions



        bottomUICanvas.sortingOrder = 3;
        CentralUITopCanvas.sortingOrder = 1;


    }

    //====================================================================
    //                 SWITCHING SHOWN DETAILS SECTION                   |
    //====================================================================

    //Called by the switch details buttons and by proposal manager
    public void SwitchDetails(Component sender, object data) {
        List<int> infoToDisplay = new List<int>();

        for(int i = 0; i < detailsPrefabList.Count; i++) {
            Destroy(detailsPrefabList[i]);
        }
        detailsPrefabList.Clear();

        //If the central UI is open
        if(centralUITopOpen == true) {
            detailsList._newlyDiscoveredDetails = 0;
        } else if (centralUITopOpen == false && data == null) {
            //TODO probably a better way of coding this
            uncheckedDetailsText.text = "" + detailsList._newlyDiscoveredDetails;
            alertLight.SetActive(true);
        }

        //TODO change this to check the sender instead of using a null check to avoid confusion (and in the bit above)
        if (data == null) { //This statement is run if the code is called by Proposal handler, in which the data just needs to be updated, not switched
            data = activeDetailsContainer;
        }

        if ((String)data == "SCPs") {
            infoToDisplay = detailsList._discoveredSCPs;
            activeDetailsContainer = "SCPs";
        } else if ((String)data == "Tales") {
            infoToDisplay = detailsList._discoveredTales;
            activeDetailsContainer = "Tales";
        } else if ((String)data == "Canons") {
            infoToDisplay = detailsList._discoveredCanons;
            activeDetailsContainer = "Canons";
        } else if ((String)data == "Series") {
            infoToDisplay = detailsList._discoveredSeries;
            activeDetailsContainer = "Series";
        } else if ((String)data == "Groups") {
            infoToDisplay = detailsList._discoveredGroups;
            activeDetailsContainer = "Groups";
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
