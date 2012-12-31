using DonorBank.Web;
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

namespace DonorBank.Views.Form
{
    public partial class RespondBloodForm : ChildWindow
    {
        public RespondBlood Current = null;
        public RespondBloodForm()
        {
            InitializeComponent();
            bloodComboBox.SelectedIndex = 0;
            purposeComboBox.SelectedIndex = 0;
            Current = new RespondBlood();
            Current.IdClinic = WebContext.Current.User.ClinicId;
            Current.TimeCreate = DateTime.Now;
            Current.WaitTime = DateTime.Now + TimeSpan.FromHours(2);
            Current.Status = "Новый";
            grid1.DataContext = Current; 
            
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Current.Purpose = purposeComboBox.SelectionBoxItem as string;
            Current.Blood = bloodComboBox.SelectionBoxItem as string;
            if (!this.Current.HasValidationErrors)
            {
                this.DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void respondBloodDomainDataSource_LoadedData_1(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }
    }
}

