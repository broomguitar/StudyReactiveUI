using DevExpress.Utils;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// TestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestWindow : IViewFor<TestVM>
    {
        public TestWindow(TestVM testVM)
        {
            InitializeComponent();
            this.WhenActivated(dispose =>
            {
                this.BindCommand(ViewModel, vm => vm.TestCommand, view => view.btn).DisposeWith(dispose);
            });
            Global.OpenFileInteraction.RegisterHandler(GetFile);
        }

        public TestVM ViewModel { get; set; }

        object? IViewFor.ViewModel { get => ViewModel; set => ViewModel = (TestVM)value; }

        private Task GetFile(IInteractionContext<Unit, string> interactionContext)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                interactionContext.SetOutput(ofd.FileName);
            }
            else
            {
                interactionContext.SetOutput(string.Empty);
            }
            return Task.CompletedTask;
        }
    }
}