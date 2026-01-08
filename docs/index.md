---
layout: home

hero:
  name: MiniPanda
  text: 轻量级脚本语言
  tagline: 专为 Unity 设计，支持字节码编译、零 GC 分配、C# 双向互操作
  actions:
    - theme: brand
      text: 快速入门
      link: /tutorial/
    - theme: alt
      text: API 参考
      link: /api/
    - theme: alt
      text: GitHub
      link: https://github.com/Azathrix/MiniPanda

features:
  - icon: 🚀
    title: 高性能
    details: 字节码编译执行，简单表达式 1.1μs/次，大部分操作零 GC 分配
  - icon: 🎮
    title: Unity 原生
    details: 专为 Unity 设计，无缝集成，支持 C# 双向互操作
  - icon: 📦
    title: 功能完整
    details: 支持类、继承、闭包、模块系统、异常处理、调试等完整特性
  - icon: 🔧
    title: 易于使用
    details: 简洁的 JavaScript 风格语法，5 分钟上手
  - icon: 🐛
    title: 调试支持
    details: 支持 DAP 协议，可在 VSCode 中断点调试
  - icon: 💡
    title: 智能提示
    details: VSCode 插件提供语法高亮、代码补全、跳转定义
---

## 特性一览

- **字节码编译** - 编译后缓存，重复执行无编译开销
- **零 GC 分配** - 大部分操作无内存分配，适合游戏热更新
- **完整语法** - 变量、函数、Lambda、类、继承、模块、异常处理
- **C# 互操作** - 双向调用，无缝集成 Unity
- **调试支持** - DAP 协议，VSCode 断点调试
- **智能提示** - LSP 协议，代码补全、跳转定义

## 快速安装

在 Unity 项目的 `Packages/manifest.json` 中添加：

```json
{
  "dependencies": {
    "com.azathrix.mini-panda": "https://github.com/Azathrix/MiniPanda.git"
  }
}
```

## 编辑器支持

推荐安装 [MiniPanda VSCode 插件](https://github.com/azathrix/MiniPanda-VSCodePlugin)，提供：

- 语法高亮
- 代码补全
- 跳转定义
- 断点调试

## 快速示例

### C# 端

```csharp
using Azathrix.MiniPanda;

var vm = new MiniPanda();
vm.Start();

// 执行脚本
vm.Run(@"
    var name = ""MiniPanda""
    print(""Hello, {name}!"")
");

// 求值表达式
var result = vm.Eval("1 + 2 * 3");
Debug.Log(result.AsNumber());  // 7

// 注册 C# 函数供脚本调用
vm.SetGlobal("heal", NativeFunction.Create((Value amount) => {
    return Value.FromNumber(amount.AsNumber() * 2);
}));

vm.Shutdown();
```

### 脚本端

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

## 性能数据

Unity 6000.3 测试结果：

| 操作 | 性能 | GC 分配 |
|------|------|---------|
| Eval 简单表达式 | 1.1μs | 0 |
| Run 编译+执行 | 2μs | 0 |
| 函数调用 | 3.1μs | 0 |
| GetGlobal | 0.1μs | 0 |
| SetGlobal | <0.1μs | 0 |
| 循环 100000 次 | 76ms | 0 |
| 递归 fib(20) | 30ms | 0 |

大部分操作 **零 GC 分配**，适合游戏热更新场景。
