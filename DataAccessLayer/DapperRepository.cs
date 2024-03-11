using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Teacher;

namespace DataAccessLayer
{
    public  class DapperRepository : IRepository
    {
            
        static string host = "DESKTOP-TCE3HGM\\SQLEXPRESS"; // Имя хоста
        static string database = "Hakaton"; // Имя базы данных
        static string user = "user2"; // Имя пользователя
        static string password = "1234"; // Пароль пользователя

        static string connectionString = $"Server={host};Database={database};User Id={user};Password={password};";
        /// <summary>
        /// Строка подключения
        /// </summary
        /// <summary>
        /// Функция добавления объекта
        /// </summary>
        /// <param name="item">Добавляемый объект</param>
        public void Add(teacher item)
        {
            using (IDbConnection connectionDB = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Teachers (Name,Email,Phone,Adress, [Work]) VALUES(@Name, @Email, @Phone, @Adress, @Work); SELECT CAST(SCOPE_IDENTITY() as int)";
                int? userId = connectionDB.Query<int>(sqlQuery, item).FirstOrDefault();
                item.Id = (int)userId;
            }
        }
        /// <summary>
        /// Функция для возвращения списка всех объектов, представленных в БД
        /// </summary>
        /// <returns>Лист со всеми объектами</returns>
        public IEnumerable<teacher> GetAll()
        {
            using (IDbConnection connectionDB = new SqlConnection(connectionString))
            {
                return new ObservableCollection<teacher>(connectionDB.Query<teacher>("SELECT * FROM Teachers").ToList());
            }
        }
        /// <summary>
        /// Функция, возвращающая объект по id
        /// </summary>
        /// <param name="Id">Id требуемого объекта</param>
        /// <returns>Объект БД</returns>
        public teacher GetById(int Id)
        {
            using (IDbConnection connectionDB = new SqlConnection(connectionString))
            {
                return connectionDB.Query<teacher>("SELECT * FROM Teachers WHERE Id = @Id", new { Id }).FirstOrDefault();
            }
        }
        /// <summary>
        /// Функция сохранения изменений
        /// </summary>
    }
}
