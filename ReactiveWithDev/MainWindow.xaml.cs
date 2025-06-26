using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using DevExpress.DirectX.NativeInterop.Direct2D;
using DevExpress.Xpf.Core;
using DevExpress.Dialogs.Core.View;
using DevExpress.Utils;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Reactive;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IViewFor<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            //if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            //    return;//设计器模式下不继续执行，以规避异常导致的设计器显示问题
            ViewModel = new MainViewModel();
            this.WhenActivated(disposable =>
            {
                this.Bind(ViewModel, x => x.Text1, x => x.tb1.Text).DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.SecondName, x => x.btn_tb1.Content).DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.ButtonCommand, x => x.btn_tb1.Command).DisposeWith(disposable);

                this.Bind(ViewModel, x => x.TextName, x => x.tb_Name.Text).DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.FirstName, x => x.btn_Name.Content).DisposeWith(disposable);
                this.BindCommand(ViewModel, x => x.ButtonNameCommand, x => x.btn_Name).DisposeWith(disposable);
                this.Bind(ViewModel, x => x.Test, x => x.tb_Test.Text).DisposeWith(disposable);
                this.BindInteraction(ViewModel, vm => vm.OpenTestWindowAction, openTestWindow).DisposeWith(disposable);
            });
        }

        public MainViewModel ViewModel
        {
            get { return (MainViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(MainViewModel), typeof(MainWindow), new PropertyMetadata(null));

        object IViewFor.ViewModel { get => ViewModel; set => ViewModel = (MainViewModel)value; }

        private Regex? regex = null;

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox textBox)
            {
                string newText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
                if (textBox.SelectionStart > 0)
                {
                    // 如果不是在开头输入，只能输入数字
                    regex = new Regex(@"^\d+$");
                    if (!regex.IsMatch(e.Text))
                    {
                        e.Handled = true;
                        if (textBox.Text.Contains(e.Text))
                        {
                            textBox.Text = textBox.Text.Replace(e.Text, null);
                            textBox.SelectionStart = textBox.Text.Length;
                        }
                    }
                }
                else
                {
                    // 如果在开头输入，允许正负号或数字
                    regex = new Regex(@"^[-]?\d?$");
                    if (!regex.IsMatch(e.Text))
                    {
                        e.Handled = true;
                        if (textBox.Text.Contains(e.Text))
                        {
                            textBox.Text = textBox.Text.Replace(e.Text, null);
                            textBox.SelectionStart = textBox.Text.Length;
                        }
                    }
                }
            }
        }

        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox textBox)
            {
                if (e.DataObject.GetDataPresent(typeof(String)))
                {
                    string pastedText = (String)e.DataObject.GetData(typeof(String));
                    if (textBox.SelectionStart > 0)
                    {
                        // 如果不是在开头输入，只能输入数字
                        regex = new Regex(@"^\d+$");
                        if (!regex.IsMatch(pastedText))
                        {
                            e.CancelCommand();
                        }
                    }
                    else
                    {
                        // 如果在开头输入，允许负号或数字
                        regex = new Regex(@"^[-]?(\d|([1,9]/d+))+$");
                        if (!regex.IsMatch(pastedText))
                        {
                            e.CancelCommand();
                        }
                    }
                }
                else
                {
                    e.CancelCommand();
                }
            }
        }

        private Task openTestWindow(IInteractionContext<Unit, string> interactionContext)
        {
            TestWindow window = new TestWindow(new TestVM());
            window.ShowDialog();
            interactionContext.SetOutput(window.Title);
            return Task.CompletedTask;
        }
    }
}