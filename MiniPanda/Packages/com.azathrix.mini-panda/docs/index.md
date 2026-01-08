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
      link: https://github.com/your-username/mini-panda

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
---

## 快速开始

```csharp
using Azathrix.MiniPanda;

var vm = new MiniPanda();
vm.Start();

vm.Run(@"
    var name = ""MiniPanda""
    print(""Hello, {name}!"")
");

vm.Shutdown();
```

## 性能亮点

| 操作 | 性能 |
|------|------|
| 简单表达式求值 | 1.1μs/次 |
| 函数调用 | 3.1μs/次 |
| 100000 次循环 | 76ms |
| 递归 fib(20) | 30ms |

大部分操作 **零 GC 分配**，适合游戏热更新场景。
