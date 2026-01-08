# 基础语法

## 变量与类型

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

## 运算符

### 算术运算符

```javascript
var a = 10 + 5    // 加法: 15
var b = 10 - 5    // 减法: 5
var c = 10 * 5    // 乘法: 50
var d = 10 / 5    // 除法: 2
var e = 10 % 3    // 取模: 1
```

### 复合赋值

```javascript
var x = 10
x += 5   // x = 15
x -= 3   // x = 12
x *= 2   // x = 24
x /= 4   // x = 6
```

### 自增自减

```javascript
var x = 10
++x      // 前置自增: x = 11
x++      // 后置自增: x = 12
--x      // 前置自减: x = 11
x--      // 后置自减: x = 10
```

### 比较运算符

```javascript
var a = 10 == 10   // 等于: true
var b = 10 != 5    // 不等于: true
var c = 10 > 5     // 大于: true
var d = 10 < 5     // 小于: false
```

### 逻辑运算符

```javascript
var a = true && false   // 与: false
var b = true || false   // 或: true
var c = !true           // 非: false
```

### 三元运算符

```javascript
var result = x > 0 ? "positive" : "negative"
```

### 空值合并

```javascript
var value = null ?? "default"     // "default"
var name = userName ?? "Guest"    // 如果 userName 为 null，使用 "Guest"
```

## 字符串与插值

```javascript
var name = "World"
var greeting = "Hello {name}!"  // 字符串插值
print(greeting)  // Hello World!

// 表达式插值
print("Result: {10 + 5}")  // Result: 15

// 转义大括号
print("\{literal}")  // {literal}
```

## 数组

```javascript
var arr = [1, 2, 3, 4, 5]
arr[0] = 100           // 修改元素
print(arr[0])          // 100
print(len(arr))        // 5

// 数组操作
push(arr, 6)           // 添加元素
var last = pop(arr)    // 移除并返回最后一个元素
```

## 对象/字典

```javascript
var obj = {name: "test", value: 42}
obj.name = "new"       // 点号访问
obj["key"] = 123       // 方括号访问

// 获取键和值
var k = keys(obj)      // ["name", "value", "key"]
var v = values(obj)    // ["new", 42, 123]
```
