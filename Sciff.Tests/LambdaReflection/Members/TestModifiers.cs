using System;
using NUnit.Framework;
using Sciff.Logic.LambdaReflection.Members;
using Sciff.Tests.LibraryDomain;

namespace Sciff.Tests.LambdaReflection.Members
{
    [TestFixture]
    public class TestModifiers
    {
        private Book _book;

        [SetUp]
        public void SetUp()
        {
            _book = new Book();
        }

        #region AsAction

        [Test]
        public void TestPropertyAsActionCache()
        {
            Assert.That(
                Modifiers<Book>.AsAction<string>(nameof(Book.Author)),
                Is.SameAs(Modifiers<Book>.AsAction<string>(nameof(Book.Author)))
            );
        }

        [Test]
        public void TestFieldAsActionCache()
        {
            Assert.That(
                Modifiers<Book>.AsAction<string>(nameof(Book.Name)),
                Is.SameAs(Modifiers<Book>.AsAction<string>(nameof(Book.Name)))
            );
        }

        [Test]
        public void TestPropertyAsAction()
        {
            var author = Modifiers<Book>.AsAction<string>(nameof(Book.Author));
            author(_book, "Me!");
            Assert.That(_book.Author, Is.EqualTo("Me!"));
        }

        [Test]
        public void TestFieldAsAction()
        {
            var name = Modifiers<Book>.AsAction<string>(nameof(Book.Name));
            name(_book, "Such Book");
            Assert.That(_book.Name, Is.EqualTo("Such Book"));
        }

        [Test]
        public void TestAsActionNameNotFound()
        {
            Assert.That(
                () => Modifiers<Book>.AsAction<string>(nameof(Book.Author) + "5"),
                Throws.InstanceOf<MissingMemberException>()
            );
        }

        [Test]
        public void TestAsActionTypeNotFound()
        {
            Assert.That(
                () => Modifiers<Book>.AsAction<long>(nameof(Book.Author)),
                Throws.InstanceOf<MissingMemberException>()
            );
        }

        #endregion

        #region AsLambda

        [Test]
        public void TestPropertyAsLambdaCache()
        {
            Assert.That(
                Modifiers<Book>.AsLambda<string>(nameof(Book.Author)),
                Is.SameAs(Modifiers<Book>.AsLambda<string>(nameof(Book.Author)))
            );
        }

        [Test]
        public void TestFieldAsLambdaCache()
        {
            Assert.That(
                Modifiers<Book>.AsLambda<string>(nameof(Book.Name)),
                Is.SameAs(Modifiers<Book>.AsLambda<string>(nameof(Book.Name)))
            );
        }

        [Test]
        public void TestPropertyAsLambda()
        {
            var author = Modifiers<Book>.AsLambda<string>(nameof(Book.Author));
            author.Compile()(_book, "Me!");
            Assert.That(_book.Author, Is.EqualTo("Me!"));
        }

        [Test]
        public void TestFieldAsLambda()
        {
            var name = Modifiers<Book>.AsLambda<string>(nameof(Book.Name));
            name.Compile()(_book, "Such Book");
            Assert.That(_book.Name, Is.EqualTo("Such Book"));
        }

        [Test]
        public void TestAsLambdaNameNotFound()
        {
            Assert.That(
                () => Modifiers<Book>.AsLambda<string>(nameof(Book.Author) + "5"),
                Throws.InstanceOf<MissingMemberException>()
            );
        }

        [Test]
        public void TestAsLambdaTypeNotFound()
        {
            Assert.That(
                () => Modifiers<Book>.AsLambda<long>(nameof(Book.Author)),
                Throws.InstanceOf<MissingMemberException>()
            );
        }

        #endregion
    }
}