using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConnectSQLiteToXamarinForm
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }
    }
}