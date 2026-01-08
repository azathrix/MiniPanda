# Value 结构体

MiniPanda 的统一值类型，用于表示所有脚本值。

**命名空间**: `Azathrix.MiniPanda.Core`

## 静态常量

```csharp
Value.Null   // 空值
Value.True   // 真值
Value.False  // 假值
```

## 工厂方法

```csharp
// 从数字创建
static Value FromNumber(double n)

// 从布尔创建
static Value FromBool(bool b)

// 从堆对象创建
static Value FromObject(MiniPandaHeapObject obj)
```

## 类型检查属性

| 属性 | 说明 |
|------|------|
| `IsNull` | 是否为空值 |
| `IsBool` | 是否为布尔值 |
| `IsNumber` | 是否为数字 |
| `IsObject` | 是否为堆对象 |
| `IsString` | 是否为字符串 |
| `IsArray` | 是否为数组 |
| `IsDict` | 是否为字典/对象 |
| `IsFunction` | 是否为可调用对象 |
| `IsClass` | 是否为类 |
| `IsInstance` | 是否为实例 |

## 类型转换方法

```csharp
// 转换为数字
double AsNumber()

// 转换为布尔值（真值判断）
bool AsBool()

// 转换为字符串
string AsString()

// 获取堆对象
MiniPandaHeapObject AsObject()

// 尝试转换为指定堆对象类型
T As<T>() where T : MiniPandaHeapObject

// 获取可调用对象
ICallable AsCallable()

// 转换为 C# 类型
T To<T>()
T To<T>(VirtualMachine vm)
object ToType(Type targetType)
object ToType(Type targetType, VirtualMachine vm)
```

## 隐式转换

```csharp
// 支持从以下类型隐式转换
double -> Value
bool -> Value
string -> Value
```

## 使用示例

```csharp
// 创建值
var num = Value.FromNumber(42);
var str = Value.FromObject(MiniPandaString.Create("hello"));
var arr = Value.FromObject(MiniPandaArray.Create());

// 类型检查
if (num.IsNumber) {
    double n = num.AsNumber();
}

// 类型转换
int result = someValue.To<int>();
string text = someValue.To<string>();
```
