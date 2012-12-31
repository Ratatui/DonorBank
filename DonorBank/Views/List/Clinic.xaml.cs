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
    /// <summary>
    /// Список всех больниц. Страница доступна только для работников предприятия.
    /// </summary>
    public partial class ClinicList : Page
    {
        public ClinicList()
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

        private void clinicDomainDataSource_SubmittedChanges(object sender, SubmittedChangesEventArgs e)
        {
            if (e.HasError)
            {
                e.MarkErrorAsHandled();
            }
        }

        private void clinicDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        // Выполняется, когда пользователь нажимает на одну из кнопок.
        private void HyperlinkButton_AddClinic(object sender, RoutedEventArgs e)
        {          
            ClinicForm winEdit = new ClinicForm();
            winEdit.Title = "Добавление данных";
            winEdit.Show();
            winEdit.Closed += AddItem;
        }

        private void HyperlinkButton_EditClinic(object sender, RoutedEventArgs e)
        {
            Clinic EditClinic = Clinic_List.SelectedItem as Clinic;

            if (EditClinic != null)
            {
                ClinicForm winEdit = new ClinicForm(EditClinic);
                winEdit.Title = "Редактирование данных";
                winEdit.Show();
                winEdit.Closed += EditItem;
            }
        }

        private void HyperlinkButton_Delete(object sender, RoutedEventArgs e)
        {
            Clinic DeleteClinic = Clinic_List.SelectedItem as Clinic;

            if (DeleteClinic != null)
            {
                ConfirmDialog winDelete = new ConfirmDialog("Вы действительно хотите удалить больницу:"+DeleteClinic.Title, DeleteClinic);
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
                LogsSystem.addTransaction((sender as ClinicForm).CurrentClinic, LogsSystem.Type.Add).commit();
                clinicDomainDataSource.DataView.Add((sender as ClinicForm).CurrentClinic);
            }
            clinicDomainDataSource.SubmitChanges();
        }

        private void EditItem(object sender, EventArgs e)
        {
            if ((sender as ChildWindow).DialogResult == true)
            {
                LogsSystem.addTransaction((sender as ClinicForm).CurrentClinic, LogsSystem.Type.Edit).commit();
                clinicDomainDataSource.SubmitChanges();
            }
        }

        private void DeleteItem(object sender, EventArgs e)
        {
            if ((sender as ChildWindow).DialogResult == true)
            {
                LogsSystem.addTransaction(((sender as ConfirmDialog).CurrentObject as Clinic), LogsSystem.Type.Remove).commit();
                clinicDomainDataSource.DataView.Remove((sender as ConfirmDialog).CurrentObject as Clinic);
                clinicDomainDataSource.SubmitChanges();
            }
            Clinic_List.UpdateLayout();
        }      
    }
}
