namespace DonorBank
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using DonorBank.LoginUI;

    /// <summary>
    /// Класс <see cref="UserControl"/> реализует основную функциональность пользовательского интерфейса приложения.
    /// </summary>
    public partial class MainPage : UserControl
    {
        /// <summary>
        /// Создает новый экземпляр класса <see cref="MainPage"/>.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            Application.Current.RootVisual = this;
        }

        /// <summary>
        /// Функция возвращает MainPage приложения, проходя все родительские элементы пока не найдет объект класса MainPage.
        /// </summary>
        /// <param name="currentPage">Текущая страница</param>
        /// <returns>Обьект класса Mainpage</returns>
        public static MainPage GetMainPage(FrameworkElement currentPage)
        {
            FrameworkElement fe = (FrameworkElement)currentPage.Parent;
            while (fe.GetType().Name != "MainPage")
                fe = (FrameworkElement)fe.Parent;
            return fe as MainPage;
        }

        /// <summary>
        /// После перехода в рамке (Frame) проверьте, что ссылка <see cref="HyperlinkButton"/> представляет текущую выбранную страницу
        /// </summary>
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            foreach (UIElement child in LinksStackPanel.Children)
            {
                HyperlinkButton hb = child as HyperlinkButton;
                if (hb != null && hb.NavigateUri != null)
                {
                    if (hb.NavigateUri.ToString().Equals(e.Uri.ToString()))
                    {
                        VisualStateManager.GoToState(hb, "ActiveLink", true);
                    }
                    else
                    {
                        VisualStateManager.GoToState(hb, "InactiveLink", true);
                    }
                }
            }
        }

        /// <summary>
        /// Если при переходе возникла ошибка, отобразить окно сообщения
        /// </summary>
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ErrorWindow.CreateNew(e.Exception);
        }
    }
}