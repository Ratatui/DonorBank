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
using System.Windows.Navigation;
using DonorBank.Web;
using System.Windows.Controls.DataVisualization.Charting;

namespace DonorBank.Views.List
{
    public partial class Report : Page
    {
        public DonorBankDomainContext context = new DonorBankDomainContext();
        List<string> Organs = new List<string>();
               
        public Report()
        {
            InitializeComponent();
            Organs.Add("Сердце");
            Organs.Add("Кожа");
            Organs.Add("Почка");
            Organs.Add("Печень");
            Organs.Add("Легкое");
            this.Loaded += Report_Loaded;

           
        }

        void Report_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime SixMonthAgo = DateTime.Now - TimeSpan.FromDays(30 * 6);
            context.Load(context.RespondGivenTransplantantQuery(SixMonthAgo, DateTime.Now)).Completed += Report_Completed;
            context.Load(context.GetAvalibleTransplantantsQuery()).Completed+=Report_Completed1;
            
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!WebContext.Current.User.IsInRole("Worker"))
            {
                ErrorWindow errorWnd = new ErrorWindow("Ошибка доступа.", "У вас нет доступа к данной странице. Возможно вы не авторизировались.");
                errorWnd.Show();
                NavigationService.Navigate(new Uri("/Home", UriKind.Relative));
            }
        }

        void Report_Completed(object sender, EventArgs e)
        {
            if (context.RespondTransplantants.Count > 0)
            {
                for (int i=0; i < 5; i++)
                {
                    Dictionary<string, int> Values = new Dictionary<string, int>();
                    DateTime CurrMonth = DateTime.Now - TimeSpan.FromDays(30 * 5);
                    for (int j = 0; j < 6; j++)
                    {
                        int sum = 0;
                        sum = context.RespondTransplantants.Where(q => q.CreateTime.Month == CurrMonth.Month && q.Type == i).Count();
                        Values.Add(CurrMonth.ToString("MMMM"), sum);
                        CurrMonth = CurrMonth + TimeSpan.FromDays(30);
                    }
                    ((LineSeries)mcChart.Series[i]).ItemsSource = Values;
                    ((LineSeries)mcChart.Series[i]).Title = Organs[i];
                    
                }
            }
        }

        void Report_Completed1(object sender, EventArgs e)
        {
            if (context.Transplantants.Count > 0)
            {
                Dictionary<string, int> Values = new Dictionary<string, int>();
                for (int i = 0; i < Organs.Count; i++)
                {
                    Values.Add(Organs[i], context.Transplantants.Where(q => q.Type == Organs[i]).Count());
                }

                ((PieSeries)mcChart1.Series[0]).ItemsSource = Values;
            }
        }

    }
}
