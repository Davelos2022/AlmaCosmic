using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _ostMusic;
    [SerializeField] private AudioClip _clickSound;
    private bool _music = true; public bool Music => _music;
    private bool _sound = true; public bool Sound => _sound;

    private void Start()
    {
        _audioSource.clip = _ostMusic[Random.Range(0, _ostMusic.Length - 1)];
        _audioSource.Play();
    }

    public void ClickPlay()
    {
        if (_sound)
            _audioSource.PlayOneShot(_clickSound);
    }

    public void OnOffSound()
    {
        _sound = !_sound;
        _audioSource.mute = _sound;
    }

    public void OnOffMusic()
    {
        _music = !_music;

        if (!_music)
            StopMusic();
        else
            PlauMusic();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }

    public void PlauMusic()
    {
        _audioSource.Play();
    }
}
