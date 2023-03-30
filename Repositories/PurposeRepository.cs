using MySql.Data.MySqlClient;
using testNetMVC.Models;

namespace testNetMVC.Repositories
{
    public class PurposeRepository
    {
        private readonly string _connecString = "Server=localhost;User=root;Password=password;Database=dotnettest1;SslMode=none";

        private readonly ILogger<PurposeRepository>? _logger;
        public PurposeRepository(ILogger logger)
        {
            _logger = (ILogger<PurposeRepository>)logger;
        }

        public PurposeRepository()
        {
        }

        public Purpose? get(int id)
        {
            Purpose? purpose = null;

            Console.WriteLine("---Retriving one property purpose");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"SELECT id, description FROM purposes WHERE id=@id;";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {

                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@id", id);
                        Console.WriteLine("---Executing query: \n\n\t" + command.CommandText.Replace("@id", id.ToString()) + "\n");

                        connection.Open();

                        var reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            purpose = new Purpose
                            {
                                Id = reader["id"].ToString() != null ? Int32.Parse(reader["id"].ToString()!) : null,
                                Description = reader["description"].ToString(),
                            };
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error retriving property purpose " + ex);

            }

            return purpose;
        }

        public List<Purpose>? getAll()
        {
            List<Purpose>? purposes = new List<Purpose> { };
            Console.WriteLine("---Retriving all property purposes");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"SELECT id, description FROM purposes;";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        if (_logger != null)
                            _logger.LogInformation("hola");
                        connection.Open();
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var purpose = new Purpose
                            {
                                Id = reader["id"].ToString() != null ? Int32.Parse(reader["id"].ToString()!) : null,
                                Description = reader["description"].ToString(),
                            };

                            purposes.Add(purpose);
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error retriving all property purposes " + ex);
                return null;
            }

            return purposes;
        }

        /*
        public int create(Purpose owner)
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


        public int update(Purpose owner)
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