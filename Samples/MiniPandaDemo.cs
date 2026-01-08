using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Azathrix.MiniPanda;
using Azathrix.MiniPanda.Core;
using Azathrix.MiniPanda.Debug.DAP;
using Azathrix.MiniPanda.VM;
using UnityEditor;

/// <summary>
/// MiniPanda 演示脚本
/// 将此脚本挂载到 GameObject 上运行示例
/// </summary>
public class MiniPandaDemo : MonoBehaviour
{
    [Header("示例选择")]
    [Tooltip("选择要运行的示例")]
    public DemoType demoType = DemoType.All;

    [Header("调试设置")]
    [Tooltip("是否启用调试服务器")]
    public bool enableDebugServer = false;

    public enum DemoType
    {
        All,
        Basic,
        Function,
        Class,
        StaticMember,
        Interop,
        Import,
        Exception,
        Closure,
        Enum,
        AdvancedFile
    }

    private MiniPanda _panda;
    private DebugServer _debugServer;
    private Task _task;
    private string _samplesPath;

    void Start()
    {
        // 在主线程获取路径（Unity API 只能在主线程调用）
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
                    _debugServer.WaitForReady();
                    Debug.Log("调试器准备完成，开始执行脚本");
                }

                LoadModules();
                RunSelectedDemo();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[MiniPanda] 脚本执行异常: {ex}");
            }
        });
    }

    void OnDestroy()
    {
        _debugServer?.Stop();
        _panda?.Shutdown();
        _task?.Dispose();
    }

    void RunSelectedDemo()
    {
        switch (demoType)
        {
            case DemoType.All:
                BasicExample();
                FunctionExample();
                ClassExample();
                StaticMemberExample();
                InteropExample();
                ExceptionExample();
                ClosureExample();
                EnumExample();
                ImportExample();
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
            case DemoType.StaticMember:
                StaticMemberExample();
                break;
            case DemoType.Interop:
                InteropExample();
                break;
            case DemoType.Import:
                ImportExample();
                break;
            case DemoType.Exception:
                ExceptionExample();
                break;
            case DemoType.Closure:
                ClosureExample();
                break;
            case DemoType.Enum:
                EnumExample();
                break;
            case DemoType.AdvancedFile:
                RunAdvancedFile();
                break;
        }
    }

    void LoadModules()
    {
        // 加载工具模块
        LoadModuleIfExists("utils.panda", "utils");

        // 加载向量模块
        LoadModuleIfExists("math/vector.panda", "math.vector");

        // 加载示例模块
        LoadModuleIfExists("example.panda", "example");

        // 加载高级示例模块
        LoadModuleIfExists("advanced/exceptions.panda", "advanced.exceptions");
        LoadModuleIfExists("advanced/closures.panda", "advanced.closures");
        LoadModuleIfExists("advanced/enums.panda", "advanced.enums");
        LoadModuleIfExists("game/inventory.panda", "game.inventory");
    }

    void LoadModuleIfExists(string relativePath, string moduleName)
    {
        var fullPath = Path.Combine(_samplesPath, relativePath);
        if (File.Exists(fullPath))
        {
            var normalizedPath = Path.GetFullPath(fullPath).Replace("\\", "/");
            _panda.LoadModule(File.ReadAllBytes(fullPath), moduleName, normalizedPath);
        }
    }

    void BasicExample()
    {
        Debug.Log("=== 基础示例 ===");

        _panda.Run(@"
            var x = 10
            var y = 20
            print(""x + y = "" + (x + y))
        ");

        _panda.Run(@"
            var arr = [1, 2, 3, 4, 5]
            var sum = 0
            for n in arr {
                sum = sum + n
            }
            print(""数组求和: "" + sum)
        ");

        _panda.Run(@"
            var player = {name: ""Hero"", hp: 100, mp: 50}
            print(""玩家: "" + player.name + "", HP: "" + player.hp)
        ");
    }

    void FunctionExample()
    {
        Debug.Log("=== 函数示例 ===");

        _panda.Run(@"
            func factorial(n) {
                if n <= 1 return 1
                return n * factorial(n - 1)
            }
            print(""5! = "" + factorial(5))
        ");

        _panda.Run(@"
            var numbers = [1, 2, 3, 4, 5]
            var double = (x) => x * 2

            for n in numbers {
                print(n + "" * 2 = "" + double(n))
            }
        ");
    }

    void ClassExample()
    {
        Debug.Log("=== 类示例 ===");

        _panda.Run(@"
            class Vector2 {
                Vector2(x, y) {
                    this.x = x
                    this.y = y
                }

                func add(other) {
                    return Vector2(this.x + other.x, this.y + other.y)
                }

                func magnitude() {
                    return sqrt(this.x * this.x + this.y * this.y)
                }

                func toString() {
                    return ""("" + this.x + "", "" + this.y + "")""
                }
            }

            var v1 = Vector2(3, 4)
            var v2 = Vector2(1, 2)
            var v3 = v1.add(v2)

            print(""v1 = "" + v1.toString())
            print(""v2 = "" + v2.toString())
            print(""v1 + v2 = "" + v3.toString())
            print(""v1 长度 = "" + v1.magnitude())
        ");
    }

    void StaticMemberExample()
    {
        Debug.Log("=== 静态成员示例 ===");

        _panda.Run(@"
            class Counter {
                static var count = 0

                static func increment() {
                    Counter.count = Counter.count + 1
                    return Counter.count
                }

                static func getCount() {
                    return Counter.count
                }

                var id

                Counter() {
                    Counter.count = Counter.count + 1
                    this.id = Counter.count
                }

                func getId() {
                    return this.id
                }
            }

            print(""初始 count: "" + Counter.count)
            print(""increment: "" + Counter.increment())
            print(""increment: "" + Counter.increment())
            print(""getCount: "" + Counter.getCount())

            Counter.count = 100
            print(""设置后 count: "" + Counter.count)

            Counter.count = 0
            var a = Counter()
            var b = Counter()
            print(""创建2个实例后, 总数: "" + Counter.count)
            print(""实例 a 的 id: "" + a.getId())
            print(""实例 b 的 id: "" + b.getId())
        ");
    }

    void InteropExample()
    {
        Debug.Log("=== C# 互操作示例 ===");

        // 注册全局变量
        _panda.SetGlobal("gameVersion", "1.0.0");
        _panda.SetGlobal("maxPlayers", 4);

        _panda.Run(@"
            print(""游戏版本: "" + gameVersion)
            print(""最大玩家数: "" + maxPlayers)
        ");

        // 获取脚本函数作为委托
        var func = _panda.Run<Func<float, float, float>>(@"
            func calculateDamage(baseDamage, multiplier) {
                return baseDamage * multiplier
            }
            return calculateDamage
        ");

        var damage = func(100, 1.5f);
        Debug.Log($"计算伤害 (C# 调用): {damage}");

        // 临时环境求值
        Dictionary<string, object> env = new()
        {
            ["hp"] = 100,
            ["damage"] = 30
        };

        var result = _panda.Eval("hp - damage", env);
        Debug.Log($"剩余 HP: {result.AsNumber()}");
    }

    void ExceptionExample()
    {
        Debug.Log("=== 异常处理示例 ===");

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

    void ClosureExample()
    {
        Debug.Log("=== 闭包示例 ===");

        _panda.Run(@"
            func makeCounter() {
                var count = 0
                return () => {
                    count = count + 1
                    return count
                }
            }

            var counter1 = makeCounter()
            var counter2 = makeCounter()

            print(""counter1: "" + counter1())
            print(""counter1: "" + counter1())
            print(""counter1: "" + counter1())
            print(""counter2: "" + counter2())
            print(""counter2: "" + counter2())
        ");

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

    void EnumExample()
    {
        Debug.Log("=== 枚举示例 ===");

        _panda.Run(@"
            enum Direction {
                Up,
                Down,
                Left,
                Right
            }

            print(""Direction.Up = "" + Direction.Up)
            print(""Direction.Down = "" + Direction.Down)
            print(""Direction.Left = "" + Direction.Left)
            print(""Direction.Right = "" + Direction.Right)

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

            state = GameState.Paused
            print(""游戏暂停: "" + state)
        ");
    }

    void ImportExample()
    {
        Debug.Log("=== 模块导入示例 ===");

        // 测试作用域
        var scopeName = "player";
        var player = _panda.GetScope(scopeName);
        player.Set("atk", Value.FromNumber(10));

        var damageExp = "atk * 1.5";
        var dmg = _panda.Eval<float>(damageExp, scopeName);
        Debug.Log("伤害:" + dmg);

        _panda.Run(@"
            import ""example""
            import ""utils"" as u
            print(""Utils VERSION: "" + u.VERSION)
            u.helper()
            print(""clamp(15, 0, 10) = "" + u.clamp(15, 0, 10))

            import ""math.vector"" as vec
            var v1 = vec.create(3, 4, 0)
            var v2 = vec.create(1, 2, 0)
            var v3 = vec.add(v1, v2)
            print(""v1 + v2 = ("" + v3.x + "", "" + v3.y + "", "" + v3.z + "")"")
        ");
    }

    void RunAdvancedFile()
    {
        Debug.Log("=== 运行高级示例文件 ===");

        // 运行异常处理示例
        var exceptionsPath = Path.Combine(_samplesPath, "advanced/exceptions.panda");
        if (File.Exists(exceptionsPath))
        {
            Debug.Log("--- 运行 exceptions.panda ---");
            _panda.Run(File.ReadAllBytes(exceptionsPath));
        }

        // 运行闭包示例
        var closuresPath = Path.Combine(_samplesPath, "advanced/closures.panda");
        if (File.Exists(closuresPath))
        {
            Debug.Log("--- 运行 closures.panda ---");
            _panda.Run(File.ReadAllBytes(closuresPath));
        }

        // 运行枚举示例
        var enumsPath = Path.Combine(_samplesPath, "advanced/enums.panda");
        if (File.Exists(enumsPath))
        {
            Debug.Log("--- 运行 enums.panda ---");
            _panda.Run(File.ReadAllBytes(enumsPath));
        }

        // 运行背包系统示例
        var inventoryPath = Path.Combine(_samplesPath, "game/inventory.panda");
        if (File.Exists(inventoryPath))
        {
            Debug.Log("--- 运行 inventory.panda ---");
            _panda.Run(File.ReadAllBytes(inventoryPath));
        }
    }
}
