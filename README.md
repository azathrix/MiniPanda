<p align="center">
  <img src="public/icon.png" alt="MiniPanda Logo" width="120">
</p>

<h1 align="center">MiniPanda</h1>

<p align="center">
  轻量级脚本语言，专为 Unity 设计
</p>

<p align="center">
  <a href="https://github.com/Azathrix/MiniPanda"><img src="https://img.shields.io/badge/GitHub-MiniPanda-black.svg" alt="GitHub"></a>
  <a href="https://www.npmjs.com/package/com.azathrix.mini-panda"><img src="https://img.shields.io/npm/v/com.azathrix.mini-panda.svg" alt="npm"></a>
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

### 方式一：Package Manager（推荐）

1. 打开 `Edit > Project Settings > Package Manager`
2. 在 `Scoped Registries` 中添加：
   - Name: `Azathrix`
   - URL: `https://registry.npmjs.org`
   - Scope(s): `com.azathrix`
3. 点击 `Save`
4. 打开 `Window > Package Manager`
5. 切换到 `My Registries`
6. 找到 `MiniPanda` 并安装

### 方式二：修改 manifest.json

在 `Packages/manifest.json` 中添加：

```json
{
  "scopedRegistries": [
    {
      "name": "Azathrix",
      "url": "https://registry.npmjs.org",
      "scopes": ["com.azathrix"]
    }
  ],
  "dependencies": {
    "com.azathrix.mini-panda": "*"
  }
}
```

> 注册 `com.azathrix` scope 后，可以在 Package Manager 的 "My Registries" 中发现更多 Azathrix 工具包。

### 方式三：Git URL

1. 打开 `Window > Package Manager`
2. 点击 `+` > `Add package from git URL...`
3. 输入：`https://github.com/Azathrix/MiniPanda.git`

## 编辑器支持

推荐安装 [MiniPanda VSCode 插件](https://github.com/azathrix/MiniPanda-VSCodePlugin)，提供语法高亮、代码补全、跳转定义、断点调试。

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
