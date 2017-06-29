using System;
using NUnit.Framework;
using Sciff.Logic.LambdaReflection.Members;
using Sciff.Tests.LibraryDomain;

namespace Sciff.Tests.LambdaReflection.Members
{
    [TestFixture]
    public class TestAccessors
    {
        private Book _book;

        [SetUp]
        public void SetUp()
        {
            _book = new Book
            {
                Isbn = 1234567890123,
                Author = "Me!",
                Name = "Such Text"
            };
        }

        #region AsFunc

        [Test]
        public void TestPropertyAsFuncCache()
        {
            Assert.That(
                Accessors<Book>.AsFunc<string>(nameof(Book.Author)),
                Is.SameAs(Accessors<Book>.AsFunc<string>(nameof(Book.Author)))
            );
        }

        [Test]
        public void TestFieldAsFuncCache()
        {
            Assert.That(
                Accessors<Book>.AsFunc<string>(nameof(Book.Name)),
                Is.SameAs(Accessors<Book>.AsFunc<string>(nameof(Book.Name)))
            );
        }

        [Test]
        public void TestPropertyAsFunc()
        {
            var author = Accessors<Book>.AsFunc<string>(nameof(Book.Author));
            Assert.That(author(_book), Is.EqualTo(_book.Author));
        }

        [Test]
        public void TestFieldAsFunc()
        {
            var name = Accessors<Book>.AsFunc<string>(nameof(Book.Name));
            Assert.That(name(_book), Is.EqualTo(_book.Name));
        }

        [Test]
        public void TestAsFuncNameNotFound()
        {
            Assert.That(
                () => Accessors<Book>.AsFunc<string>(nameof(Book.Author) + "5"),
                Throws.InstanceOf<MissingMemberException>()
            );
        }

        [Test]
        public void TestAsFuncTypeNotFound()
        {
            Assert.That(
                () => Accessors<Book>.AsFunc<long>(nameof(Book.Author)),
                Throws.InstanceOf<MissingMemberException>()
            );
        }

        #endregion

        #region AsLambda

        [Test]
        public void TestPropertyAsLambdaCache()
        {
            Assert.That(
                Accessors<Book>.AsLambda<string>(nameof(Book.Author)),
                Is.SameAs(Accessors<Book>.AsLambda<string>(nameof(Book.Author)))
            );
        }

        [Test]
        public void TestFieldAsLambdaCache()
        {
            Assert.That(
                Accessors<Book>.AsLambda<string>(nameof(Book.Name)),
                Is.SameAs(Accessors<Book>.AsLambda<string>(nameof(Book.Name)))
            );
        }

        [Test]
        public void TestPropertyAsLambda()
        {
            var author = Accessors<Book>.AsLambda<string>(nameof(Book.Author));
            Assert.That(author.Compile()(_book), Is.EqualTo(_book.Author));
        }

        [Test]
        public void TestFieldAsLambda()
        {
            var name = Accessors<Book>.AsLambda<string>(nameof(Book.Name));
            Assert.That(name.Compile()(_book), Is.EqualTo(_book.Name));
        }

        [Test]
        public void TestAsLambdaNameNotFound()
        {
            Assert.That(
                () => Accessors<Book>.AsLambda<string>(nameof(Book.Author) + "5"),
                Throws.InstanceOf<MissingMemberException>()
            );
        }

        [Test]
        public void TestAsLambdaTypeNotFound()
        {
            Assert.That(
                () => Accessors<Book>.AsLambda<long>(nameof(Book.Author)),
                Throws.InstanceOf<MissingMemberException>()
            );
        }

        #endregion
    }
}