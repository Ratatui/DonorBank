using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DonorBank.Web;

namespace DonorBank.Views
{
    public partial class BloodForm : ChildWindow
    {
        /// <summary>
        /// Текущий донор.
        /// </summary>
        public Donor donor { get; protected set; }
        /// <summary>
        /// Локальный контекст базы данных.
        /// </summary>
        public DonorBankDomainContext context = new DonorBankDomainContext();
        /// <summary>
        /// Текущая кровь.
        /// </summary>
        public Blood Currentblood { get; protected set; }

        public BloodForm()
        {
            InitializeComponent();
            Currentblood = new Blood();
            donor = new Donor();
            gridBlood.DataContext = Currentblood;
            gridDonor.DataContext = donor;
            Currentblood.StorageTime = DateTime.Now;
        }
 
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if ((donor.Series == null) || (donor.Number == 0)) {
                ErrorWindow errorWnd = new ErrorWindow("Ошибка", "Не введены паспортные данные!");
                errorWnd.Show();
                return;
            }

            if (Currentblood.HasValidationErrors)
                return;

            if (donor.FirstName != null && PurposeComboBox.SelectedItem != null)
            {
                if (donor.BloodDonor == null) donor.BloodDonor = 1;
                else donor.BloodDonor++;
                Currentblood.Purpose = (PurposeComboBox.SelectedItem as Label).Content.ToString();
                Currentblood.IdDonor = donor.Id;
                context.SubmitChanges();
                this.DialogResult = true;
            }
            else this.DialogResult = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void FindDonorByPassport(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                context.Load(context.GetDonorByPassportQuery(donor.Series, donor.Number)).Completed += FindByPassport_Completed;
        }

        private void FindByPassport_Completed(object sender, EventArgs e)
        {
            if (context.Donors.Count != 0)
            {
                // Нашли донора.
                donor = context.Donors.ElementAt(0);
            }
            else
            {
                // Не нашли донора - спрашиваем о добавлении.
                ConfirmDialog addDonorDialog = new ConfirmDialog("Нет записи с такими данными. Добавить нового донора?", donor);
                addDonorDialog.Title = "Не найдено";
                addDonorDialog.Show();
                addDonorDialog.Closed += (s, e1) => {
                    if ((s as ChildWindow).DialogResult == true)
                    {
                        // Добавить нового донора.
                        DonorForm form = new DonorForm(donor);
                        form.Show();
                        form.Closed += form_Closed;
                    }
                };
            };
            gridDonor.DataContext = donor;
        }

        private void form_Closed(object sender, EventArgs e)
        {
            if ((sender as ChildWindow).DialogResult == true)
            {
                donor = (sender as DonorForm).CurrentDonor;
                LogsSystem.addTransaction(donor, LogsSystem.Type.Add).commit();
                context.Donors.Add(donor);
            }
            context.SubmitChanges();
            gridDonor.DataContext = donor;
        }

        private void FindDonorByPassportLostFocus(object sender, RoutedEventArgs e)
        {
            if (context.Donors.Count <= 0 && donor.Number.ToString().Length == 6)
                context.Load(context.GetDonorByPassportQuery(donor.Series, donor.Number)).Completed += FindByPassport_Completed;
        }
    }
}

