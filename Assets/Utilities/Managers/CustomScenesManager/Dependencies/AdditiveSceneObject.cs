///
/// Copyright (c) 2024 Vikas Reddy Thota
///

using UnityEngine;

namespace GameManagers
{

    /// <summary>
    /// Class that is used for objects that need to be disabled when scene is loaded additively
    /// </summary>
    public class AdditiveSceneObject : AdditiveSceneBase
    {
       
    }

    /// <summary>
    /// Base class for Enable and Disable object with this class inherited
    /// </summary>
    public class AdditiveSceneBase : MonoBehaviour, IAdditiveObject
    {
        //function set GameObject off
        public void Show(bool value) => this.gameObject.SetActive(value);
    }

    public interface IAdditiveObject
    {
        public void Show(bool value);
    }
}
