<p align="center">
  <img src="docs/public/logo.png" alt="MiniPanda Logo" width="120">
</p>

<h1 align="center">MiniPanda</h1>

<p align="center">
  轻量级脚本语言，专为 Unity 设计
</p>

<p align="center">
  <a href="https://github.com/Azathrix/MiniPanda/blob/main/LICENSE"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
  <a href="https://unity.com/"><img src="https://img.shields.io/badge/Unity-6000.3+-black.svg" alt="Unity"></a>
  <a href="https://azathrix.github.io/MiniPanda/"><img src="https://img.shields.io/badge/docs-online-green.svg" alt="Documentation"></a>
</p>

<p align="center">
  <a href="https://azathrix.github.io/MiniPanda/">文档</a> •
  <a href="https://azathrix.github.io/MiniPanda/tutorial/">教程</a> •
  <a href="https://azathrix.github.io/MiniPanda/api/">API</a> •
  <a href="#安装">安装</a>
</p>

---

## 特性

- 字节码编译执行，编译缓存
- 变量、函数、Lambda、类、继承
- 模块系统（import/export）
- 异常处理（try/catch/finally）
- C# 双向互操作
- 调试支持（DAP 协议）

## 安装

### 通过 Git URL

```json
{
  "dependencies": {
    "com.azathrix.mini-panda": "https://github.com/Azathrix/MiniPanda.git"
  }
}
```

### 通过私有 Registry

```json
{
  "scopedRegistries": [
    {
      "name": "Azathrix",
      "url": "http://inputname.com:4873/",
      "scopes": ["com.azathrix"]
    }
  ],
  "dependencies": {
    "com.azathrix.mini-panda": "1.0.0"
  }
}
```

## 快速示例

### 基本使用

```csharp
using Azathrix.MiniPanda;

var vm = new MiniPanda();
vm.Start();

// Run - 执行代码
vm.Run("var x = 10");
vm.Run("print(x * 2)");  // 输出: 20

// Eval - 求值表达式
var result = vm.Eval("x + 5");
Debug.Log(result.AsNumber());  // 15

vm.Shutdown();
```

### 注册 C# 函数

```csharp
// 注册变量
vm.SetGlobal("playerHP", 100);
vm.SetGlobal("playerName", "Hero");

// 注册函数
vm.SetGlobal("heal", NativeFunction.Create((Value amount) => {
    var hp = vm.GetGlobal("playerHP").AsNumber();
    hp = Math.Min(hp + amount.AsNumber(), 100);
    vm.SetGlobal("playerHP", hp);
    return Value.FromNumber(hp);
}));

// 脚本中调用
vm.Run("heal(30)");
```

### 获取脚本值

```csharp
// 获取全局变量
var hp = vm.GetGlobal("playerHP").AsNumber();
var name = vm.GetGlobal("playerName").AsString();

// 调用脚本函数
vm.Run("func damage(a, b) return a * b");
var result = vm.Call("damage", 10, 1.5);  // 15

// 带临时环境求值
var sum = vm.Eval("a + b + c", new { a = 1, b = 2, c = 3 });  // 6
```

### 脚本语法

```javascript
// 变量与字符串插值
var name = "MiniPanda"
print("Hello {name}!")

// 函数与 Lambda
func add(a, b = 0) return a + b
var double = (x) => x * 2

// 类与继承
class Entity {
    Entity(name) { this.name = name }
}
class Player : Entity {
    Player(name, level) {
        super.Entity(name)
        this.level = level
    }
}

// 模块
import "utils" as u
export func helper() { }
```

## 性能

Unity 6000.3 测试结果：

| 操作 | 性能 |
|------|------|
| Eval 简单表达式 | 1.1μs |
| Run 编译+执行 | 2μs |
| 函数调用 | 3.1μs |
| GetGlobal | 0.1μs |
| SetGlobal | <0.1μs |
| 循环 100000 次 | 76ms |
| 递归 fib(20) | 30ms |

大部分操作零 GC 分配。

## License

MIT
