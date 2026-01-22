using System.IO;
using UnityEngine;

namespace Azathrix.MiniPanda.Samples
{
    /// <summary>
    /// 模块导入示例：import/export
    /// </summary>
    public class ModuleDemo : DemoBase
    {
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
            LoadModuleIfExists("utils");
            LoadModuleIfExists("math.vector");
            LoadModuleIfExists("example");
        }

        void LoadModuleIfExists(string moduleName)
        {
            var fullPath = Path.Combine("PandaScripts", moduleName.Replace(".", "/")).Replace("\\", "/") + ".panda";
            var ta = Resources.Load<TextAsset>(fullPath);
            if (ta != null)
            {
                _panda.LoadModule(ta.bytes, moduleName, fullPath);
            }
            else
            {
                Log("未找到模块:" + moduleName);
            }
        }
    }
}