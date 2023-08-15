using PixelCrushers.DialogueSystem;
using System.Collections;
using UnityEngine;

public class TravelTutorial : MonoBehaviour
{
    //Each line of the tutorial
    private string[] tutorialLines = new string[7];
    private int currentIndex;
    // Start is called before the first frame update
    void Start()
    {
        //Initialize index value
        currentIndex = 0;
        //Set each line of the tutorial
        tutorialLines[0] = "Left click the boot icon in the lower right hand corner to make the party walk forward.";
        tutorialLines[1] = "Left clicking the boot while walking will make the party run.";
        tutorialLines[2] = "BE AWARE: While running, you will gradually lose energy. Left click the boot while running to return to idle.";
        tutorialLines[3] = "While idle, the party will gradually recover energy.";
        tutorialLines[4] = "You can also right click the boot to loop in the opposite direction (idle -> run -> walk -> idle.)";
        tutorialLines[5] = "PAY ATTENTION TO THE CLOCK! If you don't reach camp before the moon reaches its apex, you will be doomed!";
        tutorialLines[6] = "Objective: Reach the Demonologist Camp before nightfall.";
        //Start typing the first line in the tutorial.
        this.GetComponent<TextMeshProTypewriterEffect>().StartTyping(tutorialLines[currentIndex]);
    }

    //Method triggered by "Normal Traversal" event. The Event itself is triggered when the player left clicks the boot while idle in order to walk.
    public void OnNormalTraversal()
    {
        //If the player has just started the tutorial...
        if (currentIndex == 0)
        {   
            //Advance to the next line of the tutorial
            currentIndex++;
            //Start typing the next line of the tutorial
            this.GetComponent<TextMeshProTypewriterEffect>().StartTyping(tutorialLines[currentIndex]);
        }
        //Otherwise, if the player triggers "Normal Event" while running...
        else if (currentIndex == 2)
        {
            //Skip directly to line five
            currentIndex = 5;
            //Start typing line five
            this.GetComponent<TextMeshProTypewriterEffect>().StartTyping(tutorialLines[currentIndex]);
            //Automatically advance the lines from this point onwards
            StartCoroutine(PlayFinalMessages());
        }
    }
    //Method triggered by "Fast Traversal" event. The Event itself is triggered when the player left clicks the boot while walking in order to run.
    public void OnFastTraversal()
    {
        //If triggered while walking...
        if (currentIndex == 1)
        {
            //Advance to the next line of the tutorial
            currentIndex++;
            //Start typing the next line of the tutorial
            this.GetComponent<TextMeshProTypewriterEffect>().StartTyping(tutorialLines[currentIndex]);
        }
        //Otherwise, if triggered at the start of the tutorial (while idle)
        else if (currentIndex == 0)
        {
            //Skip directly to line two
            currentIndex = 2;
            //Start typing line two
            this.GetComponent<TextMeshProTypewriterEffect>().StartTyping(tutorialLines[currentIndex]);
        }
    }
    //Method triggered by "Stop Traversal" event. The Event itself is triggered when the player left clicks the boot while running in order to stop.
    public void OnStopTraversal()
    {
        //If triggered while running...
        if (currentIndex == 2)
        {
            //Advance to the next line of the tutorial
            currentIndex++;
            //Start typing the next line of the tutorial
            this.GetComponent<TextMeshProTypewriterEffect>().StartTyping(tutorialLines[currentIndex]);
            //Automatically advance the lines from this point onwards
            StartCoroutine(PlayFinalMessages());
        }
    }
    //Method that automatically advances the lines of the tutorial
    IEnumerator PlayFinalMessages()
    {
        //While the message is being typed...
        while (this.GetComponent<TextMeshProTypewriterEffect>().IsPlaying)
        {
            //Wait for the next frame
            yield return new WaitForEndOfFrame();
        }
        //Wait 3 seconds after the message is typed (so the player has time to read the message)        
        yield return new WaitForSeconds(3);
        //Advance to the next line
        currentIndex++;
        //If there are more lines to display
        if (currentIndex < tutorialLines.Length)
        {
            //Start typing the next line
            this.GetComponent<TextMeshProTypewriterEffect>().StartTyping(tutorialLines[currentIndex]);
            //Recursively call this subroutine to automatically advance to the next message
            StartCoroutine(PlayFinalMessages());
        }
        //Otherwise (if there are no more lines to display)
        else 
        {
            //Turn on random combat/non-combat encounters
            GameManager.gm.map.encounterManager.encountersOn = true;
            //Destroy the tutorial object 
            Destroy(this.transform.parent.gameObject); 
        }
    }
    //Ran every frame
    void Update()
    {
        //If the player has passed the initial stretch of the journey (where the tutorial is displayed)...
        if (!(GameManager.gm.map.CurrentSegmentIndex == 0 && GameManager.gm.map.GetNextNodeIndex() <= 2))
        {
            //Turn on random combat/non-combat encounters
            GameManager.gm.map.encounterManager.encountersOn = true;
            //Destroy the tutorial object 
            Destroy(this.transform.parent.gameObject);
        }
    }

}
