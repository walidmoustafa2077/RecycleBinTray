using System.Windows;

namespace RecycleBinTray.Views
{
    /// <summary>
    /// Interaction logic for DialogView.xaml
    /// </summary>
    public partial class DialogView : Window
    {
        public static readonly DependencyProperty IsCancelableProperty =
            DependencyProperty.Register(nameof(IsCancelable), typeof(bool), typeof(DialogView), new PropertyMetadata(true));
        public bool IsCancelable
        {
            get => (bool)GetValue(IsCancelableProperty);
            set => SetValue(IsCancelableProperty, value);
        }

        public Action OnSubmit { get; set; } = () => { };
        public Action OnCancel { get; set; } = () => { };

        public DialogView(string title, string message)
        {
            InitializeComponent();

            Title = title;
            DialogTitle.Text = title;
            Message.Text = message;
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            OnCancel?.Invoke();
            Close();
        }

        private void OnSubmitClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            OnSubmit?.Invoke();
            Close();
        }
    }
}
