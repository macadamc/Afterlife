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


        public AudioSource s;

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

        [Button("Play", ButtonSizes.Large)]
        public void DebugPlay()
        {
            if (clip == null)
                return;

            if (s == null)
                s = new AudioSource();

            s.clip = clip;
            s.volume = Random.Range(volume.x, volume.y);
            s.pitch = Random.Range(pitch.x, pitch.y);
            s.outputAudioMixerGroup = mixerGroup;
            s.Play();
        }
    }
}

