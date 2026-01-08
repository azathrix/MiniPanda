using System.IO;
using UnityEngine;
using UnityEditor;
using Azathrix.MiniPanda.Core;

namespace Azathrix.MiniPanda.Samples
{
    /// <summary>
    /// 模块导入示例：import/export
    /// </summary>
    public class ModuleDemo : DemoBase
    {
        private string _samplesPath;

        protected override void Start()
        {
            _samplesPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(this)));
            base.Start();
        }

        protected override void RunDemo()
        {
            Log("=== 模块导入示例 ===");

            LoadModules();

            _panda.Run(@"
                import ""utils"" as u
                print(""Utils VERSION: "" + u.VERSION)
                u.helper()
                print(""clamp(15, 0, 10) = "" + u.clamp(15, 0, 10))
            ");

            _panda.Run(@"
                import ""math.vector"" as vec
                var v1 = vec.create(3, 4, 0)
                var v2 = vec.create(1, 2, 0)
                var v3 = vec.add(v1, v2)
                print(""v1 + v2 = ("" + v3.x + "", "" + v3.y + "", "" + v3.z + "")"")
            ");
        }

        void LoadModules()
        {
            LoadModuleIfExists("utils.panda", "utils");
            LoadModuleIfExists("math/vector.panda", "math.vector");
            LoadModuleIfExists("example.panda", "example");
        }

        void LoadModuleIfExists(string relativePath, string moduleName)
        {
            var fullPath = Path.Combine(_samplesPath, "..", relativePath);
            if (File.Exists(fullPath))
            {
                var normalizedPath = Path.GetFullPath(fullPath).Replace("\\", "/");
                _panda.LoadModule(File.ReadAllBytes(fullPath), moduleName, normalizedPath);
            }
        }
    }
}
