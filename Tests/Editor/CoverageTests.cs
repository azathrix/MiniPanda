using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Azathrix.MiniPanda;
using Azathrix.MiniPanda.Compiler;
using Azathrix.MiniPanda.Core;
using Azathrix.MiniPanda.Exceptions;
using Azathrix.MiniPanda.VM;
using MPEnvironment = Azathrix.MiniPanda.Core.Environment;

namespace Azathrix.MiniPanda.Tests
{
    [TestFixture]
    public class BytecodeTests
    {
        [Test]
        public void Bytecode_SerializeDeserialize_RoundTrip()
        {
            var funcCode = new Bytecode();
            funcCode.Emit(Opcode.Null, 1);
            funcCode.Emit(Opcode.Return, 1);

            var proto = new FunctionPrototype
            {
                Name = "f",
                ClassName = "C",
                Arity = 2,
                UpvalueCount = 1,
                Code = funcCode
            };

            var bytecode = new Bytecode();
            bytecode.AddConstant(null);
            bytecode.AddConstant(1.5);
            bytecode.AddConstant("hello");
            bytecode.AddConstant(true);
            bytecode.AddConstant(proto);
            bytecode.Emit(Opcode.Const, 1);
            bytecode.EmitShort(1, 1);
            bytecode.Emit(Opcode.Return, 1);

            var data = bytecode.Serialize();
            var restored = Bytecode.Deserialize(data);

            Assert.AreEqual(bytecode.Constants.Count, restored.Constants.Count);
            var restoredProto = restored.Constants[4] as FunctionPrototype;
            Assert.IsNotNull(restoredProto);
            Assert.AreEqual("f", restoredProto.Name);
            Assert.AreEqual("C", restoredProto.ClassName);
            Assert.AreEqual(2, restoredProto.Arity);
            Assert.AreEqual(1, restoredProto.UpvalueCount);
            Assert.IsNotNull(restoredProto.Code);
        }

        [Test]
        public void Bytecode_Serialize_UnsupportedConstant_Throws()
        {
            var bytecode = new Bytecode();
            bytecode.AddConstant(new object());
            Assert.Throws<InvalidOperationException>(() => bytecode.Serialize());
        }

        [Test]
        public void Bytecode_Deserialize_InvalidMagic_Throws()
        {
            var data = new byte[] { 0, 1, 2, 3, 0, 0, 0, 0 };
            Assert.Throws<InvalidDataException>(() => Bytecode.Deserialize(data));
        }

        [Test]
        public void Bytecode_Deserialize_UnsupportedVersion_Throws()
        {
            var bytecode = new Bytecode();
            bytecode.Emit(Opcode.Null, 1);
            bytecode.Emit(Opcode.Return, 1);
            var data = bytecode.Serialize();
            data[4] = 0;
            Assert.Throws<InvalidDataException>(() => Bytecode.Deserialize(data));
        }

        [Test]
        public void Bytecode_Deserialize_UnknownConstantType_Throws()
        {
            var bytecode = new Bytecode();
            bytecode.AddConstant(1.0);
            bytecode.Emit(Opcode.Return, 1);
            var data = bytecode.Serialize();
            data[9] = 99;
            Assert.Throws<InvalidDataException>(() => Bytecode.Deserialize(data));
        }

        [Test]
        public void Bytecode_PatchJump_TooLarge_Throws()
        {
            var bytecode = new Bytecode();
            var offset = bytecode.EmitJump(Opcode.Jump, 1);
            for (int i = 0; i < 70000; i++)
            {
                bytecode.Code.Add(0);
            }
            Assert.Throws<InvalidOperationException>(() => bytecode.PatchJump(offset));
        }

        [Test]
        public void Bytecode_EmitLoop_TooLarge_Throws()
        {
            var bytecode = new Bytecode();
            for (int i = 0; i < 70000; i++)
            {
                bytecode.Code.Add(0);
            }
            Assert.Throws<InvalidOperationException>(() => bytecode.EmitLoop(0, 1));
        }

        [Test]
        public void ClassPrototype_Basic()
        {
            var proto = new ClassPrototype { Name = "C" };
            proto.Methods["m"] = new FunctionPrototype { Name = "m", Arity = 0, Code = new Bytecode() };
            Assert.AreEqual("C", proto.Name);
            Assert.AreEqual(1, proto.Methods.Count);
        }
    }

    [TestFixture]
    public class CompiledScriptTests
    {
        [Test]
        public void CompiledScript_SaveLoad_RoundTrip()
        {
            var bytecode = new Bytecode();
            bytecode.Emit(Opcode.Null, 1);
            bytecode.Emit(Opcode.Return, 1);
            var compiled = new CompiledScript(bytecode, "hash");

            var directory = "Temp";
            Directory.CreateDirectory(directory);
            var path = Path.Combine(directory, "compiled_test.mpbc");

            try
            {
                compiled.SaveToFile(path);
                var loaded = CompiledScript.LoadFromFile(path);
                Assert.IsNotNull(loaded.Bytecode);
                Assert.AreEqual("hash", compiled.SourceHash);
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }

    [TestFixture]
    public class CoreObjectTests
    {
        [Test]
        public void MiniPandaObject_Contains_ToString()
        {
            var obj = new MiniPandaObject();
            obj.Set("a", Value.FromNumber(1));
            Assert.IsTrue(obj.Contains("a"));
            var text = obj.ToString();
            Assert.IsTrue(text.Contains("a"));
        }

        [Test]
        public void MiniPandaBoundMethod_Call()
        {
            var vm = new MiniPanda();
            vm.Start();
            try
            {
                var instance = vm.Run(@"
                    class Foo {
                        Foo() { }
                        func bar() { return 7 }
                    }
                    return Foo()
                ").As<MiniPandaInstance>();
                var method = instance.Class.FindMethod("bar");
                var bound = new MiniPandaBoundMethod(instance, method);
                var result = bound.Call(vm.VM, Array.Empty<Value>());
                Assert.AreEqual(7.0, result.AsNumber());
                Assert.IsTrue(bound.ToString().Contains("bar"));
            }
            finally
            {
                vm.Shutdown();
            }
        }

        [Test]
        public void Upvalue_Set_OpenAndClosed()
        {
            var stack = new Value[1];
            stack[0] = Value.FromNumber(1);
            var upvalue = new Upvalue { Index = 0 };

            upvalue.Set(stack, Value.FromNumber(2));
            Assert.AreEqual(2.0, stack[0].AsNumber());

            upvalue.Close(stack);
            upvalue.Set(stack, Value.FromNumber(3));
            Assert.AreEqual(2.0, stack[0].AsNumber());
            Assert.AreEqual(3.0, upvalue.Closed.AsNumber());
        }

        [Test]
        public void RangeIterable_Reset_Reflection()
        {
            var asm = typeof(MiniPanda).Assembly;
            var type = asm.GetType("Azathrix.MiniPanda.VM.RangeIterable");
            Assert.IsNotNull(type);

            var instance = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null, new object[] { 1, 2, 3 }, null);
            Assert.IsNotNull(instance);

            var reset = type.GetMethod("Reset", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            reset.Invoke(instance, new object[] { 4, 5, 6 });

            Assert.AreEqual(4, (int)type.GetField("Start").GetValue(instance));
            Assert.AreEqual(5, (int)type.GetField("End").GetValue(instance));
            Assert.AreEqual(6, (int)type.GetField("Step").GetValue(instance));
        }
    }

    [TestFixture]
    public class VirtualMachineApiTests
    {
        private MiniPanda _vm;

        [SetUp]
        public void Setup()
        {
            _vm = new MiniPanda();
            _vm.Start();
        }

        [TearDown]
        public void TearDown()
        {
            _vm.Shutdown();
        }

        [Test]
        public void VM_SetGlobal_Overloads()
        {
            _vm.VM.SetGlobal("n", 1.5);
            _vm.VM.SetGlobal("b", true);
            _vm.VM.SetGlobal("s", "hi");
            _vm.VM.SetGlobal("nf", NativeFunction.Create(() => Value.FromNumber(3)));

            Assert.AreEqual(1.5, _vm.VM.GetGlobal("n").AsNumber());
            Assert.AreEqual(true, _vm.VM.GetGlobal("b").AsBool());
            Assert.AreEqual("hi", _vm.VM.GetGlobal("s").AsString());
            Assert.AreEqual(3.0, _vm.VM.Call("nf").AsNumber());
        }

        [Test]
        public void VM_GetGlobal_Missing_Throws()
        {
            Assert.Throws<MiniPandaRuntimeException>(() => _vm.VM.GetGlobal("missing"));
        }

        [Test]
        public void VM_Scope_Clear()
        {
            var scope = _vm.VM.GetScope("s1");
            scope.Define("x", Value.FromNumber(1));
            Assert.IsTrue(scope.ContainsLocal("x"));
            _vm.VM.ClearScope("s1");
            Assert.IsFalse(scope.ContainsLocal("x"));
        }

        [Test]
        public void VM_Run_Bytecode_Data_And_IsBytecode()
        {
            var compiled = _vm.Compile("return 2");
            var data = compiled.Bytecode.Serialize();

            Assert.IsTrue(MiniPanda.IsBytecode(data));
            Assert.IsFalse(MiniPanda.IsBytecode(new byte[] { 1, 2, 3, 4 }));

            var result = _vm.Run(data);
            Assert.AreEqual(2.0, result.AsNumber());
        }

        [Test]
        public void VM_ConvertPath()
        {
            Assert.AreEqual("a/b/c", MiniPanda.ConvertPath("a.b.c"));
        }

        [Test]
        public void VM_LoadFile_DefaultAndGuard()
        {
            var directory = "Temp";
            Directory.CreateDirectory(directory);
            var basePath = Path.Combine(directory, "minipanda_tmp");
            var pandaPath = basePath + ".panda";

            File.WriteAllText(pandaPath, "return 1");
            try
            {
                var result = _vm.RunFile(basePath);
                Assert.AreEqual(1.0, result.AsNumber());
            }
            finally
            {
                if (File.Exists(pandaPath))
                {
                    File.Delete(pandaPath);
                }
            }

            var (data, fullPath) = _vm.LoadFile("../bad");
            Assert.IsNull(data);
            Assert.IsNull(fullPath);
        }

        [Test]
        public void VM_Eval_WithEnvironment_Dictionary_Provider()
        {
            var env = new MPEnvironment();
            env.Define("x", Value.FromNumber(2));
            env.Define("y", Value.FromNumber(3));
            Assert.AreEqual(5.0, _vm.Eval("x + y", env).AsNumber());

            var dict = new Dictionary<string, object> { ["x"] = 4, ["y"] = 6 };
            Assert.AreEqual(10.0, _vm.Eval("x + y", dict).AsNumber());

            var provider = new TestEnvProvider();
            Assert.AreEqual(9.0, _vm.Eval("x + y", provider).AsNumber());
        }

        [Test]
        public void VM_GetLocalVariables_FromNativeFunction()
        {
            Dictionary<string, Value> locals = null;
            MPEnvironment closure = null;
            int depth = 0;
            int frameId = -1;

            _vm.VM.SetGlobal("captureLocals", NativeFunction.CreateWithVM((vm, args) =>
            {
                var frames = vm.GetStackTrace();
                depth = frames.Length;
                if (frames.Length > 0)
                {
                    frameId = frames[0].Id;
                    locals = vm.GetLocalVariables(frameId);
                    closure = vm.GetFrameEnvironment(frameId);
                }
                return Value.Null;
            }));

            _vm.Run(@"
                func test(a) {
                    var b = 2
                    captureLocals()
                    return b
                }
                return test(1)
            ");

            Assert.IsTrue(depth > 0);
            Assert.IsTrue(frameId >= 0);
            Assert.IsNotNull(locals);
            Assert.IsTrue(locals.Count > 0);
            Assert.IsTrue(locals.ContainsKey("a") || locals.ContainsKey("arg0"));
            Assert.IsNotNull(closure);
        }

        [Test]
        public void VM_GetLocalVariables_InvalidFrame_ReturnsEmpty()
        {
            var locals = _vm.VM.GetLocalVariables(-1);
            Assert.AreEqual(0, locals.Count);
        }

        [Test]
        public void VM_Call_WithScopedEnvironment()
        {
            _vm.Run(@"global func add() { return a + b }");
            var scope = new Dictionary<string, object> { ["a"] = 1, ["b"] = 2 };
            var result = _vm.Call(scope, "add");
            Assert.AreEqual(3.0, result.AsNumber());
        }

        private class TestEnvProvider : IEnvironmentProvider
        {
            public Value Get(string name)
            {
                if (name == "x") return Value.FromNumber(4);
                if (name == "y") return Value.FromNumber(5);
                return Value.Null;
            }

            public bool Contains(string name) => name == "x" || name == "y";
        }
    }

    [TestFixture]
    public class VirtualMachineOpcodeTests
    {
        private MiniPanda _vm;

        [SetUp]
        public void Setup()
        {
            _vm = new MiniPanda();
            _vm.Start();
        }

        [TearDown]
        public void TearDown()
        {
            _vm.Shutdown();
        }

        [Test]
        public void Upvalue_Assignment_OpenAndClosed()
        {
            Assert.AreEqual(2.0, _vm.Run(@"
                func outer() {
                    var x = 1
                    func inner() { x = x + 1 }
                    inner()
                    return x
                }
                return outer()
            ").AsNumber());

            Assert.AreEqual(2.0, _vm.Run(@"
                func make() {
                    var x = 1
                    func inner() { x = x + 1; return x }
                    return inner
                }
                var f = make()
                return f()
            ").AsNumber());
        }
    }
}
