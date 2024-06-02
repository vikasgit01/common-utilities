///
/// Copyright (c) 2024 Vikas Reddy Thota
///

using UnityEngine;

namespace GameManagers
{
    //class that saves state of music and sfx sounds
    public class AudioPrefs
    {

        //used to turn on and off the music in application
        //False - OFF
        //True - ON
        public static bool MusicSwitch
        {
            get => PlayerPrefs.GetInt("MusicSwitch", 1) == 1;
            set => PlayerPrefs.SetInt("MusicSwitch", value ? 1 : 0);
        }

        //used to turn on and off the sfx in application
        //False - OFF
        //True - ON
        public static bool SFXSwitch
        {
            get => PlayerPrefs.GetInt("SFXSwitch", 1) == 1;
            set => PlayerPrefs.SetInt("SFXSwitch", value ? 1 : 0);
        }
    }
}
