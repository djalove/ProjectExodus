using System.Collections.Generic;
using System.Collections;
using PixelCrushers.DialogueSystem;
using UnityEngine;
public class RandomEventTrigger : MonoBehaviour
{
	[SerializeField] DialogueSystemTrigger dialogueSystemTrigger;
	List<Conversation> eventConversations;
	public GameObject refugeePrefab;
	public int currentEventIndex = 0;
	public void TriggerRandomEvent()
	{
		// Get all possible event conversations **JOESH EDIT: Unnecessary. List of random event conversations are stored in so_rep now.
		//eventConversations = DialogueManager.masterDatabase.conversations.FindAll(conversation => conversation.Title.StartsWith("Event/"));
		// Gets a random index between 0 and the number of random events in-game.
		int randomIndex = Random.Range(0, GameManager.gm.so_Rep.eventConversationTitles.Count);
		//Checks to see if all of the random events have already been triggered in game. 
		if (GameManager.gm.usedIndices.Count == GameManager.gm.so_Rep.eventConversationTitles.Count)
		{
			//If so, clear the list. The player will now experience all of the random events again.
			GameManager.gm.usedIndices.Clear();
		}
		//If this is the first event the player experiences...
		if (GameManager.gm.usedIndices.Count == 0)
		{
			//If the index is 0 (FearsomeBeast which requires event 1, BeastWarning, be triggered first)
			while (randomIndex == 0)
			{
				//Pick another random index
				randomIndex = Random.Range(0, GameManager.gm.so_Rep.eventConversationTitles.Count);
			}
		}
		//Otherwise (if this is the player's 2nd or more random event...)
		else
		{
			//Make sure the same event won't play again and that event 0 doesn't happen before event 1 (if a previously experienced event comes up...)
			while (GameManager.gm.usedIndices.Contains(randomIndex) || (randomIndex == 0 && !GameManager.gm.usedIndices.Contains(1)))
			{
				//... pick another random event.
				randomIndex = Random.Range(0, GameManager.gm.so_Rep.eventConversationTitles.Count);				
			}			
		}
		//If the event isn't (0) Fearsome beast, (2) Pursuing bandits or (5) Friendly boar...
		if (randomIndex != 0 && randomIndex != 2 && randomIndex != 5)
		{
			//Spawn a refugee
			RefugeeBehavior refugeeBehavior = Instantiate(refugeePrefab, GameManager.gm.travelContainer.gameObject.transform).GetComponent<RefugeeBehavior>();
			//Initialize the refugee
			refugeeBehavior.Init(new Vector3(-3f, 2f, 0f), randomIndex);
			//If the event is "Wounded Couple," instantiate another refugee shortly after the first.
			if (randomIndex == 14) { StartCoroutine(DelayedInstantiation()); }
		}
		//Set the pseudorandomly selected event to be triggered
		dialogueSystemTrigger.conversation = "Event/" + GameManager.gm.so_Rep.eventConversationTitles[randomIndex];
		currentEventIndex = randomIndex;

		//JOESH EDIT: Jay's old code. Keeping it in case I need to revert the changes..
		//dialogueSystemTrigger.conversation = eventConversations[Random.Range(0, eventConversations.Count)].Title;

		Debug.Log($"<color=cyan>{dialogueSystemTrigger.conversation} conversation chosen</color>");		
		//Add the index to the list of experienced events
		GameManager.gm.usedIndices.Add(randomIndex);
		Debug.Log("Used Indices Count: " + GameManager.gm.usedIndices.Count);
		// Trigger chosen conversation
		dialogueSystemTrigger.OnUse();
	}
	//Instantiates another refugee
	IEnumerator DelayedInstantiation()
	{
		//Wait half a second
		yield return new WaitForSeconds(0.5f);
		//Spawn a refugee
		RefugeeBehavior refugeeBehavior = Instantiate(refugeePrefab, GameManager.gm.travelContainer.gameObject.transform).GetComponent<RefugeeBehavior>();
		//Initialize the refugee (their destination is a little farther back from the first and their refugee id is 1)
		refugeeBehavior.Init(new Vector3(-5f, 2f, 0f), 14, 1);
	}
	//Method triggered by "Spawn Refugee" event. Used for refugees in story events (folksy refugee, bandit and dying refugee)
	public void SpawnRefugee(int _refugeeID)
	{
		//Spawn a refugee
		RefugeeBehavior refugeeBehavior = Instantiate(refugeePrefab, GameManager.gm.travelContainer.gameObject.transform).GetComponent<RefugeeBehavior>();
		//Initialize the refugee. If the id is 5, set the conversation id and refugee id to 12 and 2 respectively (done so proper NPC is spawned. Specifically a bandit for the right path and a male refugee for the other two story events)
		refugeeBehavior.Init(new Vector3(-3f, 2f, 0f), _refugeeID == 5 ? 12 : _refugeeID, _refugeeID == 5 ? 2 : _refugeeID);
	}
}
