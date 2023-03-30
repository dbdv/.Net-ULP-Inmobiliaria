using MySql.Data.MySqlClient;
using Type = testNetMVC.Models.Type;

namespace testNetMVC.Repositories
{
    public class TypeRepository
    {
        private readonly string _connecString = "Server=localhost;User=root;Password=password;Database=dotnettest1;SslMode=none";

        private readonly ILogger<TypeRepository>? _logger;
        public TypeRepository(ILogger logger)
        {
            _logger = (ILogger<TypeRepository>)logger;
        }

        public TypeRepository()
        {
        }

        public Type? get(int id)
        {
            Type? type = null;

            Console.WriteLine("---Retriving one type");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"SELECT id, label FROM types WHERE id=@id;";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {

                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@id", id);
                        Console.WriteLine("---Executing query: \n\n\t" + command.CommandText.Replace("@id", id.ToString()) + "\n");

                        connection.Open();

                        var reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            type = new Type
                            {
                                Id = reader["id"].ToString() != null ? Int32.Parse(reader["id"].ToString()!) : null,
                                Label = reader["label"].ToString(),
                            };
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error retriving type " + ex);

            }

            return type;
        }

        public List<Type>? getAll()
        {
            List<Type>? types = new List<Type> { };
            Console.WriteLine("---Retriving all types");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"SELECT id, label FROM types;";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        if (_logger != null)
                            _logger.LogInformation("hola");
                        connection.Open();
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var type = new Type
                            {
                                Id = reader["id"].ToString() != null ? Int32.Parse(reader["id"].ToString()!) : null,
                                Label = reader["label"].ToString(),
                            };

                            types.Add(type);
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error retriving all types " + ex);
                return null;
            }

            return types;
        }

        /*
        public int create(Type owner)
        {
            int id = -1;

            try
            {
                Console.WriteLine("Creating owner");

                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {

                    string sql = @"INSERT INTO Owners(dni, first_name, last_name, email" + (owner.Phone != string.Empty ? ", phone)" : ")") + "  VALUES (@dni, @first_name, @last_name, @email" + (owner.Phone != string.Empty ? ", @phone)" : ")") + "; SELECT LAST_INSERT_ID()";
                    Console.WriteLine(sql);
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@dni", owner.Dni);
                        command.Parameters.AddWithValue("@first_name", owner.First_name);
                        command.Parameters.AddWithValue("@last_name", owner.Last_name);
                        command.Parameters.AddWithValue("@email", owner.Email);
                        if (owner.Phone != string.Empty)
                            command.Parameters.AddWithValue("@phone", owner.Phone);
                        connection.Open();
                        id = Convert.ToInt32(command.ExecuteScalar());
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error creating the owner " + ex);
            }

            return id;
        }

        public int delete(int id)
        {
            int result = -1;
            Console.WriteLine("---Deleting owner");
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"DELETE FROM Owners WHERE id=@id";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@id", id);
                        connection.Open();
                        result = (int)command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error executing delete owner " + ex);
            }

            return result;
        }


        public int update(Type owner)
        {
            int result = -1;
            Console.WriteLine("---Updating owner");
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"UPDATE Owners SET dni=@dni, first_name=@first_name, last_name=@last_name, email=@email"
                     + (owner.Phone != string.Empty ? ", phone=@phone" : "")
                     + " WHERE id=@id;";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@dni", owner.Dni);
                        command.Parameters.AddWithValue("@first_name", owner.First_name);
                        command.Parameters.AddWithValue("@last_name", owner.Last_name);
                        command.Parameters.AddWithValue("@email", owner.Email);
                        command.Parameters.AddWithValue("@phone", owner.Phone);
                        command.Parameters.AddWithValue("@id", owner.Id);
                        connection.Open();
                        result = Convert.ToInt32(command.ExecuteNonQuery());
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating owner" + ex);
            }

            return result;
        }
        */
    }
}