using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using Azathrix.MiniPanda.Debug.DAP;

namespace Azathrix.MiniPanda.Samples
{
    /// <summary>
    /// 调试示例：启动调试服务器，支持 VSCode 断点调试
    /// </summary>
    public class DebugDemo : DemoBase
    {
        [Header("调试设置")]
        [Tooltip("是否启用调试服务器")]
        public bool enableDebugServer = true;

        [Tooltip("调试端口")]
        public int debugPort = 4711;

        private DebugServer _debugServer;
        private Task _task;
        private string _samplesPath;

        protected override void Start()
        {
            _samplesPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(this)));

            _task = Task.Run(() =>
            {
                try
                {
                    _panda = new MiniPanda();
                    _panda.Start();

                    if (enableDebugServer)
                    {
                        _debugServer = new DebugServer(_panda.VM);
                        _debugServer.Start();
                        //这是一个同步方法，会阻断当前线程，直到调试器连接
                        //_debugServer.WaitForReady();
                        UnityEngine.Debug.Log($"[DebugDemo] 调试服务器已启动，端口: {debugPort}");
                        UnityEngine.Debug.Log("[DebugDemo] 请在 VSCode 中连接调试器");
                    }

                    RunDemo();
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"[DebugDemo] 异常: {ex}");
                }
            });
        }

        protected override void OnDestroy()
        {
            _debugServer?.Stop();
            base.OnDestroy();
            _task?.Dispose();
        }

        protected override void RunDemo()
        {
            Log("=== 调试示例 ===");

            // 运行一个简单的脚本，可以在 VSCode 中设置断点
            _panda.Run(@"
                var count = 0
                for i in [1, 2, 3, 4, 5] {
                    count = count + i
                    print(""count = "" + count)
                }
                print(""最终结果: "" + count)
            ");

            // 运行外部脚本文件（可以在文件中设置断点）
            var scriptPath = Path.Combine(_samplesPath, "..", "example.panda");
            if (File.Exists(scriptPath))
            {
                Log("运行 example.panda");
                _panda.Run(File.ReadAllBytes(scriptPath));
            }
        }
    }
}
