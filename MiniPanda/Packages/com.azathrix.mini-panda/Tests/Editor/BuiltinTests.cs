using NUnit.Framework;
using Azathrix.MiniPanda;
using Azathrix.MiniPanda.Core;
using Azathrix.MiniPanda.Exceptions;
using Azathrix.MiniPanda.VM;

namespace Azathrix.MiniPanda.Tests
{
    /// <summary>
    /// 内置函数测试
    /// </summary>
    [TestFixture]
    public class BuiltinTests
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

        // ========== 类型函数 ==========

        [Test]
        public void Builtin_Type()
        {
            Assert.AreEqual("number", _vm.Eval("type(42)").AsString());
            Assert.AreEqual("string", _vm.Eval("type(\"hello\")").AsString());
            Assert.AreEqual("bool", _vm.Eval("type(true)").AsString());
            Assert.AreEqual("null", _vm.Eval("type(null)").AsString());
        }

        [Test]
        public void Builtin_Type_Extended()
        {
            var result = _vm.Run(@"
                func f() { return 1 }
                class A { A() { } }
                var inst = A()
                return [type([1, 2]), type({a: 1}), type(f), type(A), type(inst)]
            ").As<MiniPandaArray>();
            Assert.AreEqual("array", result.Get(0).AsString());
            Assert.AreEqual("object", result.Get(1).AsString());
            Assert.AreEqual("function", result.Get(2).AsString());
            Assert.AreEqual("class", result.Get(3).AsString());
            Assert.AreEqual("instance", result.Get(4).AsString());
        }

        [Test]
        public void Builtin_ToString()
        {
            Assert.AreEqual("42", _vm.Eval("str(42)").AsString());
            Assert.AreEqual("true", _vm.Eval("str(true)").AsString());
            Assert.AreEqual("null", _vm.Eval("str(null)").AsString());
        }

        [Test]
        public void Builtin_ToNumber()
        {
            Assert.AreEqual(42.0, _vm.Eval("num(\"42\")").AsNumber());
            Assert.AreEqual(3.14, _vm.Eval("num(\"3.14\")").AsNumber());
        }

        // ========== 数学函数 ==========

        [Test]
        public void Builtin_Math()
        {
            Assert.AreEqual(5.0, _vm.Eval("abs(-5)").AsNumber());
            Assert.AreEqual(3.0, _vm.Eval("floor(3.7)").AsNumber());
            Assert.AreEqual(4.0, _vm.Eval("ceil(3.2)").AsNumber());
            Assert.AreEqual(3.0, _vm.Eval("round(3.4)").AsNumber());
            Assert.AreEqual(4.0, _vm.Eval("sqrt(16)").AsNumber());
            Assert.AreEqual(8.0, _vm.Eval("pow(2, 3)").AsNumber());
        }

        [Test]
        public void Builtin_MinMax()
        {
            Assert.AreEqual(1.0, _vm.Eval("min(3, 1, 4, 1, 5)").AsNumber());
            Assert.AreEqual(5.0, _vm.Eval("max(3, 1, 4, 1, 5)").AsNumber());
        }

        [Test]
        public void Builtin_Len_Object()
        {
            Assert.AreEqual(2.0, _vm.Run("return len({a: 1, b: 2})").AsNumber());
        }

        [Test]
        public void Builtin_KeysValues_Instance()
        {
            var result = _vm.Run(@"
                class A { A() { } }
                var inst = A()
                inst.x = 1
                inst.y = 2
                return [keys(inst), values(inst)]
            ").As<MiniPandaArray>();
            var keys = result.Get(0).As<MiniPandaArray>();
            var values = result.Get(1).As<MiniPandaArray>();
            Assert.AreEqual(2, keys.Length);
            Assert.AreEqual(2, values.Length);
        }

        [Test]
        public void Builtin_Contains_Unsupported()
        {
            Assert.IsFalse(_vm.Run("return contains(123, 1)").AsBool());
        }

        [Test]
        public void Builtin_Range_Step()
        {
            Assert.AreEqual(22.0, _vm.Run(@"
                var sum = 0
                for i in range(10, 0, -3) { sum = sum + i }
                return sum
            ").AsNumber());
        }

        // ========== JSON 函数 ==========

        [Test]
        public void Builtin_JSON_Parse()
        {
            _vm.SetGlobal("jsonText", "{\"name\":\"test\",\"value\":42}");
            var obj = _vm.Run(@"return json.parse(jsonText)").As<MiniPandaObject>();
            Assert.AreEqual("test", obj.Get("name").AsString());
            Assert.AreEqual(42.0, obj.Get("value").AsNumber());

            _vm.SetGlobal("jsonText", "[1,2,3]");
            var arr = _vm.Run(@"return json.parse(jsonText)").As<MiniPandaArray>();
            Assert.AreEqual(3, arr.Length);
            Assert.AreEqual(2.0, arr.Get(1).AsNumber());

            _vm.SetGlobal("jsonText", "42");
            Assert.AreEqual(42.0, _vm.Run(@"return json.parse(jsonText)").AsNumber());
            _vm.SetGlobal("jsonText", "true");
            Assert.AreEqual(true, _vm.Run(@"return json.parse(jsonText)").AsBool());
            _vm.SetGlobal("jsonText", "null");
            Assert.IsTrue(_vm.Run(@"return json.parse(jsonText)").IsNull);
        }

        [Test]
        public void Builtin_JSON_Parse_EscapesAndFalse()
        {
            var jsonText = "{\"s\":\"line1\\nline2\\t\\\"q\\\"\\\\end\"}";
            _vm.SetGlobal("jsonText", jsonText);
            var obj = _vm.Run(@"return json.parse(jsonText)").As<MiniPandaObject>();
            Assert.AreEqual("line1\nline2\t\"q\"\\end", obj.Get("s").AsString());
            Assert.IsFalse(_vm.Run(@"return json.parse(""false"")").AsBool());
        }

        [Test]
        public void Builtin_JSON_Stringify()
        {
            Assert.AreEqual("{\"name\":\"test\",\"value\":42}", _vm.Run(@"return json.stringify({name: ""test"", value: 42})").AsString());
            Assert.AreEqual("[1,2,3]", _vm.Run(@"return json.stringify([1, 2, 3])").AsString());
            Assert.AreEqual("42", _vm.Run(@"return json.stringify(42)").AsString());
            Assert.AreEqual("true", _vm.Run(@"return json.stringify(true)").AsString());
            Assert.AreEqual("null", _vm.Run(@"return json.stringify(null)").AsString());
        }

        [Test]
        public void Builtin_JSON_Stringify_InstanceAndEscapes()
        {
            _vm.SetGlobal("msgText", "a\n\t\"b\"\\");
            var text = _vm.Run(@"class A { A() { this.msg = msgText } } var inst = A() return json.stringify(inst)").AsString();
            Assert.IsTrue(text.Contains("\\n"));
            Assert.IsTrue(text.Contains("\\t"));
            Assert.IsTrue(text.Contains("\\\""));
            Assert.IsTrue(text.Contains("\\\\"));
        }

        // ========== 调试函数 ==========

        [Test]
        public void Builtin_Assert_Pass()
        {
            _vm.Run("assert(true)");
            _vm.Run("assert(1 == 1)");
            _vm.Run("assert(5 > 3, \"5 should be greater than 3\")");
        }

        [Test]
        public void Builtin_Assert_Fail()
        {
            Assert.Throws<MiniPandaRuntimeException>(() => _vm.Run("assert(false)"));
            Assert.Throws<MiniPandaRuntimeException>(() => _vm.Run("assert(1 == 2, \"Numbers should be equal\")"));
        }

        [Test]
        public void Builtin_Stacktrace()
        {
            var result = _vm.Run(@"
                func inner() { return stacktrace() }
                func outer() { return inner() }
                return outer()
            ");
            var trace = result.AsString();
            Assert.IsTrue(trace.Contains("inner"));
            Assert.IsTrue(trace.Contains("outer"));
        }

        [Test]
        public void Builtin_Trace_Debug_PrintStackTrace()
        {
            Builtins.PrintStackTrace = true;
            try
            {
                _vm.Run("trace(\"a\", \"b\")");
                _vm.Run("debug(\"x\")");
                _vm.Run("print(\"hello\")");
            }
            finally
            {
                Builtins.PrintStackTrace = false;
            }
        }

        [Test]
        public void Builtin_Date_Module()
        {
            var result = _vm.Run(@"
                var ts = date.create(2020, 2, 3, 4, 5, 6)
                return [date.year(ts), date.month(ts), date.day(ts), date.hour(ts), date.minute(ts), date.second(ts)]
            ").As<MiniPandaArray>();
            Assert.AreEqual(2020.0, result.Get(0).AsNumber());
            Assert.AreEqual(2.0, result.Get(1).AsNumber());
            Assert.AreEqual(3.0, result.Get(2).AsNumber());
            Assert.AreEqual(4.0, result.Get(3).AsNumber());
            Assert.AreEqual(5.0, result.Get(4).AsNumber());
            Assert.AreEqual(6.0, result.Get(5).AsNumber());

            Assert.IsTrue(_vm.Run("return date.parse(\"not-a-date\")").IsNull);
        }

        [Test]
        public void Builtin_Regex_Module()
        {
            Assert.IsTrue(_vm.Run("return regex.test(\"a+\", \"caa\")").AsBool());
            Assert.IsFalse(_vm.Run("return regex.test(\"[\", \"a\")").AsBool());

            var match = _vm.Run("return regex.match(\"a+\", \"caa\")").As<MiniPandaObject>();
            Assert.AreEqual("aa", match.Get("value").AsString());

            var matches = _vm.Run("return regex.matchAll(\"a\", \"banana\")").As<MiniPandaArray>();
            Assert.AreEqual(3, matches.Length);

            Assert.AreEqual("b_n_n_", _vm.Run("return regex.replace(\"a\", \"banana\", \"_\")").AsString());

            var parts = _vm.Run("return regex.split(\"a\", \"banana\")").As<MiniPandaArray>();
            Assert.AreEqual(4, parts.Length);
        }

        [Test]
        public void Builtin_Randoms()
        {
            var r = _vm.Run("return random()").AsNumber();
            Assert.IsTrue(r >= 0.0 && r < 1.0);
            var ri = _vm.Run("return randomInt(1, 3)").AsNumber();
            Assert.IsTrue(ri == 1.0 || ri == 2.0);
        }
    }
}
