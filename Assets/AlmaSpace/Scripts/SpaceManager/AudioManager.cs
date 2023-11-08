using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSourceMusic;
    [SerializeField] private AudioSource _audioSourceSound;
    [SerializeField] private AudioClip[] _ostMusic;
    [SerializeField] private AudioClip _clickSound;
    private bool _music = true; public bool Music => _music;
    private bool _sound = true; public bool Sound => _sound;

    private void Start()
    {
        _audioSourceMusic.clip = _ostMusic[Random.Range(0, _ostMusic.Length - 1)];
        _audioSourceMusic.Play();
    }

    public void ClickSound()
    {
        if (_sound)
            _audioSourceSound.PlayOneShot(_clickSound);
    }

    public void PlayClip(AudioClip audioClip)
    {
        if (_sound)
            _audioSourceSound.PlayOneShot(audioClip);
    }

    public void OnOffSound()
    {
        _sound = !_sound;
        _audioSourceMusic.mute = _sound;
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
        _audioSourceMusic.Stop();
    }

    public void PlauMusic()
    {
        _audioSourceMusic.Play();
    }
}
