using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SkyShifter : MonoBehaviour
{
    //A reference to the in-game Timer
    Timer ingameTimer;
    //The current color of the sky
    private Color skyColor;
    //Initializing method (sets the in-game timer variable.)
    public void Init(Timer _timer)
    {
        //Set the in-game timer variable
        ingameTimer = _timer;
    }

    // Update is called once per frame
    void Update()
    {
        //If the in-game timer is present...
        if (ingameTimer != null)
        {
            //Get the current color of the sky
            Color currentColor = this.gameObject.GetComponent<Light2D>().color;
            //Set the red value to a linearly interpolated value between 32/255 and 1 using the ratio of the current time on the in-game timer / the starting time
            float red = Mathf.Lerp(32f / 255f, 1, ingameTimer.GetCurrentTime() / GameManager.gm.so_Rep.timerStartingTime);
            //Set the value of green to red.
            float green = red;
            //Set the blue value to a linearly interpolated value between 159/255 and 1 using the ratio of the current time on the in-game timer / the starting time
            float blue = Mathf.Lerp(159f / 255f, 1, ingameTimer.GetCurrentTime() / GameManager.gm.so_Rep.timerStartingTime);
            //Change the color of the sky using the r(ed), g(reen) and b(lue) values above
            this.gameObject.GetComponent<Light2D>().color = new Color(red, green, blue);
            //Store the current color in skyColor
            skyColor = new Color(red, green, blue);
        }
        
    }
    //Gets the current color of the sky. Used in the "Rotator" script to set the background color of the in-game clock
    public Color GetSkyColor()
    {
        //Self explanatory
        return skyColor;
    }
    //Used by the DayTransition game event called to transition to a Day time after a certain cutscene
    public void DayTransition()
    {
        //Get the current color (is this necessary?)
        Color currentColor = this.gameObject.GetComponent<Light2D>().color;
        //Set the current color of the sky to "white"
        this.gameObject.GetComponent<Light2D>().color = new Color(1, 1, 1, 0); 

    }
}
