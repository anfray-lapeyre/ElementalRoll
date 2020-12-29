using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Slider slider;
    public TMPro.TMP_Text text;
    public IntVariable difficultyLife;
    public IntVariable difficultyChrono;
    public IntVariable livesLeft;
    public IntVariable currentLevel;


    public void ShowLoader()
    {
        this.GetComponent<CanvasGroup>().alpha = 1f;
    }

    public void LoadCharacterCutscene(int character)
    {
        handleMusicScript persistantHandler = GameObject.FindGameObjectsWithTag("PersistentObject")[0].GetComponent<handleMusicScript>();
        persistantHandler.setVolume(0.3f, 3f);

        if (character == 1)
        {
            StartCoroutine(loadAsynchronously("Tracy"));
        }
    }

    public void LoadNextLevel(int levelToLoad, bool mustPassIntro = false)
    {
        //If the current level is negative (level was chosen in level Loader), so we force the loading to reload the current level until the player quits it himself.
        //In level selection, we make sur that the players has enough lives
        //We also check if the leveltoLoad wanted is really a level and not the menu, to allow quitting
        if (currentLevel.value < 0 && levelToLoad>=0)
        {
            if (!mustPassIntro)
            {
                livesLeft.value = 99; //That way we keep the current difficulty setting chosen by the player
            }    


            levelToLoad = -currentLevel.value-1;

            updateMusic(levelToLoad);

            string loadedJsonFile = Resources.Load<TextAsset>("levels").text;
            LevelsContainer levelsInJson = JsonUtility.FromJson<LevelsContainer>(loadedJsonFile);
            CrossLevelInfo.LevelName = levelsInJson.levels[levelToLoad].name;
            CrossLevelInfo.mustPassIntro = mustPassIntro;
            CrossLevelInfo.time = levelsInJson.levels[levelToLoad].time[0];//We set speed to minimum
            CrossLevelInfo.maxSlimes = levelsInJson.levels[levelToLoad].bonusCount;

            StartCoroutine(loadAsynchronously(levelsInJson.levels[levelToLoad].sceneName));

        }
        else
        {
            //If level to load is bigger than zero, and the currentlevel value isnt negative, then it means that we are loading a level
            if (levelToLoad >= 0)
            {

                Debug.Log("Level number : " + levelToLoad);

                updateMusic(levelToLoad);
                string loadedJsonFile = Resources.Load<TextAsset>("levels").text;
                LevelsContainer levelsInJson = JsonUtility.FromJson<LevelsContainer>(loadedJsonFile);
                CrossLevelInfo.LevelName = levelsInJson.levels[levelToLoad].name;
                CrossLevelInfo.mustPassIntro = mustPassIntro;
                CrossLevelInfo.time = levelsInJson.levels[levelToLoad].time[difficultyChrono.value];
                CrossLevelInfo.maxSlimes = levelsInJson.levels[levelToLoad].bonusCount;

                if (difficultyLife.value == 1)
                {
                    //We avoid any glitch where someone gets more that 5 lives
                    if (livesLeft.value > 5)
                    {
                        livesLeft.value = 5;
                    }
                    if (!mustPassIntro)
                    {
                        livesLeft.value = 5;
                    }
                    else
                    {
                        livesLeft.value = livesLeft.value - 1;
                        if (livesLeft.value < 0)
                        {
                            currentLevel.value--;
                            cancelLevelProgress(levelToLoad);
                            livesLeft.value = 5;
                            LoadNextLevel(Mathf.Max(0, levelToLoad - 1), false);
                            return;
                        }
                    }
                }
                else if (difficultyLife.value == 2)
                {
                    //We avoid any glitch where someone gets more that 5 lives
                    if(livesLeft.value > 5)
                    {
                        livesLeft.value = 5;
                    }
                    if (mustPassIntro)
                    {
                        livesLeft.value = livesLeft.value - 1;

                        if (livesLeft.value < 0)
                        {
                            cancelAllProgress();
                            livesLeft.value = 5;
                            currentLevel.value = 0;
                            LoadNextLevel(0);
                            return;
                        }
                    }
                }
                StartCoroutine(loadAsynchronously(levelsInJson.levels[levelToLoad].sceneName));
            }
            else if (levelToLoad == -1)
            {
                updateMusic(0);

                StartCoroutine(loadAsynchronously("LevelSelection"));
            }
            else
            {
                updateMusic(0);

                StartCoroutine(loadAsynchronously("MainMenu"));
            }
        }

       
        LeanTween.reset();
        LeanTween.init(800);
        /*Debug.Log(levelsInJson.levels[levelToLoad].objName);
        Debug.Log(levelsInJson.levels[levelToLoad].previewScale[0]);
        Debug.Log(levelsInJson.levels[levelToLoad].previewRotation[0]);
        Debug.Log(levelsInJson.levels[levelToLoad].bonusCount);*/


    }

    private void updateMusic(int levelToLoad)
    {
        //If the right musical atmosphere isnt set, we set it
        handleMusicScript persistantHandler = GameObject.FindGameObjectsWithTag("PersistentObject")[0].GetComponent<handleMusicScript>();
        persistantHandler.setVolume(1f, 3f);
        if (persistantHandler.currentMusicAtmosphere != (levelToLoad - 1) / 20)
            persistantHandler.changeMusic((levelToLoad - 1) / 20, 5f);
    }


    private void cancelLevelProgress(int leveltoErase)
    {
        ActualSave.actualSave.levels[leveltoErase].collectedSlime = 0;
        ActualSave.actualSave.levels[leveltoErase].bestTime = 999f;
        ActualSave.actualSave.levels[leveltoErase].beaten = false;
        ActualSave.actualSave.levels[leveltoErase].beatenInNormalLife = false;
        ActualSave.actualSave.levels[leveltoErase].beatenInDifficultLife = false;
        ActualSave.actualSave.levels[leveltoErase].beatenInEasyLife = false;
        ActualSave.actualSave.levels[leveltoErase].beatinInEasyTime = false;
        ActualSave.actualSave.levels[leveltoErase].beatinInNormalTime = false;
        ActualSave.actualSave.levels[leveltoErase].beatinInDifficultTime = false;
        SaveSystem.SaveGame(ActualSave.actualSave, ActualSave.saveSlot);
    }

    private void cancelAllProgress()
    {
        for (int leveltoErase = 0; leveltoErase < ActualSave.actualSave.levels.Length; leveltoErase++)
        {
            ActualSave.actualSave.levels[leveltoErase].collectedSlime = 0;
            ActualSave.actualSave.levels[leveltoErase].bestTime = 999f;
            ActualSave.actualSave.levels[leveltoErase].beaten = false;
            ActualSave.actualSave.levels[leveltoErase].beatenInNormalLife = false;
            ActualSave.actualSave.levels[leveltoErase].beatenInDifficultLife = false;
            ActualSave.actualSave.levels[leveltoErase].beatenInEasyLife = false;
            ActualSave.actualSave.levels[leveltoErase].beatinInEasyTime = false;
            ActualSave.actualSave.levels[leveltoErase].beatinInNormalTime = false;
            ActualSave.actualSave.levels[leveltoErase].beatinInDifficultTime = false;
        }

        SaveSystem.SaveGame(ActualSave.actualSave, ActualSave.saveSlot);
    }


    public void handleDifficultySaveData(int _currentLevel)
    {
        switch (difficultyLife.value)
        {
            case 1:
                ActualSave.actualSave.levels[_currentLevel].beatenInEasyLife = true;
                ActualSave.actualSave.levels[_currentLevel].beatenInNormalLife = true;
                break;
            case 2:
                ActualSave.actualSave.levels[_currentLevel].beatenInDifficultLife = true;
                ActualSave.actualSave.levels[_currentLevel].beatenInEasyLife = true;
                ActualSave.actualSave.levels[_currentLevel].beatenInNormalLife = true;
                break;
            default:
                ActualSave.actualSave.levels[_currentLevel].beatenInEasyLife = true;
                break;
        }
        switch (difficultyChrono.value)
        {
            case 1:
                ActualSave.actualSave.levels[_currentLevel].beatinInEasyTime = true;
                ActualSave.actualSave.levels[_currentLevel].beatinInNormalTime = true;
                break;
            case 2:
                ActualSave.actualSave.levels[_currentLevel].beatinInDifficultTime = true;
                ActualSave.actualSave.levels[_currentLevel].beatinInEasyTime = true;
                ActualSave.actualSave.levels[_currentLevel].beatinInNormalTime = true;
                break;
            default:
                ActualSave.actualSave.levels[_currentLevel].beatinInEasyTime = true;
                break;
        }

       
    }


    IEnumerator loadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp(operation.progress, 0, 1);
            if(progress <0.1f || (progress >0.3f&& progress<=0.4f) || (progress > 0.6f && progress <= 0.7f))
            {
                text.text = "Loading.";
            }else if ((progress >= 0.1f && progress <0.2f) || (progress > 0.4f && progress <= 0.5f) || (progress > 0.7f && progress <= 0.8f))
            {
                text.text = "Loading..";
            }
            else
            {
                text.text = "Loading...";
            }

            slider.value = progress;
            yield return null;

        }
    }
}
