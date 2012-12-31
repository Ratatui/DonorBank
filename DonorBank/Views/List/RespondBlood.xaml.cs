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
    public partial class RespondBloodList : Page
    {
        public RespondBloodList()
        {
            InitializeComponent();
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            if (WebContext.Current.User.IsInRole("Clinic"))
            {
                respondBloodDomainDataSource.QueryName = "GetOwnRespondBlood";
                RejectButton.Visibility = ConfirmButton.Visibility = Visibility.Collapsed;
                Parameter p = new Parameter();
                p.ParameterName = "ClinicId";
                p.Value = WebContext.Current.User.ClinicId;
                respondBloodDomainDataSource.QueryParameters.Add(p);
               
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
        
        private void AddItem(object sender, EventArgs e)
        {
            if ((sender as ChildWindow).DialogResult == true)
            {
                LogsSystem.addTransaction((sender as RespondBloodForm).Current, LogsSystem.Type.Add).commit();
                respondBloodDomainDataSource.DataView.Add((sender as RespondBloodForm).Current);
            }
            respondBloodDomainDataSource.SubmitChanges();
        }
        private void AddButton_Click_1(object sender, RoutedEventArgs e)
        {
            RespondBloodForm f = new RespondBloodForm();
            f.Title = "Новый запрос";
            f.Show();
            f.Closed += AddItem;
        }

        private void CancelButton_Click_1(object sender, RoutedEventArgs e)
        {
            if ((respondBloodDataGrid.SelectedItem as RespondBlood).Status == "Новый")
            {
                (respondBloodDataGrid.SelectedItem as RespondBlood).Status = "Отменен";
                respondBloodDomainDataSource.SubmitChanges();
                LogsSystem.addTransaction((respondBloodDataGrid.SelectedItem as RespondBlood), LogsSystem.Type.Edit).commit();
                respondBloodDataGrid.UpdateLayout();
            }
        }

        private void ConfirmButton_Click_1(object sender, RoutedEventArgs e)
        {
            if ((respondBloodDataGrid.SelectedItem as RespondBlood).Status == "Новый")
            {
                (respondBloodDataGrid.SelectedItem as RespondBlood).Status = "Удовлетворен";
                respondBloodDomainDataSource.SubmitChanges();
                LogsSystem.addTransaction((respondBloodDataGrid.SelectedItem as RespondBlood), LogsSystem.Type.Edit).commit();
                respondBloodDataGrid.UpdateLayout();
            }
        }

        private void RejectButton_Click_1(object sender, RoutedEventArgs e)
        {
            if ((respondBloodDataGrid.SelectedItem as RespondBlood).Status == "Новый")
            {
                (respondBloodDataGrid.SelectedItem as RespondBlood).Status = "Отклонен";
                respondBloodDomainDataSource.SubmitChanges();
                LogsSystem.addTransaction((respondBloodDataGrid.SelectedItem as RespondBlood), LogsSystem.Type.Edit).commit();
                respondBloodDataGrid.UpdateLayout();
            }
        }

        private void respondBloodDomainDataSource_LoadedData_1(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void respondBloodDomainDataSource1_LoadedData_1(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void respondBloodDomainDataSource_LoadedData_2(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

    }
}
