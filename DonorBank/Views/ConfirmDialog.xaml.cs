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
    /// Диалог для подтвеждения действий, может хранить обьект над которым производятся измения.
    /// </summary>
    public partial class ConfirmDialog : ChildWindow
    {
        public Object CurrentObject { get; protected set; }

        /// <summary>
        /// Конструктор задает текст подтверждения и хранит объект при необходимости.
        /// </summary>
        /// <param name="ConfirmText">Текст подтверждения.</param>
        /// <param name="currentObject">Объект для хранения.</param>
        public ConfirmDialog(String ConfirmText, Object currentObject)
        {
            InitializeComponent();

            ConfirmTextBlock.Text = ConfirmText;
            CurrentObject = currentObject;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

