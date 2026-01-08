# 高级特性

## 异常处理

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

## 模块系统

### 导出模块 (utils.panda)

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

### 导入模块

```javascript
// 导入整个模块
import "utils" as u

print(u.VERSION)           // 1.0.0
u.helper()                 // Helper function called!
print(u.clamp(15, 0, 10))  // 10

// 路径转换 - 点号转目录
import "math.vector" as vec  // 加载 math/vector.panda
```

## 全局变量

### 使用 global 关键字

```javascript
global var config = {debug: true}

func test() {
    print(config.debug)  // true - 可以在任何作用域访问
}

test()
```

### 使用 _G 全局表

```javascript
_G.newGlobal = 100
print(_G.newGlobal)  // 100
print(_G.abs(-5))    // 5 (访问内置函数)
```

## 枚举

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
