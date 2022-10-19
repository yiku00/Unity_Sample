using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string m_name;
    public List<AudioClip> m_clips = null;

    [Range(0f, 1f)]
    public float volume = 1.0f;
    [Range(0f, 1.5f)]
    public float pitch = 1.0f;
    public Vector2 m_randomVolumeRange = new Vector2(1.0f, 1.0f);
    public Vector2 m_randomPitchRange = new Vector2(1.0f, 1.0f);
    
    private AudioSource m_source = null;

    public Sound()
    {
        m_randomPitchRange = new Vector2(1.0f, 1.0f);
        m_randomVolumeRange = new Vector2(1.0f, 1.0f);
        m_clips = new List<AudioClip>();
        pitch = 1.0f;
        volume = 1.0f;
        m_name = "";
    }
    public void SetSource(AudioSource source)
    {
        m_source = source;
        int randomClip = Random.Range(0, m_clips.Count - 1);
        m_source.clip = m_clips[randomClip];
    }

    public void Play()
    {
        if(m_clips.Count > 1)
        {
            int randomClip = Random.Range(0, m_clips.Count);
            m_source.clip = m_clips[randomClip];
        }
        m_source.volume = volume * Random.Range(m_randomVolumeRange.x, m_randomVolumeRange.y);
        m_source.pitch = pitch * Random.Range(m_randomPitchRange.x, m_randomPitchRange.y);
        m_source.Play();
    }
}

public class AudioManager_PrototypeHero : MonoBehaviour
{
    // Make it a singleton class that can be accessible everywhere
    public static AudioManager_PrototypeHero instance;

    [SerializeField]
    List<Sound> m_sounds;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one AudioManger in scene");
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        for(int i = 0; i < m_sounds.Count; i++)
        {
            GameObject go = new GameObject("Sound_" + i + "_" + m_sounds[i].m_name);
            go.transform.SetParent(transform);
            m_sounds[i].SetSource(go.AddComponent<AudioSource>());
        }
        //BGM.Add()
        //AddEffectSound("Character/Prototype_Hero_Demo/Audio/Footstep1.wav", "Footstep");
    }

    public void PlaySound (string name)
    {
        for(int i = 0; i < m_sounds.Count; i++)
        {
            if(m_sounds[i].m_name == name)
            {
                m_sounds[i].Play();
                return;
            }
        }

        Debug.LogWarning("AudioManager: Sound name not found in list: " + name);
    }

    public void AddSound(string path, string SoundCategory = "default")
    {
        AudioClip ClipFile = Resources.Load<AudioClip>(path);
        if (ClipFile == null)
        {
            Debug.Log("AddEffectSound Error: " + path + " File Is Not Valid");
            return;
        }

        int TargetIdx = GetSoundIdx(SoundCategory);
        Instantiate(ClipFile);

        if (TargetIdx < 0)
        {
            Sound _sound = new Sound();
            GameObject go = new GameObject("Sound_" + SoundCategory);
            go.transform.SetParent(transform);
            _sound.m_name = SoundCategory;
            _sound.m_clips.Add(ClipFile);
            m_sounds.Add(_sound);
        }
        else
        {
            if (GetClipIdx(SoundCategory, TargetIdx) > 0)
            {
                Debug.Log("The Client Already Has Sound Data:" + path);
            }
            else
            {
                m_sounds[TargetIdx].m_clips.Add(ClipFile);
            }
        }
    }

    private int GetSoundIdx(string name)
    {
        int Idx = -1;
        for (int i = 0; i < m_sounds.Count; i++)
        {
            if (m_sounds[i].m_name == name)
            {
                Idx = i;
                break;
            }
        }
        return Idx;
    }

    private int GetClipIdx(string name, int targetIdx)
    {
        int Idx = -1;
        for (int i = 0; i < m_sounds[targetIdx].m_clips.Count; i++)
        {
            if (m_sounds[targetIdx].m_clips[i].name == name)
            {
                Idx = i;
                break;
            }
        }
        return Idx;
    }
}
