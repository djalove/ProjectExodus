using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using TMPro;

public class EnterNameWidget : MonoBehaviour
{
    //The pronoun dropdown menu
    public TMP_Dropdown pronoun;
    //The text input field that the player types their name into
    public TMP_InputField playerName;
    //The text label that displays an error when the player doesn't have any text in the input field put hits enter
    public TMP_Text errorField; 
    //The event that is triggered when the Start Game button is clicked (StartOpeningLetter)
    public GameEvent startGame;
    //The event responsible for setting the PC's name in combat
    public GameEvent setPCName;

    public void OnStartGameClicked()
    {
        //If there is text in the playerName input field...
        if (playerName.text != "")
        {
            //Make sure the error message is no longer displayed
            errorField.gameObject.SetActive(false);
            //Set the playerName variable in the dialogue manager.
            DialogueLua.SetVariable("playerName", playerName.text);
            //Trigger the event setting the PC's name in combat
            setPCName.TriggerEvent();
            //Set the player Actor's name to playerName
            DialogueManager.ChangeActorName("Player", playerName.text);
            //Based on the value of the dropdown (0 = he/him; 1 = she/her; 2 = they/them)
            switch (pronoun.value)
            {
                //Assign the variables for masculine pronouns.
                case 0:
                    DialogueLua.SetVariable("playerNoun", "does");
                    DialogueLua.SetVariable("playerNouns", "is");
                    DialogueLua.SetVariable("playerVerb", "was");
                    DialogueLua.SetVariable("playerPronoun", "he");
                    DialogueLua.SetVariable("playerPronounDO", "him");
                    DialogueLua.SetVariable("playerPronounDO2", "himself");
                    DialogueLua.SetVariable("playerPronounPoss", "his");
                    DialogueLua.SetVariable("playerPronounPossPlural", "his");
                    break;
                //Assign the variables for feminine pronouns.
                case 1:
                    DialogueLua.SetVariable("playerNoun", "does");
                    DialogueLua.SetVariable("playerNouns", "is");
                    DialogueLua.SetVariable("playerVerb", "was");
                    DialogueLua.SetVariable("playerPronoun", "she");
                    DialogueLua.SetVariable("playerPronounDO", "her");
                    DialogueLua.SetVariable("playerPronounDO2", "herself");
                    DialogueLua.SetVariable("playerPronounPoss", "her");
                    DialogueLua.SetVariable("playerPronounPossPlural", "hers");
                    break;
                //Assign the variables for non-binary pronouns.
                case 2:
                    DialogueLua.SetVariable("playerNoun", "do");
                    DialogueLua.SetVariable("playerNouns", "are");
                    DialogueLua.SetVariable("playerVerb", "were");
                    DialogueLua.SetVariable("playerPronoun", "they");
                    DialogueLua.SetVariable("playerPronounDO", "them");
                    DialogueLua.SetVariable("playerPronounDO2", "themselves");
                    DialogueLua.SetVariable("playerPronounPoss", "their");
                    DialogueLua.SetVariable("playerPronounPossPlural", "theirs");
                    break;

                default:
                    Debug.Log("Couldn't pull pronoun from dropdown.");
                    break;
            }
            //Trigger the event and go to the OpeningLetter Scene
            startGame.TriggerEvent();
            playerName.text = "";
            pronoun.value = 0;
            //Stop displaying this window.
            this.gameObject.SetActive(false);
        }
        else
        {
            //Display an error
            errorField.gameObject.SetActive(true);            
        }
        
    }
    //Method triggered when "Cancel" is clicked
    public void OnCancelClick()
    {
        //Make the name an empty string
        playerName.text = "";
        //Return the value of the pronoun dropdown to its default
        pronoun.value = 0;
    }
}
