using System;
using UnityEngine;

[System.Serializable]
public class SaveFileInfo
{
    public LevelSaveFormat[] levels;
    public int chosenPlayer;
    public int chosenLivesDifficulty;
    public int chosenTimeDifficulty;

    //New save
    public SaveFileInfo(int nbLevels = 120, int _chosenPlayer= 0)
    {
        chosenPlayer = _chosenPlayer;
        chosenTimeDifficulty = 1;
        chosenLivesDifficulty = 1;
        levels = new LevelSaveFormat[nbLevels];
        for(int i = 0; i < nbLevels; i++)
        {
            levels[i] = new LevelSaveFormat();
            levels[i].beaten = false;
            levels[i].beatenInDifficultLife = false;
            levels[i].beatenInEasyLife = false;
            levels[i].beatenInNormalLife = false;
            levels[i].beatinInDifficultTime = false;
            levels[i].beatinInEasyTime = false;
            levels[i].beatinInNormalTime = false;
            levels[i].bestTime = 999f;
            levels[i].collectedSlime = 0;
        }
    }


    //Existing save
    public SaveFileInfo(LevelSaveFormat[] _levels, int _chosenPlayer=0)
    {

        levels =(LevelSaveFormat[]) _levels.Clone();
        chosenTimeDifficulty = 1;
        chosenLivesDifficulty = 1;
        chosenPlayer = _chosenPlayer;
    }

    //Existing save
    public SaveFileInfo(LevelSaveFormat[] _levels, int _chosenPlayer = 0, int _chosenLife=1, int _chosenTime = 1)
    {

        levels = (LevelSaveFormat[])_levels.Clone();
        chosenPlayer = _chosenPlayer;
        chosenTimeDifficulty = _chosenTime;
        chosenLivesDifficulty = _chosenLife;
    }

    public void fillWithBeaten(int nb)
    {
        for(int i = 0; i < Mathf.Min(this.levels.Length,nb); i++){
            this.levels[i].beaten = true;
        }
    }

    public int getPercentage()
    {
        float percentage = 0f;
        int count = 0;
        int slimecount = 0;
        int maxDifficultyCount = 0;
        for(int i = 0; i < this.levels.Length; i++)
        {
            if (this.levels[i].beaten)
            {
                count++;
            }

            slimecount += this.levels[i].collectedSlime;

            if (this.levels[i].beatinInDifficultTime && this.levels[i].beatenInDifficultLife)
            {
                maxDifficultyCount++;
            }
        }

        percentage = ((float)count / this.levels.Length)*0.8f;//The first 80% are juste finishing each level
        percentage += ((float)slimecount / (this.levels.Length * 2f))*0.2f; //There is  twice more slimes than levels, and the total amount of slime accounts for 20%

        //In case someone finishes it, we take 10 more % to complete the challenges
        //The count to 110% is only calculated when the player has reached 100%. This will avoid a "fake" 100% when the player hasn't collected all slimes or finished all levels
        if(count == this.levels.Length && slimecount == this.levels.Length * 2f)
        {
            percentage += ((float)maxDifficultyCount / this.levels.Length) * 0.1f;

        }

        return (int)(percentage*100f);
    }

    public int getSlimesCollected()
    {
        int slimecount = 0;
        for (int i = 0; i < this.levels.Length; i++)
        {


            slimecount += this.levels[i].collectedSlime;

        }

        return slimecount;
    }

    public int NextLevel()
    {
        for(int i=0; i < this.levels.Length; i++)
        {
            if (!this.levels[i].beaten)
            {
                return i;
            }
        }
        return  this.levels.Length;
    }


    public void Verbose(int count)
    {
        for (int i = 0; i < Mathf.Min(this.levels.Length,count); i++)
        {
            Debug.Log("Level no."+i+", have been beaten : "+this.levels[i].beaten+"\nHave been beaten at max life difficulty : " + this.levels[i].beatenInDifficultLife + "\nHave been beaten at max time difficulty : " + this.levels[i].beatinInDifficultTime + "\nSlimes collected : "+ this.levels[i].collectedSlime);
        }
    }

    public int LastBeatenLevel()
    {

        return NextLevel()-1;
    }
}