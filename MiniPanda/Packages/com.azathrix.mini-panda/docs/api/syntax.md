# 语法参考

MiniPanda 语言的完整语法参考。

## 关键字

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

## 运算符优先级

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

## 语句语法

### 变量声明

```javascript
var name = value
global var name = value  // 全局变量
```

### 函数声明

```javascript
func name(params) { body }
func name(params) return expression  // 单行
global func name(params) { body }    // 全局函数

// 默认参数
func greet(name = "World") {
    print("Hello, {name}!")
}

// 剩余参数
func sum(...numbers) {
    var total = 0
    for n in numbers {
        total += n
    }
    return total
}
```

### 类声明

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
class Child : Parent {
    Child() {
        super()  // 调用父类构造函数
    }
}
```

### 控制流

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

### 异常处理

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

### 模块

```javascript
// 导入
import "module" as alias
import "path.to.module" as alias

// 导出
export var name = value
export func name() { }
export class Name { }
```

### 枚举

```javascript
enum Name {
    Value1,
    Value2,
    Value3
}
```

## 表达式语法

### 字面量

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

### Lambda 表达式

```javascript
(x) => x * 2           // 单表达式
(a, b) => a + b        // 多参数
(x = 10) => x * 2      // 默认参数
(x) => { return x * 2 } // 多语句
```

### 运算符

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

### 访问表达式

```javascript
obj.property     // 属性访问
obj["key"]       // 索引访问
func(args)       // 函数调用
arr[index]       // 数组索引
```

## 注释

```javascript
// 单行注释

/*
 * 多行注释
 */
```
