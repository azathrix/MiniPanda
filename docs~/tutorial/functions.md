# 函数

## 函数定义

### 标准函数

```javascript
func add(a, b) {
    return a + b
}

print(add(3, 5))  // 8
```

### 单行函数

```javascript
func double(x) return x * 2

print(double(5))  // 10
```

## 默认参数

```javascript
func greet(name, greeting = "Hello") {
    return "{greeting}, {name}!"
}

print(greet("World"))           // Hello, World!
print(greet("World", "Hi"))     // Hi, World!
```

## 剩余参数

```javascript
func sum(...numbers) {
    var total = 0
    for n in numbers {
        total = total + n
    }
    return total
}

print(sum(1, 2, 3, 4, 5))  // 15
```

## Lambda 表达式

```javascript
// 单表达式 Lambda
var double = (x) => x * 2
print(double(5))  // 10

// 带默认参数的 Lambda
var add = (a, b = 10) => a + b
print(add(5))     // 15
print(add(5, 3))  // 8

// 多语句 Lambda
var process = (x) => {
    var result = x * 2
    return result + 1
}
```

## 闭包

```javascript
func makeCounter() {
    var count = 0
    return () => {
        count = count + 1
        return count
    }
}

var counter = makeCounter()
print(counter())  // 1
print(counter())  // 2
print(counter())  // 3

// 每个计数器独立
var counter2 = makeCounter()
print(counter2())  // 1
```

## 高阶函数

```javascript
// map
func map(arr, fn) {
    var result = []
    for item in arr {
        push(result, fn(item))
    }
    return result
}

var numbers = [1, 2, 3, 4, 5]
var squared = map(numbers, (x) => x * x)
print(join(squared, ", "))  // 1, 4, 9, 16, 25

// filter
func filter(arr, predicate) {
    var result = []
    for item in arr {
        if predicate(item) {
            push(result, item)
        }
    }
    return result
}

var evens = filter(numbers, (x) => x % 2 == 0)
print(join(evens, ", "))  // 2, 4
```
