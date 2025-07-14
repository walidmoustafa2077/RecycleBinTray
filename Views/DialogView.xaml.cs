using RecycleBinTray.Helpers;
using System.Windows;

namespace RecycleBinTray.Views
{
    /// <summary>
    /// Interaction logic for DialogView.xaml
    /// </summary>
    public partial class DialogView : Window
    {
        public bool IsCancelable
        {
            get => (bool)GetValue(IsCancelableProperty);
            set => SetValue(IsCancelableProperty, value);
        }

        public bool ShowSupTitle
        {
            get => (bool)GetValue(ShowSupTitleProperty);
            set => SetValue(ShowSupTitleProperty, value);
        }

        public bool ShowMessage
        {
            get => (bool)GetValue(ShowMessageProperty);
            set => SetValue(ShowMessageProperty, value);
        }

        public Action OnSubmit { get; set; } = () => { };
        public Action OnCancel { get; set; } = () => { };

        public DialogView(string title, string? supTitle = null, string? message = null, string? submitText = null, string? cancelText = null)
        {
            InitializeComponent();

            // Set the properties of the dialog view based on the parameters provided
            ShowSupTitle = !string.IsNullOrEmpty(supTitle);
            ShowMessage = !string.IsNullOrEmpty(message);
            IsCancelable = !string.IsNullOrEmpty(cancelText);

            Title = title;
            DialogTitle.Text = title;
            DialogSubTitle.Text = supTitle;
            Message.Text = message;

            SubmitButton.Content = submitText ?? "OK";
            CancelButton.Content = cancelText ?? "Cancel";
        }

        public void ChangeTextColor(DialogText text, string color)
        {
            // Change the text color of the specified dialog text element
            switch (text)
            {
                case DialogText.Title:
                    DialogTitle.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color));
                    break;
                case DialogText.Subtitle:
                    DialogSubTitle.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color));
                    break;
                case DialogText.Message:
                    Message.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color));
                    break;
            }
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

        // Dependency properties for the dialog view
        // These properties can be used to control the dialog's behavior and appearance

        // IsCancelable: Determines if the dialog can be canceled 
        public static readonly DependencyProperty IsCancelableProperty =
           DependencyProperty.Register(nameof(IsCancelable), typeof(bool), typeof(DialogView), new PropertyMetadata(true));

        // ShowSupTitle: Determines if the subtitle is shown
        public static readonly DependencyProperty ShowSupTitleProperty =
           DependencyProperty.Register(nameof(ShowSupTitle), typeof(bool), typeof(DialogView), new PropertyMetadata(true));

        // ShowMessage: Determines if the message is shown
        public static readonly DependencyProperty ShowMessageProperty =
           DependencyProperty.Register(nameof(ShowMessage), typeof(bool), typeof(DialogView), new PropertyMetadata(true));
    }
}
