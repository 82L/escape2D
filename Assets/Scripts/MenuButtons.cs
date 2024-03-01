using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {
        DontDestroyOnLoad(_audioSource);
    }

    public void Quit()
    {
        PlaySound();
        Application.Quit();
    }

    public void PlaySound()
    {
        _audioSource.Play();
    }

    public void PlayScene()
    {
        PlaySound();
        SceneManager.LoadScene("SampleScene");
    }
}
