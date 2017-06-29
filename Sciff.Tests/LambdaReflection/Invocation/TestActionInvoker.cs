using System;
using NUnit.Framework;
using Sciff.Logic.LambdaReflection.Invocation;

namespace Sciff.Tests.LambdaReflection.Invocation
{
    [TestFixture]
    public class TestActionInvoker
    {
        private class Tester
        {
            public static bool StaticHasInvoked;

            public static void SetInvokedTrue()
            {
                StaticHasInvoked = true;
            }

            public static void SetInvoked(bool value)
            {
                StaticHasInvoked = value;
            }

            public bool HasInvoked;

            public void Zero()
            {
                HasInvoked = true;
            }

            public void One(int _)
            {
                HasInvoked = true;
            }

            public void Two(int x, string y)
            {
                HasInvoked = true;
            }
        }

        private Tester _tester;

        [SetUp]
        public void SetUp()
        {
            _tester = new Tester();
        }

        public void FiveParameters(int one, int two, int three, int four, int five)
        {
            throw new NotImplementedException();
        }

        [Test]
        public void TestThatFiveParametersIsTooMany()
        {
            Assert.That(
                () => ActionInvoker.Create(typeof(TestActionInvoker).GetMethod(nameof(FiveParameters))),
                Throws.TypeOf<NotSupportedException>()
            );
        }

        public void RefMethod(ref int x)
        {
            throw new NotImplementedException();
        }

        [Test]
        public void TestInvokeRef()
        {
            Assert.That(
                () => ActionInvoker.Create(typeof(TestActionInvoker).GetMethod(nameof(RefMethod))),
                Throws.TypeOf<NotSupportedException>()
            );
        }

        public void OutMethod(out int x)
        {
            throw new NotImplementedException();
        }

        [Test]
        public void TestInvokeOut()
        {
            Assert.That(
                () => ActionInvoker.Create(typeof(TestActionInvoker).GetMethod(nameof(OutMethod))),
                Throws.TypeOf<NotSupportedException>()
            );
        }

        public int AFunc()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void TestInvokeFunc()
        {
            Assert.That(
                () => ActionInvoker.Create(typeof(TestActionInvoker).GetMethod(nameof(AFunc))),
                Throws.TypeOf<NotSupportedException>()
            );
        }

        [Test]
        public void TestStaticInvokeThunk()
        {
            var setTrueStaticInvoked = ActionInvoker.Create(typeof(Tester).GetMethod(nameof(Tester.SetInvokedTrue)));
            setTrueStaticInvoked.Invoke();
            Assert.That(Tester.StaticHasInvoked, Is.True);
        }

        [Test]
        public void TestInvoke0()
        {
            var one = ActionInvoker.Create(typeof(Tester).GetMethod(nameof(Tester.Zero)));
            one.Invoke(_tester);
            Assert.That(_tester.HasInvoked, Is.True);
        }

        [Test]
        public void TestInvoke1()
        {
            var one = ActionInvoker.Create(typeof(Tester).GetMethod(nameof(Tester.One)));
            one.Invoke(_tester, 0);
            Assert.That(_tester.HasInvoked, Is.True);
        }

        [Test]
        public void TestInvoke2()
        {
            var one = ActionInvoker.Create(typeof(Tester).GetMethod(nameof(Tester.Two)));
            one.Invoke(_tester, 0, string.Empty);
            Assert.That(_tester.HasInvoked, Is.True);
        }

        [Test]
        public void TestStaticInvoke1()
        {
            var setStaticInvoked = ActionInvoker.Create(typeof(Tester).GetMethod(nameof(Tester.SetInvoked)));
            setStaticInvoked.Invoke(true);
            Assert.That(Tester.StaticHasInvoked, Is.True);
        }
    }
}