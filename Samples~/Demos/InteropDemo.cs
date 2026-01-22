using System;
using System.Collections.Generic;
using UnityEngine;
using Azathrix.MiniPanda.Core;
using Azathrix.MiniPanda.VM;

namespace Azathrix.MiniPanda.Samples
{
    /// <summary>
    /// C# 互操作示例：注册变量、函数、获取脚本值
    /// </summary>
    public class InteropDemo : DemoBase
    {
        protected override void RunDemo()
        {
            Log("=== C# 互操作示例 ===");

            // 注册全局变量
            _panda.SetGlobal("gameVersion", "1.0.0");
            _panda.SetGlobal("maxPlayers", 4);

            _panda.Run(@"
                print(""游戏版本: "" + gameVersion)
                print(""最大玩家数: "" + maxPlayers)
            ");

            // 注册 C# 函数
            _panda.SetGlobal("heal", NativeFunction.Create((Value amount) =>
            {
                UnityEngine.Debug.Log($"[C#] heal called with: {amount.AsNumber()}");
                return Value.FromNumber(amount.AsNumber() * 2);
            }));

            _panda.Run(@"
                var result = heal(50)
                print(""heal(50) = "" + result)
            ");

            // 获取脚本函数作为委托
            var func = _panda.Run<Func<float, float, float>>(@"
                func calculateDamage(baseDamage, multiplier) {
                    return baseDamage * multiplier
                }
                return calculateDamage
            ");

            var damage = func(100, 1.5f);
            Log($"计算伤害 (C# 调用): {damage}");

            // 临时环境求值
            Dictionary<string, object> env = new()
            {
                ["hp"] = 100,
                ["damage"] = 30
            };

            var result = _panda.Eval("hp - damage", env);
            Log($"剩余 HP: {result.AsNumber()}");

            // 使用作用域
            var scope = _panda.GetScope("player");
            scope.Set("atk", Value.FromNumber(10));
            scope.Set("def", Value.FromNumber(5));

            var dmg = _panda.Eval<float>("atk * 1.5 - def", "player");
            Log($"伤害计算: {dmg}");
        }
    }
}
