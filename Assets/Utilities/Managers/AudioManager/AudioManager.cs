///
/// Copyright (c) 2024 Vikas Reddy Thota
///

namespace GameManagers
{

    /// <summary>
    /// This is a singleton class for game audio Play, pause or stop.
    /// </summary>
    public class AudioManager : BaseAudioManager
    {
        //private variable of this class
        private static AudioManager _instance = null;

        //public getter variable of this class
        public static AudioManager Instance
        {
            get
            {
                //returns the private instance of a base class
                return _instance;
            }
        }

        /// <summary>
        /// Protected unity override function
        /// </summary>
        protected override void Awake()
        {
            //checking if instance exists or else assign it
            if (_instance == null)
            {
                //assign generic class to the variable
                _instance = this;
                //Keep this instance thorought game
                DontDestroyOnLoad(this);
            }
            else if (_instance != this)
            {
                //if instance already exits delete this gameObject
                Destroy(this.gameObject);
            }

            //call function from base class
            base.Awake();
        }

        /// <summary>
        /// Protected unity function 
        /// called when object is Destroyed
        /// </summary>
        private void OnDestroy()
        {
            //if instance not null then set it to null
            if (_instance) _instance = null;
        }
    }

}
