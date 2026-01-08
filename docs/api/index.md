# API 概述

本文档提供 MiniPanda 的完整 API 参考。

## C# API

- [MiniPanda 类](./minipanda) - 主入口类
- [Value 结构体](./value) - 统一值类型
- [NativeFunction](./native-function) - 原生函数

## 脚本语言 API

- [内置函数](./builtins) - 标准库函数
- [内置对象](./objects) - JSON、Date、Regex
- [语法参考](./syntax) - 关键字、运算符

## 快速参考

### 常用 C# 方法

```csharp
// 执行代码
vm.Run(code)
vm.RunFile(path)
vm.Eval(expression)

// 变量操作
vm.SetGlobal(name, value)
vm.GetGlobal(name)

// 函数调用
vm.Call(funcName, args...)
```

### 常用内置函数

| 函数 | 说明 |
|------|------|
| `print(value)` | 打印值 |
| `type(value)` | 返回类型名 |
| `len(arr/str)` | 返回长度 |
| `range(n)` | 生成范围 |
| `push(arr, val)` | 添加元素 |
| `pop(arr)` | 移除元素 |
