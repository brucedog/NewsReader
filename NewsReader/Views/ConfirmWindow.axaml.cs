using Avalonia.Controls;
using Avalonia.Interactivity;
using NewsReader.Core.Dialog;
using System;

namespace NewsReader.Views
{
    public partial class ConfirmWindow : Window
    {
        public event EventHandler<DialogResultEventArgs<string>> CloseRequested;

        public ConfirmWindow()
        {
            InitializeComponent();
        }

        private void Yes_OnClick(object? sender, RoutedEventArgs e)
        {
            var args = new DialogResultEventArgs<string>("yes");

            CloseRequested.Invoke(this, args);
            Close();
            //Close(new DialogResultEventArgs<string>("yes"));
        }

        private void Cancel_OnClick(object? sender, RoutedEventArgs e)
        {
            Close(new DialogResultEventArgs<string>("cancel"));
        }
    }
}