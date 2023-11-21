namespace API.Interfaces
{
    public interface ITaskAuthhorizationService
    {
        Task<bool> UserCanEditTask(string userId,int taskId);
    }
}
