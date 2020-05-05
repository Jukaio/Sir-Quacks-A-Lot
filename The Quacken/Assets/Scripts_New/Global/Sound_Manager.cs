using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Clip
{
    public string m_name;
    public AudioClip m_clip;
    public bool m_loop;
}

[System.Serializable]
public class Sound_Package
{
    public string m_package_name;
    public Clip[] m_clips;
}

public class Sound_Manager : MonoBehaviour
{
    private AudioSource[] m_channels;

    Sound_Manager()
    {
        //Service<Sound_Manager>.Set(this);
    }

    [SerializeField] private Sound_Package[] m_packages;

    private Dictionary<string, Dictionary<string, Clip>> m_dictionary = new Dictionary<string, Dictionary<string, Clip>>();


    private void Start()
    {
        // Setup channels
        m_channels = GetComponents<AudioSource>();

        foreach (Sound_Package package in m_packages)
        {
            if (package.m_package_name.Length <= 2)
            {
                Debug.LogWarning("Name of Package to short or Name not set", gameObject);
                continue;
            }

            m_dictionary.Add(package.m_package_name, new Dictionary<string, Clip>());
            foreach(Clip clip in package.m_clips)
            {
                if (clip.m_name.Length <= 2)
                {
                    Debug.LogWarning(" ID of Clip in " + package.m_package_name + " to short or ID not set", gameObject);
                    continue;
                }
                m_dictionary[package.m_package_name].Add(clip.m_name, clip);
            }
        }
    }

    // -1 as channel chooses the "next free channel"
    public void Play(string p_package_name, string p_clip_name)
    {
        foreach(AudioSource source in m_channels)
        {
            if(!source.isPlaying)
            {
                Clip temp = m_dictionary[p_package_name][p_clip_name];
                source.clip = temp.m_clip;
                source.loop = temp.m_loop;
                source.Play();
                return;
            }
        }
    }

}
