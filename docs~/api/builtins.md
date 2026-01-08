# 内置函数

MiniPanda 提供的标准库函数。

## 输出函数

| 函数 | 说明 | 示例 |
|------|------|------|
| `print(value)` | 打印值到控制台 | `print("Hello")` |
| `trace(...values)` | 打印值并显示位置信息 | `trace("debug", x)` |
| `debug(...values)` | 同 trace | `debug("info")` |

```javascript
print("Hello World")
print(123)
print({name: "test"})

trace("调试信息")  // [TRACE] 调试信息 (at script.panda:10)
```

## 类型函数

| 函数 | 说明 | 示例 |
|------|------|------|
| `type(value)` | 返回值的类型名 | `type(123)` → `"number"` |
| `str(value)` | 转换为字符串 | `str(123)` → `"123"` |
| `num(value)` | 转换为数字 | `num("123")` → `123` |
| `bool(value)` | 转换为布尔值 | `bool(1)` → `true` |

```javascript
print(type(123))      // "number"
print(type("hello"))  // "string"
print(type([1,2,3]))  // "array"
print(type({a: 1}))   // "object"

print(str(123))       // "123"
print(num("3.14"))    // 3.14
print(bool(0))        // false
print(bool(1))        // true
```

## 数学函数

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

```javascript
print(abs(-10))        // 10
print(floor(3.9))      // 3
print(ceil(3.1))       // 4
print(round(3.5))      // 4
print(sqrt(25))        // 5
print(pow(2, 10))      // 1024
print(min(5, 3, 8))    // 3
print(max(5, 3, 8))    // 8
print(random())        // 0.123456...
print(randomInt(1, 6)) // 1-6 之间的整数
```

## 集合函数

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

```javascript
var arr = [1, 2, 3]
print(len(arr))           // 3
push(arr, 4)              // arr = [1, 2, 3, 4]
var last = pop(arr)       // last = 4, arr = [1, 2, 3]

// range 函数
for i in range(5) {
    print(i)  // 0, 1, 2, 3, 4
}

for i in range(1, 5) {
    print(i)  // 1, 2, 3, 4
}

for i in range(0, 10, 2) {
    print(i)  // 0, 2, 4, 6, 8
}

// 对象操作
var obj = {a: 1, b: 2}
print(keys(obj))    // ["a", "b"]
print(values(obj))  // [1, 2]

// 数组操作
print(contains([1, 2, 3], 2))  // true
print(slice([1, 2, 3, 4], 1, 3))  // [2, 3]
print(slice([1, 2, 3, 4], -2))    // [3, 4]
print(join(["a", "b", "c"], "-")) // "a-b-c"
print(split("a,b,c", ","))        // ["a", "b", "c"]
```

## 时间函数

| 函数 | 说明 | 示例 |
|------|------|------|
| `time()` | 当前 Unix 时间戳（秒） | `time()` → `1234567890.123` |
| `now()` | 当前 Unix 时间戳（毫秒） | `now()` → `1234567890123` |

```javascript
print(time())  // 1704067200.123
print(now())   // 1704067200123
```

## 调试函数

| 函数 | 说明 | 示例 |
|------|------|------|
| `stacktrace()` | 返回调用栈字符串 | `stacktrace()` |
| `assert(condition)` | 断言，条件为 false 时抛出错误 | `assert(x > 0)` |
| `assert(condition, msg)` | 带消息的断言 | `assert(x > 0, "x must be positive")` |

```javascript
func inner() {
    return stacktrace()
}

func outer() {
    return inner()
}

print(outer())  // 输出调用栈

var x = 10
assert(x > 0)                        // 通过
assert(x > 0, "x must be positive")  // 通过
assert(x < 0)                        // 失败，抛出错误
```
