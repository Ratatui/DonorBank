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
    public partial class RespondTransplantantForm : ChildWindow
    {
        public RespondTransplantant Current = null;

        public RespondTransplantantForm()
        {
            InitializeComponent();
            Current = new RespondTransplantant();
            Current.IdClinic = WebContext.Current.User.ClinicId;
            Current.CreateTime = DateTime.Now;
            Current.WaitTime = DateTime.Now + TimeSpan.FromHours(2);
            Current.Status = "Новый";
            grid1.DataContext = Current; 
            typeComboBox.SelectedIndex = 0;
            
          
        }

 
      
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Current.Type = typeComboBox.SelectedIndex;
            if (!this.Current.HasValidationErrors)
            {
                
                this.DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void respondTransplantantDomainDataSource_LoadedData_1(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }
    }
}

