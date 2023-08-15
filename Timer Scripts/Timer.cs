using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class Timer : MonoBehaviour
{
    //What the timer is initally set to
    public float startingTime;
    //The timer's current value 
    private float currentTime;
    //A flag indicating whether the timer is running/counting down or not
    private bool isTimerRunning;
    //The value that is decremented from the timer's current time. Used to control how quickly the timer counts down
    private float decrementor;
    //The text UI Object for the time on the debug timer display
    public TMP_Text displayedTime;
	//Initialize the timer. Ran every time the scene is changed. 
	public void Init(float _startingTime)
    {
        //Sets the starting tiime to whatever is passed in
        startingTime = _startingTime;
        //Sets the current time to the above starting time
        currentTime = startingTime;
        //The initial value for the variable responsible for dictating how fast the timer moves
        decrementor = 1;
        //Start the timer.
        StartTimer();
    }

    //Run the timer. 
    //**ERROR**: Either the timer stops at 1:31 or it stops when an event is triggered (which may or may not be a problem if we keep events)
    public IEnumerator RunTimer()
    {
        //The timer is only running if "isTimerRunning" is true
        while (isTimerRunning)
        {
            //Stops the timer once current time reaches 0
            if (currentTime <= 0)
            {
                //Sets currentTime to zero
                currentTime = 0;
                //Stops the timer
                StopTimer();
                //Sets the next battle to the unwinnable fight
                GameManager.gm.map.encounterManager.nextBattleType = eBattleType.youLose;
                //Loads the unwinnable fight
                GameManager.gm.sceneLoader.LoadCombat();
            }
            //Wait for the amount of seconds decrementor is set to (1 by default; 2 when choosing actions in combat)       
            yield return new WaitForSeconds(decrementor);
            //Decrements currentTime by 1 (signaling that one "second" has elapased on the timer)
            currentTime -= 1;
        }
        

    }
    //Turn the timer on (triggered by StartTimer GameEvent)
    public void StartTimer()
    {
        if (GameManager.gm.sceneLoader.currentScene != eScene.combat)
        {
            isTimerRunning = true;
            StartCoroutine(RunTimer());
        }
        
        
    }
    //Turn the timer off (I know I can use a single method for on and off but I figure it's best to divide them for clarity's sake) 
    public void StopTimer()
    {
        isTimerRunning = false;
        StopCoroutine(RunTimer());
        
    }
    //Reset the timer so that it's back to its starting time. **NEVER USED: Is it necessary? Plus "startingTime" changes when the scene changes. Should it use gm.so_Rep.timerStartingTime instead?
    public void ResetTimer()
    {
        currentTime = startingTime;
    }
    //Set the decrementor (used in combat to slow the countdown during the command input phase)
    public void SetDecrementor(float _val)
    {
        decrementor = _val; 
    }
    //Gets the currentTime. Used to store the currentTime in gm.sceneLoader.remainingTime so it can be tracked across scenes.
    public float GetCurrentTime()
    {
        return currentTime;
    }
    //Adjust the value of current time (used in combat to decrement 2 seconds during each execution phase)
    public void AdjustCurrentTime(float _adjustmentValue)
    {
        currentTime += _adjustmentValue;
    }
    //Check if the timer is currently running
    public bool GetIsTimerRunning()
    {
        return isTimerRunning;
    }
}
