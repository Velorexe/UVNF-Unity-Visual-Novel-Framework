using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace UVNF.Core
{
    /// <summary>
    /// A manager class that handles audio, such as background music and SFX's
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public AudioSource Music;
        public AudioSource Dialogue;

        [Space]
        public AudioMixerGroup SfxGroup;
        public Transform SfxParent;

        private bool _currentlyPlayingMusic = false;

        #region Music

        /// <summary>
        /// Instantly plays the given <see cref="AudioClip"/> as music
        /// </summary>
        /// <param name="musicClip">The clip of music that should be played</param>
        /// <param name="volume">The volume at which the music should play at</param>
        /// <param name="loop">Set to <see langword="true"/> if the music should loop when it ends</param>
        public void PlayMusic(AudioClip musicClip, float volume = 1f, bool loop = true)
        {
            if (musicClip == null)
            {
                Debug.LogError("Parameter musicClip was null, can't play music");
                return;
            }

            if (volume < 0f || volume > 1f)
            {
                Debug.LogWarning("Parameter volume was not between 0f and 1f, clamping given volume");
                volume = Mathf.Clamp(volume, 0f, 1f);
            }

            Music.Stop();

            Music.volume = volume;
            Music.clip = musicClip;
            Music.loop = loop;

            Music.Play();

            _currentlyPlayingMusic = true;
        }

        /// <summary>
        /// Fades in the given <see cref="AudioClip"/> as music
        /// </summary>
        /// <param name="musicClip">The clip of music that should be played</param>
        /// <param name="fadeInTime">The time it should take for the music to fade in</param>
        /// <param name="volume">The volume at which the music should play at</param>
        /// <param name="loop">Set to <see langword="true"/> if the music should loop when it ends</param>
        /// <returns>A Unity <see cref="Coroutine"/></returns>
        public IEnumerator PlayMusic(AudioClip musicClip, float fadeInTime, float volume = 1f, bool loop = true)
        {
            if (musicClip == null)
            {
                Debug.LogError("Parameter musicClip was null, can't play music");
                yield break;
            }

            if (volume < 0f || volume > 1f)
            {
                Debug.LogWarning("Parameter volume was not between 0f and 1f, clamping given volume");
                volume = Mathf.Clamp(volume, 0f, 1f);
            }

            fadeInTime /= 2f;

            if (_currentlyPlayingMusic)
            {
                while (Music.volume > 0f)
                {
                    Music.volume -= Time.deltaTime / fadeInTime;
                    yield return null;
                }

                Music.Stop();
            }

            Music.volume = 0f;

            Music.clip = musicClip;
            Music.loop = loop;

            Music.Play();

            _currentlyPlayingMusic = true;

            while (Music.volume < volume)
            {
                Music.volume += Time.deltaTime / fadeInTime;
                yield return null;
            }
        }

        /// <summary>
        /// Instantly pauses the currently playing music
        /// </summary>
        public void PauseMusic()
        {
            Music.Pause();
            _currentlyPlayingMusic = false;
        }

        /// <summary>
        /// Fades the currently playing music out
        /// </summary>
        /// <param name="fadeOutTime">The time it should take for the music to fade out</param>
        public IEnumerator PauseMusic(float fadeOutTime)
        {
            if (_currentlyPlayingMusic)
            {
                while (Music.volume > 0f)
                {
                    Music.volume -= Time.deltaTime / fadeOutTime;
                    yield return null;
                }

                Music.Stop();

                _currentlyPlayingMusic = false;
            }
        }

        #endregion Music

        #region Dialogue

        /// <summary>
        /// Plays the given <see cref="AudioClip"/> as dialogue
        /// </summary>
        /// <param name="dialogueClip">The clip of dialogue that should be played</param>
        /// <param name="volume">The volume at which the dialogue should play at</param>
        public void PlayDialogue(AudioClip dialogueClip, float volume = 1f)
        {
            if (dialogueClip == null)
            {
                Debug.LogError("Parameter dialogueClip was null, can't play dialogue");
                return;
            }

            if (volume < 0f || volume > 1f)
            {
                Debug.LogWarning("Parameter volume was not between 0f and 1f, clamping given volume");
                volume = Mathf.Clamp(volume, 0f, 1f);
            }

            Dialogue.Stop();

            Dialogue.volume = volume;
            Dialogue.clip = dialogueClip;

            Dialogue.Play();
        }

        /// <summary>
        /// Pauses the currently playing dialogue
        /// </summary>
        public void PauseDialogue()
        {
            Dialogue.Pause();
        }

        #endregion

        #region SFX

        /// <summary>
        /// Instantly plays a given <see cref="AudioClip"/> as SFX
        /// </summary>
        /// <param name="sfxClip">The SFX clip that should be played</param>
        /// <param name="volume">The volume at which the SFX should be play at</param>
        public IEnumerator PlaySfx(AudioClip sfxClip, float volume = 1f)
        {
            if (sfxClip == null)
            {
                Debug.LogError("Parameter sfxClip was null, can't play SFX");
                yield break;
            }

            if (volume < 0f || volume > 1f)
            {
                Debug.LogWarning("Parameter volume was not between 0f and 1f, clamping given volume");
                volume = Mathf.Clamp(volume, 0f, 1f);
            }

            GameObject sfxObject = new GameObject(sfxClip.name);
            sfxObject.transform.SetParent(SfxParent, false);

            AudioSource source = sfxObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = SfxGroup;

            source.clip = sfxClip;
            source.volume = volume;

            source.loop = false;

            source.Play();

            while (source.isPlaying)
            {
                yield return null;
            }
        }

        #endregion
    }
}