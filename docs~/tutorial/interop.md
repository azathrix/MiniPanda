# C# 互操作

## 注册变量

```csharp
vm.SetGlobal("PI", 3.14159);
vm.SetGlobal("playerName", "Hero");
vm.SetGlobal("isDebug", true);
```

## 注册函数

```csharp
// 无参数
vm.SetGlobal("getTime", NativeFunc.Create(() =>
    Value.FromNumber(Time.time)));

// 单参数
vm.SetGlobal("square", NativeFunc.Create((Value v) =>
    Value.FromNumber(v.AsNumber() * v.AsNumber())));

// 双参数
vm.SetGlobal("add", NativeFunc.Create((Value a, Value b) =>
    Value.FromNumber(a.AsNumber() + b.AsNumber())));

// 可变参数
vm.SetGlobal("sum", NativeFunc.Create(args => {
    double total = 0;
    foreach (var arg in args)
        total += arg.AsNumber();
    return Value.FromNumber(total);
}));

// 带 VM 上下文
vm.SetGlobal("getLocation", NativeFunc.CreateWithVM((vm, args) => {
    var location = vm.GetCurrentLocation();
    return Value.FromObject(MiniPandaString.Create(location));
}));
```

## 调用脚本函数

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

## 临时环境求值

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

## 自定义文件加载

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

## 获取脚本函数作为委托

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
