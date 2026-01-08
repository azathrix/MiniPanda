# 内置对象

MiniPanda 提供的内置对象及其方法。

## json 对象

用于 JSON 序列化和反序列化。

| 方法 | 说明 | 示例 |
|------|------|------|
| `json.parse(str)` | 解析 JSON 字符串 | `json.parse("{\"a\":1}")` → `{a: 1}` |
| `json.stringify(value)` | 转换为 JSON 字符串 | `json.stringify({a: 1})` → `"{\"a\":1}"` |

```javascript
// 解析 JSON
var data = json.parse('{"name": "test", "value": 123}')
print(data.name)   // "test"
print(data.value)  // 123

// 序列化为 JSON
var obj = {
    name: "player",
    score: 100,
    items: ["sword", "shield"]
}
var jsonStr = json.stringify(obj)
print(jsonStr)  // {"name":"player","score":100,"items":["sword","shield"]}
```

## date 对象

用于日期和时间操作。

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

```javascript
// 获取当前时间
var ts = date.now()
print(ts)  // 1704067200000

// 获取时间组件
print(date.year(ts))    // 2024
print(date.month(ts))   // 1
print(date.day(ts))     // 1
print(date.hour(ts))    // 12
print(date.minute(ts))  // 30
print(date.second(ts))  // 45
print(date.weekday(ts)) // 1 (周一)

// 格式化时间
print(date.format(ts, "yyyy-MM-dd"))         // "2024-01-01"
print(date.format(ts, "yyyy-MM-dd HH:mm:ss")) // "2024-01-01 12:30:45"

// 解析日期字符串
var parsed = date.parse("2024-06-15")
print(date.year(parsed))  // 2024
print(date.month(parsed)) // 6

// 创建时间戳
var custom = date.create(2024, 12, 25, 10, 30, 0)
print(date.format(custom, "yyyy-MM-dd HH:mm")) // "2024-12-25 10:30"
```

## regex 对象

用于正则表达式操作。

| 方法 | 说明 | 示例 |
|------|------|------|
| `regex.test(pattern, str)` | 测试是否匹配 | `regex.test("\\d+", "123")` → `true` |
| `regex.match(pattern, str)` | 返回第一个匹配 | `regex.match("\\d+", "a123b")` |
| `regex.matchAll(pattern, str)` | 返回所有匹配 | `regex.matchAll("\\d+", "1a2b3")` |
| `regex.replace(pattern, str, repl)` | 替换匹配 | `regex.replace("\\d", "a1b2", "x")` → `"axbx"` |
| `regex.split(pattern, str)` | 按模式分割 | `regex.split("\\s+", "a b c")` → `["a","b","c"]` |

```javascript
// 测试匹配
print(regex.test("\\d+", "abc123"))  // true
print(regex.test("\\d+", "abc"))     // false

// 获取匹配
var match = regex.match("(\\d+)", "price: 100")
print(match)  // ["100", "100"]

// 获取所有匹配
var matches = regex.matchAll("\\d+", "a1b2c3")
print(matches)  // ["1", "2", "3"]

// 替换
var result = regex.replace("\\d", "a1b2c3", "X")
print(result)  // "aXbXcX"

// 分割
var parts = regex.split("\\s+", "hello   world  test")
print(parts)  // ["hello", "world", "test"]
```

::: tip 正则表达式语法
MiniPanda 使用 .NET 正则表达式语法。在字符串中需要双重转义反斜杠：`"\\d"` 表示 `\d`。
:::

## _G 全局表

全局变量表，可用于动态访问和设置全局变量。

```javascript
// 设置全局变量
_G.myGlobal = 100
print(_G.myGlobal)  // 100

// 动态访问
var name = "myGlobal"
print(_G[name])  // 100

// 访问内置函数
print(_G.abs(-5))  // 5
print(_G.len([1, 2, 3]))  // 3

// 检查变量是否存在
if _G.someVar != null {
    print("变量存在")
}
```
