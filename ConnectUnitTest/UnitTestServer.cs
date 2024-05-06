using Connect.server;

namespace ConnectServerUnitTest
{
    /// <summary>
    /// Unit test kits to the server
    /// </summary>
    [TestClass]
    public class UnitTestServer
    {
        /// <summary>
        /// Connect server
        /// </summary>
        Server server = new Server("127.0.0.1", 7777);

        /// <summary>
        /// Check whether a TCP server open
        /// </summary>
        [TestMethod]
        public void TestMethodConnected()
        {
            Assert.AreEqual(true, server.Connected);
        }
        /// <summary>
        /// Check whether a mySql connected
        /// </summary>
        [TestMethod]
        public void TestMethodDbPathForMySql()
        {
            server.SetDBPath("server=localhost;uid=root;pwd=1234;database=mydb;", "mongodb://localhost:27017");

            Assert.AreEqual(true, server.IsMySqlConnected);
        }
        /// <summary>
        /// Check whether a command created
        /// </summary>
        [TestMethod]
        public void TestMethodDbMySqlCommandCreated()
        {
            server.SetDBPath("server=localhost;uid=root;pwd=1234;database=mydb;", "mongodb://localhost:27017");

            Assert.AreEqual(true, server.IsMySqlCommandCreated);
        }
        /// <summary>
        /// Check whether a MongoDB connected
        /// </summary>
        [TestMethod]
        public void TestMethodDbPathForMongoDB()
        {
            server.SetDBPath("server=localhost;uid=root;pwd=1234;database=mydb;", "mongodb://localhost:27017");

            Assert.AreEqual(true, server.IsMongoDBConnected);
        }
        /// <summary>
        /// Check whether collection was found
        /// </summary>
        [TestMethod]
        public void TestMethodDbMongoDBCollectionFound()
        {
            server.SetDBPath("server=localhost;uid=root;pwd=1234;database=mydb;", "mongodb://localhost:27017");

            Assert.AreEqual(true, server.IsMongoDBCollectionFound);
        }
        /// <summary>
        /// Check whether a server is running
        /// </summary>
        [TestMethod]
        public void TestMethodIsServerRunning()
        {
            server.Start();

            Assert.AreEqual(true, server.IsServerRunning);
        }
    }
}