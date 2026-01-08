# 面向对象

## 类定义

```javascript
class Player {
    // 构造函数（与类同名）
    Player(name) {
        this.name = name
        this.hp = 100
    }

    // 方法
    func takeDamage(amount) {
        this.hp = this.hp - amount
        if this.hp < 0 {
            this.hp = 0
        }
    }

    func isAlive() {
        return this.hp > 0
    }
}

var player = Player("Hero")
print(player.name)  // Hero
print(player.hp)    // 100

player.takeDamage(30)
print(player.hp)    // 70
```

## 继承

```javascript
class Entity {
    Entity(name) {
        this.name = name
        this.hp = 100
    }

    func takeDamage(amount) {
        this.hp = this.hp - amount
    }
}

class Player : Entity {
    Player(name, level) {
        super.Entity(name)  // 调用父类构造函数
        this.level = level
        this.hp = 100 + level * 10
    }

    func levelUp() {
        this.level = this.level + 1
        this.hp = this.hp + 10
    }
}

var hero = Player("Hero", 1)
print(hero.hp)  // 110
hero.levelUp()
print(hero.level)  // 2
```

## 静态成员

```javascript
class Counter {
    // 静态变量
    static var count = 0

    // 静态方法
    static func increment() {
        Counter.count = Counter.count + 1
        return Counter.count
    }

    static func getCount() {
        return Counter.count
    }

    // 实例变量
    var id

    Counter() {
        Counter.count = Counter.count + 1
        this.id = Counter.count
    }
}

// 访问静态成员
print(Counter.count)       // 0
print(Counter.increment()) // 1
print(Counter.increment()) // 2

// 创建实例
var a = Counter()
var b = Counter()
print(Counter.count)  // 4
print(a.id)           // 3
print(b.id)           // 4
```
