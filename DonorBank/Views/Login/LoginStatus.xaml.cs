namespace DonorBank.LoginUI
{
    using System.Linq;
    using System.Collections.Generic;
    using DonorBank.Web;
    using System.ComponentModel;
    using System.Globalization;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.ServiceModel.DomainServices.Client;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System;
    using System.Windows.Navigation;

    /// <summary>
    /// Класс <see cref="UserControl"/>, который показывает текущее состояние входа и позволяет выполнить вход и выход.
    /// </summary>
    public partial class LoginStatus : UserControl
    {
        private DonorBankDomainContext context = new DonorBankDomainContext();
        /// <summary>
        /// Создает новый экземпляр класса <see cref="LoginStatus"/>.
        /// </summary>
        public LoginStatus()
        {
            
            this.InitializeComponent();

            if (DesignerProperties.IsInDesignTool)
            {
                VisualStateManager.GoToState(this, "loggedOut", false);
            }
            else
            {
                this.DataContext = WebContext.Current;
                WebContext.Current.Authentication.LoggedIn += this.Authentication_LoggedIn;
                WebContext.Current.Authentication.LoggedOut += this.Authentication_LoggedOut;
                this.UpdateLoginState();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginRegistrationWindow loginWindow = new LoginRegistrationWindow();
            loginWindow.Show();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            WebContext.Current.Authentication.Logout(logoutOperation =>
            {
                if (logoutOperation.HasError)
                {
                    ErrorWindow.CreateNew(logoutOperation.Error);
                    logoutOperation.MarkErrorAsHandled();
                }
            }, /* userState */ null);
        }

        private void Authentication_LoggedIn(object sender, AuthenticationEventArgs e)
        {
            this.UpdateLoginState();

            context.Load(context.GetClinicByUserNameQuery(WebContext.Current.User.DisplayName)).Completed += LoginStatus_Completed;

            MainPage mainPage = MainPage.GetMainPage(this);
            if (WebContext.Current.User.IsInRole("Clinic"))
            {
                mainPage.DividerRespondBlood.Visibility = Visibility.Visible;
                mainPage.LinkRespondBlood.Visibility = Visibility.Visible;
                mainPage.DividerRespondTransplantant.Visibility = Visibility.Visible;
                mainPage.LinkRespondTransplantant.Visibility = Visibility.Visible;
            }

            if (WebContext.Current.User.IsInRole("Worker"))
            {           
                // Список больниц
                mainPage.DividerClinic.Visibility = Visibility.Visible;
                mainPage.LinkClinic.Visibility = Visibility.Visible;

                // Логи
                mainPage.DividerLogs.Visibility = Visibility.Visible;
                mainPage.LinkLogs.Visibility = Visibility.Visible;

                // Доноры
                mainPage.DividerDonor.Visibility = Visibility.Visible;
                mainPage.LinkDonor.Visibility = Visibility.Visible;

                // Органы
                mainPage.DividerTransplantant.Visibility = Visibility.Visible;
                mainPage.LinkTransplantant.Visibility = Visibility.Visible;

                // Кровь
                mainPage.DividerBlood.Visibility = Visibility.Visible;
                mainPage.LinkBlood.Visibility = Visibility.Visible;

                // Запросы
                mainPage.DividerRespondBlood.Visibility = Visibility.Visible;
                mainPage.LinkRespondBlood.Visibility = Visibility.Visible;
                mainPage.DividerRespondTransplantant.Visibility = Visibility.Visible;
                mainPage.LinkRespondTransplantant.Visibility = Visibility.Visible;

                // Отчеты
                mainPage.DividerReport.Visibility = Visibility.Visible;
                mainPage.LinkReport.Visibility = Visibility.Visible;
            }            
        }

        void LoginStatus_Completed(object sender, System.EventArgs e)
        {
            if (WebContext.Current.User.IsInRole("Clinic"))
                WebContext.Current.User.ClinicId = context.Clinics.ElementAt(0).Id;
            else
                WebContext.Current.User.ClinicId = 0;
        }

        private void Authentication_LoggedOut(object sender, AuthenticationEventArgs e)
        {
            this.UpdateLoginState();

            MainPage mainPage = MainPage.GetMainPage(this);
            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("/#/Home", UriKind.Relative));

            // Список больниц
            mainPage.DividerClinic.Visibility = Visibility.Collapsed;
            mainPage.LinkClinic.Visibility = Visibility.Collapsed;

            // Логи
            mainPage.DividerLogs.Visibility = Visibility.Collapsed;
            mainPage.LinkLogs.Visibility = Visibility.Collapsed;

            // Доноры
            mainPage.DividerDonor.Visibility = Visibility.Collapsed;
            mainPage.LinkDonor.Visibility = Visibility.Collapsed;

            // Органы
            mainPage.DividerTransplantant.Visibility = Visibility.Collapsed;
            mainPage.LinkTransplantant.Visibility = Visibility.Collapsed;

            // Кровь
            mainPage.DividerBlood.Visibility = Visibility.Collapsed;
            mainPage.LinkBlood.Visibility = Visibility.Collapsed;

            // Запросы
            mainPage.DividerRespondBlood.Visibility = Visibility.Collapsed;
            mainPage.LinkRespondBlood.Visibility = Visibility.Collapsed;
            mainPage.DividerRespondTransplantant.Visibility = Visibility.Collapsed;
            mainPage.LinkRespondTransplantant.Visibility = Visibility.Collapsed;

            // Отчеты
            mainPage.DividerReport.Visibility = Visibility.Collapsed;
            mainPage.LinkReport.Visibility = Visibility.Collapsed;
        }

        private void UpdateLoginState()
        {
            if (WebContext.Current.User.IsAuthenticated)
            {
                this.welcomeText.Text = string.Format(
                    CultureInfo.CurrentUICulture,
                    ApplicationStrings.WelcomeMessage,
                    WebContext.Current.User.DisplayName);
            }
            else
            {
                this.welcomeText.Text = ApplicationStrings.AuthenticatingMessage;
            }

            if (WebContext.Current.Authentication is WindowsAuthentication)
            {
                VisualStateManager.GoToState(this, "windowsAuth", true);
            }
            else
            {
                VisualStateManager.GoToState(this, (WebContext.Current.User.IsAuthenticated) ? "loggedIn" : "loggedOut", true);
            }
        }
    }
}
