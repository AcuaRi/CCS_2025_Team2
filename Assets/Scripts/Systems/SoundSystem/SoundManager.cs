using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class AudioData
{
    public string key;
    public AudioClip Clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    [SerializeField] private AudioData[] soundResources;
    private Dictionary<string, AudioClip> soundDB = new();

    [SerializeField] private int poolSize;
    [SerializeField] private GameObject soundNodePrefab;
    private Queue<AudioNode> soundPool = new();
    
    [SerializeField] private AudioSource audioSource_BGM;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            foreach (var soundResource in soundResources)
            {
                soundDB.Add(soundResource.key, soundResource.Clip);
            }

            for (int i = 0; i < poolSize; i++)
            {
                MakeNode();
            }
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    private void MakeNode()
    {
        var audioNode = Instantiate(soundNodePrefab, transform).GetComponent<AudioNode>();
        soundPool.Enqueue(audioNode);
    }

    public AudioNode GetNode()
    {
        if (soundPool.Count < 1)
        {
            MakeNode();
        }

        var node = soundPool.Dequeue();
        return node;
    }

    public void SetNode(AudioNode node)
    {
        node.transform.SetParent(this.transform);
        soundPool.Enqueue(node);
    }

    public void PlaySound(string key, Vector2 pos)
    {
        if (!soundDB.ContainsKey(key))
        {
            Debug.LogError($"Unknown SoundDB key( )");
            return;
        }
        
        var node = GetNode();
        node.transform.position = pos;
        node.Play(soundDB[key]);
    }
    
    public void PlaySound(string key, Transform parent)
    {
        if (!soundDB.ContainsKey(key))
        {
            Debug.LogError($"Unknown SoundDB key( )");
            return;
        }
        
        var node = GetNode();
        node.transform.SetParent(parent);
        node.transform.localPosition = Vector2.zero;
        node.Play(soundDB[key]);
    }
    
    public void PlayBGM(string key)
    {
        if (!soundDB.ContainsKey(key))
        {
            Debug.LogError($"Unknown SoundDB key( )");
            StopBGM();
            return;
        }
        
        SetBGM(soundDB[key]);
    }
    
    public void StopBGM()
    {
        audioSource_BGM.Stop();
        audioSource_BGM.clip = null;
    }
    
    public void SetBGM(AudioClip bgm)
    {
        audioSource_BGM.Stop();
        audioSource_BGM.clip = bgm;
        audioSource_BGM.Play();
        audioSource_BGM.loop = true;
    }
}

