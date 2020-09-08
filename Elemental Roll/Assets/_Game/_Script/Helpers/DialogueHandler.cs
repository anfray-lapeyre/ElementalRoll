using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Globalization;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using UnityEngine.EventSystems;


public class DialogueHandler : MonoBehaviour
{
    private Dialogue dialogue1;
    public int startId;
    public SoundStorageScript soundStorage;
    private AudioSource audioSource;
    private Dictionary<string, Dictionary<string, AudioClip>> values;
    private Dictionary<string, VideoClip> videoValues;
    public VideoClip[] videos;
    private VideoPlayer videoPlayer;
    public Canvas DialoguePanel;
    private Canvas instantiatedDialoguePanel;
    private TextMeshProUGUI m_TextMeshPro;

    private string videoValue="";

    public UIFader fader;

    private bool nextLine = false;
    private bool faster = false;
    private bool nextDialogue=false;

    private bool waitForVideoToFinish = false;
    private bool levelIsLoaded = false;

    public GameObject LevelLoader; //Base
    private LevelLoader _levelLoader; //Instantiated child


    public GameObject SaveSelectionScreen;
    public EventSystem eventSystem;
    public IntVariable LivesLeft;

    private UIDialogSoundScript uiSound;

    public void Start()
    {
        uiSound = this.GetComponentInParent<UIDialogSoundScript>();
        GameObject levelLoader = Instantiate(LevelLoader);
        _levelLoader = levelLoader.GetComponent<LevelLoader>();
        Invoke("waitABit", 2f);
        videoPlayer = this.GetComponent<VideoPlayer>();
        videoPlayer.isLooping = true;
        loadVideos();

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
        videoValues = new Dictionary<string, VideoClip>();
        foreach (VideoClip video in videos)
        {
            videoValues.Add(video.name, video);
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
            LoadDialog(startId);
            StartCoroutine("ReadLine");
            yield return new WaitUntil(() => nextDialogue == true);
        }
        
    }

    private IEnumerator ReadLine()
    {
        uiSound.Play();
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

        //Plus tard on chargera un JSON
        string loadedJsonFile = Resources.Load<TextAsset>("dialogues").text;
        DialogueContainer dialoguesInJson = JsonUtility.FromJson<DialogueContainer>(loadedJsonFile);
        fader.FadeIn(0.3f);
        Invoke("afterFaded", 0.4f);
        
        int dialogueID = findDialogueByID(_dialogueID);
        startId = dialoguesInJson.dialogues[dialogueID].next;
        if (startId < 0)
        {
            waitForVideoToFinish = true;
            videoPlayer.isLooping = false;
        }
        videoValue = dialoguesInJson.dialogues[dialogueID].backgroundVideo;
        Character dialogueCharacter = new Character();
        switch (dialoguesInJson.dialogues[dialogueID].character)
        {
            case "Ice": dialogueCharacter = dialogueCharacter.Ice();
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
        
        dialogue1 = new Dialogue(dialogueCharacter, dialoguesInJson.dialogues[dialogueID].lines,0);
    }

    public int findDialogueByID(int ID)
    {
        string loadedJsonFile = Resources.Load<TextAsset>("dialogues").text;
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
        videoPlayer.clip = videoValues[videoValue];
        videoPlayer.Play();
        Invoke("fadeAgain", 0.5f);
    }

    public void fadeAgain()
    {
        fader.FadeOut(0.3f);

    }



    private void Update()
    {
        if (waitForVideoToFinish && videoPlayer.isPaused && !levelIsLoaded)
        {
            _levelLoader.ShowLoader();
            Invoke("OutOfSave", 0.5f);
            levelIsLoaded = true;
        }
    }



    public void OutOfSave()
    {
        
        _levelLoader.LoadNextLevel(0, false);
    }


    //INPUT


    public void OnConfirm(InputValue value)
    {
        nextLine = true;
    }

    public void OnReturn(InputValue value)
    {
        faster = true;
        nextLine = true;
    }

}

