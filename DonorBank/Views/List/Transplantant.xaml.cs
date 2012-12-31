using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DonorBank.Views.Form;
using DonorBank.Web;

namespace DonorBank.Views
{
    /// <summary>
    /// Класс списка органов.
    /// </summary>
    public partial class TransplantantList : Page
    {
        public TransplantantList()
        {
            InitializeComponent();
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!WebContext.Current.User.IsInRole("Worker"))
            {
                ErrorWindow errorWnd = new ErrorWindow("Ошибка доступа", "У вас нет доступа к данной странице. Возможно вы не авторизировались.");
                errorWnd.Show();
                NavigationService.Navigate(new Uri("/Home", UriKind.Relative));
            }
        }

        private void transplantantDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                MessageBox.Show(e.Error.ToString(), "Load Error", MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void HyperlinkButton_AddTransplantant(object sender, RoutedEventArgs e)
        {
            AddTransplantantForm form = new AddTransplantantForm();
            form.Show();
            form.Closed += form_Closed;
        }

        /// <summary>
        /// Событие возникает при завершении диалога добавления.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void form_Closed(object sender, EventArgs e)
        {
            AddTransplantantForm transForm = sender as AddTransplantantForm;
            if (transForm.DialogResult == true)
            {
                LogsSystem ls = LogsSystem.getInst;

                for (int i = 0; i < transForm.list.Count; i++)
                {
                    Transplantant trans = transForm.list[i];
                    if (trans.Factor1 != null && trans.Factor6 != null)
                    {
                        transplantantDomainDataSource.DataView.Add(trans);
                        LogsSystem.addTransaction(trans, LogsSystem.Type.Add);
                    }
                }
                ls.commit();

            }
            transplantantDomainDataSource.SubmitChanges();
        }

        /// <summary>
        /// Функция, выкидывающая диалог подтверждения удаления.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HyperlinkButton_DeleteTransplantant(object sender, RoutedEventArgs e)
        {
            Transplantant DeleteTrans = gridTrans.SelectedItem as Transplantant;

            if (DeleteTrans != null)
            {
                ConfirmDialog winDelete = new ConfirmDialog("Вы действительно хотите удалить орган: " + DeleteTrans.Type, DeleteTrans);
                winDelete.Title = "Удаление записи";
                winDelete.Show();
                winDelete.Closed += winDelete_Closed;
            }
        }

        void winDelete_Closed(object sender, EventArgs e)
        {
            if ((sender as ChildWindow).DialogResult == true)
            {
                LogsSystem.addTransaction(((sender as ConfirmDialog).CurrentObject as Transplantant), LogsSystem.Type.Remove).commit();
                transplantantDomainDataSource.DataView.Remove((sender as ConfirmDialog).CurrentObject as Transplantant);
                transplantantDomainDataSource.SubmitChanges();
            }
            gridTrans.UpdateLayout();
        }

    }
}
