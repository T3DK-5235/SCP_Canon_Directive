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
    
    [SerializeField] GameObject proposalClipboard;

    [SerializeField] GameObject extraInfoClipboard;
    
    private TextMeshProUGUI proposalTitle;
    private TextMeshProUGUI proposalDesc;

    [SerializeField] TextMeshProUGUI extraInfoTitle;
    [SerializeField] TextMeshProUGUI extraInfoDesc;

    [SerializeField] GameObject genericInfoContainer;
    [SerializeField] GameObject personnelContainer;
    [SerializeField] GameObject personnelPrefab;
    List<GameObject> personnelPrefabList;

    [Header("UI Stat Bars")]

    [SerializeField] Slider totalMtfBar;
    [SerializeField] Slider availableMtfBar;

    [SerializeField] Slider totalResearcherBar;
    [SerializeField] Slider availableResearcherBar;

    private int currentPrefabNum = 0;

    private bool startUIFlashing;

    private bool flashStatBar = false;

    private IEnumerator activeBlinkTimer; 

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

            for(int i = 0; i < extraInfoNum; i++) {
                GameObject personnelPrefabInstance = Instantiate(personnelPrefab, personnelContainer.transform) as GameObject;
                
                TextMeshProUGUI prefabTitle = personnelPrefabInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI prefabDescription = personnelPrefabInstance.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

                prefabTitle.text = hiddenGameVariables._currentExtraInfo.getInfoTitle();
                prefabDescription.text = hiddenGameVariables._currentExtraInfo.getInfoDescription()[i];

                personnelPrefabList.Add(personnelPrefabInstance);

                personnelPrefabInstance.SetActive(false);
            }
        } else {
            extraInfoTitle.text = hiddenGameVariables._currentExtraInfo.getInfoTitle();
            extraInfoDesc.text = hiddenGameVariables._currentExtraInfo.getInfoDescription()[0];
        }

        //Set the first prefab to be active
        personnelPrefabList[currentPrefabNum].SetActive(true);
    }

    public void getNextPrefab(Component sender, object data) { 
        int prevPrefabNum = 0;

        // if the current prefab number is less than the possible total prefab number
        // as currentPrefabNum starts at 0, rather than 1, personnelPrefabList.Count has to be decreased as well
        if (currentPrefabNum < personnelPrefabList.Count - 1) {
            Debug.Log("Going up!" + currentPrefabNum);
            //Set prev prefab to inactive
            prevPrefabNum = currentPrefabNum;
            //Increase the current prefab num
            currentPrefabNum++;
            //Set next prefab to active (currentPrefabNum starts at 1, whilst lists start at 0)
            personnelPrefabList[currentPrefabNum].SetActive(true);
        } else {
            Debug.Log("Back to start");
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

    //TODO need to call this from proposal manager when stats change at the end of a proposal
    public void updateStatUI(Component sender, object data) 
    {
        //TODO change sliders for UI based on the data given

        //For every stat changed
        if(hiddenGameVariables._myStatCopy.__statsChanged.Count != 0) {
            for (int i = 0; i < hiddenGameVariables._myStatCopy.__statsChanged.Count; i++) {
                switch (hiddenGameVariables._myStatCopy.__statsChanged[i]) 
                {
                    case 0:
                        availableMtfBar.value = hiddenGameVariables._availableMTF;
                        break;
                    case 1:
                        break;
                    case 2:
                        availableResearcherBar.value = hiddenGameVariables._availableResearchers;
                        break;
                }
            }
        }
        
        startUIFlashing = false;
    }

    IEnumerator BlinkTimer()
    {
        //Used to flash between new and old value
        while (startUIFlashing) {
            if (flashStatBar == false) {
                availableMtfBar.value = hiddenGameVariables._myStatCopy.__availableMTF;
                availableResearcherBar.value = hiddenGameVariables._myStatCopy.__availableResearchers;

                yield return new WaitForSeconds(0.1f);
                flashStatBar = true;
            } else if (flashStatBar == true) {
                availableMtfBar.value = hiddenGameVariables._availableMTF;
                availableResearcherBar.value = hiddenGameVariables._availableResearchers;

                yield return new WaitForSeconds(0.1f);
                flashStatBar = false;
            }

            //Delay between flashes
            yield return new WaitForSeconds(0.5f);
        }
    }
}
