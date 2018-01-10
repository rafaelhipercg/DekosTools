/*
MIT License

Copyright (c) 2017 Rafael cosentino garcia

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

//Sound Player V2.1.0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DekosTools.Audio
{
    /// <summary>
    /// Sound Player made for use in 2D or 3D games
    /// </summary>
    public class SoundPlayer : MonoBehaviour
    {
        static SoundPlayer instance;
        public static SoundPlayer Instance
        {
            get
            {
                return instance;
            }
        }

        public AudioClip[] SoundEffects;
        public AudioClip[] BackgroundMusic;

        List<AudioSource> SoundEffectCurrentPlaying;
        Dictionary<string, float> SoundEffectVolume;
        List<AudioSource> BackgroundMusicCurrentPlaying;
        Dictionary<string, float> BackgroundMusicVolume;

        public bool Mute
        {
            get
            {
                return (MuteSFX || muteBGM);
            }

            set
            {
                MuteSFX = value;
                muteBGM = value;
            }
        }

        static bool muteSFX = false;
        public bool MuteSFX
        {
            get
            {
                return muteSFX;
            }

            set
            {
                if (value)
                {
                    SoundEffectCurrentPlaying.ForEach((i) => i.volume = 0);
                }
                else
                {
                    SoundEffectCurrentPlaying.ForEach((i) =>
                    {
                        float Vol = 0;
                        SoundEffectVolume.TryGetValue(i.clip.name, out Vol);
                        i.volume = Vol;
                    });
                }
                muteSFX = value;
            }
        }

        static bool muteBGM = false;
        public bool MuteBGM
        {
            get
            {
                return muteBGM;
            }

            set
            {
                if (value)
                {
                    BackgroundMusicCurrentPlaying.ForEach((i) => i.volume = 0);
                }
                else
                {
                    BackgroundMusicCurrentPlaying.ForEach((i) =>
                    {
                        float Vol = 0;
                        BackgroundMusicVolume.TryGetValue(i.clip.name, out Vol);
                        i.volume = Vol;
                    });
                }
                muteBGM = value;
            }
        }

        void Awake()
        {
            if (instance != null)
            {
                this.BackgroundMusicCurrentPlaying = instance.BackgroundMusicCurrentPlaying;
                this.SoundEffectCurrentPlaying = instance.SoundEffectCurrentPlaying;
                BackgroundMusicCurrentPlaying.RemoveAll((i) => i == null);
                SoundEffectCurrentPlaying.RemoveAll((i) => i == null);
                DestroyImmediate(instance);
            }
            else
            {
                SoundEffectCurrentPlaying = new List<AudioSource>();
                BackgroundMusicCurrentPlaying = new List<AudioSource>();
                SoundEffectVolume = new Dictionary<string, float>();
                BackgroundMusicVolume = new Dictionary<string, float>();
            }
            instance = this;
        }

        /// <summary>
        /// Play Sound Effect
        /// </summary>
        /// <param name="ID">ID of the list</param>
        /// <param name="OnTheObject">Object when the audio will play</param>
        /// <param name="volume">AudioSource volume</param>
        /// <param name="pitch">AudioSource Pitch</param>
        /// <param name="spatialBlend">AudioSource SpatialBlend</param>
        /// <param name="bypassEffects">AudioSource BypassEffects</param>
        /// <param name="bypassListenerEffects">AudioSource BypassListenerEffects</param>
        /// <param name="bypassReverbZones">AudioSource BypassReverbZones</param>
        /// <param name="Group">Audio Mixer Group</param>
        public void PlaySoundEffect(int ID,
            GameObject OnTheObject,
            float volume = 0.5f,
            float pitch = 1,
            float spatialBlend = 0,
            bool bypassEffects = true,
            bool bypassListenerEffects = true,
            bool bypassReverbZones = true,
            AudioMixerGroup Group = null
            )
        {
            AudioSource Source = OnTheObject.AddComponent<AudioSource>();
            Source.clip = SoundEffects[ID];
            Source.spatialBlend = spatialBlend;
            Source.volume = muteSFX ? 0 : volume;
            Source.pitch = pitch;
            Source.bypassEffects = bypassEffects;
            Source.bypassListenerEffects = bypassListenerEffects;
            Source.bypassReverbZones = bypassReverbZones;
            Source.loop = false;
            Source.outputAudioMixerGroup = Group;
            Source.Play();
            SoundEffectVolume.Remove(SoundEffects[ID].name);
            SoundEffectVolume.Add(SoundEffects[ID].name, volume);
            StartCoroutine(SoundEffectCurrentPlaying_Controller(Source));
        }

        /// <summary>
        /// Play Sound Effect by name
        /// </summary>
        /// <param name="name">Name in the list<</param>
        /// <param name="OnTheObject">Object when the audio will play</param>
        /// <param name="volume">AudioSource volume</param>
        /// <param name="pitch">AudioSource Pitch</param>
        /// <param name="spatialBlend">AudioSource SpatialBlend</param>
        /// <param name="bypassEffects">AudioSource BypassEffects</param>
        /// <param name="bypassListenerEffects">AudioSource BypassListenerEffects</param>
        /// <param name="bypassReverbZones">AudioSource BypassReverbZones</param>
        public void PlaySoundEffect(string name,
            GameObject OnTheObject,
            float volume = 0.5f,
            float pitch = 1,
            float spatialBlend = 0,
            bool bypassEffects = true,
            bool bypassListenerEffects = true,
            bool bypassReverbZones = true,
            AudioMixerGroup Group = null
            )
        {

            for (int i = 0; i < SoundEffects.Length; i++)
            {
                if (SoundEffects[i].name == name)
                {
                    PlaySoundEffect(i,
                        OnTheObject,
                        volume,
                        pitch,
                        spatialBlend,
                        bypassEffects,
                        bypassListenerEffects,
                        bypassReverbZones,
                        Group);
                    break;
                }
            }
        }

        IEnumerator SoundEffectCurrentPlaying_Controller(AudioSource Source)
        {
            SoundEffectCurrentPlaying.Add(Source);
            yield return new WaitForSeconds(Source.clip.length);
            Destroy(Source);
            SoundEffectCurrentPlaying.Remove(Source);
        }

        /// <summary>
        /// Play Background Music By ID
        /// </summary>
        /// <param name="ID">ID of the list</param>
        /// <param name="OnTheObject">Object when the audio will play</param>
        /// <param name="volume">AudioSource volume</param>
        /// <param name="pitch">AudioSource Pitch</param>
        /// <param name="spatialBlend">AudioSource SpatialBlend</param>
        /// <param name="bypassEffects">AudioSource BypassEffects</param>
        /// <param name="bypassListenerEffects">AudioSource BypassListenerEffects</param>
        /// <param name="bypassReverbZones">AudioSource BypassReverbZones</param>
        /// <param name="Group">Audio Mixer Group</param>
        public void PlayBackgroundMusic(int ID,
            GameObject OnTheObject,
            float volume = 0.5f,
            float pitch = 1,
            float spatialBlend = 0,
            bool bypassEffects = true,
            bool bypassListenerEffects = true,
            bool bypassReverbZones = true,
            AudioMixerGroup Group = null
            )
        {
            AudioSource Source = OnTheObject.AddComponent<AudioSource>();
            Source.clip = BackgroundMusic[ID];
            Source.spatialBlend = spatialBlend;
            Source.volume = muteBGM ? 0 : volume;
            Source.pitch = pitch;
            Source.bypassEffects = bypassEffects;
            Source.bypassListenerEffects = bypassListenerEffects;
            Source.bypassReverbZones = bypassReverbZones;
            Source.outputAudioMixerGroup = Group;
            Source.loop = true;
            Source.Play();
            SoundEffectVolume.Remove(BackgroundMusic[ID].name);
            BackgroundMusicVolume.Add(BackgroundMusic[ID].name, volume);
            BackgroundMusicCurrentPlaying.Add(Source);

        }

        /// <summary>
        /// Play Background Music by name
        /// </summary>
        /// <param name="name">Name in the list<</param>
        /// <param name="OnTheObject">Object when the audio will play</param>
        /// <param name="volume">AudioSource volume</param>
        /// <param name="pitch">AudioSource Pitch</param>
        /// <param name="spatialBlend">AudioSource SpatialBlend</param>
        /// <param name="bypassEffects">AudioSource BypassEffects</param>
        /// <param name="bypassListenerEffects">AudioSource BypassListenerEffects</param>
        /// <param name="bypassReverbZones">AudioSource BypassReverbZones</param>
        /// <param name="Group">Audio Mixer Group</param>
        public void PlayBackgroundMusic(string name,
            GameObject OnTheObject,
            float volume = 0.5f,
            float pitch = 1,
            float spatialBlend = 0,
            bool bypassEffects = true,
            bool bypassListenerEffects = true,
            bool bypassReverbZones = true,
            AudioMixerGroup Group = null)
        {

            for (int i = 0; i < BackgroundMusic.Length; i++)
            {
                if (BackgroundMusic[i].name == name)
                {
                    PlayBackgroundMusic(i,
                        OnTheObject,
                        volume,
                        pitch,
                        spatialBlend,
                        bypassEffects,
                        bypassListenerEffects,
                        bypassReverbZones,
                        Group);
                    break;
                }
            }
        }

        public void StopBackgroundMusic(string Name)
        {
            List<AudioSource> Sources = BackgroundMusicCurrentPlaying.FindAll((i) => (i.clip.name == Name));
            BackgroundMusicCurrentPlaying.RemoveAll((i) => Sources.Contains(i));
            Sources.ForEach((i) => Destroy(i));
        }

    }
}
