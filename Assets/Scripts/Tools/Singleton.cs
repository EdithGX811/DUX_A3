using UnityEngine;

    /// <summary>
    /// 单例模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : MonoBehaviour where T :  Singleton<T> {
        private static T _instance;

        public static T Instance => _instance;

        protected virtual void Awake() {
            _instance = (T)this;
        }

        protected virtual void OnDestory() {
            _instance = null;
        }
    }
