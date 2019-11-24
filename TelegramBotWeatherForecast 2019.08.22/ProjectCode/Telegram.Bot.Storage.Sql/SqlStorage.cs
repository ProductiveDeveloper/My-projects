using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Telegram.Storage.Core;
using System.Linq;

namespace Telegram.Bot.Storage.Sql
{
    public class SqlStorage : IUserStorage
    {
        public static string _connectionString;

        public SqlStorage(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Count
        {
            get
            {
                using (SqlConnection sqlConnection = GetOpenedSqlConnection())
                {
                    var sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandText = $"SELECT COUNT(*) FROM Users";

                    return (int)sqlCommand.ExecuteScalar();
                }
            }
        }

        public void AddUser(UserItem user)
        {
            string dateString = $"{user.DateOfRegistration.Day}/{user.DateOfRegistration.Month}/{user.DateOfRegistration.Year} {user.DateOfRegistration.Hour}:{user.DateOfRegistration.Minute}";
            user.DateOfRegistration = DateTimeOffset.Parse(dateString);
            using (SqlConnection sqlConnection = GetOpenedSqlConnection())
            {
                var sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "INSERT INTO [dbo].[Users]([Id],[ChatId],[FirstName],[Status],[DateOfRegistration])" +
                    $"VALUES(N'{user.Id}' , '{user.ChatId}' , N'{user.FirstName}' , '{Convert.ToString(user.Status)}' , N'{user.DateOfRegistration}')";
                 
                sqlCommand.ExecuteNonQuery();
            }
        }

        public void Clear()
        {
            using (SqlConnection sqlConnection = GetOpenedSqlConnection())
            {
                var sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = $"DELETE FROM Users";

                sqlCommand.ExecuteNonQuery();
            }
        }

        public UserItem GetUser(Guid Id)
        {
            List<UserItem> userItems = GetUsers();
            
            foreach (var item in userItems)
            {
                if (item.Id == Id)
                    return item;
            }
            return null;
        }

        public List<UserItem> GetUsers()
        {
            List<UserItem> result = new List<UserItem>();

            using (SqlConnection connection = SqlStorage.GetOpenedSqlConnection())
            {
                var sqlCommand = connection.CreateCommand();
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = $"SELECT * FROM Users";

                using (var reader = sqlCommand.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return result;

                    int ChatIdColumnIndex = reader.GetOrdinal("ChatId");
                    int FirstNameColumnIndex = reader.GetOrdinal("FirstName");
                    int DateOfRegistration = reader.GetOrdinal("DateOfRegistration");
                    int Id = reader.GetOrdinal("Id");
                    int Status = reader.GetOrdinal("Status");

                    while (reader.Read())
                    {
                        var chatId = reader.GetInt32(ChatIdColumnIndex);
                        var firstName = reader.GetString(FirstNameColumnIndex);
                        var dateOfRegistration = reader.GetString(DateOfRegistration);
                        var id = reader.GetGuid(Id);
                        var status = reader.GetString(Status);
                        StatusEnum _status;
                        switch (status)
                        {
                            case "Admin":
                                _status = StatusEnum.Admin;
                                break;
                            case "Ban":
                                _status = StatusEnum.Ban;
                                break;
                            case "User":
                                _status = StatusEnum.User;
                                break;
                            default:
                                _status = StatusEnum.Null;
                                break;
                        }
                        
                        result.Add(new UserItem()
                        {
                            ChatId = chatId,
                            FirstName = firstName,
                            Status = _status,
                            Message = "Null",
                            DateOfRegistration = DateTimeOffset.Parse(dateOfRegistration),
                            Id = id
                        });
                    }
                }

                return result;
            }
        }

        public void RemoveUser(Guid Id)
        {
            using (SqlConnection sqlConnection = GetOpenedSqlConnection())
            {
                var sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandText = $"DELETE FROM Users WHERE Users.Id = '{Id}'";

                sqlCommand.ExecuteNonQuery();
            }
        }

        public void UpdateStatus(UserItem user)
        {
            List<UserItem> userItems = GetUsers();

            foreach (var item in userItems)
            {
                if (item.Id == user.Id)
                {
                    RemoveUser(user.Id);
                    AddUser(user);
                }
            }
        }


        public static SqlConnection GetOpenedSqlConnection()
        {
            var sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();

            return sqlConnection;
        }

        public UserItem GetUser(long ChatId)
        {
            List<UserItem> userItems = GetUsers();

            foreach (var item in userItems)
            {
                if (item.ChatId == ChatId)
                    return item;
            }
            return null;
        }
    }
}
