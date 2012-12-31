using System;
using System.ServiceModel.DomainServices.Client;
using System.Text;
using DonorBank.Web;

namespace DonorBank
{
    /// <summary>
    /// Класс логирования изменений в таблицах.
    /// </summary>
    public class LogsSystem
    {
        /// <summary>
        /// Перечисление возможных операций.
        /// </summary>
        public enum Type {
            Add,
            Edit,
            Remove
        };

        /// <summary>
        /// Добавить данные об изменении.
        /// </summary>
        /// <param name="data">Объект таблицы, в которой произошли изменения.</param>
        /// <param name="type">Тип изменения (Add, Edit, Remove).</param>
        /// <returns>Возвращает единственный экземпляр класса для дальнейшего применения метода commit().</returns>
        public static LogsSystem addTransaction(object data, Type type) {
            instance.newTransaction(data, type);

            return instance;
        }
        
        /// <summary>
        /// Единственный экземпляр этого класса.
        /// </summary>
        private static readonly LogsSystem instance = new LogsSystem();

        public static LogsSystem getInst
        {
            get { return instance; }
        }

        /// <summary>
        /// Идентификатор таблиц.
        /// </summary>
        private enum TABLE {
            NONE,
            BLOOD,
            CLINIC,
            DONOR,
            TRANSPLANTANT,
            RESPOND_BLOOD,
            RESPOND_TRANSPLANTANT
        };

        /// <summary>
        /// Название таблиц.
        /// </summary>
        private readonly string[] TABLE_NAMES = {
             "",
             "Кровь",
             "Больницы",
             "Доноры",
             "Органы",
             "Запросы на кровь",
             "Запросы на органы"
        };

        /// <summary>
        /// Описание для каждого типа изменений.
        /// </summary>
        private readonly string[] TYPE_NAMES = {
             "Новая запись в таблице ",
             "Изменена запись в таблице ",
             "Удалена запись из таблицы "
        };

        /// <summary>
        /// Контекст домена для работы с таблицами.
        /// </summary>
        private DonorBankDomainContext context;

        private LogsSystem() {
            context = new DonorBankDomainContext();
            context.Load(context.GetTransactionInfoQuery());
        }

        /// <summary>
        /// Подтвердить изменения таблицы.
        /// </summary>
        public void commit() {
            context.SubmitChanges();
        }

        /// <summary>
        /// Создать новую запись об изменении.
        /// </summary>
        /// <param name="data">Объект таблицы, в которой произошли изменения.</param>
        /// <param name="type">Тип изменения (Add, Edit, Remove).</param>
        private void newTransaction(object data, Type type) {
            TABLE table = getType(data);
            if (table == TABLE.NONE) return;

            int tableId = (int) table;
            int typeId = (int) type;

            StringBuilder about = new StringBuilder();
            about.Append(TYPE_NAMES[typeId]);
            about.Append('"').Append(TABLE_NAMES[tableId]).AppendLine("\"");
            about.Append( extractInfo(data) );
            addTransactions(type.ToString(), about.ToString().TrimEnd());
        }

        /// <summary>
        /// Получить подробную информацию из данных таблицы.
        /// </summary>
        /// <param name="data">Объект данных одной из таблиц.</param>
        /// <returns>Строка с подробной информацией об объекте.</returns>
        private string extractInfo(object data) {
            Entity entity = data as Entity;
            if (entity == null) return "";

            return entity.ToString();
        }

        /// <summary>
        /// Добавить запись об изменении в таблицу.
        /// </summary>
        private void addTransactions(string type, string about) {
            addTransactions(type, WebContext.Current.User.DisplayName, DateTime.Now, about);
        }

        /// <summary>
        /// Добавить запись об изменении в таблицу.
        /// </summary>
        private void addTransactions(string type, string user, DateTime date, string about) {
            TransactionInfo info = new TransactionInfo();
            info.Type = type;
            info.User = user;
            info.TimeDate = date;
            info.About = about;

            context.TransactionInfos.Add(info);
        }

        /// <summary>
        /// Получаем по id таблицы тип объекта.
        /// </summary>
        private TABLE getType(object obj) {
            if (obj is Blood) return TABLE.BLOOD;
            if (obj is Clinic) return TABLE.CLINIC;
            if (obj is Donor) return TABLE.DONOR;
            if (obj is Transplantant) return TABLE.TRANSPLANTANT;
            if (obj is RespondBlood) return TABLE.RESPOND_BLOOD;
            if (obj is RespondTransplantant) return TABLE.RESPOND_TRANSPLANTANT;
            return TABLE.NONE;
        }
    }
}
