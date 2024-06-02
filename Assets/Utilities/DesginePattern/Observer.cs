///
/// Copyright (c) 2024 Vikas Reddy Thota
///

using UnityEngine;
using System.Collections.Generic;

namespace DesignPatterns
{
    /// <summary>
    /// This is a class that can be inherited into other class to use observers
    /// </summary>
    public class Observer : MonoBehaviour
    {
        //private variable with list of Observers 
        private List<IObserver> _observers;

        /// <summary>
        /// protected unity function
        /// called only once
        /// </summary>
        protected virtual void Awake()
        {
            _observers = new List<IObserver>();
        }

        /// <summary>
        /// public function to add observer to list
        /// </summary>
        /// <param name="observer">observer to add</param>
        public void RegisterObserver(IObserver observer)
        {
            //Add observer to list
            _observers.Add(observer);
        }

        /// <summary>
        /// public function to remove observer from list
        /// </summary>
        /// <param name="observer">observer to remove</param>
        public void UnregisterObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        /// <summary>
        /// public function to invoke all the observers in the list
        /// </summary>
        public void NotifyObservers()
        {
            //total length of the observer list
            int count = _observers.Count;

            //loop through all the observers and invoke them
            for (int i = 0; i < count; i++)
                _observers[i]?.OnNotify();
        }
    }

    //interface for observer class
    public interface IObserver
    {
        //On Notify Function
        void OnNotify();
    }
}