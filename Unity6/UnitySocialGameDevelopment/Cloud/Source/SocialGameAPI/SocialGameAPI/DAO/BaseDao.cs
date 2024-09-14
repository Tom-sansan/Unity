using Microsoft.Data.SqlClient;
using System.Data;

namespace SocialGameAPI.DAO
{
    /// <summary>
    /// BaseDao Class
    /// </summary>
    public class BaseDao
    {
        /// <summary>
        /// connectionString
        /// </summary>
        private string _connectionString;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public BaseDao(string connectionString) =>
            _connectionString = connectionString;

        /// <summary>
        /// Get connection
        /// </summary>
        /// <returns></returns>
        protected SqlConnection GetConnection() =>
            new SqlConnection(_connectionString);

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameterList"></param>
        /// <returns></returns>
        protected int ExecuteNonQuery(string commandText, CommandType commandType, List<SqlParameter> parameterList = null)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    if (parameterList != null) command.Parameters.AddRange(parameterList.ToArray());
                    return command.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="readFunc"></param>
        /// <param name="parameterList"></param>
        /// <returns></returns>
        protected T ExecuteReader<T>(string commandText, CommandType commandType, Func<SqlDataReader, T> readFunc, List<SqlParameter> parameterList = null) where T : class, new()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    if (parameterList != null) command.Parameters.AddRange(parameterList.ToArray());
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) return readFunc(reader);
                    }
                }
            }
            return new T();
        }
    }
}
