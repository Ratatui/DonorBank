using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DonorBank.Web;

namespace DonorBank.Views
{
    /// <summary>
    /// Форма для редактирования/добавления доноров.
    /// </summary>
    public partial class DonorForm : ChildWindow
    {
        // Текущий объект.
        public Donor CurrentDonor { get; protected set; }

        public DonorForm()
        {
            InitializeComponent();

            CurrentDonor = new Donor();
            // Объявляем текущий контект для Binding.
            donorInfoGrid.DataContext = CurrentDonor;

            DatePicker a = new DatePicker();
        }

        public DonorForm(Donor donor)
        {
            InitializeComponent();

            CurrentDonor = donor;
            // Объявляем текущий контект для Binding.
            donorInfoGrid.DataContext = CurrentDonor;

            int i = 0;
            foreach(var arg in bloodComboBox.Items.ToList())
            {
                if ((arg as Label).Content.ToString() == donor.Blood)
                {
                    bloodComboBox.SelectedIndex= i;
                }

                i++;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentDonor.ValidationErrors.Count == 0)
            {
                CurrentDonor.Blood = (bloodComboBox.SelectedItem as Label).Content.ToString();
                this.DialogResult = true;                
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem == null)
            {
                CurrentDonor.Blood = "Err";
            }
            else
            {
                String str = ((sender as ComboBox).SelectedItem as Label).Content.ToString();
                CurrentDonor.Blood = str;
            }
        }
    }
}

