using UnityEngine;

namespace Azathrix.MiniPanda.Samples
{
    /// <summary>
    /// MiniPanda 演示入口
    /// 选择要运行的示例类型，或查看 Demos 目录下的独立示例
    /// </summary>
    public class MiniPandaDemo : DemoBase
    {
        [Header("示例选择")]
        [Tooltip("选择要运行的示例")]
        public DemoType demoType = DemoType.All;

        public enum DemoType
        {
            All,
            Basic,
            Function,
            Class,
            Interop,
            Advanced
        }

        protected override void RunDemo()
        {
            switch (demoType)
            {
                case DemoType.All:
                    BasicExample();
                    FunctionExample();
                    ClassExample();
                    InteropExample();
                    AdvancedExample();
                    break;
                case DemoType.Basic:
                    BasicExample();
                    break;
                case DemoType.Function:
                    FunctionExample();
                    break;
                case DemoType.Class:
                    ClassExample();
                    break;
                case DemoType.Interop:
                    InteropExample();
                    break;
                case DemoType.Advanced:
                    AdvancedExample();
                    break;
            }
        }

        void BasicExample()
        {
            Log("=== 基础示例 ===");
            _panda.Run(@"
                var x = 10
                var y = 20
                print(""x + y = "" + (x + y))

                var arr = [1, 2, 3, 4, 5]
                var sum = 0
                for n in arr { sum = sum + n }
                print(""数组求和: "" + sum)
            ");
        }

        void FunctionExample()
        {
            Log("=== 函数示例 ===");
            _panda.Run(@"
                func factorial(n) {
                    if n <= 1 return 1
                    return n * factorial(n - 1)
                }
                print(""5! = "" + factorial(5))

                var double = (x) => x * 2
                print(""double(5) = "" + double(5))
            ");
        }

        void ClassExample()
        {
            Log("=== 类示例 ===");
            _panda.Run(@"
                class Vector2 {
                    Vector2(x, y) {
                        this.x = x
                        this.y = y
                    }
                    func magnitude() {
                        return sqrt(this.x * this.x + this.y * this.y)
                    }
                }
                var v = Vector2(3, 4)
                print(""v 长度 = "" + v.magnitude())
            ");
        }

        void InteropExample()
        {
            Log("=== C# 互操作示例 ===");
            _panda.SetGlobal("gameVersion", "1.0.0");
            _panda.Run(@"print(""游戏版本: "" + gameVersion)");

            var result = _panda.Eval("10 + 20");
            Log($"Eval 结果: {result.AsNumber()}");
        }

        void AdvancedExample()
        {
            Log("=== 高级特性示例 ===");
            _panda.Run(@"
                try {
                    throw ""测试异常""
                } catch e {
                    print(""捕获: "" + e)
                }

                enum State { Idle, Running }
                print(""State.Running = "" + State.Running)
            ");
        }
    }
}
