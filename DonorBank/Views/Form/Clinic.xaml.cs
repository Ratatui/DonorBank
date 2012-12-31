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

namespace DonorBank.Views
{ 
    /// <summary>
    /// Форма для редактирования/добавления клиник.
    /// </summary>
    public partial class ClinicForm : ChildWindow
    {
        // Текущий объект.
        public Clinic CurrentClinic { get; protected set;}

        public ClinicForm(Clinic clinic)
        {
            CurrentClinic = clinic;
            InitializeComponent();
            // Объявляем текущий контект для Binding.
            grid1.DataContext = CurrentClinic;
            // Адрес доступно только для просмотра.
            addresTextBox.IsReadOnly = true;
            usernameTextBox.IsReadOnly = true;           
        }

        public ClinicForm()
        {
            InitializeComponent();
            // Пустой объект.
            CurrentClinic = new Clinic();
            // Объявляем текущий контект для Binding.
            grid1.DataContext = CurrentClinic;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            // если в форме нет ошибок
            if (!CurrentClinic.HasValidationErrors)
                this.DialogResult = true;       
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;           
        }
    }
}

