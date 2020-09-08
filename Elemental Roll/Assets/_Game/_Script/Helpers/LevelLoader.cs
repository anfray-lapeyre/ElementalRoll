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
            if (levelToLoad >= 0)
            {


                string loadedJsonFile = Resources.Load<TextAsset>("levels").text;
                LevelsContainer levelsInJson = JsonUtility.FromJson<LevelsContainer>(loadedJsonFile);
                CrossLevelInfo.LevelName = levelsInJson.levels[levelToLoad].name;
                CrossLevelInfo.mustPassIntro = mustPassIntro;
                CrossLevelInfo.time = levelsInJson.levels[levelToLoad].time[difficultyChrono.value];
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
                StartCoroutine(loadAsynchronously("LevelSelection"));
            }
            else
            {
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
