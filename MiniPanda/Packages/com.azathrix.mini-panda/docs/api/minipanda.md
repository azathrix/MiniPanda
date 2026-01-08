# MiniPanda 类

主入口类，提供脚本执行的所有功能。

**命名空间**: `Azathrix.MiniPanda`

## 属性

| 属性 | 类型 | 说明 |
|------|------|------|
| `IsStarted` | `bool` | 是否已启动 |
| `VM` | `VirtualMachine` | 内部虚拟机实例 |
| `CacheEnabled` | `bool` | 是否启用字节码缓存 |
| `CustomLoader` | `FileLoader` | 自定义文件加载器 |

## 生命周期方法

```csharp
// 创建实例
var vm = new MiniPanda();

// 启动引擎
vm.Start();

// 重置状态
vm.Reset();

// 关闭引擎
vm.Shutdown();
```

## 执行方法

### Run

```csharp
// 执行脚本代码
Value Run(string code, string scopeName = "main", bool clearScope = true)

// 执行字节码
Value Run(byte[] data, string scopeName = "main", bool clearScope = true)

// 执行并转换返回值
T Run<T>(string code, string scopeName = "main", bool clearScope = true)
```

### Eval

```csharp
// 求值表达式
Value Eval(string expression, string scopeName = "main", bool clearScope = true)

// 带环境变量
Value Eval(string expression, Dictionary<string, object> env, ...)
Value Eval(string expression, Environment env, ...)

// 转换返回值
T Eval<T>(string expression, ...)
```

## 编译方法

```csharp
CompiledScript Compile(string code)
```

## 文件操作

```csharp
Value RunFile(string path)
(byte[] data, string fullPath) LoadFile(string path)
void LoadModule(byte[] data, string moduleName, string sourcePath = null)
```

## 全局变量

```csharp
void SetGlobal(string name, Value value)
void SetGlobal(string name, double value)
void SetGlobal(string name, bool value)
void SetGlobal(string name, string value)
void SetGlobal(string name, NativeFunction func)
Value GetGlobal(string name)
```

## 函数调用

```csharp
Value Call(string funcName, params object[] args)
Value Call(Environment scope, string funcName, params object[] args)
Value Call(Dictionary<string, object> scope, string funcName, params object[] args)
```

## 作用域管理

```csharp
Environment GetScope(string name)
void ClearScope(string name)
```

## 缓存管理

```csharp
void ClearCache()
```

## 静态方法

```csharp
static bool IsBytecode(byte[] data)
static string ConvertPath(string path)
```
