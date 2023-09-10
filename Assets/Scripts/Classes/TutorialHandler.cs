using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
//using UnityEngine.UIElements;

using TMPro;

public class TutorialHandler : MonoBehaviour
{
    [SerializeField] HiddenGameVariables hiddenGameVariables;

    [SerializeField] GameObject projectClipboardOverlay;

    [SerializeField] GameObject initialTabletScreen;
    [SerializeField] GameObject scorpLogo;

    [SerializeField] GameObject foundationStatScreen;

    [SerializeField] GameObject GoIStatScreen;

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
            tabletOn = true;
        }
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
        
        yield return new WaitForSeconds(0.4f);

        foundationStatScreen.SetActive(true);
        GoIStatScreen.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        //Resets the image back to being invisible
        temp = scorpImage.color;
        temp.a = 0f; //Time.deltaTime
        scorpImage.color = temp;
        scorpLogo.SetActive(false);
    }
}
