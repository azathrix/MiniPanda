using UnityEngine;

namespace Azathrix.MiniPanda.Samples
{
    /// <summary>
    /// 高级特性示例：异常处理、枚举
    /// </summary>
    public class AdvancedDemo : DemoBase
    {
        protected override void RunDemo()
        {
            Log("=== 高级特性示例 ===");

            ExceptionExample();
            EnumExample();
        }

        void ExceptionExample()
        {
            Log("--- 异常处理 ---");

            _panda.Run(@"
                func safeDivide(a, b) {
                    if b == 0 {
                        throw ""除数不能为零""
                    }
                    return a / b
                }

                try {
                    print(""10 / 2 = "" + safeDivide(10, 2))
                    print(""10 / 0 = "" + safeDivide(10, 0))
                } catch e {
                    print(""捕获异常: "" + e)
                } finally {
                    print(""异常处理完成"")
                }
            ");
        }

        void EnumExample()
        {
            Log("--- 枚举 ---");

            _panda.Run(@"
                enum Direction {
                    Up,
                    Down,
                    Left,
                    Right
                }

                print(""Direction.Up = "" + Direction.Up)
                print(""Direction.Down = "" + Direction.Down)

                var currentDir = Direction.Up
                if currentDir == Direction.Up {
                    print(""当前方向: 上"")
                }
            ");

            _panda.Run(@"
                enum GameState {
                    Menu,
                    Playing,
                    Paused,
                    GameOver
                }

                var state = GameState.Menu
                print(""初始状态: "" + state)

                state = GameState.Playing
                print(""游戏开始: "" + state)
            ");
        }
    }
}
