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
using DonorBank.Web;

namespace DonorBank.Views
{
    public partial class DonorList : Page
    {
        public DonorList()
        {
            InitializeComponent();
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!WebContext.Current.User.IsInRole("Worker"))
            {
                ErrorWindow errorWnd = new ErrorWindow("Ошибка доступа.", "У вас нет доступа к данной странице. Возможно вы не авторизировались.");
                errorWnd.Show();
                NavigationService.Navigate(new Uri("/Home", UriKind.Relative));
            }
        }

        private void donorDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void donorDomainDataSource_SubmittedChanges(object sender, SubmittedChangesEventArgs e)
        {
            if (e.HasError)
            {
                e.MarkErrorAsHandled();
            }
        }

        private void HyperlinkButton_AddDonor(object sender, RoutedEventArgs e)
        {
            DonorForm winEdit = new DonorForm();
            winEdit.Title = "Добавление данных";
            winEdit.Show();
            winEdit.Closed += AddItem;
        }

        private void HyperlinkButton_EditDonor(object sender, RoutedEventArgs e)
        {
            Donor EditDonor = Donor_List.SelectedItem as Donor;

            if (EditDonor != null)
            {
                DonorForm winEdit = new DonorForm(EditDonor);
                winEdit.Title = "Редактирование данных";
                winEdit.Show();
                winEdit.Closed += EditItem;
            }
        }

        private void HyperlinkButton_DeleteDonor(object sender, RoutedEventArgs e)
        {
            Donor DeleteDonor = Donor_List.SelectedItem as Donor;

            if (DeleteDonor != null)
            {
                ConfirmDialog winDelete = new ConfirmDialog("Вы действительно хотите удалить донора:" + DeleteDonor.LastName + " " + DeleteDonor.FirstName, DeleteDonor);
                winDelete.Title = "Удаление записи";
                winDelete.Show();
                winDelete.Closed += DeleteItem;
            }
        }

        // Событие выполняется, когда закрывается форма работы с обьектом.
        private void AddItem(object sender, EventArgs e)
        {
            if ((sender as ChildWindow).DialogResult == true)
            {
                LogsSystem.addTransaction((sender as DonorForm).CurrentDonor, LogsSystem.Type.Add).commit();
                donorDomainDataSource.DataView.Add((sender as DonorForm).CurrentDonor);
            }
            donorDomainDataSource.SubmitChanges();
        }

        private void EditItem(object sender, EventArgs e)
        {
            if ((sender as ChildWindow).DialogResult == true)
            {
                LogsSystem.addTransaction((sender as DonorForm).CurrentDonor, LogsSystem.Type.Edit).commit();
                donorDomainDataSource.SubmitChanges();                
            }
        }

        private void DeleteItem(object sender, EventArgs e)
        {
            if ((sender as ChildWindow).DialogResult == true)
            {
                donorDomainDataSource.DataView.Remove((sender as ConfirmDialog).CurrentObject as Donor);
                LogsSystem.addTransaction((sender as ConfirmDialog).CurrentObject as Donor, LogsSystem.Type.Remove).commit();
                donorDomainDataSource.SubmitChanges();
            }
            Donor_List.UpdateLayout();
        } 

    }
}
