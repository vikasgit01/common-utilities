///
/// Copyright (c) 2024 Vikas Reddy Thota
///

using UnityEngine;
using System;

namespace GameManagers
{
    /// <summary>
    /// This is a class that can be used to manage audio Play, pause or stop.
    /// </summary>
    public class BaseAudioManager : MonoBehaviour
    {
        [Tooltip("Attach AudioManagerData Scriptable here with data configured as required")]
        [SerializeField] private AudioManagerData _audioManagerData;

        [Tooltip("Class to configure data for single audio Source. No need to add clip or SoundId for this")]
        [SerializeField] private Source _sfxAudioSource;

        //Private variable for array of sound class to config the audio clip
        private SoundClip[] _sounds;

        /// <summary>    
        /// protected unity function
        /// called only once
        /// </summary>
        protected virtual void Awake()
        {
            if (_audioManagerData == null || _audioManagerData.GetSounds() == null || _audioManagerData.GetSounds().Length <= 0)
                return;

            _sounds = _audioManagerData.GetSounds();

            //set sfx audio source
            SetUpSFXSound();

            //set sounds audio source
            SetUpOtherSound();
        }

        /// <summary>
        /// function is used to setup common audio source for sfx
        /// </summary>
        private void SetUpSFXSound()
        {
            //check if audio source exits
            if (_sfxAudioSource != null)
            {
                //adds common Audio source for sfx sounds
                _sfxAudioSource.source = gameObject.AddComponent<AudioSource>();

                //set settings for the audio source.
                _sfxAudioSource.source.loop = _sfxAudioSource.loop;

                _sfxAudioSource.source.volume = _sfxAudioSource.volume;
                _sfxAudioSource.source.pitch = _sfxAudioSource.pitch;
            }
        }

        /// <summary>
        /// function is used to setup other sounds for sfx and music
        /// </summary>
        private void SetUpOtherSound()
        {
            //return if sound is null
            if (_sounds == null) return;

            //Count of list of sounds
            int count = _sounds.Length;

            //loop through each sound and add a audioSource for it
            for (int i = 0; i < count; i++)
            {
                //check if the sound needs a new source or use common source
                if (_sounds[i].createNewSource)
                {
                    //Setup audio source of the sound
                    _sounds[i].source = gameObject.AddComponent<AudioSource>();
                    _sounds[i].source.clip = _sounds[i].clip;
                    _sounds[i].source.loop = _sounds[i].loop;

                    _sounds[i].source.volume = _sounds[i].volume;
                    _sounds[i].source.pitch = _sounds[i].pitch;
                }
                else
                {
                    //setup audio source for the sound as the common source
                    _sounds[i].source = _sfxAudioSource.source;
                }
            }
        }

        /// <summary>
        /// Function to play the sound
        /// </summary>
        /// <param name="name">name of the sound to play</param>
        public void Play(String name)
        {
            //return if sound is null
            if (_sounds == null) return;

            //find the sound from the array of sounds and return the name that matches with param
            SoundClip s = Array.Find(_sounds, sound => sound.soundId == name);

            //s is null then return
            if (s == null) return;

            //check if sound type is None and assign sound type from sound if empty
            //this step is when createNewSource is false
            if (!s.createNewSource)
                _sfxAudioSource.soundType = s.soundType;

            //check if clip is empty and assign the clip from sound if empty
            //this step is when createNewSource is false
            if (s.source.clip)
                s.source.clip = s.clip;

            //check the sound type of the sound
            switch (s.soundType)
            {
                case SOUNDTYPE.MUSIC:
                    //music switch is on then play the sound
                    if (AudioPrefs.MusicSwitch)
                    {
                        //play the audio
                        s.source.Play();
                    }
                    break;
                case SOUNDTYPE.SFX:
                    //sfx switch is on then play the sound
                    if (AudioPrefs.SFXSwitch)
                    {
                        //play the audio
                        s.source.Play();
                    }
                    break;
            }
        }

        /// <summary>
        /// Function to stop the sound
        /// </summary>
        /// <param name="name">name of the sound to stop if playing</param>
        public void Stop(String name)
        {
            //return if sound is null
            if (_sounds == null) return;

            //find the sound from the array of sounds and return the name that matches with param
            SoundClip s = Array.Find(_sounds, sound => sound.soundId == name);

            //s is null then return
            if (s == null) return;

            //stop the audio if audio is playing
            if (s.source.isPlaying) s.source.Stop();

            #region SpecificSoundType Logic
            //check the sound type of the sound
           /* switch (s.soundType)
            {
                case SOUNDTYPE.MUSIC:
                    //stop the audio if audio is playing
                    if (s.source.isPlaying) s.source.Stop();
                    break;
                case SOUNDTYPE.SFX:
                    //stop the audio if audio is playing
                    if (s.source.isPlaying) s.source.Stop();
                    break;
            }*/
            #endregion
        }

        /// <summary>
        /// Function to pause the sound
        /// </summary>
        /// <param name="name">name of the sound to pause if playing</param>
        public void Pause(string name)
        {
            //return if sound is null
            if (_sounds == null) return;

            //find the sound from the array of sounds and return the name that matches with param
            SoundClip s = Array.Find(_sounds, sound => sound.soundId == name);

            //s is null then return
            if (s == null) return;

            if (s.source.isPlaying) s.source.Pause();

            #region SpecificSoundType Logic

            //check the sound type of the sound
            /*switch (s.soundType)
            {
                case SOUNDTYPE.MUSIC:
                    //Pause the audio if audio is playing
                    if (s.source.isPlaying) s.source.Pause();
                    break;
                case SOUNDTYPE.SFX:
                    //Pause the audio if audio is playing
                    if (s.source.isPlaying) s.source.Pause();
                    break;
            }*/
            #endregion
        }

    }

}