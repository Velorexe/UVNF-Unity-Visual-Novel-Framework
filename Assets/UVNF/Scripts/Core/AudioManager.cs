using System.Collections;
using UnityEngine;

namespace UVNF.Core
{
    /// <summary>
    /// A manager class that handles audio, such as background music and SFX's
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        // TODO: rework this entire class (including the use of Volume Mixers)

        [Header("Background Music")]
        public AudioSource BackgroundMusic;
        public bool CurrentlyPlayingBackgroundMusic = false;

        // All new AudioComponents playing SFX will be a child of SFXParent
        [Header("SFX")]
        public Transform SFXParent;

        private void Awake()
        {
            BackgroundMusic.loop = true;
        }

        /// <summary>
        /// Replaces the current background <see cref="AudioClip"/> with given one
        /// </summary>
        /// <param name="clip">The new background music clip that should be played</param>
        /// <param name="volume">The volume at which the clip should play at</param>
        public void PlayBackgroundMusic(AudioClip clip, float volume = 1f)
        {
            BackgroundMusic.clip = clip;
            BackgroundMusic.Play();

            BackgroundMusic.volume = volume;

            CurrentlyPlayingBackgroundMusic = true;
        }

        /// <summary>
        /// Instantly stops the currently playing background music
        /// </summary>
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
            while (source.volume != 1f)
            {
                source.volume += Time.deltaTime / crossfadeTime;
                yield return null;
            }
        }

        private IEnumerator CrossfadeAudioSourceDown(AudioSource source, float crossfadeTime = 1f, bool deleteOnDone = true)
        {
            while (source.volume != 0f)
            {
                source.volume -= Time.deltaTime / crossfadeTime;
                yield return null;
            }

            if (deleteOnDone) Destroy(source.gameObject);
        }
    }
}