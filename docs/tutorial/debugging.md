# 调试

## 调试函数

### print

```javascript
print("Hello")
print(123)
print({name: "test"})
```

### trace / debug

带位置信息的打印：

```javascript
trace("debug info")  // [TRACE] debug info (at script.panda:10)
debug("message")     // 同 trace
```

### stacktrace

获取调用栈：

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

### assert

断言：

```javascript
var x = 10
assert(x > 0)                        // 通过
assert(x > 0, "x must be positive")  // 带消息的断言
assert(x < 0)                        // 失败，抛出错误
```

## 断点调试 (DAP)

MiniPanda 支持 Debug Adapter Protocol (DAP)，可以与 VS Code 等 IDE 集成进行断点调试。

### 启动调试服务器

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

### 调试功能

- 设置断点（支持条件断点）
- 单步执行（Step Over / Step In / Step Out）
- 查看变量值
- 查看调用栈
- 异常断点

## 编译缓存

```csharp
// 相同代码只编译一次
vm.Run("print(1)");  // 编译 + 执行
vm.Run("print(1)");  // 直接执行（从缓存）

// 禁用缓存
vm.CacheEnabled = false;

// 清除缓存
vm.ClearCache();
```
