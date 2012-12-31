using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DonorBank.Web;

namespace DonorBank.Views.Form
{
    public partial class AddTransplantantForm : ChildWindow
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
        /// Список трансплантантов.
        /// </summary>
        public List<Transplantant> list { get; protected set; }
        /// <summary>
        /// Массив органов для динамического добавления в форму.
        /// </summary>
        private string[] TypeTransplantant = new string[] {
            "Сердце",
            "Кожа",
            "Почка",
            "Печень",
            "Легкое"
        };

        public AddTransplantantForm()
        {
            InitializeComponent();
            list = new List<Transplantant>();
            donor = new Donor();
            gridDonor.DataContext = donor;
        }
        /// <summary>
        /// Объявляет список трансплантантов.
        /// </summary>
        private void SetTransplantantList()
        {
            if (donor.Number == 0 || donor == null)
                return;

            list.Clear();
            TransplantantPanel.Children.Clear();
            foreach (string Type in TypeTransplantant)
            {
                Transplantant transplantant = new Transplantant();
                transplantant.Type = Type;
                transplantant.IdDonor = donor.Id;
                transplantant.StorageTime = DateTime.Now;

                Expander expander = new Expander();
                expander.Content = transplantant;
                expander.Header = transplantant;
                expander.HeaderTemplate = LayoutRoot.Resources.ElementAt(0).Value as DataTemplate;
                expander.ContentTemplate = LayoutRoot.Resources.ElementAt(1).Value as DataTemplate;
                expander.ExpandDirection = ExpandDirection.Down;
                expander.HorizontalAlignment = HorizontalAlignment.Left;
                expander.VerticalAlignment = VerticalAlignment.Top;
                expander.Width = 212;
                expander.IsExpanded = false;

                expander.DataContext = transplantant;
                TransplantantPanel.Children.Add(expander);
                list.Add(transplantant);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if ((donor.Series == null) || (donor.Number == 0))
            {
                ErrorWindow errorWnd = new ErrorWindow("Ошибка", "Не введены паспортные данные!");
                errorWnd.Show();
                return;
            }

            foreach (Transplantant transplantant in list)
            {
                if (transplantant.HasValidationErrors)
                    return;
            }
            this.DialogResult = true;
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
                gridDonor.DataContext = donor;
                SetTransplantantList();
            }
            else
            {
                // Не нашли донора - спрашиваем о добавлении.
                ConfirmDialog addDonorDialog = new ConfirmDialog("Нет записи с такими данными. Добавить нового донора?", donor);
                addDonorDialog.Title = "Не найдено";
                addDonorDialog.Show();
                addDonorDialog.Closed += (s, e1) =>
                {
                    if ((s as ChildWindow).DialogResult == true)
                    {
                        // Добавить нового донора.
                        DonorForm form = new DonorForm(donor);
                        form.Show();
                        form.Closed += form_Closed;
                    }
                };
            };
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
            SetTransplantantList();
        }

        private void FindDonorByPassportLostFocus(object sender, RoutedEventArgs e)
        {
            if (context.Donors.Count <= 0 && donor.Number.ToString().Length == 6)
                context.Load(context.GetDonorByPassportQuery(donor.Series, donor.Number)).Completed += FindByPassport_Completed;
        }
    }
}

