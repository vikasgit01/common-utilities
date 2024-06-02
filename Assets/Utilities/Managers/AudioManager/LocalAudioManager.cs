///
/// Copyright (c) 2024 Vikas Reddy Thota
///

namespace GameManagers
{

    /// <summary>
    /// This is a class for audio to use only when necessary 
    /// For Example add to a prefab or scene that uses audio only when is loaded or created
    /// game audio Play, pause or stop.
    /// </summary>
    public class LocalAudioManager : BaseAudioManager
    {
        /// <summary>
        /// Protected unity override function
        /// </summary>
        protected override void Awake()
        {
            //call function from base class
            base.Awake();
        }
    }
}
