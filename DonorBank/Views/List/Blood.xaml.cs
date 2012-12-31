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
    public partial class BloodList : Page
    {
        public BloodList()
        {
            InitializeComponent();

            bloodComboBox.SelectionChanged += new SelectionChangedEventHandler(OnFilterSelectionChanged);
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

        private void bloodDomainDataSource_SubmittedChanges(object sender, SubmittedChangesEventArgs e)
        {
            if (e.HasError)
            {
                e.MarkErrorAsHandled();
            }
        }

        private void bloodDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        /// <summary>
        /// Выполняется, когда пользователь нажимает на одну из кнопок.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HyperlinkButton_AddBlood(object sender, RoutedEventArgs e)
        {
            BloodForm form = new BloodForm();
            form.Show();
            form.Closed += form_Closed;
        }
        private void HyperlinkButton_DeleteBlood(object sender, RoutedEventArgs e)
        {
            Blood DeleteBlood = Blood_List.SelectedItem as Blood;

            if (DeleteBlood != null)
            {
                ConfirmDialog winDelete = new ConfirmDialog("Вы действительно хотите удалить кровь №" + DeleteBlood.Id, DeleteBlood);
                winDelete.Title = "Удаление записи";
                winDelete.Show();
                winDelete.Closed += DeleteItem;
            }
        }
        /// <summary>
        /// Событие возникает при завершении диалога добавления.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void form_Closed(object sender, EventArgs e)
        {
            BloodForm bloodForm = sender as BloodForm;
            if (bloodForm.DialogResult == true)
            {
                bloodDomainDataSource.DataView.Add(bloodForm.Currentblood);
                bloodDomainDataSource.SubmitChanges();
                LogsSystem.addTransaction(bloodForm.Currentblood, LogsSystem.Type.Add).commit();
                //LogsSystem.addTransaction(bloodForm.donor, LogsSystem.Type.Edit).commit();
            }
            Blood_List.UpdateLayout();
        }
        /// <summary>
        /// Событие возникает при удалении крови.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteItem(object sender, EventArgs e)
        {
            if ((sender as ChildWindow).DialogResult == true)
            {
                ConfirmDialog confirmdlg = sender as ConfirmDialog;
                bloodDomainDataSource.DataView.Remove(confirmdlg.CurrentObject as Blood);
                bloodDomainDataSource.SubmitChanges();
                LogsSystem.addTransaction((confirmdlg.CurrentObject as Blood), LogsSystem.Type.Remove).commit();
            }
            Blood_List.UpdateLayout();
        }  

        private void OnFilterSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFilterValue = (bloodComboBox.SelectedItem as Label).Content.ToString();

            if (selectedFilterValue == selectAll)
            {
                bloodDomainDataSource.FilterDescriptors.Clear();
                return;
            }

            FilterDescriptor bloodFilter = new FilterDescriptor()
            {
                PropertyPath = "Donor.Blood",
                Operator = FilterOperator.IsEqualTo,
                Value = selectedFilterValue
            };
            bloodDomainDataSource.FilterDescriptors.Clear();
            bloodDomainDataSource.FilterDescriptors.Add(bloodFilter);
        }

        private const string selectAll = "*";
    }
}
