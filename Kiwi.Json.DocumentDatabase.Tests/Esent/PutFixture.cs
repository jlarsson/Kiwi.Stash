using NUnit.Framework;

namespace Kiwi.Json.DocumentDatabase.Tests.Esent
{
    [TestFixture]
    public class PutFixture: DatabaseTestFixtureBase
    {
        [Test]
        public void KeysAreCaseInsensitive()
        {
            var coll = Db.GetCollection("test");

            coll.Put("KEY", "hello world");

            Assert.That("hello world", Is.EqualTo(coll.Get<string>("key")));
        }
        
        [Test]
        public void PutOverwritesExisting()
        {
            var coll = Db.GetCollection("test");

            coll.Put("key", "hello");
            coll.Put("Key", "world");

            Assert.That("world", Is.EqualTo(coll.Get<string>("key")));
        }
    }
}