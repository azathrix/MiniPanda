# MiniPanda 教程

MiniPanda 是一个轻量级脚本语言，专为 Unity 设计。本教程将带你从零开始学习 MiniPanda 的所有特性。

## 目录

1. [快速入门](#1-快速入门)
2. [基础语法](#2-基础语法)
3. [控制流](#3-控制流)
4. [函数](#4-函数)
5. [面向对象](#5-面向对象)
6. [高级特性](#6-高级特性)
7. [C# 互操作](#7-c-互操作)
8. [调试](#8-调试)

---

## 1. 快速入门

### 1.1 安装

通过 Unity Package Manager 添加此包：

**方式一：通过 Git URL**
1. 打开 Unity，选择 `Window > Package Manager`
2. 点击 `+` 按钮，选择 `Add package from git URL...`
3. 输入仓库地址

**方式二：本地安装**
1. 将包文件夹复制到项目的 `Packages/` 目录下
2. Unity 会自动识别并加载

### 1.2 第一个脚本

```csharp
using Azathrix.MiniPanda;

// 创建虚拟机
var vm = new MiniPanda();
vm.Start();

// 执行代码
vm.Run("print(\"Hello, MiniPanda!\")");

// 关闭
vm.Shutdown();
```

### 1.3 在 Unity 中运行

创建一个 MonoBehaviour 脚本：

```csharp
using UnityEngine;
using Azathrix.MiniPanda;

public class MiniPandaRunner : MonoBehaviour
{
    private MiniPanda _panda;

    void Start()
    {
        _panda = new MiniPanda();
        _panda.Start();

        _panda.Run(@"
            var message = ""Hello from MiniPanda!""
            print(message)
        ");
    }

    void OnDestroy()
    {
        _panda?.Shutdown();
    }
}
```

---

## 2. 基础语法

### 2.1 变量与类型

MiniPanda 支持以下基本类型：

```javascript
// 数字
var x = 10
var pi = 3.14159

// 字符串
var name = "MiniPanda"

// 布尔值
var isReady = true
var isEmpty = false

// 空值
var nothing = null
```

### 2.2 运算符

**算术运算符**
```javascript
var a = 10 + 5    // 加法: 15
var b = 10 - 5    // 减法: 5
var c = 10 * 5    // 乘法: 50
var d = 10 / 5    // 除法: 2
var e = 10 % 3    // 取模: 1
```

**复合赋值**
```javascript
var x = 10
x += 5   // x = 15
x -= 3   // x = 12
x *= 2   // x = 24
x /= 4   // x = 6
x %= 4   // x = 2
```

**自增自减**
```javascript
var x = 10
++x      // 前置自增: x = 11
x++      // 后置自增: x = 12
--x      // 前置自减: x = 11
x--      // 后置自减: x = 10
```

**比较运算符**
```javascript
var a = 10 == 10   // 等于: true
var b = 10 != 5    // 不等于: true
var c = 10 > 5     // 大于: true
var d = 10 < 5     // 小于: false
var e = 10 >= 10   // 大于等于: true
var f = 10 <= 5    // 小于等于: false
```

**逻辑运算符**
```javascript
var a = true && false   // 与: false
var b = true || false   // 或: true
var c = !true           // 非: false
```

**三元运算符**
```javascript
var result = x > 0 ? "positive" : "negative"
```

**空值合并**
```javascript
var value = null ?? "default"     // "default"
var name = userName ?? "Guest"    // 如果 userName 为 null，使用 "Guest"
var x = a ?? b ?? c ?? "fallback" // 链式空值合并
```

### 2.3 字符串与插值

```javascript
var name = "World"
var greeting = "Hello {name}!"  // 字符串插值
print(greeting)  // Hello World!

// 表达式插值
print("Result: {10 + 5}")  // Result: 15

// 转义大括号
print("\{literal}")  // {literal}
```

### 2.4 数组与对象

**数组**
```javascript
var arr = [1, 2, 3, 4, 5]
arr[0] = 100           // 修改元素
print(arr[0])          // 100
print(len(arr))        // 5

// 数组操作
push(arr, 6)           // 添加元素
var last = pop(arr)    // 移除并返回最后一个元素
```

**对象/字典**
```javascript
var obj = {name: "test", value: 42}
obj.name = "new"       // 点号访问
obj["key"] = 123       // 方括号访问

// 获取键和值
var k = keys(obj)      // ["name", "value", "key"]
var v = values(obj)    // ["new", 42, 123]
```

---

## 3. 控制流

### 3.1 条件语句

**单行形式**
```javascript
if x > 0 print("positive")
```

**多行形式**
```javascript
if x > 0 {
    print("positive")
} else if x < 0 {
    print("negative")
} else {
    print("zero")
}
```

### 3.2 循环语句

**while 循环**
```javascript
var i = 0
while i < 5 {
    print(i)
    i = i + 1
}
```

**for-in 循环**
```javascript
// 遍历数组
var arr = [1, 2, 3]
for item in arr {
    print(item)
}

// 遍历范围
for i in range(10) {
    print(i)  // 0 到 9
}

// 遍历对象（键值对）
var obj = {a: 1, b: 2}
for k, v in obj {
    print("{k}: {v}")
}
```

### 3.3 break 和 continue

```javascript
// break - 跳出循环
for i in range(10) {
    if i == 5 break
    print(i)  // 0, 1, 2, 3, 4
}

// continue - 跳过当前迭代
for i in range(10) {
    if i % 2 == 0 continue
    print(i)  // 1, 3, 5, 7, 9
}
```

---

## 4. 函数

### 4.1 函数定义

**标准函数**
```javascript
func add(a, b) {
    return a + b
}

print(add(3, 5))  // 8
```

**单行函数**
```javascript
func double(x) return x * 2

print(double(5))  // 10
```

### 4.2 默认参数与剩余参数

**默认参数**
```javascript
func greet(name, greeting = "Hello") {
    return "{greeting}, {name}!"
}

print(greet("World"))           // Hello, World!
print(greet("World", "Hi"))     // Hi, World!
```

**剩余参数**
```javascript
func sum(...numbers) {
    var total = 0
    for n in numbers {
        total = total + n
    }
    return total
}

print(sum(1, 2, 3, 4, 5))  // 15
```

### 4.3 Lambda 表达式

```javascript
// 单表达式 Lambda
var double = (x) => x * 2
print(double(5))  // 10

// 带默认参数的 Lambda
var add = (a, b = 10) => a + b
print(add(5))     // 15
print(add(5, 3))  // 8

// 多语句 Lambda
var process = (x) => {
    var result = x * 2
    return result + 1
}
```

### 4.4 闭包

```javascript
func makeCounter() {
    var count = 0
    return () => {
        count = count + 1
        return count
    }
}

var counter = makeCounter()
print(counter())  // 1
print(counter())  // 2
print(counter())  // 3

// 每个计数器独立
var counter2 = makeCounter()
print(counter2())  // 1
```

---

## 5. 面向对象

### 5.1 类定义

```javascript
class Player {
    // 构造函数（与类同名）
    Player(name) {
        this.name = name
        this.hp = 100
    }

    // 方法
    func takeDamage(amount) {
        this.hp = this.hp - amount
        if this.hp < 0 {
            this.hp = 0
        }
    }

    func isAlive() {
        return this.hp > 0
    }
}

var player = Player("Hero")
print(player.name)  // Hero
print(player.hp)    // 100

player.takeDamage(30)
print(player.hp)    // 70
```

### 5.2 继承

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
        super.Entity(name)  // 调用父类构造函数
        this.level = level
        this.hp = 100 + level * 10
    }

    func levelUp() {
        this.level = this.level + 1
        this.hp = this.hp + 10
    }
}

var hero = Player("Hero", 1)
print(hero.hp)  // 110
hero.levelUp()
print(hero.level)  // 2
```

### 5.3 静态成员

```javascript
class Counter {
    // 静态变量
    static var count = 0

    // 静态方法
    static func increment() {
        Counter.count = Counter.count + 1
        return Counter.count
    }

    static func getCount() {
        return Counter.count
    }

    // 实例变量
    var id

    Counter() {
        Counter.count = Counter.count + 1
        this.id = Counter.count
    }
}

// 访问静态成员
print(Counter.count)       // 0
print(Counter.increment()) // 1
print(Counter.increment()) // 2

// 创建实例
var a = Counter()
var b = Counter()
print(Counter.count)  // 4
print(a.id)           // 3
print(b.id)           // 4
```

---

## 6. 高级特性

### 6.1 异常处理

```javascript
func divide(a, b) {
    if b == 0 {
        throw "除数不能为零"
    }
    return a / b
}

try {
    print(divide(10, 2))  // 5
    print(divide(10, 0))  // 抛出异常
    print("这行不会执行")
} catch e {
    print("捕获异常: {e}")
} finally {
    print("清理完成")  // 总是执行
}
```

### 6.2 模块系统

**导出模块 (utils.panda)**
```javascript
export var VERSION = "1.0.0"

export func helper() {
    print("Helper function called!")
}

export func clamp(value, min, max) {
    if value < min return min
    if value > max return max
    return value
}
```

**导入模块**
```javascript
// 导入整个模块
import "utils" as u

print(u.VERSION)           // 1.0.0
u.helper()                 // Helper function called!
print(u.clamp(15, 0, 10))  // 10

// 路径转换 - 点号转目录
import "math.vector" as vec  // 加载 math/vector.panda
```

### 6.3 全局变量

**使用 global 关键字**
```javascript
global var config = {debug: true}

func test() {
    print(config.debug)  // true - 可以在任何作用域访问
}

test()
```

**使用 _G 全局表**
```javascript
_G.newGlobal = 100
print(_G.newGlobal)  // 100
print(_G.abs(-5))    // 5 (访问内置函数)
```

### 6.4 枚举

```javascript
enum State {
    Idle,
    Running,
    Jumping,
    Falling
}

var currentState = State.Idle
print(currentState)  // 0

if currentState == State.Idle {
    print("角色空闲中")
}

// 枚举值是从 0 开始的整数
print(State.Running)  // 1
print(State.Jumping)  // 2
```

---

## 7. C# 互操作

### 7.1 注册变量与函数

**注册全局变量**
```csharp
vm.SetGlobal("PI", 3.14159);
vm.SetGlobal("playerName", "Hero");
vm.SetGlobal("isDebug", true);
```

**注册原生函数**
```csharp
// 简单函数
vm.SetGlobal("square", NativeFunc.Create((Value v) =>
    Value.FromNumber(v.AsNumber() * v.AsNumber())));

// 可变参数函数
vm.SetGlobal("log", NativeFunc.Create((Value[] args) => {
    Debug.Log(args[0].AsString());
    return Value.Null;
}));

// 访问 VM 的函数
vm.SetGlobal("getLocation", NativeFunc.CreateWithVM((vm, args) => {
    var location = vm.GetCurrentLocation();
    return Value.FromString(location);
}));
```

### 7.2 调用脚本函数

```csharp
// 定义脚本函数
vm.Run("func multiply(a, b) return a * b");

// 从 C# 调用
var result = vm.Call("multiply", 6, 7);  // 42

// 带临时作用域调用
vm.Run("func greet(name) { return prefix + name + suffix }");
var greeting = vm.Call(
    new { prefix = "Hello, ", suffix = "!" },
    "greet",
    "World"
);
// greeting = "Hello, World!"
```

### 7.3 临时环境求值

```csharp
// 使用匿名对象
var result = vm.Eval("x + y", new { x = 10, y = 20 });  // 30

// 使用 Dictionary
var env = new Dictionary<string, object> {
    ["x"] = 10,
    ["y"] = 20
};
var result = vm.Eval("x + y", env);  // 30
```

### 7.4 自定义文件加载

```csharp
// 从 Unity Resources 加载
vm.FileLoader = path => Resources.Load<TextAsset>(path)?.text;

// 从 StreamingAssets 加载
vm.FileLoader = path => {
    var fullPath = Path.Combine(Application.streamingAssetsPath, path);
    return File.ReadAllText(fullPath);
};

// 使用
vm.RunFile("scripts/main.panda");
```

### 7.5 获取脚本函数作为委托

```csharp
// 获取脚本函数作为 C# 委托
var func = vm.Run<Func<float, float, float>>(@"
    func calculateDamage(baseDamage, multiplier) {
        return baseDamage * multiplier
    }
    return calculateDamage
");

// 像普通 C# 函数一样调用
var damage = func(100, 1.5f);  // 150
```

---

## 8. 调试

### 8.1 调试函数

**print - 打印输出**
```javascript
print("Hello")
print(123)
print({name: "test"})
```

**trace / debug - 带位置信息的打印**
```javascript
trace("debug info")  // [TRACE] debug info (at script.panda:10)
debug("message")     // 同 trace
```

**stacktrace - 获取调用栈**
```javascript
func inner() {
    return stacktrace()
}

func outer() {
    return inner()
}

print(outer())
// 输出调用栈信息
```

**assert - 断言**
```javascript
var x = 10
assert(x > 0)                    // 通过
assert(x > 0, "x must be positive")  // 带消息的断言
assert(x < 0)                    // 失败，抛出错误
```

### 8.2 断点调试 (DAP)

MiniPanda 支持 Debug Adapter Protocol (DAP)，可以与 VS Code 等 IDE 集成进行断点调试。

**启动调试服务器**
```csharp
var vm = new MiniPanda();
vm.Start();

var debugServer = new DebugServer(vm.VM);
debugServer.Start();

// 等待调试器连接
debugServer.WaitForReady();

// 执行脚本
vm.RunFile("script.panda");
```

**调试功能**
- 设置断点（支持条件断点）
- 单步执行（Step Over / Step In / Step Out）
- 查看变量值
- 查看调用栈
- 异常断点

### 8.3 编译缓存

```csharp
// 相同代码只编译一次
vm.Run("print(1)");  // 编译 + 执行
vm.Run("print(1)");  // 直接执行（从缓存）

// 禁用缓存
vm.CacheEnabled = false;

// 清除缓存
vm.ClearCache();
```

---

## 下一步

- 查看 [API 参考文档](API.md) 了解完整的 API 列表
- 查看 `Samples/` 目录中的示例代码
- 在 Unity 中运行 Demo 场景体验所有功能
