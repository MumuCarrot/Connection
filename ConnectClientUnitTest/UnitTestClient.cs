using Connect.server;
using LogInPage;

namespace ConnectClientUnitTest
{
    /// <summary>
    /// Unit test kits to the client
    /// </summary>
    [TestClass]
    public class UnitTestClient
    {
        /// <summary>
        /// Connect server
        /// </summary>
        Server server = new Server("127.0.0.1", 7777);
        /// <summary>
        /// Connect client
        /// </summary>
        Client client = new Client();

        /// <summary>
        /// Connecting to the server
        /// </summary>
        [TestMethod]
        public void TestMethodClientConnected()
        {
            server.SetDBPath("server=localhost;uid=root;pwd=1234;database=mydb;", "mongodb://localhost:27017");
            server.Start();

            client.Start();
            Assert.AreEqual(true, Client.Connected);
        }
        /// <summary>
        /// Disconnecting from the server
        /// </summary>
        [TestMethod]
        public void TestMethodClientClose()
        {
            client.Close();

            Assert.AreEqual(false, Client.Connected);
        }
    }
}