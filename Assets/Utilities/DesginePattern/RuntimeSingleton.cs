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
    public class RuntimeSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        //private variable to store instance of the base(T) class
        private static T _instance = null;
        //private variable to check if application is quitting or obj is destroyed 
        private static bool _applicationIsQuitting = false;

        //public getter variable of the base class
        public static T Instance
        {
            get
            {
                //checking if application is quitting
                if (_applicationIsQuitting) return null;

                //check for instance is null
                if (_instance == null)
                {
                    //set the instance of generic type passed if already exits in the application
                    _instance = (T)FindObjectOfType(typeof(T));

                    //if instance is still null
                    if (_instance == null)
                    {
                        //create gameobject and add component of the type T
                        GameObject singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";
                    }
                }

                //returns the private instance of a base class
                return _instance;
            }
        }

        /// <summary>
        /// Protected unity function 
        /// called only once
        /// </summary>
        protected virtual void Awake()
        {
            //checking if instance exists or else asign it
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
            _applicationIsQuitting = true;
            //instance not null then set it to null
            if (_instance) _instance = null;
        }

        /// <summary>
        /// Protected unity function
        /// called when application is quit
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
            //instance not null then set it to null
            if (_instance) _instance = null;
        }
    }
}