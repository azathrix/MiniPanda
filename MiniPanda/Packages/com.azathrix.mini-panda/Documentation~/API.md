# MiniPanda API 参考

本文档提供 MiniPanda 的完整 API 参考，包括 C# API 和脚本语言 API。

## 目录

1. [C# API](#1-c-api)
   - [MiniPanda 类](#11-minipanda-类)
   - [Value 结构体](#12-value-结构体)
   - [NativeFunction 类](#13-nativefunction-类)
   - [Environment 类](#14-environment-类)
2. [脚本语言 API](#2-脚本语言-api)
   - [内置函数](#21-内置函数)
   - [内置对象](#22-内置对象)
   - [全局变量](#23-全局变量)
3. [语法参考](#3-语法参考)
   - [关键字](#31-关键字)
   - [运算符优先级](#32-运算符优先级)
   - [语句语法](#33-语句语法)
   - [表达式语法](#34-表达式语法)

---

## 1. C# API

### 1.1 MiniPanda 类

主入口类，提供脚本执行的所有功能。

**命名空间**: `Azathrix.MiniPanda`

#### 属性

| 属性 | 类型 | 说明 |
|------|------|------|
| `IsStarted` | `bool` | 是否已启动 |
| `VM` | `VirtualMachine` | 内部虚拟机实例（用于调试器） |
| `CacheEnabled` | `bool` | 是否启用字节码缓存 |
| `CustomLoader` | `FileLoader` | 自定义文件加载器 |

#### 生命周期方法

```csharp
// 创建实例
var vm = new MiniPanda();

// 启动引擎（注册内置函数）
vm.Start();

// 重置虚拟机状态
vm.Reset();

// 关闭引擎
vm.Shutdown();
```

#### 执行方法

```csharp
// 执行脚本代码
Value Run(string code, string scopeName = "main", bool clearScope = true)

// 执行字节码
Value Run(byte[] data, string scopeName = "main", bool clearScope = true)

// 执行脚本并转换返回值类型
T Run<T>(string code, string scopeName = "main", bool clearScope = true)

// 求值表达式
Value Eval(string expression, string scopeName = "main", bool clearScope = true)

// 求值表达式（带环境变量）
Value Eval(string expression, Dictionary<string, object> env, string scopeName = "main", bool clearScope = true)
Value Eval(string expression, Environment env, string scopeName = "main", bool clearScope = true)
Value Eval(string expression, IEnvironmentProvider env, string scopeName = "main", bool clearScope = true)

// 求值表达式并转换返回值类型
T Eval<T>(string expression, string scopeName = "main", bool clearScope = true)
T Eval<T>(string expression, Dictionary<string, object> env, string scopeName = "main", bool clearScope = true)
```

#### 编译方法

```csharp
// 编译脚本为字节码
CompiledScript Compile(string code)
```

#### 文件操作

```csharp
// 执行脚本文件
Value RunFile(string path)

// 加载文件内容
(byte[] data, string fullPath) LoadFile(string path)

// 加载模块
void LoadModule(byte[] data, string moduleName, string sourcePath = null)
```

#### 全局变量

```csharp
// 设置全局变量
void SetGlobal(string name, Value value)
void SetGlobal(string name, double value)
void SetGlobal(string name, bool value)
void SetGlobal(string name, string value)
void SetGlobal(string name, NativeFunction func)

// 获取全局变量
Value GetGlobal(string name)
```

#### 函数调用

```csharp
// 调用全局函数
Value Call(string funcName, params object[] args)

// 调用指定作用域的函数
Value Call(Environment scope, string funcName, params object[] args)
Value Call(Dictionary<string, object> scope, string funcName, params object[] args)
```

#### 作用域管理

```csharp
// 获取指定作用域
Environment GetScope(string name)

// 清除指定作用域
void ClearScope(string name)
```

#### 缓存管理

```csharp
// 清除字节码缓存
void ClearCache()
```

#### 静态工具方法

```csharp
// 检查数据是否为字节码格式
static bool IsBytecode(byte[] data)

// 转换路径格式
static string ConvertPath(string path)
```

---

### 1.2 Value 结构体

MiniPanda 的统一值类型，用于表示所有脚本值。

**命名空间**: `Azathrix.MiniPanda.Core`

#### 静态常量

```csharp
Value.Null   // 空值
Value.True   // 真值
Value.False  // 假值
```

#### 工厂方法

```csharp
// 从数字创建
static Value FromNumber(double n)

// 从布尔创建
static Value FromBool(bool b)

// 从堆对象创建
static Value FromObject(MiniPandaHeapObject obj)
```

#### 类型检查属性

| 属性 | 说明 |
|------|------|
| `IsNull` | 是否为空值 |
| `IsBool` | 是否为布尔值 |
| `IsNumber` | 是否为数字 |
| `IsObject` | 是否为堆对象 |
| `IsString` | 是否为字符串 |
| `IsArray` | 是否为数组 |
| `IsDict` | 是否为字典/对象 |
| `IsFunction` | 是否为可调用对象 |
| `IsClass` | 是否为类 |
| `IsInstance` | 是否为实例 |

#### 类型转换方法

```csharp
// 转换为数字
double AsNumber()

// 转换为布尔值（真值判断）
bool AsBool()

// 转换为字符串
string AsString()

// 获取堆对象
MiniPandaHeapObject AsObject()

// 尝试转换为指定堆对象类型
T As<T>() where T : MiniPandaHeapObject

// 获取可调用对象
ICallable AsCallable()

// 转换为 C# 类型
T To<T>()
T To<T>(VirtualMachine vm)
object ToType(Type targetType)
object ToType(Type targetType, VirtualMachine vm)
```

#### 隐式转换

```csharp
// 支持从以下类型隐式转换
double -> Value
bool -> Value
string -> Value
```

---

### 1.3 NativeFunction 类

用于创建可从脚本调用的 C# 函数。

**命名空间**: `Azathrix.MiniPanda.VM`

#### 工厂方法

```csharp
// 可变参数函数
static NativeFunction Create(Func<Value[], Value> func, int arity = -1)

// 无返回值的可变参数函数
static NativeFunction Create(Action<Value[]> action, int arity = -1)

// 带 VM 上下文的函数
static NativeFunction Create(Func<VirtualMachine, Value[], Value> func, int arity = -1)
static NativeFunction CreateWithVM(Func<VirtualMachine, Value[], Value> func, int arity = -1)

// 无参数函数
static NativeFunction Create(Func<Value> func)

// 单参数函数
static NativeFunction Create(Func<Value, Value> func)

// 双参数函数
static NativeFunction Create(Func<Value, Value, Value> func)

// 三参数函数
static NativeFunction Create(Func<Value, Value, Value, Value> func)
```

#### 使用示例

```csharp
// 无参数
vm.SetGlobal("getTime", NativeFunction.Create(() =>
    Value.FromNumber(Time.time)));

// 单参数
vm.SetGlobal("square", NativeFunction.Create((Value v) =>
    Value.FromNumber(v.AsNumber() * v.AsNumber())));

// 双参数
vm.SetGlobal("add", NativeFunction.Create((Value a, Value b) =>
    Value.FromNumber(a.AsNumber() + b.AsNumber())));

// 可变参数
vm.SetGlobal("sum", NativeFunction.Create(args => {
    double total = 0;
    foreach (var arg in args)
        total += arg.AsNumber();
    return Value.FromNumber(total);
}));

// 带 VM 上下文
vm.SetGlobal("getLocation", NativeFunction.CreateWithVM((vm, args) => {
    var location = vm.GetCurrentLocation();
    return Value.FromObject(MiniPandaString.Create(location));
}));
```

---

### 1.4 Environment 类

作用域环境，用于存储变量。

**命名空间**: `Azathrix.MiniPanda.Core`

#### 方法

```csharp
// 定义变量
void Define(string name, Value value)

// 设置变量值
void Set(string name, Value value)

// 获取变量值
Value Get(string name)

// 检查变量是否存在
bool Contains(string name)
```

---

## 2. 脚本语言 API

### 2.1 内置函数

#### 输出函数

| 函数 | 说明 | 示例 |
|------|------|------|
| `print(value)` | 打印值到控制台 | `print("Hello")` |
| `trace(...values)` | 打印值并显示位置信息 | `trace("debug", x)` |
| `debug(...values)` | 同 trace | `debug("info")` |

#### 类型函数

| 函数 | 说明 | 示例 |
|------|------|------|
| `type(value)` | 返回值的类型名 | `type(123)` → `"number"` |
| `str(value)` | 转换为字符串 | `str(123)` → `"123"` |
| `num(value)` | 转换为数字 | `num("123")` → `123` |
| `bool(value)` | 转换为布尔值 | `bool(1)` → `true` |

#### 数学函数

| 函数 | 说明 | 示例 |
|------|------|------|
| `abs(n)` | 绝对值 | `abs(-5)` → `5` |
| `floor(n)` | 向下取整 | `floor(3.7)` → `3` |
| `ceil(n)` | 向上取整 | `ceil(3.2)` → `4` |
| `round(n)` | 四舍五入 | `round(3.5)` → `4` |
| `sqrt(n)` | 平方根 | `sqrt(16)` → `4` |
| `pow(a, b)` | a 的 b 次方 | `pow(2, 3)` → `8` |
| `min(...values)` | 最小值 | `min(1, 2, 3)` → `1` |
| `max(...values)` | 最大值 | `max(1, 2, 3)` → `3` |
| `random()` | 0-1 随机数 | `random()` → `0.xxx` |
| `randomInt(min, max)` | 随机整数 | `randomInt(1, 10)` → `5` |

#### 集合函数

| 函数 | 说明 | 示例 |
|------|------|------|
| `len(collection)` | 返回长度 | `len([1,2,3])` → `3` |
| `push(arr, value)` | 向数组末尾添加元素 | `push(arr, 4)` |
| `pop(arr)` | 移除并返回数组末尾元素 | `pop(arr)` → `4` |
| `range(n)` | 生成 0 到 n-1 的范围 | `range(5)` → `[0,1,2,3,4]` |
| `range(start, end)` | 生成 start 到 end-1 的范围 | `range(1, 5)` → `[1,2,3,4]` |
| `range(start, end, step)` | 带步长的范围 | `range(0, 10, 2)` → `[0,2,4,6,8]` |
| `keys(obj)` | 返回对象所有键的数组 | `keys({a:1})` → `["a"]` |
| `values(obj)` | 返回对象所有值的数组 | `values({a:1})` → `[1]` |
| `contains(col, item)` | 检查是否包含元素 | `contains([1,2], 1)` → `true` |
| `slice(arr, start, end)` | 切片，支持负索引 | `slice([1,2,3], 0, 2)` → `[1,2]` |
| `join(arr, sep)` | 数组连接成字符串 | `join([1,2], ",")` → `"1,2"` |
| `split(str, sep)` | 字符串分割成数组 | `split("a,b", ",")` → `["a","b"]` |

#### 时间函数

| 函数 | 说明 | 示例 |
|------|------|------|
| `time()` | 当前 Unix 时间戳（秒） | `time()` → `1234567890.123` |
| `now()` | 当前 Unix 时间戳（毫秒） | `now()` → `1234567890123` |

#### 调试函数

| 函数 | 说明 | 示例 |
|------|------|------|
| `stacktrace()` | 返回调用栈字符串 | `stacktrace()` |
| `assert(condition)` | 断言，条件为 false 时抛出错误 | `assert(x > 0)` |
| `assert(condition, msg)` | 带消息的断言 | `assert(x > 0, "x must be positive")` |

---

### 2.2 内置对象

#### json 对象

| 方法 | 说明 | 示例 |
|------|------|------|
| `json.parse(str)` | 解析 JSON 字符串 | `json.parse("{\"a\":1}")` → `{a: 1}` |
| `json.stringify(value)` | 转换为 JSON 字符串 | `json.stringify({a: 1})` → `"{\"a\":1}"` |

#### date 对象

| 方法 | 说明 | 示例 |
|------|------|------|
| `date.now()` | 当前时间戳（毫秒） | `date.now()` |
| `date.time()` | 当前时间戳（秒，浮点） | `date.time()` |
| `date.year(ts)` | 获取年份 | `date.year(ts)` |
| `date.month(ts)` | 获取月份 | `date.month(ts)` |
| `date.day(ts)` | 获取日期 | `date.day(ts)` |
| `date.hour(ts)` | 获取小时 | `date.hour(ts)` |
| `date.minute(ts)` | 获取分钟 | `date.minute(ts)` |
| `date.second(ts)` | 获取秒 | `date.second(ts)` |
| `date.weekday(ts)` | 获取星期几（0=周日） | `date.weekday(ts)` |
| `date.format(ts, fmt)` | 格式化时间戳 | `date.format(ts, "yyyy-MM-dd")` |
| `date.parse(str)` | 解析日期字符串 | `date.parse("2024-01-01")` |
| `date.create(y,m,d,h?,m?,s?)` | 创建时间戳 | `date.create(2024, 1, 1)` |

#### regex 对象

| 方法 | 说明 | 示例 |
|------|------|------|
| `regex.test(pattern, str)` | 测试是否匹配 | `regex.test("\\d+", "123")` → `true` |
| `regex.match(pattern, str)` | 返回第一个匹配 | `regex.match("\\d+", "a123b")` |
| `regex.matchAll(pattern, str)` | 返回所有匹配 | `regex.matchAll("\\d+", "1a2b3")` |
| `regex.replace(pattern, str, repl)` | 替换匹配 | `regex.replace("\\d", "a1b2", "x")` → `"axbx"` |
| `regex.split(pattern, str)` | 按模式分割 | `regex.split("\\s+", "a b c")` → `["a","b","c"]` |

---

### 2.3 全局变量

| 变量 | 说明 | 示例 |
|------|------|------|
| `_G` | 全局表，可读写全局变量 | `_G.myVar = 100` |

---

## 3. 语法参考

### 3.1 关键字

| 关键字 | 说明 |
|--------|------|
| `var` | 变量声明 |
| `func` | 函数声明 |
| `class` | 类声明 |
| `if` / `else` | 条件语句 |
| `while` | while 循环 |
| `for` / `in` | for-in 循环 |
| `break` | 跳出循环 |
| `continue` | 跳过当前迭代 |
| `return` | 返回值 |
| `this` | 当前实例 |
| `super` | 父类引用 |
| `static` | 静态成员 |
| `global` | 全局声明 |
| `import` / `as` | 模块导入 |
| `export` | 模块导出 |
| `try` / `catch` / `finally` | 异常处理 |
| `throw` | 抛出异常 |
| `enum` | 枚举声明 |
| `true` / `false` | 布尔字面量 |
| `null` | 空值字面量 |

### 3.2 运算符优先级

从高到低：

| 优先级 | 运算符 | 说明 |
|--------|--------|------|
| 1 | `()` `[]` `.` | 调用、索引、属性访问 |
| 2 | `!` `-` `++` `--` | 一元运算符 |
| 3 | `*` `/` `%` | 乘除取模 |
| 4 | `+` `-` | 加减 |
| 5 | `<` `<=` `>` `>=` | 比较 |
| 6 | `==` `!=` | 相等 |
| 7 | `&&` | 逻辑与 |
| 8 | `\|\|` | 逻辑或 |
| 9 | `??` | 空值合并 |
| 10 | `? :` | 三元运算符 |
| 11 | `=` `+=` `-=` `*=` `/=` `%=` | 赋值 |

### 3.3 语句语法

#### 变量声明
```javascript
var name = value
global var name = value  // 全局变量
```

#### 函数声明
```javascript
func name(params) { body }
func name(params) return expression  // 单行
global func name(params) { body }    // 全局函数
```

#### 类声明
```javascript
class Name {
    // 静态成员
    static var field = value
    static func method() { }

    // 实例成员
    var field

    // 构造函数
    Name(params) { }

    // 方法
    func method() { }
}

// 继承
class Child : Parent { }
```

#### 控制流
```javascript
// 条件
if condition { } else if condition { } else { }
if condition statement  // 单行

// 循环
while condition { }
for item in collection { }
for key, value in object { }

// 跳转
break
continue
return value
```

#### 异常处理
```javascript
try {
    // 可能抛出异常的代码
} catch e {
    // 处理异常
} finally {
    // 清理代码
}

throw "error message"
throw value
```

#### 模块
```javascript
// 导入
import "module" as alias
import "path.to.module" as alias

// 导出
export var name = value
export func name() { }
export class Name { }
```

#### 枚举
```javascript
enum Name {
    Value1,
    Value2,
    Value3
}
```

### 3.4 表达式语法

#### 字面量
```javascript
123          // 数字
3.14         // 浮点数
"string"     // 字符串
"Hello {x}"  // 字符串插值
true / false // 布尔
null         // 空值
[1, 2, 3]    // 数组
{a: 1, b: 2} // 对象
```

#### Lambda
```javascript
(x) => x * 2           // 单表达式
(a, b) => a + b        // 多参数
(x = 10) => x * 2      // 默认参数
(x) => { return x * 2 } // 多语句
```

#### 运算符
```javascript
// 算术
a + b, a - b, a * b, a / b, a % b

// 比较
a == b, a != b, a < b, a <= b, a > b, a >= b

// 逻辑
a && b, a || b, !a

// 三元
condition ? trueValue : falseValue

// 空值合并
a ?? b

// 赋值
a = b, a += b, a -= b, a *= b, a /= b, a %= b

// 自增自减
++a, a++, --a, a--
```

#### 访问
```javascript
obj.property     // 属性访问
obj["key"]       // 索引访问
func(args)       // 函数调用
arr[index]       // 数组索引
```

---

## 更多信息

- 查看 [教程](Tutorial.md) 了解详细用法
- 查看 `Samples/` 目录中的示例代码
