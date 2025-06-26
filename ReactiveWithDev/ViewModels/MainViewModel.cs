using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Diagram;
using DevExpress.CodeParser;
using DevExpress.Utils.DirectXPaint;
using DevExpress.Xpf.Dialogs;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ReactiveWithDev.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        public MainViewModel()
        {
            ButtonCommand = ReactiveCommand.CreateFromTask(ExecuteTextCommand);
            ButtonNameCommand = ReactiveCommand.CreateFromObservable(ExecuteNameCommand);
            this.WhenAnyValue(x => x.TextName)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => x.Split(' ')[0])
                 .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, x => x.FirstName, out _firstName);
            this.WhenAnyValue(x => x.Text1)
                .ObserveOn(RxApp.MainThreadScheduler)
               .Where(x => !string.IsNullOrEmpty(x) && x.Split(' ').Length > 1)
               .Select(x => x.Split(' ')[1])
               .ToProperty(this, x => x.SecondName, out _secondName);

            this.WhenAnyValue(x => x.TextName, x => x.Text1, (t0, t1) => t0 + t1)
             .Where(x => !string.IsNullOrEmpty(x))
             .Select(x => x)
             .Subscribe(x => Test = x);
            this.WhenAny(x => x.Test, _ => "TGG").Subscribe(x => Test = x);
            OpenTestWindowAction = new Interaction<Unit, string>();
        }

        public ReactiveCommand<Unit, Unit> ButtonCommand { get; }
        public ReactiveCommand<Unit, Unit> ButtonNameCommand { get; }

        public Interaction<Unit, string> OpenTestWindowAction { get; }

        [Reactive]
        public string Text1 { get; set; } = string.Empty;

        private string _textName = "让我们奔向大海吧";

        public string TextName
        {
            get { return _textName; }
            set { this.RaiseAndSetIfChanged(ref _textName, value); }
        }

        private async Task ExecuteTextCommand()
        {
            MessageBox.Show("Main" + await OpenTestWindowAction.Handle(Unit.Default));
            Text1 = "Hello ReactiveUI";
        }

        private IObservable<Unit> ExecuteNameCommand()
        {
            TextName = "Hello SS" + App.GetService<Test2>().Value;
            return Observable.Return(Unit.Default);
        }

        private readonly ObservableAsPropertyHelper<string> _firstName;
        public string FirstName => _firstName.Value;
        private readonly ObservableAsPropertyHelper<string> _secondName;
        public string SecondName => _secondName.Value;

        [Reactive]
        public string Test { get; set; } = string.Empty;

        public string Test1 { [ObservableAsProperty] get; } = default!;
    }

    public class Test
    {
        public string Value1 { get; init; }

        public Test()
        {
            Value1 = "Test";
        }
    }

    public class Test2 : Test
    {
        public string Value { get; init; }

        public Test2()
        {
            Value1 = "12";
            Value = Value1 + 2;
        }
    }

    public record DDD(string Name, string value);
    public record BBB : DDD
    {
        public BBB(string Name, string value) : base(Name, value)
        {
        }
    }
}