///
/// Copyright (c) 2024 Vikas Reddy Thota
///

using System;
using UnityEngine;

namespace GameManagers
{

    [CreateAssetMenu(fileName = "AudioManagerData", menuName = "ScriptableObjects/AudioManagerData", order = 1)]
    public class AudioManagerData : ScriptableObject
    {
        public SoundClip[] sounds;
        public SoundClip[] GetSounds() => sounds;
    }

    //class for configuring the sound that are add to the application
    [Serializable]
    public class SoundClip : AudioSourceConfig
    {
        public string soundId; //name of the sound
        public AudioClip clip; //audio clip for this sound

        public SOUNDTYPE soundType = SOUNDTYPE.NONE; //soundType of the sound
        public bool createNewSource = true; //should it have new source
    }

    [Serializable]
    public class Source : AudioSourceConfig
    {
        public SOUNDTYPE soundType { get; set; } //soundType of the sound
    }

    //base class for sound
    [Serializable]
    public class AudioSourceConfig
    {
        public bool loop; //will this sound be looping ***IGNORED >> if createNewSource is false << IGNORED***

        [Range(0f, 1f)]
        public float volume; //volume of the sound ***IGNORED >> if createNewSource is false << IGNORED***
        [Range(.1f, 3f)]
        public float pitch; //pitch of the sound ***IGNORED >> if createNewSource is false << IGNORED***

        public AudioSource source { get; set; } //audio source of the sound
    }

    //Sound Types of the sound added
    public enum SOUNDTYPE
    {
        NONE,
        MUSIC,
        SFX
    }

}
