using System;
using NUnit.Framework;
using Sciff.Logic.LambdaReflection.Invocation;

namespace Sciff.Tests.LambdaReflection.Invocation
{
    [TestFixture]
    public class TestFuncInvoker
    {
        private string _string;

        [SetUp]
        public void SetUp()
        {
            _string = "fubar";
        }

        public int FiveParameters(int one, int two, int three, int four, int five)
        {
            throw new NotImplementedException();
        }

        [Test]
        public void TestThatFiveParametersIsTooMany()
        {
            Assert.That(
                () => FuncInvoker.Create(typeof(TestFuncInvoker).GetMethod(nameof(FiveParameters))),
                Throws.TypeOf<NotSupportedException>()
            );
        }

        public int RefMethod(ref int x)
        {
            throw new NotImplementedException();
        }

        [Test]
        public void TestInvokeRef()
        {
            Assert.That(
                () => FuncInvoker.Create(typeof(TestFuncInvoker).GetMethod(nameof(RefMethod))),
                Throws.TypeOf<NotSupportedException>()
            );
        }

        [Test]
        public void TestInvokeOut()
        {
            Assert.That(
                () => FuncInvoker.Create(typeof(Guid).GetMethod(nameof(Guid.TryParse))),
                Throws.TypeOf<NotSupportedException>()
            );
        }

        [Test]
        public void TestInvokeVoid()
        {
            Assert.That(
                () => FuncInvoker.Create(typeof(TestFuncInvoker).GetMethod(nameof(SetUp))),
                Throws.TypeOf<NotSupportedException>()
            );
        }

        [Test]
        public void TestInvokeThunk()
        {
            var newGuid = FuncInvoker.Create(typeof(Guid).GetMethod(nameof(Guid.NewGuid)));
            Assert.That((Guid) newGuid.Invoke(), Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public void TestInvoke0()
        {
            // ReSharper disable once PossibleNullReferenceException
            var getLength = FuncInvoker.Create(typeof(string).GetProperty(nameof(string.Length)).GetMethod);
            Assert.That((int) getLength.Invoke(_string), Is.EqualTo(_string.Length));
        }

        [Test]
        public void TestInvoke1()
        {
            var contains = FuncInvoker.Create(typeof(string).GetMethod("Contains", new[] { typeof(string) }));
            Assert.That((bool) contains.Invoke(_string, "uba"), Is.True);
        }

        [Test]
        public void TestInvoke2()
        {
            var endsWith = FuncInvoker.Create(
                typeof(string).GetMethod("EndsWith", new[] { typeof(string), typeof(StringComparison) })
            );
            Assert.That((bool) endsWith.Invoke(_string, "BAR", StringComparison.OrdinalIgnoreCase), Is.True);
        }

        [Test]
        public void TestStaticInvoke1()
        {
            var checkHostName = FuncInvoker.Create(typeof(Uri).GetMethod(nameof(Uri.CheckHostName)));
            Assert.That((UriHostNameType) checkHostName.Invoke("www.test.com"), Is.EqualTo(UriHostNameType.Dns));
        }
    }
}