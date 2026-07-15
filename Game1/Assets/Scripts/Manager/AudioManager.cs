using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioSource effectAudioSource;
    [SerializeField] AudioSource sceneryAudioSource;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        sceneryAudioSource.loop = true;
    }

    public void Listener(string name)
    {
        AudioClip clip = Resources.Load<AudioClip>(name);
        if (clip == null)
        {
            Debug.LogWarning($"AudioClip을 찾지 못함: \"Audio/" + name);
            return;
        }
        effectAudioSource.PlayOneShot(clip);
    }

    public void ScenerySound(string name)
    {
        sceneryAudioSource.clip = Resources.Load<AudioClip>(name);

        sceneryAudioSource.Play();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        ScenerySound(scene.name);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}