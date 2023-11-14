using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

public class GameLogic : MonoBehaviour
{
    public SmoothFollow smoothFollow;
    public NPCController[] nPCControllers;
    public NPCItems nPCItems;
    public ServerLogic serverLogic;

    public Dictionary<string, NPCController> users = new Dictionary<string, NPCController>();

    public AudioSource audioSource;

    public SceneLogic sceneLogic;

    private int index = 0;
    private NPCController target;
    public bool isPlayingPrompt = false;
    public int promptIdx = 0;

    #region UI
    public WndMessagePanel wndMessagePanel;
    public WndMessageLog wndMessageLog;
    public WndQueue wndQueue;
    #endregion


    #region TEST DATA
    public List<string> names = new List<string>();
    public List<string> contents = new List<string>();
    public List<byte[]> soundBytesArray = new List<byte[]>();

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Init();

        GameObject gb = GameObject.Find("RabbitmqServer");
        while (gb == null)
        {
            gb = GameObject.Find("RabbitmqServer");
        }
        
        serverLogic = gb.GetComponent<ServerLogic>();
        serverLogic.gameLogic = this;
        sceneLogic = gb.GetComponent<SceneLogic>();
        smoothFollow = Camera.main.GetComponent<SmoothFollow>();
        audioSource = Camera.main.GetComponent<AudioSource>();

        ClownDataWrapper list = GetServerLogic().GetClownData();
        if (list != null)
        {
            AddPromptData(list);
        }
        
        //InitData();
    }

    /** 
     Get Object Names
     */
    ServerLogic GetServerLogic()
    {
        if(serverLogic == null)
        {
            GameObject gb = GameObject.Find("RabbitmqServer");
            if (gb != null)
            {
                serverLogic = gb.GetComponent<ServerLogic>();
                serverLogic.gameLogic = this;
            }
        }

        return serverLogic;
    }

    SceneLogic GetSceneLogic()
    {
        if (sceneLogic == null)
        {
            GameObject gb = GameObject.Find("RabbitmqServer");
            if (gb != null)
            {
                sceneLogic = gb.GetComponent<SceneLogic>();
            }
        }
        return sceneLogic;
    }

    // Update is called once per frame
    void Update()
    {

        if ( isPlayingPrompt && !audioSource.isPlaying) {

            //isPlayingPrompt = false;
            //StartCoroutine(PlayPrompt());
            OnPlayAudio(promptIdx);
            OnSetMessageLog(promptIdx);
            promptIdx++;
        }
    }

    private void Init()
    {
        nPCControllers = nPCItems.nPCs;

        users.Clear();
        foreach (NPCController nPC in nPCControllers)
        {
            /*
            float radius = Mathf.FloorToInt(Random.RandomRange(0, 30));

            float x = radius * Mathf.Cos(Mathf.Deg2Rad * Mathf.Floor(Random.RandomRange(0, 360)));
            float y = radius * Mathf.Sin(Mathf.Deg2Rad * Mathf.Floor(Random.RandomRange(0, 360)));


            NPCController p = Instantiate(nPC, Vector3.zero + new Vector3(x, 0, y), Quaternion.identity);
            */
            users.Add(nPC.name, nPC);
            smoothFollow.target = nPC.gameObject.transform;
            index++;
        }
    }

    private void InitData()
    {
        names.Add("test@gmail.com");
        names.Add("hour@gmail.com");
        names.Add("aaaa@gmail.com");
        names.Add("bbbb@gmail.com");
        names.Add("cccc@gmail.com");
        names.Add("dddd@gmail.com");
        names.Add("eeee@gmail.com");

        contents.Add("I wanted to share the results of my effort with the community, so I've setup an open source project here:");
        contents.Add("Be sure to sign up for our newsletter so you don't miss any future Phaser 3 game development tips and techniques!");
        contents.Add("Using the SpinePlugin should be fairly straight forward now that you have a working project with proper definition files and VS Code IntelliSense.");
        contents.Add("Your browser should automatically refresh at localhost:8000 and show an idling Spine Boy! 🎉");
        contents.Add("We have a constant SPINEBOY_KEY because we are using that string literal in two places. First on line 15 and later–shown below–when we create the animations.");
        contents.Add("There are Spine Boy assets that you can use here. You can use other animations if you have them.");
        contents.Add("Now let's preload the assets in SpineDemo.");
    }

    public void setTargetNPC( NPCController nPC )
    {
        foreach (NPCController user in users.Values)
        {
            user.target = nPC.transform;
        }

        target = nPC;
    }

    public void AddPlayer()
    {
        /*
        string name = names[Mathf.FloorToInt(Random.RandomRange(0, 100)) % names.Count];
        wndMessagePanel.AddMessage(name, "has been joinned");


        NPCController nPC = nPCControllers[Mathf.FloorToInt(Random.RandomRange(0, nPCControllers.Length - 1))];

        float radius = Mathf.FloorToInt(Random.RandomRange(0, 30));

        float x = radius * Mathf.Cos(Mathf.Deg2Rad * Mathf.Floor(Random.RandomRange(0, 360)));
        float y = radius * Mathf.Sin(Mathf.Deg2Rad * Mathf.Floor(Random.RandomRange(0, 360)));


        NPCController p = Instantiate(nPC, Vector3.zero + new Vector3(x, 0, y), Quaternion.identity);
        users.Add(index, p);
        smoothFollow.target = p.gameObject.transform;
        index++;
        */

    }

    IEnumerator PlayPrompt()
    {
        Debug.Log("Coroutine started");

        yield return new WaitForSeconds(1f);

        //OnPlayAudio(promptIdx);
        OnSetMessageLog(promptIdx);
        promptIdx++;
        //isPlayingPrompt = true;
    }

    public void NextPrompt()
    {
        StartCoroutine(PlayPrompt());
    }

    public void AddPromptData(ClownDataWrapper clownDatas) {
        int i = 0;
        foreach (ClownData clownData in clownDatas.prompts)
        {
            string name = clownData.name;
            string content = clownData.content;
            byte[] bytes = Convert.FromBase64String(clownData.sound_bytearray);

            names.Add(name);
            contents.Add(content);
            soundBytesArray.Add(bytes);


            i++;    

            Debug.Log("Name: " + clownData.name);
            Debug.Log("Sound Byte Array: " + clownData.sound_bytearray);
        }
        isPlayingPrompt = true;

        if(serverLogic != null)
        {
            wndQueue.queueText.text = (serverLogic.clownDataList.Count - serverLogic.promptIdx).ToString();
            wndMessagePanel.AddMessage(clownDatas.user, clownDatas.topic);
        }
    }

    public void OnReceiveMessageFromServer()
    {
        if (!isPlayingPrompt)
        {
            ClownDataWrapper clownDatas = GetServerLogic().GetClownData();

            if (clownDatas == null) return;

            AddPromptData(clownDatas);
        }
    }

    public void OnPlayAudio(int i = 0)
    {
        foreach (string name in names)
        {
            if (users.ContainsKey(name))
            {
                users[name].gameObject.SetActive(true);
            } else
            {
                NPCController nPC = nPCControllers[1];
                float radius = Mathf.FloorToInt(Random.RandomRange(0, 7));

                float x = radius * Mathf.Cos(Mathf.Deg2Rad * Mathf.Floor(Random.RandomRange(0, 360)));
                float y = radius * Mathf.Sin(Mathf.Deg2Rad * Mathf.Floor(Random.RandomRange(0, 360)));
                NPCController p = Instantiate(nPC, nPC.transform.position + new Vector3(x, 0, y), Quaternion.identity);
                users.Add(name, p);
            }
        }

        if (i == soundBytesArray.Count)
        {
            isPlayingPrompt=false;
            promptIdx = i;
            
            GetSceneLogic().loadRandomIntermission();
            return;
        }

        if (soundBytesArray[i].Length == 0) return;

        // Convert the byte data into an AudioClip using WavUtility
        AudioClip audioClip = WavUtility.ToAudioClip(soundBytesArray[i]);

        // Play the audio clip
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void OnSetMessageLog(int i = 0)
    {
        if (i >= names.Count)
        {
            //isPlayingPrompt = false;
            //promptIdx = i;

            //GetSceneLogic().loadRandomIntermission();
            return;
        }
        wndMessageLog.SetContent(contents[i]);

        string name = names[i];
        
        smoothFollow.target = users[name].gameObject.transform;

    }

    public void addTestData()
    {
        serverLogic.AddTest();
    }
}
