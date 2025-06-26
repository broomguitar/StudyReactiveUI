using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class TestVM : ReactiveObject
    {
        public TestVM()
        {
            TestCommand = ReactiveCommand.Create(Test);
        }

        public ReactiveCommand<Unit, Unit> TestCommand { get; }

        private void Test()
        {
            var d = Task.Run(async () => await Global.OpenFileInteraction.Handle(Unit.Default)).Result;
            System.Windows.MessageBox.Show(d);
        }
    }
}