using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using DonorBank.Views.Form;
using DonorBank.Web;

namespace DonorBank.Views.List
{
    public partial class RespondTransplantantList : Page
    {
        public RespondTransplantantList()
        {
            InitializeComponent();
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (WebContext.Current.User.IsInRole("Clinic"))
            {
                RejectButton.Visibility = ConfirmButton.Visibility = Visibility.Collapsed;
                respondTransplantantDomainDataSource.QueryName = "RespondOwnTransplantant";
                Parameter p = new Parameter();
                p.Value = WebContext.Current.User.ClinicId;
                p.ParameterName = "ClinicId";
                respondTransplantantDomainDataSource.QueryParameters.Add(p);
                
            }
            else if (WebContext.Current.User.IsInRole("Worker"))
            {
                AddButton.Visibility = CancelButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                ErrorWindow errorWnd = new ErrorWindow("Ошибка доступа.", "У вас нет доступа к данной странице. Возможно вы не авторизировались.");
                errorWnd.Show();
                NavigationService.Navigate(new Uri("/Home", UriKind.Relative));
            }
        }

        private void respondTransplantantDomainDataSource_LoadedData_1(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }
        private void AddItem(object sender, EventArgs e)
        {
            if ((sender as ChildWindow).DialogResult == true)
            {
                LogsSystem.addTransaction((sender as RespondTransplantantForm).Current, LogsSystem.Type.Add).commit();
                respondTransplantantDomainDataSource.DataView.Add((sender as RespondTransplantantForm).Current);
                
     
            }
            respondTransplantantDomainDataSource.SubmitChanges();
        }
        private void AddButton_Click_1(object sender, RoutedEventArgs e)
        {
            RespondTransplantantForm f = new RespondTransplantantForm();
            f.Title = "Новый запрос";
            f.Show();
            f.Closed += AddItem;
        }

        private void CancelButton_Click_1(object sender, RoutedEventArgs e)
        {
            if ((respondTransplantantDataGrid.SelectedItem as RespondTransplantant).Status == "Новый")
            {
                (respondTransplantantDataGrid.SelectedItem as RespondTransplantant).Status = "Отменен";
                respondTransplantantDomainDataSource.SubmitChanges();
                LogsSystem.addTransaction((respondTransplantantDataGrid.SelectedItem as RespondTransplantant), LogsSystem.Type.Edit).commit();
                respondTransplantantDataGrid.UpdateLayout();
            }
        }

        private void ConfirmButton_Click_1(object sender, RoutedEventArgs e)
        {
            if ((respondTransplantantDataGrid.SelectedItem as RespondTransplantant).Status == "Новый")
            {
                (respondTransplantantDataGrid.SelectedItem as RespondTransplantant).Status = "Удовлетворен";
                respondTransplantantDomainDataSource.SubmitChanges();
                LogsSystem.addTransaction((respondTransplantantDataGrid.SelectedItem as RespondTransplantant), LogsSystem.Type.Edit).commit();
                respondTransplantantDataGrid.UpdateLayout();
            }
        }

        private void RejectButton_Click_1(object sender, RoutedEventArgs e)
        {
            if ((respondTransplantantDataGrid.SelectedItem as RespondTransplantant).Status == "Новый")
            {
                (respondTransplantantDataGrid.SelectedItem as RespondTransplantant).Status = "Отклонен";
                respondTransplantantDomainDataSource.SubmitChanges();
                LogsSystem.addTransaction((respondTransplantantDataGrid.SelectedItem as RespondTransplantant), LogsSystem.Type.Edit).commit();
                respondTransplantantDataGrid.UpdateLayout();
            }
        }

      


    }
}
