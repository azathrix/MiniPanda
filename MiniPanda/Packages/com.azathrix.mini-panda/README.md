# MiniPanda

轻量级脚本语言，专为 Unity 设计。支持变量、函数、类、数组、对象以及 C# 双向互操作。

## 特性

- 字节码编译执行（非 AST 解释）
- 编译缓存（相同代码只编译一次）
- 变量、函数、Lambda、类、继承
- 数组、对象/字典
- 控制流（if/else, while, for, break, continue）
- 全局变量（`global` 关键字、`_G` 全局表）
- 模块系统（import/export）
- 异常处理（try/catch/finally）
- C# 互操作（注册/获取变量、调用函数）
- 调试支持（DAP 协议、断点、单步执行）
- 自定义文件加载

## 文档

- [完整教程](Documentation~/Tutorial.md) - 从入门到精通
- [API 参考](Documentation~/API.md) - 完整的 API 文档

## 安装

### 通过私有 Registry 安装（推荐）

1. 编辑项目的 `Packages/manifest.json`，添加 scoped registry：

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

2. 保存后 Unity 会自动下载安装

### 通过 Git URL 安装

1. 打开 Unity，选择 `Window > Package Manager`
2. 点击 `+` 按钮，选择 `Add package from git URL...`
3. 输入：`git@github.com:azathrix/MiniPanda.git`

或在 `Packages/manifest.json` 中添加：

```json
{
  "dependencies": {
    "com.azathrix.mini-panda": "git@github.com:azathrix/MiniPanda.git"
  }
}
```

### 本地安装

将包文件夹复制到项目的 `Packages/` 目录下。

## 快速开始

```csharp
using Azathrix.MiniPanda;

// 创建虚拟机
var vm = new MiniPanda();
vm.Start();

// 执行代码
vm.Run("var x = 10");
vm.Run("print(x)");

// 求值表达式
var result = vm.Eval("x + 5");  // 15

// 关闭
vm.Shutdown();
```

## 语法示例

### 变量与字符串

```javascript
var x = 10
var name = "MiniPanda"
var greeting = "Hello {name}!"  // 字符串插值
print(greeting)  // Hello MiniPanda!
```

### 函数与 Lambda

```javascript
// 函数
func add(a, b) {
    return a + b
}

// 默认参数
func greet(name, greeting = "Hello") {
    return "{greeting}, {name}!"
}

// Lambda
var double = (x) => x * 2
```

### 类与继承

```javascript
class Entity {
    Entity(name) {
        this.name = name
        this.hp = 100
    }

    func takeDamage(amount) {
        this.hp = this.hp - amount
    }
}

class Player : Entity {
    Player(name, level) {
        super.Entity(name)
        this.level = level
    }
}

var hero = Player("Hero", 1)
hero.takeDamage(30)
print(hero.hp)  // 70
```

### 异常处理

```javascript
try {
    throw "Something went wrong"
} catch e {
    print("Error: {e}")
} finally {
    print("Cleanup")
}
```

### 模块系统

```javascript
// utils.panda
export var VERSION = "1.0.0"
export func helper() {
    print("Helper called!")
}

// main.panda
import "utils" as u
print(u.VERSION)
u.helper()
```

## C# 互操作

### 注册变量和函数

```csharp
// 注册变量
vm.SetGlobal("PI", 3.14159);
vm.SetGlobal("playerName", "Hero");

// 注册函数
vm.SetGlobal("square", NativeFunc.Create((Value v) =>
    Value.FromNumber(v.AsNumber() * v.AsNumber())));
```

### 调用脚本函数

```csharp
vm.Run("func multiply(a, b) return a * b");
var result = vm.Call("multiply", 6, 7);  // 42
```

### 临时环境求值

```csharp
var result = vm.Eval("x + y", new { x = 10, y = 20 });  // 30
```

## 内置函数

| 函数 | 说明 |
|------|------|
| `print(value)` | 打印值 |
| `type(value)` | 返回类型名 |
| `len(arr/str)` | 返回长度 |
| `range(n)` | 生成 0 到 n-1 的范围 |
| `abs(n)` | 绝对值 |
| `floor(n)` / `ceil(n)` / `round(n)` | 取整 |
| `sqrt(n)` / `pow(a, b)` | 数学运算 |
| `min(...)` / `max(...)` | 最值 |
| `push(arr, val)` / `pop(arr)` | 数组操作 |
| `keys(obj)` / `values(obj)` | 对象操作 |
| `contains(col, item)` | 包含检查 |
| `slice(arr, start, end)` | 切片 |
| `join(arr, sep)` / `split(str, sep)` | 字符串操作 |
| `json.parse(str)` / `json.stringify(obj)` | JSON 操作 |
| `trace(...)` / `debug(...)` | 调试输出 |
| `assert(condition, msg?)` | 断言 |

更多内置函数请参考 [API 文档](Documentation~/API.md)。

## 示例

查看 `Samples/` 目录获取更多示例：

- `MiniPandaDemo.cs` - C# 集成示例
- `example.panda` - 语法示例
- `utils.panda` - 模块示例
- `math/vector.panda` - 向量模块
- `advanced/` - 高级特性示例

## 性能测试

在 Unity 6000.3 环境下的测试结果：

### 内存分配 (GC)

| 操作 | 次数 | 分配 |
|------|------|------|
| 数组操作 | x1000 | 0 KB |
| 类实例化 | x1000 | 84 KB (86B/次) |
| 闭包创建+调用 | x1000 | 0 KB |
| Eval 简单表达式 | x1000 | 0 KB |
| 函数调用 | x1000 | 0 KB |
| 方法调用 | x1000 | 84 KB (86B/次) |
| 嵌套循环 50x50 | x100 | 0 KB |
| 对象创建 100个 | x100 | 2868 KB (29KB/次) |
| Run 循环 1000次 | x100 | 0 KB |
| Set+Eval | x1000 | 0 KB |
| 字符串拼接 | x1000 | 0 KB |

### 执行性能

| 操作 | 次数 | 耗时 | 单次耗时 |
|------|------|------|----------|
| 数组索引访问 | x10000 | 21ms | 2.1μs |
| 数组迭代 10000 元素 | - | 21ms | - |
| 数组 push | x10000 | 13ms | 1.3μs |
| 类继承+实例化 | x1000 | 6ms | 6μs |
| 闭包调用 | x10000 | 20ms | 2μs |
| 新建VM+编译+执行复杂代码 | x100 | 28ms | 280μs |
| 编译+执行简单代码 | x1000 | 2ms | 2μs |
| 条件分支 | x10000 | 15ms | 1.5μs |
| Run 函数定义+调用 | x10000 | 31ms | 3.1μs |
| Eval 简单表达式 | x10000 | 11ms | 1.1μs |
| Eval 字符串拼接 | x10000 | 14ms | 1.4μs |
| GetGlobal | x10000 | 1ms | 0.1μs |
| 数学运算 | x10000 | 12ms | 1.2μs |
| 方法调用 | x10000 | 21ms | 2.1μs |
| Native 函数调用 | x10000 | 22ms | 2.2μs |
| 嵌套循环 100x100 | - | 9ms | - |
| 对象属性访问 | x10000 | 21ms | 2.1μs |
| Run 循环 100000 次 | - | 76ms | - |
| Run 对象创建 10000 个 | - | 25ms | - |
| Run 递归 fib(20) | - | 30ms | - |
| Set+Eval | x10000 | 18ms | 1.8μs |
| SetGlobal | x10000 | 0ms | <0.1μs |
| 字符串插值 | x10000 | 32ms | 3.2μs |

### 性能亮点

- 零 GC 分配：大部分操作无内存分配
- 快速求值：简单表达式 1.1μs/次
- 高效循环：100000 次循环仅 76ms
- 快速编译：简单代码编译+执行 2μs/次

## 测试

- Unity Test Runner：选择 EditMode 测试并运行全部

## License

MIT
