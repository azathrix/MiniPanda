# NativeFunction 类

用于创建可从脚本调用的 C# 函数。

**命名空间**: `Azathrix.MiniPanda.VM`

## 工厂方法

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

## 使用示例

### 无参数函数

```csharp
vm.SetGlobal("getTime", NativeFunction.Create(() =>
    Value.FromNumber(Time.time)));
```

### 单参数函数

```csharp
vm.SetGlobal("square", NativeFunction.Create((Value v) =>
    Value.FromNumber(v.AsNumber() * v.AsNumber())));
```

### 双参数函数

```csharp
vm.SetGlobal("add", NativeFunction.Create((Value a, Value b) =>
    Value.FromNumber(a.AsNumber() + b.AsNumber())));
```

### 可变参数函数

```csharp
vm.SetGlobal("sum", NativeFunction.Create(args => {
    double total = 0;
    foreach (var arg in args)
        total += arg.AsNumber();
    return Value.FromNumber(total);
}));
```

### 带 VM 上下文

```csharp
vm.SetGlobal("getLocation", NativeFunction.CreateWithVM((vm, args) => {
    var location = vm.GetCurrentLocation();
    return Value.FromObject(MiniPandaString.Create(location));
}));
```

### 无返回值函数

```csharp
vm.SetGlobal("log", NativeFunction.Create((Action<Value[]>)(args => {
    foreach (var arg in args)
        Debug.Log(arg.AsString());
})));
```

## 参数说明

| 参数 | 说明 |
|------|------|
| `func` | 要执行的 C# 函数 |
| `arity` | 参数数量，-1 表示可变参数 |

## 注意事项

- 使用 `Create` 方法创建的函数会自动处理参数转换
- `CreateWithVM` 允许访问虚拟机上下文，可用于获取调用位置等信息
- 返回值必须是 `Value` 类型，使用 `Value.FromNumber()` 等方法创建
