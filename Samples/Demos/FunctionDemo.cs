using UnityEngine;

namespace Azathrix.MiniPanda.Samples
{
    /// <summary>
    /// 函数示例：普通函数、Lambda、递归、闭包
    /// </summary>
    public class FunctionDemo : DemoBase
    {
        protected override void RunDemo()
        {
            Log("=== 函数示例 ===");

            // 递归函数
            _panda.Run(@"
                func factorial(n) {
                    if n <= 1 return 1
                    return n * factorial(n - 1)
                }
                print(""5! = "" + factorial(5))
            ");

            // Lambda 表达式
            _panda.Run(@"
                var numbers = [1, 2, 3, 4, 5]
                var double = (x) => x * 2

                for n in numbers {
                    print(n + "" * 2 = "" + double(n))
                }
            ");

            // 默认参数
            _panda.Run(@"
                func greet(name, greeting = ""Hello"") {
                    print(greeting + "", "" + name + ""!"")
                }
                greet(""World"")
                greet(""MiniPanda"", ""Welcome"")
            ");

            // 闭包
            _panda.Run(@"
                func makeCounter() {
                    var count = 0
                    return () => {
                        count = count + 1
                        return count
                    }
                }

                var counter = makeCounter()
                print(""counter: "" + counter())
                print(""counter: "" + counter())
                print(""counter: "" + counter())
            ");

            // 高阶函数
            _panda.Run(@"
                func makeMultiplier(factor) {
                    return (x) => x * factor
                }

                var double = makeMultiplier(2)
                var triple = makeMultiplier(3)

                print(""double(5) = "" + double(5))
                print(""triple(5) = "" + triple(5))
            ");
        }
    }
}
