using UnityEngine;
using PixelCrushers.DialogueSystem;

public class OpeningLetter : MonoBehaviour
{
    //The text of the letter.
    private string letterText;
    //The object for the flashing text telling the player to click to continue
    public GameObject clickToContinue;
    
    void Awake()
    {
        //Set the text of the letter. **NOTE: This is the initial draft of the letter
        //letterText = "My dearest daughter, Alisa,\n\nBy the time you read this letter, I'll be dead. With the demons running loose on the continent, it's only a matter of time before they start searching for " + DialogueLua.GetVariable("playerName").AsString + ". I can't let that happen. I swore an oath to protect " + DialogueLua.GetVariable("playerPronounDO").AsString + " no matter the cost. I expect to sacrifice my life to honor that oath.\n\nBy now you must have realized " + DialogueLua.GetVariable("playerName").AsString + " is no mere human. Demon blood courses through " + DialogueLua.GetVariable("playerPronounPoss").AsString + " veins. Even so, I took " + DialogueLua.GetVariable("playerPronounDO").AsString + " in and raised " + DialogueLua.GetVariable("playerPronounDO").AsString + " as one of us, as your sibling, as a normal human being. I kept the truth of " + DialogueLua.GetVariable("playerPronounPoss").AsString + " demon lineage from " + DialogueLua.GetVariable("playerPronounDO").AsString + " in the hopes that it would keep " + DialogueLua.GetVariable("playerPronounDO").AsString + " out of the Demon Lord's reach. I should have known that I was just delaying the inevitable.\n\nIt's selfish, but I must ask that you carry on protecting " + DialogueLua.GetVariable("playerName").AsString + " for me. There is a ship leaving from the nearby city, sailing to another continent, one where the Demon Lord shouldn't be able to follow. Find " + DialogueLua.GetVariable("playerName").AsString + " and flee to the coast as quickly as you can. You don't have much time before the Demon Lord and her army fully seize control of the land.\n\nPlease Alisa, protect " + DialogueLua.GetVariable("playerName").AsString + ".\nFather";

        //Set the text of the letter **NOTE: This is what is used in the current build.
        letterText = "My dearest daughter, Alisa,\n\n The Demon Lord is coming for " + DialogueLua.GetVariable("playerName").AsString + ". By now, the lesser demons will have breached the rift. Soon, greater demons will arrive. There's nothing more I can do for " + DialogueLua.GetVariable("playerName").AsString + " now.  It's up to you to help " + DialogueLua.GetVariable("playerPronounDO").AsString + ". "  + DialogueLua.GetVariable("playerName").AsString + " is family, regardless of whether " + DialogueLua.GetVariable("playerPronounPoss").AsString + " parents are humans or demons. Get "  + DialogueLua.GetVariable("playerPronounDO").AsString + " off of this continent and far from the clutches of the Demon Lord. \n\nPlease Alisa, protect " + DialogueLua.GetVariable("playerName").AsString + ".\nFather";
        //Start typing the letter.
        this.GetComponent<TextMeshProTypewriterEffect>().StartTyping(letterText);
    }

    public void SkipTyping()
    {
        //If the letter is still being typed...
        if (this.GetComponent<TextMeshProTypewriterEffect>().isPlaying)
        {
            //Skip to the end.
            this.GetComponent<TextMeshProTypewriterEffect>().StartTyping(letterText, letterText.Length);
            Destroy(clickToContinue);
        }
        
    }

    void Update()
    {
        //If the letter has finished typing...
        if (!this.GetComponent<TextMeshProTypewriterEffect>().isPlaying)
        {
            //Destroy the "Click to Continue" prompt.
            Destroy(clickToContinue);
        }
    }
}
