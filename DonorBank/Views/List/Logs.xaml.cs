using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using DonorBank.Web;

namespace DonorBank.Views
{
    public partial class LogsList : Page
    {
        public LogsList()
        {
            InitializeComponent();
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!WebContext.Current.User.IsInRole("Worker")) {
                ErrorWindow errorWnd = new ErrorWindow("Ошибка доступа.", "У вас нет доступа к данной странице. Возможно вы не авторизировались.");
                errorWnd.Show();
                NavigationService.Navigate(new Uri("/Home", UriKind.Relative));
            }
        }

        private void transactionInfoDomainDataSource_LoadedData_1(object sender, LoadedDataEventArgs e) {
            if (e.HasError) {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

    }
}
