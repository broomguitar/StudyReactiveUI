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
    public class Global
    {
        static Global()
        {
            OpenFileInteraction = new Interaction<Unit, string>();
        }

        public static Interaction<Unit, string> OpenFileInteraction { get; }

        public static async Task<string> ShowSelectFileDialogWithSupportMulti() => await OpenFileInteraction.Handle(Unit.Default);
    }
}