using System.Collections;
using UnityEngine;

public class RefugeeBehavior : MonoBehaviour
{
    //An array holding all the refugee prefabs that are can be instantiated
    public GameObject[] refugees;
    //How quickly the refugee movves
    private float moveSpeed;
    //The refugee's starting position
    private Vector3 startingPosition;
    //The id of the refugee (used to determine their gender or whether or not they are a bandit)
    private int refugeeID;
    //The id of the conversation. (used to determine whether the refugee is using weak animations or not)
    private int conversationID;
    //The initialization method. Arguments are: 1) Destination, 2) Conversation ID and 3) Refugee ID (Defaults to 0)
    public void Init( Vector3 _dest, int _cID, int rID = 0)
    {
        //Set the conversation ID
        conversationID = _cID;
        //Set the refugee ID
        refugeeID = rID;
        //Set a spawn index. Used to pull a prefab from the refugees array
        int refugeeSpawnIndex = 0;
        //Based on the refugee ID, pick...
        switch (refugeeID)
        {
            //Any random refugee
            case 0:
                refugeeSpawnIndex = Random.Range(0, refugees.Length - 1);
                break;
            //A random female refugee
            case 1:
                refugeeSpawnIndex = Random.Range(0, 6);
                break;
            //A random male refugee (used for folksy refugee and dying refugee events)
            case 2:
                refugeeSpawnIndex = Random.Range(6, refugees.Length - 1);
                break;
            //A bandit
            case 3:
                refugeeSpawnIndex = refugees.Length - 1;
                break;
            //Any other value should return an error
            default:
                Debug.LogError("Refugee ID is invalid");
                break;
        }
        //Instantiate a refugee using the spawn index set in the switch statement above.
        Instantiate(refugees[refugeeSpawnIndex], transform);
        //If the conversation is in the "Wounded" set... 
        if (conversationID > 11 && conversationID < 16)
        {
            //...then make the refugee use their weak animations
            this.GetComponentInChildren<Animator>().SetBool("isWeak", true);
        }
        //Set the starting position of the refugee
        startingPosition = this.transform.position;
        //Set the movespeed
        moveSpeed = 0.5f;
        //Start the coroutine that moves the refugee
        StartCoroutine(MoveIntoFrame(_dest));

    }
    //Method to move the refugee. Argument is for the destination
    IEnumerator MoveIntoFrame(Vector3 _destination)
    {
        //Initialize the variable that calculates how far the refugee has moved
        float distanceMoved = 0;
        //Set the destination of the refugee
        Vector3 destination = _destination; //new Vector3(-3f, 2f, 0f);
        //While the refugee is not at the destination...
        while (this.transform.position != destination)
        {
            //Increment the value of the variable that calculates how far the refugee moves
            distanceMoved += moveSpeed * Time.deltaTime;
            //Move the refugee using linear interpolation between the starting position and destination
            this.transform.position = Vector3.Lerp(startingPosition, destination, distanceMoved);
            //Wait for the next frame
            yield return new WaitForFixedUpdate();
        }
        //Save the position of the refugee at the destination (done to prevent the refugee from sliding into position when changing from walking to idle animation)        
        Vector3 childPosition = this.GetComponentInChildren<Animator>().gameObject.transform.position;
        //Change to the idle animation.
        this.GetComponentInChildren<Animator>().SetBool("isWalking", false);
        //Set the position of the refugee to their last position before switching to idle.
        this.GetComponentInChildren<Animator>().gameObject.transform.position = childPosition;
        //Enable the component that makes the refugee move by themselves once the conversation ends.
        this.GetComponent<BackgroundMove>().enabled = true;
    }
    //Method triggered by "Start Walking" event (which is itself triggered at the end of all conversations)
    public void OnStartWalking()
    {
        //If the refugee's speed while the player is stopped is set to 0...
        if (this.GetComponent<BackgroundMove>().stopValue == 0)
        {
            //Set the speed while the player is stopped to 6 (so that the refugee moves even while the player has stopped)
            this.GetComponent<BackgroundMove>().stopValue = 6;
            //Transition to the refugee's walking animation
            this.GetComponentInChildren<Animator>().SetBool("isWalking", true);
            //Trigger the player stopped event (so that the refugee can start moving)
            this.GetComponent<BackgroundMove>().StopSpeed();
        }
        
    }
    //Method triggered by "On Recover from Weak" event. Event triggered when the player chooses to heal a refugee
    public void OnRecoverFromWeak()
    {
        //If the refugee ID is equal to 0 (they are the default refugee)
        if (refugeeID == 0)
        {
            //Transition to non-weak animations
            this.GetComponentInChildren<Animator>().SetBool("isWeak", false);
        }
    }
    //Method triggered by "On Partner Recover from Weak" event. Event triggered when the player chooses to heal the partner of a refugee for an event
    public void OnPartnerRecoverFromWeak()
    {
        //If the refugee ID is equal to 1 (they are the partner refugee in a specific event)
        if (refugeeID == 1)
        {
            //Transition to non-weak animations
            this.GetComponentInChildren<Animator>().SetBool("isWeak", false);
        }
    }
    //Method triggered by "On Destroy Child" event. Event triggered when a refugee is killed
    public void OnDestroyChild()
    {
        //If the refugee ID is equal to 0 (they are the default refugee)
        if (refugeeID == 0)
        {
            //Kill the refugee
            Destroy(this.gameObject);
        }
    }

    //Method triggered by "On Destroy Child" event. Event triggered when a refugee in a specific event is killed
    public void OnDestroyOtherChild()
    {
        //If the refugee ID is equal to 1 (they are the partner refugee in a specific event)
        if (refugeeID != 0)
        {
            //Kill the refugee
            Destroy(this.gameObject);
        }
    }
}
