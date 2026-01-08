using UnityEngine;

namespace Azathrix.MiniPanda.Samples
{
    /// <summary>
    /// 类示例：类定义、继承、静态成员
    /// </summary>
    public class ClassDemo : DemoBase
    {
        protected override void RunDemo()
        {
            Log("=== 类示例 ===");

            // 基本类
            _panda.Run(@"
                class Vector2 {
                    Vector2(x, y) {
                        this.x = x
                        this.y = y
                    }

                    func add(other) {
                        return Vector2(this.x + other.x, this.y + other.y)
                    }

                    func magnitude() {
                        return sqrt(this.x * this.x + this.y * this.y)
                    }

                    func toString() {
                        return ""("" + this.x + "", "" + this.y + "")""
                    }
                }

                var v1 = Vector2(3, 4)
                var v2 = Vector2(1, 2)
                var v3 = v1.add(v2)

                print(""v1 = "" + v1.toString())
                print(""v2 = "" + v2.toString())
                print(""v1 + v2 = "" + v3.toString())
                print(""v1 长度 = "" + v1.magnitude())
            ");

            // 继承
            _panda.Run(@"
                class Entity {
                    Entity(name) {
                        this.name = name
                    }

                    func describe() {
                        return ""Entity: "" + this.name
                    }
                }

                class Player : Entity {
                    Player(name, level) {
                        super.Entity(name)
                        this.level = level
                    }

                    func describe() {
                        return ""Player: "" + this.name + "" (Lv."" + this.level + "")""
                    }
                }

                var entity = Entity(""NPC"")
                var player = Player(""Hero"", 10)

                print(entity.describe())
                print(player.describe())
            ");

            // 静态成员
            _panda.Run(@"
                class Counter {
                    static var count = 0

                    static func increment() {
                        Counter.count = Counter.count + 1
                        return Counter.count
                    }

                    Counter() {
                        Counter.count = Counter.count + 1
                        this.id = Counter.count
                    }
                }

                print(""初始 count: "" + Counter.count)
                print(""increment: "" + Counter.increment())
                print(""increment: "" + Counter.increment())

                Counter.count = 0
                var a = Counter()
                var b = Counter()
                print(""创建2个实例后, 总数: "" + Counter.count)
            ");
        }
    }
}
