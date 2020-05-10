namespace MainServer
{
    public interface IMessageQueueListener
    {
        void Listen();
        void TestMethodWithArgs(string arg1, int arg2);
        void Dispose();
    }
}
