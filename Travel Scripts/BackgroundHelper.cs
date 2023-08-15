using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundHelper : MonoBehaviour
{
    //A list of cloud sprites to cycle through at random
    public List<Sprite> clouds;
    //A list of backgroundMountain sprites. 0 = plains; 1 = mountains
    public List<Sprite> backgroundMountains;
    //A list of bush sprites. May not need to be separate from props. Currently unused.
    public List<Sprite> bushes;
    //A list of environmental props (tall grass, rocks, etc.)
    public List<Sprite> props;
    //A list of grass that is offset (to make it look more natural)
    public List<Sprite> offsetGrass;
    //A list of normal tree sprites. 0 and 1 = oaks; 2 = pine
    public List<Sprite> trees;
    //A list of apple trees
    public List<Sprite> appleTrees;
    //A list of empty sprites. Used to clear a tile when there are no trees/props in the background.
    public List<Sprite> emptySprites;
    //The prefab for the sign (spawned at the end of the first segment)
    public GameObject signPrefab;
    //The prefab for the farm house (spawned near the end of segment 2)
    public GameObject farmHousePrefab;
    //The prefab for the demonologist camp (spawned at the end of the journey)
    public GameObject demonologistCampPrefab;
    //The object that holds all the props that are spawned
    private GameObject storyProp;

    //Initializing method
    public void Init()
    {
        //Searches for any story props that might already exist.
        GameObject go = transform.parent.gameObject.GetComponentInChildren<StoryProp>().gameObject;
        //If a prop exists...
        if (go != null)
        {
            //Set storyProp to that object
            storyProp = go;
        }
    }

    //Sets the next sprite to scroll in from the left side of the screen
    public void SetNextSprite(GameObject _obj, int _levelIndex)
    {
        switch (_levelIndex)
        {
            //Background Mountains
            case 2:
                _obj.GetComponent<SpriteRenderer>().sprite = GetMountainSprite();
                break;
            //Clouds
            case 3:
                _obj.GetComponent<SpriteRenderer>().sprite = clouds[Random.Range(0, clouds.Count)];
                break;
            //Random Props 01, 02 and 03
            case 5:
            case 6:
            case 7:
                _obj.GetComponent<SpriteRenderer>().sprite = GetPropSprite(_levelIndex);
                break;
            //Random Trees 01 and 02
            case 8:
            case 9:
                _obj.GetComponent<SpriteRenderer>().sprite = GetTreeSprite(_levelIndex);
                break;
            //Midground, Midground path and Background Sky (Nothing is done to them)
            case 0:
            case 1:
            case 4:
                break;
            default:
                Debug.Log("Index out of bounds. There are only 10 levels in BackgroundLoop.");
                break;
        }
    }
    //Gets a tree sprite. Argument _index is used alongwith the next node to get the appropriate tree (either apple, oak or pine)
    public Sprite GetTreeSprite(int _index)
    {
        //Whether or not there are trees depends on the current segment the party is on.
        switch (GameManager.gm.map.CurrentSegmentIndex)
        {
            //In the case of segment 1 (left path)...
            case 1:
                //For this range of nodes (1 - 9)..
                if (GameManager.gm.map.GetNextNodeIndex() >= 1 && GameManager.gm.map.GetNextNodeIndex() <= 9)
                {
                    //If the sprite is the background tree...
                    if (_index == 8)
                    {
                        //Get an apple tree
                        return appleTrees[Random.Range(0, appleTrees.Count)];
                    }
                    //Otherwise, if it's a foreground tree (which ends before the background, hence the smaller node)
                    else if (GameManager.gm.map.GetNextNodeIndex() <= 8)
                    {
                        //Get a normal tree
                        return trees[Random.Range(0, trees.Count)];
                    }                    
                }          
                break;
            //In the case of segment 2 (the right path)
            case 2:
                //For the range of nodes (6 - 11), if the tree is a background tree...
                if (_index == 8 && GameManager.gm.map.GetNextNodeIndex() >= 4 && GameManager.gm.map.GetNextNodeIndex() <= 10)
                {
                    //Get an apple tree
                    return appleTrees[Random.Range(0, appleTrees.Count)];
                }
                //Otherwise, for the range of nodes (1 - 16), if the tree is a foreground tree...
                else if (_index == 9 && GameManager.gm.map.GetNextNodeIndex() >= 1 && GameManager.gm.map.GetNextNodeIndex() <= 16)
                {  
                    //Get an apple tree
                    return appleTrees[Random.Range(0, appleTrees.Count)];
                }
                break;
            //There are no trees in the first or last stretch of the journey.
            case 0:
            case 3:
                break;

            default:
                Debug.Log("You went past the total segment numbers.");
                break;
        }
        //If no tree is to be placed, get an empty sprite        
        return emptySprites[emptySprites.Count - 1];
    }

    //Used to get grass sprite 
    public Sprite GetPropSprite(int _index)
    {
        //Based on the current segment.
        switch (GameManager.gm.map.CurrentSegmentIndex)
        {
            //Spawn grass on the left path 
            case 1:
                if (_index % 2 == 0)
                {
                    return props[Random.Range(0, props.Count)];
                }
                else
                {
                    return offsetGrass[Random.Range(0, offsetGrass.Count)]; 
                }
            //Spawn grass on the initial path. Grass is offset in a different way than on the left path. 
            case 0:
                if (_index % 2 == 0)
                {
                    return offsetGrass[Random.Range(0, offsetGrass.Count)];
                }
                else
                {
                    return props[Random.Range(0, props.Count)];
                }
            case 2:
            case 3:
                break;

            default:
                Debug.Log("You went past the total segment numbers.");
                break;
        }
        //In the event that the party is not on the left path, get an empty sprite.        
        return emptySprites[0];
    }
    //Get a mountain sprite (used to transition from plains to mountains on the final stretch)
    public Sprite GetMountainSprite()
    {
        //If the party is on the last stretch...
        bool conditionA = GameManager.gm.map.CurrentSegmentIndex == 3;
        //...OR the party is near the end of the left path...
        bool conditionB = GameManager.gm.map.CurrentSegmentIndex == 1 && GameManager.gm.map.GetNextNodeIndex() > 8;
        //...OR the party is near the end of the right path...
        bool conditionC = GameManager.gm.map.CurrentSegmentIndex == 2 && GameManager.gm.map.GetNextNodeIndex() > 16;
        //... get the mountain background, otherwise stick to the plains.
        return backgroundMountains[(conditionA || conditionB || conditionC) ? backgroundMountains.Count - 1 : 0];
    }
    //Spawn one of the 3 story props (sign, farm house, demonologist camp)
    public void SpawnStoryProp()
    {
        //Make sure the storyProp is null before spawning
        if (storyProp != null) { storyProp = null; }
        //The prop depends on the current segment.
        switch (GameManager.gm.map.CurrentSegmentIndex)
        {
            //If the player is on the initial segment...
            case 0:
                //Spawn the sign
                storyProp = Instantiate(signPrefab, this.gameObject.transform.parent);
                break;
            //If the player is on the 2nd segment (the right path)...
            case 2:
                //...and the player has made it past the 10th node (near the end of the segment)...
                if (GameManager.gm.map.GetNextNodeIndex() >= 10)
                {
                    //Spawn the farm house
                    storyProp = Instantiate(farmHousePrefab, this.gameObject.transform.parent);
                }
                break;
            //If the player is on the final segment...
            case 3:
                //...and the player has made it past the second node...
                if (GameManager.gm.map.GetNextNodeIndex() >= 2)
                {
                    //Spawn the demonologist camp
                    storyProp = Instantiate(demonologistCampPrefab, this.gameObject.transform.parent);
                }
                break;

            case 1:
            default:
                Debug.Log("There are no story props on this segment");
                break;
        }
    }
    //Initial way in which the sign was spawned. May still be triggered by an event.
    public void SpawnSign()
    {
        //Spawn the sign
        storyProp = Instantiate(signPrefab, this.gameObject.transform.parent);
    }
    //Sets the story prop's position (used after battle so that the position persists even after the scene change)
    public void SetStoryPropPosition(Vector3 _pos)
    {
        //If a story prop is present...
        if (storyProp != null)
        {
            //Sets the local position of the story prop to the passed in value 
            storyProp.transform.localPosition = _pos;
        }
    }
    //Gets the story prop's position (used before loading the combat scene to store the story prop's position so that it can be recalled after combat is complete)
    public Vector3 GetStoryPropPosition()
    {
        //If a story prop is present...
        if (storyProp != null)
        {
            //Get the story prop's position
            return storyProp.transform.position;
        }
        //Otherwise (if there is no story prop)
        else
        {
            //Return a Vector3 with negative infinity for all 3 coordinates (used to check whether a story prop exists in SceneLoader)
            return Vector3.negativeInfinity;
        }
        
    }

    

    
}
