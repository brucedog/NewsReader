using NewsReader.Core.Dialog;
using NewsReader.ViewModels;
using ReactiveUI;
using System.Windows.Input;
using System;
using NewsReader.Utilities;

namespace NewReader.ViewModels.Services.Dialog;

public class DialogViewModelBase<TResult> : ViewModelBase
    where TResult : DialogResultBase
{
    public event EventHandler<DialogResultEventArgs<TResult>> CloseRequested;

    public ICommand CloseCommand { get; }

    protected DialogViewModelBase()
    {
        CloseCommand = ReactiveCommand.Create(Close);
    }

    protected void Close() => Close(default);

    protected void Close(TResult result)
    {
        var args = new DialogResultEventArgs<TResult>(result);

        CloseRequested.Raise(this, args);
    }
}

public class DialogViewModelBase : DialogViewModelBase<DialogResultBase>
{

}