﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Globalization;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;
using Cinemachine;
using UnityEngine.Playables;




public class CharacterDialogueHandler : Observer
{

    public int levelLoadedInTheEnd = 0;
    private Dialogue dialogue1;
    public int startId;
    public SoundStorageScript soundStorage; 
    private AudioSource audioSource;
    private Dictionary<string, Dictionary<string, AudioClip>> values;
    private Dictionary<string, PlayableDirector> timelineValues;
    public PlayableDirector[] timelines;
    public Canvas DialoguePanel;
    private Canvas instantiatedDialoguePanel;
    private TextMeshProUGUI m_TextMeshPro;

    private string videoValue="";

    public UIFader fader;

    private bool nextLine = false;
    private bool faster = false;
    private bool nextDialogue=false;

    public GameObject LevelLoader; //Base
    private LevelLoader _levelLoader; //Instantiated child

    public IntVariable LivesLeft;

    public bool fades = false;
    private int staticStartId;
    private int tmp_dialogue_ID;
    private bool dialogueMustStart = true;

    public void Start()
    {
        staticStartId = startId;
        GameObject.FindGameObjectsWithTag("PersistentObject")[0].GetComponent<InputHandler>().addObserver(this);

        GameObject levelLoader = Instantiate(LevelLoader);
        _levelLoader = levelLoader.GetComponent<LevelLoader>();
        Invoke("waitABit", 2f);

        loadVideos();
        fader.FadeOut(0.5f);

    }

    override public void OnNotify(GameObject entity, object notifiedEvent)
    {
        switch (notifiedEvent.GetType().ToString())
        {
            case "MoveCommand":
                // OnDirection(((MoveCommand)notifiedEvent).getMove());
                break;
            case "SpellCommand":
                OnConfirm(((SpellCommand)notifiedEvent).isPressed());
                break;
            case "RestartCommand":
                OnReturn(((RestartCommand)notifiedEvent).isPressed());
                break;
            case "PauseCommand":
                //OnPause(((RestartCommand)notifiedEvent).isPressed());
                break;
            case "EagleViewCommand":
                //OnPause(((EagleViewCommand)notifiedEvent).isPressed());
                break;
            case "TopViewCommand":
                //OnPause(((TopViewCommand)notifiedEvent).isPressed());
                break;
            default:
                break;
        }
    }

    public void waitABit()
    {
        instantiatedDialoguePanel = Instantiate(DialoguePanel);
        audioSource = this.GetComponent<AudioSource>();
        m_TextMeshPro = instantiatedDialoguePanel.GetComponentInChildren<TextMeshProUGUI>();
        loadDictionary();
        StartCoroutine("ReadAllDialogue");
    }

    private void loadVideos()
    {
        timelineValues = new Dictionary<string, PlayableDirector>();
        foreach (PlayableDirector timeline in timelines)
        {
            timelineValues.Add(timeline.name, timeline);
        }
    }

    private void loadDictionary()
    {
        values = soundStorage.getAudio();
    }
    
    private IEnumerator ReadAllDialogue()
    {
        while (startId >= 0)
        {
            nextDialogue = false;
            //We wait to make sure the proper dialogue is initialized
            LoadDialog(startId);
            m_TextMeshPro.text = "";
            yield return new WaitUntil(() => dialogueMustStart == true);

            StartCoroutine("ReadLine");
            yield return new WaitUntil(() => nextDialogue == true);
        }
        fader.FadeIn(0.5f);
        Invoke("OutOfSave", 0.5f);
        
    }

    private IEnumerator ReadLine()
    {
        audioSource.pitch = dialogue1.interlocutor.pitch;
        foreach (string stringLine in dialogue1.lines)
        {
            m_TextMeshPro.text = stringLine;
            int totalVisibleCharacters = m_TextMeshPro.textInfo.characterCount;
            m_TextMeshPro.maxVisibleCharacters = 0;
            m_TextMeshPro.ForceMeshUpdate();
            char[] line = RemoveAccents(m_TextMeshPro.GetParsedText().ToLower()).ToCharArray();
            
            for (int i = 0; i < line.Length; i++)
            {
                m_TextMeshPro.maxVisibleCharacters = i+1;

                if (line[i] != ' ' && line[i] != ',' && line[i] != '\'')
                {
                    if (line[i] == 'y')
                    {
                        line[i] = 'i';
                    }
                    if (isVoyelle(line[i]))
                    {
                        audioSource.PlayOneShot(values[dialogue1.interlocutor.name]["" + line[i]]);
                    }
                    else if (isConsonne(line[i]))
                    {

                        if (i + 1 < line.Length)
                        {
                            if (isComplex(line[i]) && line[i + 1] == 'h')
                            {
                                if (i + 2 < line.Length && isVoyelle(line[i + 2]))
                                {
                                    audioSource.PlayOneShot(values[dialogue1.interlocutor.name]["" + line[i] + line[i + 1] + line[i + 2]]);
                                }
                                else
                                {
                                    audioSource.PlayOneShot(values[dialogue1.interlocutor.name]["" + line[i] + line[i + 1] + ('e')]);

                                }
                                i++;
                            }
                            else
                            {

                                if (line[i] == 'y')
                                {
                                    line[i] = 'i';
                                }
                                if (isVoyelle(line[i + 1]))
                                {
                                    audioSource.PlayOneShot(values[dialogue1.interlocutor.name]["" + line[i] + line[i + 1]]);
                                }
                                else
                                {
                                    audioSource.PlayOneShot(values[dialogue1.interlocutor.name]["" + line[i] + ('e')]);

                                }
                            }
                        }
                        else
                        {
                            audioSource.PlayOneShot(values[dialogue1.interlocutor.name]["" + line[i] + ('e')]);

                        }
                    }
                    else
                    {
                        audioSource.PlayOneShot(values[dialogue1.interlocutor.name]["_punctuation"]);
                    }
                }

                yield return new WaitForSecondsRealtime((faster == true) ? 0.01f : 0.06f);
            }
            nextLine = false;
            faster = false;
            yield return new WaitUntil(() => nextLine == true);

        }
        nextDialogue = true;
        dialogueMustStart = false;
    }

    private bool isVoyelle(char test)
    {
        char[] voyelles = { 'a','e','i','o','u' };
        foreach (char lettre in voyelles)
        {
            if (test == lettre)
                return true;
        }
        return false;
    }

    private bool isConsonne(char test)
    {
        char[] consonnes = { 'z', 'r', 't', 'p', 'q', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'w', 'x', 'c', 'v', 'b', 'n' };
        foreach(char lettre in consonnes)
        {
            if (test == lettre)
                return true;
        }
        return false;
    }

    private bool isComplex(char test)
    {
        char[] consonnes = { 't', 'p', 'w', 'c', 's' };
        foreach (char lettre in consonnes)
        {
            if (test == lettre)
                return true;
        }
        return false;
    }

    private string RemoveAccents(string s)
    {
        string formD = s.Normalize(NormalizationForm.FormD);
        StringBuilder sb = new StringBuilder();

        foreach (char ch in formD)
        {
            UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (uc != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(ch);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }






    private void LoadDialog(int _dialogueID)
    {
        tmp_dialogue_ID = _dialogueID;

        //Plus tard on chargera un JSON
        if (fades && _dialogueID!=staticStartId)
        {
            fader.FadeIn(0.3f);
            Invoke("loadDialogueafterWait", 0.4f);
            Invoke("afterFaded", 0.5f);
        }
        else
        {
            loadDialogueafterWait();
        }
       
    }

    public void loadDialogueafterWait()
    {
        //Plus tard on chargera un JSON
        string loadedJsonFile = Resources.Load<TextAsset>(ActualLanguage.actualLanguage.chosenLanguage.name).text;
        DialogueContainer dialoguesInJson = JsonUtility.FromJson<DialogueContainer>(loadedJsonFile);

        int dialogueID = findDialogueByID(tmp_dialogue_ID);
        startId = dialoguesInJson.dialogues[dialogueID].next;

        if (videoValue != "" && timelineValues[videoValue].playableGraph.IsValid())
        {
            timelineValues[videoValue].playableGraph.GetRootPlayable(0).SetSpeed(100);
        }
        videoValue = dialoguesInJson.dialogues[dialogueID].backgroundVideo;
        timelineValues[videoValue].Play();

        if (fades && tmp_dialogue_ID != staticStartId)
        {
            Invoke("loadEndOfDialogueData", 0.6f);
        }
        else
        {
            loadEndOfDialogueData();
        }

    }

    public void loadEndOfDialogueData()
    {
        string loadedJsonFile = Resources.Load<TextAsset>(ActualLanguage.actualLanguage.chosenLanguage.name).text;
        DialogueContainer dialoguesInJson = JsonUtility.FromJson<DialogueContainer>(loadedJsonFile);

        int dialogueID = findDialogueByID(tmp_dialogue_ID);

        Character dialogueCharacter = new Character();
        switch (dialoguesInJson.dialogues[dialogueID].character)
        {
            case "Ice":
                dialogueCharacter = dialogueCharacter.Ice();
                break;
            case "Earth":
                dialogueCharacter = dialogueCharacter.Earth();
                break;
            case "Death":
                dialogueCharacter = dialogueCharacter.Death();
                break;
            case "Wizard":
                dialogueCharacter = dialogueCharacter.Wizard();
                break;
            default:
                dialogueCharacter = dialogueCharacter.Fire();
                break;
        }

        dialogue1 = new Dialogue(dialogueCharacter, dialoguesInJson.dialogues[dialogueID].lines, 0);
        dialogueMustStart = true;

    }

    public int findDialogueByID(int ID)
    {
        string loadedJsonFile = Resources.Load<TextAsset>(ActualLanguage.actualLanguage.chosenLanguage.name).text;
        DialogueContainer dialoguesInJson = JsonUtility.FromJson<DialogueContainer>(loadedJsonFile);
        for(int i = 0; i < dialoguesInJson.dialogues.Length; i++)
        {
            if (dialoguesInJson.dialogues[i].id == ID)
                return i;
        }
        return -1;
    }


    public void afterFaded()
    {
        fader.FadeOut(0.3f);

    }

    public void fadeAgain()
    {
        

    }


    private void Update()
    {
        /*if (waitForVideoToFinish && videoPlayer.isPaused && !levelIsLoaded)
        {
            _levelLoader.ShowLoader();
            Invoke("OutOfSave", 0.5f);
            levelIsLoaded = true;
        }*/
    }



    public void OutOfSave()
    {
        Debug.Log("On arrive bien là");
        _levelLoader.ShowLoader();
        _levelLoader.LoadNextLevel(levelLoadedInTheEnd, false);
    }


    //INPUT


    public void OnConfirm(bool value)
    {
        if (value)
        {
            nextLine = true;
        }
    }

    public void OnReturn(bool value)
    {
        if (value)
        {
            faster = true;
            nextLine = true;
        }
    }

}

