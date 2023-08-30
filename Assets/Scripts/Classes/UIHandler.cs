using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
//using UnityEngine.UIElements;

using TMPro;

public class UIHandler : MonoBehaviour
{
    [SerializeField] HiddenGameVariables hiddenGameVariables;

    [SerializeField] GameObject projectClipboardOverlay;

    //Maybe instead try make a UI scriptable object
    
    [SerializeField] GameObject proposalClipboard;

    [SerializeField] GameObject initialTabletScreen;
    [SerializeField] GameObject scorpLogo;
    
    private TextMeshProUGUI proposalTitle;
    private TextMeshProUGUI proposalDesc;

    //TODO utilize events to fire the below actions instead of update?
    //TODO update background of window 
    //TODO add screen overlays

    void Update() {
        int currentID = hiddenGameVariables._currentProposal.getProposalID();

        //hardcoded at only proposal 0 for initial experimentation
        if (currentID == 0){
            ProjectClipboardOverlay();
        } else {
            projectClipboardOverlay.SetActive(false);
        }
    }

    void Start() {
        // Maybe put in try catch
        // get the text from the proposal UI object (And cache it to prevent unneeded GetComponent calls)
        proposalTitle = proposalClipboard.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        proposalDesc = proposalClipboard.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        // and set it to the current proposal's description
        proposalTitle.text = hiddenGameVariables._currentProposal.getProposalTitle();
        proposalDesc.text = hiddenGameVariables._currentProposal.getProposalDescription();

        StartCoroutine(TabletOnCoroutine());
    }

    public void updateProposal(Component sender, object data) {


        //TODO Add animation or movement here of the proposal
        proposalTitle.text = hiddenGameVariables._currentProposal.getProposalTitle();
        proposalDesc.text = hiddenGameVariables._currentProposal.getProposalDescription();
    }

    private void ProjectClipboardOverlay() {
        projectClipboardOverlay.SetActive(true);
    }


    IEnumerator TabletOnCoroutine()
    {
        
        //TODO Lerp opacity of tablet main section
        //TODO Lerp opacity of logo
        //TODO disable both and display stat screen
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(3);

        Image initialTabletImage = initialTabletScreen.GetComponent<Image>();

        // Color c = image.color;
        // c.a = 0;
        // initialTabletImage.color = c;
        Color temp = initialTabletImage.color;

        while (temp.a != 1) {
            Debug.Log(temp);
            temp = initialTabletImage.color;
            temp.a = Mathf.Lerp(temp.a, 1, Time.deltaTime);
            initialTabletImage.color = temp;
        }

    }
}
