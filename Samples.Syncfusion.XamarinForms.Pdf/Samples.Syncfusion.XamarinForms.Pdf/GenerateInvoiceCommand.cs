using System.Threading.Tasks;
using Wibci.LogicCommand;

namespace Samples.Syncfusion.XamarinForms.Pdf
{
    public interface IGenerateInvoiceCommand : IAsyncLogicCommand<Invoice, CommandResult>
    {
    }

    public class GenerateInvoiceCommand : AsyncLogicCommand<Invoice, CommandResult>
    {
        public override Task<CommandResult> ExecuteAsync(Invoice request)
        {
            return Task.FromResult(new CommandResult());
        }
    }
}