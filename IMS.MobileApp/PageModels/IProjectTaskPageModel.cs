using CommunityToolkit.Mvvm.Input;
using IMS.MobileApp.Models;

namespace IMS.MobileApp.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}