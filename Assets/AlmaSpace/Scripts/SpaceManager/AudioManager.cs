using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSourceMusic;
    [SerializeField] private AudioSource _audioSourceSound;
    [SerializeField] private AudioClip[] _ostMusic;
    [SerializeField] private AudioClip _clickSound;
    private bool _music = true; public bool Music => _music;

    private void Start()
    {
        _audioSourceMusic.clip = _ostMusic[Random.Range(0, _ostMusic.Length - 1)];
        _audioSourceMusic.Play();
    }

    public void ClickSound()
    {
        _audioSourceSound.PlayOneShot(_clickSound);
    }

    public void PlayClip(AudioClip audioClip)
    {
        if (_audioSourceSound.isPlaying)
            _audioSourceSound.Stop();

        _audioSourceSound.PlayOneShot(audioClip);
    }

    public void StopClip()
    {
        if (_audioSourceSound.isPlaying)
            _audioSourceSound.Stop();
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
