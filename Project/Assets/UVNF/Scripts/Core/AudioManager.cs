using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UVNF.Core
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Background Music")]
        public AudioSource BackgroundMusic;
        public bool CurrentlyPlayingBackgroundMusic = false;

        [Header("SFX")]
        public Transform SFXParent;

        private void Awake()
        {
            BackgroundMusic.loop = true;
        }

        public void PlayBackgroundMusic(AudioClip clip, float volume = 1f)
        {
            BackgroundMusic.clip = clip;
            BackgroundMusic.Play();

            CurrentlyPlayingBackgroundMusic = true;
        }

        public void StopBackgroundMusic()
        {
            BackgroundMusic.Stop();
        }

        public void StopBackgroundMusic(float fadeOutTime = 1f, bool destroy = true)
        {
            CrossfadeAudioSourceDown(BackgroundMusic, fadeOutTime, destroy);
        }

        public void PauseBackgroundMusic()
        {
            BackgroundMusic.Pause();
        }

        public void CrossfadeBackgroundMusic(AudioClip clip, float crossfadeTime = 1f)
        {
            AudioSource newBGSource = Instantiate(BackgroundMusic.gameObject, transform).GetComponent<AudioSource>();
            newBGSource.gameObject.name = BackgroundMusic.gameObject.name;
            BackgroundMusic.gameObject.name = BackgroundMusic.gameObject.name + " [OLD]";

            newBGSource.clip = clip;
            newBGSource.volume = 0f;

            AudioSource oldBGSource = BackgroundMusic;

            BackgroundMusic = newBGSource;
            BackgroundMusic.Play();

            StartCoroutine(CrossfadeAudioSourceDown(oldBGSource, crossfadeTime));
            StartCoroutine(CrossfadeAudioSourceUp(BackgroundMusic, crossfadeTime));
        }

        public void PlaySound(AudioClip clip, float volume)
        {
            StartCoroutine(PlaySoundCoroutine(clip, volume));
        }

        public void PlaySound(AudioClip clip, float volume, float extraPitch)
        {
            StartCoroutine(PlaySoundCoroutine(clip, volume, extraPitch));
        }

        public IEnumerator PlaySoundCoroutine(AudioClip clip, float volume)
        {
            GameObject sfxPlayer = new GameObject(clip.name);
            sfxPlayer.transform.SetParent(SFXParent);

            AudioSource source = sfxPlayer.AddComponent<AudioSource>();
            source.clip = clip;

            //TODO volume * UVNFManager.UserSettings.Volume;
            source.volume = volume;
            source.Play();

            while (source.isPlaying) yield return null;

            Destroy(sfxPlayer);
        }

        public IEnumerator PlaySoundCoroutine(AudioClip clip, float volume, float extraPitch)
        {
            GameObject sfxPlayer = new GameObject(clip.name);
            sfxPlayer.transform.SetParent(SFXParent);

            AudioSource source = sfxPlayer.AddComponent<AudioSource>();
            source.clip = clip;
            source.pitch += extraPitch;

            //TODO volume * UVNFManager.UserSettings.Volume;
            source.volume = volume;
            source.Play();

            while (source.isPlaying) yield return null;

            Destroy(sfxPlayer);
        }

        private IEnumerator CrossfadeAudioSourceUp(AudioSource source, float crossfadeTime = 1f)
        {
            //TODO get the max volume set by the UVNFManager
            while (source.volume != 1f) { source.volume += Time.deltaTime / crossfadeTime; yield return null; }
        }

        private IEnumerator CrossfadeAudioSourceDown(AudioSource source, float crossfadeTime = 1f, bool deleteOnDone = true)
        {
            while (source.volume != 0f) { source.volume -= Time.deltaTime / crossfadeTime; yield return null; }
            if (deleteOnDone) Destroy(source.gameObject);
        }
    }
}