# 快速入门

MiniPanda 是一个轻量级脚本语言，专为 Unity 设计。

## 安装

### 通过 Git URL（推荐）

1. 打开 Unity，选择 `Window > Package Manager`
2. 点击 `+` 按钮，选择 `Add package from git URL...`
3. 输入仓库地址

### 本地安装

将包文件夹复制到项目的 `Packages/` 目录下。

## 第一个脚本

```csharp
using Azathrix.MiniPanda;

// 创建虚拟机
var vm = new MiniPanda();
vm.Start();

// 执行代码
vm.Run("print(\"Hello, MiniPanda!\")");

// 关闭
vm.Shutdown();
```

## 在 Unity 中运行

创建一个 MonoBehaviour 脚本：

```csharp
using UnityEngine;
using Azathrix.MiniPanda;

public class MiniPandaRunner : MonoBehaviour
{
    private MiniPanda _panda;

    void Start()
    {
        _panda = new MiniPanda();
        _panda.Start();

        _panda.Run(@"
            var message = ""Hello from MiniPanda!""
            print(message)
        ");
    }

    void OnDestroy()
    {
        _panda?.Shutdown();
    }
}
```

## 下一步

- [基础语法](./basics) - 学习变量、运算符、数组等
- [函数](./functions) - 学习函数定义和 Lambda
- [面向对象](./oop) - 学习类和继承
