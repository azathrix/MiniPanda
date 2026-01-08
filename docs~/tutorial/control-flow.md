# 控制流

## 条件语句

### 单行形式

```javascript
if x > 0 print("positive")
```

### 多行形式

```javascript
if x > 0 {
    print("positive")
} else if x < 0 {
    print("negative")
} else {
    print("zero")
}
```

## 循环语句

### while 循环

```javascript
var i = 0
while i < 5 {
    print(i)
    i = i + 1
}
```

### for-in 循环

```javascript
// 遍历数组
var arr = [1, 2, 3]
for item in arr {
    print(item)
}

// 遍历范围
for i in range(10) {
    print(i)  // 0 到 9
}

// 遍历对象（键值对）
var obj = {a: 1, b: 2}
for k, v in obj {
    print("{k}: {v}")
}
```

## break 和 continue

```javascript
// break - 跳出循环
for i in range(10) {
    if i == 5 break
    print(i)  // 0, 1, 2, 3, 4
}

// continue - 跳过当前迭代
for i in range(10) {
    if i % 2 == 0 continue
    print(i)  // 1, 3, 5, 7, 9
}
```
