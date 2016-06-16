namespace OpenData.Common.AppEngine
{
    public interface IStartupTask
    {
        void Execute();

        int Order { get; }
    }
}
