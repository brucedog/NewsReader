﻿using System;
using NewsReader.Core.Dialog;

namespace NewsReader.ViewModels;

public class DialogViewModelBase<TResult> : ViewModelBase
    where TResult : DialogResultBase
{
    //public event EventHandler<DialogResultEventArgs<TResult>> CloseRequested;

    //protected void Close() => Close(default);

    //protected void Close(TResult result)
    //{
    //    var args = new DialogResultEventArgs<TResult>(result);

    //    CloseRequested.Raise(this, args);
    //}
}
