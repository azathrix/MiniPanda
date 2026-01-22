using UnityEngine;
using Azathrix.MiniPanda;

namespace Azathrix.MiniPanda.Samples
{
    /// <summary>
    /// Demo 基类，提供 MiniPanda 实例和通用功能
    /// </summary>
    public abstract class DemoBase : MonoBehaviour
    {
        protected MiniPanda _panda;

        protected virtual void Start()
        {
            _panda = new MiniPanda();
            _panda.Start();
            RunDemo();
        }

        protected virtual void OnDestroy()
        {
            _panda?.Shutdown();
        }

        protected abstract void RunDemo();

        protected void Log(string message)
        {
            UnityEngine.Debug.Log($"[{GetType().Name}] {message}");
        }
    }
}
