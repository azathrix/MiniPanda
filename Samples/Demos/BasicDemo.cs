using UnityEngine;

namespace Azathrix.MiniPanda.Samples
{
    /// <summary>
    /// 基础语法示例：变量、数组、对象
    /// </summary>
    public class BasicDemo : DemoBase
    {
        protected override void RunDemo()
        {
            Log("=== 基础示例 ===");

            // 变量和运算
            _panda.Run(@"
                var x = 10
                var y = 20
                print(""x + y = "" + (x + y))
            ");

            // 数组
            _panda.Run(@"
                var arr = [1, 2, 3, 4, 5]
                var sum = 0
                for n in arr {
                    sum = sum + n
                }
                print(""数组求和: "" + sum)
            ");

            // 对象
            _panda.Run(@"
                var player = {name: ""Hero"", hp: 100, mp: 50}
                print(""玩家: "" + player.name + "", HP: "" + player.hp)
            ");

            // 字符串插值
            _panda.Run(@"
                var name = ""MiniPanda""
                var version = ""1.0""
                print(""Welcome to {name} v{version}!"")
            ");
        }
    }
}
