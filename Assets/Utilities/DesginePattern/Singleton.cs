///
/// Copyright (c) 2024 Vikas Reddy Thota
/// ---- NOTE : Use override method to add "DontDestroyOnLoad" this class does not use it
///

using UnityEngine;

namespace DesignPatterns
{
    /// <summary>
    /// This is a class that can be inherited into other class to use Singleton feature
    /// This class will also be created if the base class gameObject does not exits
    /// </summary>
    /// <typeparam name="T">Generic type : base class can be parsed</typeparam>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        //private variable of the base class
        private static T _instance = null;

        //public getter variable of the base class
        public static T Instance
        {
            get
            {
                //returns the private instance of a base class
                return _instance;
            }
        }

        /// <summary>
        /// Protected unity function 
        /// called 
        /// </summary>
        protected virtual void Awake()
        {
            //checking if instance exists or else assign it
            if (_instance == null)
            {
                //assign generic class to the variable
                _instance = this as T;
            }
            else if (_instance != this)
            {
                //if instance already exits delete this gameObject
                Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// Protected unity function 
        /// called when object is Destroyed
        /// </summary>
        protected virtual void OnDestroy()
        {
            //if instance not null then set it to null
            if (_instance) _instance = null;
        }
    }
}
