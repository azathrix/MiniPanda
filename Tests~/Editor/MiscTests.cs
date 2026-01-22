using NUnit.Framework;
using Azathrix.MiniPanda;
using Azathrix.MiniPanda.Core;
using Azathrix.MiniPanda.Exceptions;
using Azathrix.MiniPanda.Parser;
using Azathrix.MiniPanda.VM;

namespace Azathrix.MiniPanda.Tests
{
    /// <summary>
    /// 错误处理测试
    /// </summary>
    [TestFixture]
    public class ErrorHandlingTests
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
        public void Error_DivisionByZero()
        {
            var result = _vm.Eval("10 / 0");
            Assert.IsTrue(double.IsInfinity(result.AsNumber()));
        }

        [Test]
        public void Error_InvalidFunctionCall()
        {
            Assert.Throws<MiniPandaRuntimeException>(() => _vm.Run("var x = 42; x()"));
        }

        [Test]
        public void Error_SyntaxError()
        {
            Assert.Throws<ParserException>(() => _vm.Run("var x = "));
        }

        // ========== try/catch/finally 测试 ==========

        [Test]
        public void TryCatch_ReturnsExceptionValue()
        {
            var result = _vm.Run(@"
                try {
                    throw ""boom""
                } catch (e) {
                    return e
                }
            ");
            Assert.AreEqual("boom", result.AsString());
        }

        [Test]
        public void TryCatch_NoVar_Works()
        {
            var result = _vm.Run(@"
                var x = 0
                try {
                    throw 1
                } catch {
                    x = 2
                }
                return x
            ");
            Assert.AreEqual(2.0, result.AsNumber());
        }

        [Test]
        public void TryFinally_AlwaysRuns()
        {
            var result = _vm.Run(@"
                var x = 0
                try {
                    x = 1
                } finally {
                    x = x + 1
                }
                return x
            ");
            Assert.AreEqual(2.0, result.AsNumber());
        }

        [Test]
        public void TryCatchFinally_Order()
        {
            var result = _vm.Run(@"
                var x = 0
                try {
                    throw 5
                } catch (e) {
                    x = e
                } finally {
                    x = x + 10
                }
                return x
            ");
            Assert.AreEqual(15.0, result.AsNumber());
        }

        [Test]
        public void Finally_Rethrows()
        {
            Assert.Throws<MiniPandaRuntimeException>(() => _vm.Run(@"
                try {
                    throw ""err""
                } finally {
                    var x = 1
                }
            "));
        }
    }

    /// <summary>
    /// 边界情况测试
    /// </summary>
    [TestFixture]
    public class EdgeCaseTests
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
        public void EdgeCase_NullComparison()
        {
            Assert.IsTrue(_vm.Eval("null == null").AsBool());
            Assert.IsFalse(_vm.Eval("null == 0").AsBool());
            Assert.IsFalse(_vm.Eval("null == false").AsBool());
        }

        [Test]
        public void EdgeCase_StringConcatenation()
        {
            Assert.AreEqual("hello world", _vm.Eval("\"hello\" + \" \" + \"world\"").AsString());
        }

        [Test]
        public void EdgeCase_FunctionAsValue()
        {
            Assert.AreEqual("hello", _vm.Run(@"
                func greet() { return ""hello"" }
                var f = greet
                return f()
            ").AsString());
        }
    }

    /// <summary>
    /// Value 类型测试
    /// </summary>
    [TestFixture]
    public class ValueTypeTests
    {
        [Test]
        public void Value_FromNumber()
        {
            var v = Value.FromNumber(42);
            Assert.IsTrue(v.IsNumber);
            Assert.AreEqual(42.0, v.AsNumber());
        }

        [Test]
        public void Value_FromBool()
        {
            var vTrue = Value.FromBool(true);
            var vFalse = Value.FromBool(false);
            Assert.IsTrue(vTrue.IsBool);
            Assert.IsTrue(vTrue.AsBool());
            Assert.IsFalse(vFalse.AsBool());
        }

        [Test]
        public void Value_FromString()
        {
            var v = Value.FromObject(MiniPandaString.Create("hello"));
            Assert.IsTrue(v.IsString);
            Assert.AreEqual("hello", v.AsString());
        }

        [Test]
        public void Value_Null()
        {
            var v = Value.Null;
            Assert.IsTrue(v.IsNull);
        }

        [Test]
        public void Value_Equality()
        {
            var a = Value.FromNumber(42);
            var b = Value.FromNumber(42);
            var c = Value.FromNumber(43);
            Assert.AreEqual(a, b);
            Assert.AreNotEqual(a, c);
        }
    }
}
