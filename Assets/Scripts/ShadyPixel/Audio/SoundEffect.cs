using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Audio;

namespace ShadyPixel.Audio
{
    [ExecuteInEditMode]
    [CreateAssetMenu(menuName = "ShadyPixel/Audio/New Sound Effect")]
    public class SoundEffect : ScriptableObject
    {
        [PropertyTooltip("AudioClip that will get passed into AudioSource.")]
        public AudioClip clip;

        [MinMaxSlider(0f, 1f, true)]
        [PropertyTooltip("Possible volume range of audioclip when passed into an AudioSource.")]
        public Vector2 volume = Vector2.one;                                                     //  x component is min value, y componenet is max value

        [MinMaxSlider(0f, 2f, true)]
        [PropertyTooltip("Possible pitch range of audioclip when passed into an AudioSource.")]
        public Vector2 pitch = Vector2.one;                                                     //  x component is min value, y componenet is max value

        [PropertyTooltip("AudioMixerGroup that gets passed to the AudioSource.")]
        public AudioMixerGroup mixerGroup;

        [Button("DebugPlay", ButtonSizes.Large)]
        public void DebugPlay()
        {
            FindObjectOfType<AudioManager>().PlaySFX(this);
        }

        public void Play(AudioSource source)
        {
            if (clip == null)
                return;

            source.clip = clip;
            source.volume = Random.Range(volume.x, volume.y);
            source.pitch = Random.Range(pitch.x, pitch.y);
            source.outputAudioMixerGroup = mixerGroup;

            source.Play();
        }
    }
}

