using System;
using NUnit.Framework;
using Sciff.Logic.LambdaReflection;

namespace Sciff.Tests.LambdaReflection
{
    [TestFixture]
    public class TestContractHelpers
    {
        [Test]
        public void TestMakeTypeForNonGeneric()
        {
            Assert.That(
                () => typeof(Action).MakeType(),
                Throws.TypeOf<ArgumentException>()
            );
        }

        [Test]
        public void TestMakeTypeWithoutAllArguments()
        {
            Assert.That(
                () => typeof(Action<,>).MakeType(typeof(int)),
                Throws.TypeOf<ArgumentException>()
            );
        }
    }
}