using Wibci.LogicCommand;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public enum TaskResult
    {
        Success,
        Failed,
        AccessDenied,
        Canceled
    }

    public class DeviceCommandResult : CommandResult
    {
        public DeviceCommandResult()
        {
            TaskResult = TaskResult.Success;
        }

        public TaskResult TaskResult { get; set; }
    }
}