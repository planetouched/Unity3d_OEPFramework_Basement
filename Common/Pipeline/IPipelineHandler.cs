namespace Basement.Common.Pipeline
{
    public interface IPipelineHandler
    {
        void Create(object data);
        int GetError();
        void SetError(int errorCode);
        void Sleep();
        void Wakeup();
        object ReturnItem();
    }
}
