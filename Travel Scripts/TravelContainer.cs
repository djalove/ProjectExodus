using System.Collections;
using UnityEngine;
// Incredibly subject to change; all this script basically does right now is stop and start animations depending on if the party's moving or not
public class TravelContainer : MonoBehaviour
{
    //JAY'S OLD CODE: Held reference to the things in the Travel scene that moved?
    public BackgroundMove[] sceneryTiles;
    //A reference to the game manager. Used to manipulate party member's energy stat
    public GameManager gm;

    public BackgroundHelper backgroundHelper;
    //Used to monitor the speed at which the party is traveling. 0 = stop; 1 = normal speed; 2 = fast speed
    private int traversalSpeed;


    //Ran in SceneLoader when switching to Travel Scene;
    public void Init(GameManager _gm)
    {
        gm = _gm;

    }
   ////JAY'S OLD CODE: Still needed?
    public void GetAllTiles()
    {
        sceneryTiles = GetComponentsInChildren<BackgroundMove>();
    }
    public void OnNormalTraversal()
    {
        //Sets traversal speed to 1; Used to break out of energy depletion/replenishment loops
        traversalSpeed = 1;

        //JAY'S OLD CODE: No longer being used. 
        /* foreach (BackgroundMove anim in sceneryTiles)
        {
            anim.SetSpeed(anim.startSpeed);
        } */
    }
    public void OnStopTraversal()
    {
        //Sets traversal speed to 0; The party has stopped.
        traversalSpeed = 0;
        //Replenishes each party member's energy
        foreach (PartyMember partyMember in gm.partyMembers)
        {
            StartCoroutine(RegainEnergy(partyMember));
        }
        //JAY'S OLD CODE: No longer being used. 
        /* foreach (BackgroundMove anim in sceneryTiles)
        {
            anim.SetSpeed(anim.stopValue);
        } */
    }

    public void OnFastTraversal()
    {
        //Sets traversal speed to 2; The party is running
        traversalSpeed = 2;
        //Depletes each party member's energy
        foreach (PartyMember partyMember in gm.partyMembers)
        {
            StartCoroutine(DepleteEnergy(partyMember));
        }
        
        //JAY'S OLD CODE: No longer being used. 
        /* foreach (BackgroundMove anim in sceneryTiles)
        {
            float fastSpeed = anim.startSpeed * 4;
            anim.SetSpeed(fastSpeed);
        } */
    }

    //Coroutine that depletes the party's energy overtime as they run
    public IEnumerator DepleteEnergy(PartyMember _pm)
    {
        //The counter that is incremented below and used to deplete each party member's energy
        float energyModifier = 0;
        //The loop that keeps energy depleting while the party is running and the scene is still "Travel." **DOESN'T TAKE INTO ACCOUNT EVENTS THAT OCCUR DURING TRAVERSAL**
        while (traversalSpeed == 2 && gm.sceneLoader.currentScene == eScene.travel)
        {
            //Increment energyModifier such that it takes 1/2 a second to deplete 1 Energy.
            energyModifier += 4 * Time.deltaTime;
            //Only deplete energy when energyModifer is 1
            if (Mathf.FloorToInt(energyModifier) == 1)
            {
                //Deplete the energy (I'm trusting Jay's code will keep energy from being less than than 0)
                _pm.GetComponent<UnitStats>().Energy -= 1;
                //Set energyModifier back to 0
                energyModifier = 0;
            }
            //Suspend the coroutine until the next frame 
            yield return new WaitForFixedUpdate();
        }
        
    }

    //Coroutine that replenishes the party's energy overtime while they are stopped
    public IEnumerator RegainEnergy(PartyMember _pm)
    {
        //The counter that is incremented below and used to deplete each party member's energy
        float energyModifier = 0;
        //Another counter that controls how quickly energy is replenished
        float incrementor = 0.5f;

        //The loop that keeps energy depleting while the party is running, no conversations are active and the scene is still "Travel." **DOESN'T TAKE INTO ACCOUNT EVENTS THAT OCCUR DURING TRAVERSAL**
        while (traversalSpeed == 0 && gm.sceneLoader.currentScene == eScene.travel && !PixelCrushers.DialogueSystem.DialogueManager.instance.isConversationActive)
        {
            //Increment energyModifier such that it takes a steadily faster rate to deplete 1 Energy. (Starts at 0.5, then 1, then 1.5 ... to 3)
            energyModifier += incrementor * Time.deltaTime;
            //Once the energyModifier reaches 1...
            if (Mathf.FloorToInt(energyModifier) == 1)
            {
                //Replenish energy (I'm trusting Jay's code will keep energy from being greater than maxEnergy)
                _pm.GetComponent<UnitStats>().Energy += 1;
                //Reset energy modifier to 0
                energyModifier = 0;
                //Increase incrementor by .5 until it reaches 3.
                incrementor = Mathf.Min(3f, incrementor + 0.5f);
            }
            _pm.GetComponentInChildren<Animator>().SetBool("hasLowEnergyOrHealth", GameManager.gm.IsEnergyOrHealthLow(_pm));
            //Suspend the coroutine until the next frame 
            yield return new WaitForFixedUpdate();
        }   
    }
}
