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

    [SerializeField] GameObject foundationStatScreen;

    [SerializeField] GameObject GoIStatScreen;
    
    private TextMeshProUGUI proposalTitle;
    private TextMeshProUGUI proposalDesc;

    private bool tabletOn = false;

    //TODO utilize events to fire the below actions instead of update?
    //TODO update background of window 
    //TODO add screen overlays

    void Update() {
        int currentID = hiddenGameVariables._currentProposal.getProposalID();

        //hardcoded at only proposal 0 for initial experimentation
        if (currentID == 0){
            ProjectClipboardOverlay();
        } else if (currentID == 1){
            projectClipboardOverlay.SetActive(false);
            TurnOnTablet();
        }
    }

    private void TurnOnTablet() {
        if(tabletOn == false) {
            StartCoroutine(ITabletOn());
            StartCoroutine(IDisplayLogo());
            //TODO disable both and display stat screen
            Debug.Log("Done When?");
            tabletOn = true;
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
    }

    public void updateProposal(Component sender, object data) {


        //TODO Add animation or movement here of the proposal
        proposalTitle.text = hiddenGameVariables._currentProposal.getProposalTitle();
        proposalDesc.text = hiddenGameVariables._currentProposal.getProposalDescription();
    }

    private void ProjectClipboardOverlay() {
        projectClipboardOverlay.SetActive(true);
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

        Debug.Log("Done Tablet");

        //Syncs this co-routine with the other co-routine below
        yield return new WaitForSeconds(1f);

        temp = initialTabletImage.color;
        temp.a = 0f; //Time.deltaTime
        initialTabletImage.color = temp;
        initialTabletScreen.SetActive(false);
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
        
        activateFoundationStatPage();

        yield return new WaitForSeconds(0.5f);

        //Resets the image back to being invisible
        temp = scorpImage.color;
        temp.a = 0f; //Time.deltaTime
        scorpImage.color = temp;
        scorpLogo.SetActive(false);
    }

    private void activateFoundationStatPage() {
        foundationStatScreen.SetActive(true);
        GoIStatScreen.SetActive(false);
    }

    private void activateGoIStatPage() {
        GoIStatScreen.SetActive(true);
        foundationStatScreen.SetActive(false);
    }
}
