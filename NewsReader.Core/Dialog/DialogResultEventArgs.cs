using System;
namespace NewsReader.Core.Dialog;

public class DialogResultEventArgs<TResult> : EventArgs
{
    public TResult Result { get; }

    public DialogResultEventArgs(TResult result)
    {
        Result = result;
    }
}