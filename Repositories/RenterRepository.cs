using MySql.Data.MySqlClient;
using testNetMVC.Models;

namespace testNetMVC.Repositories
{
    public class RenterRepository
    {
        private readonly string _connecString = "Server=localhost;User=root;Password=password;Database=dotnettest1;SslMode=none";

        private readonly ILogger<RenterRepository>? _logger;
        public RenterRepository(ILogger logger)
        {
            _logger = (ILogger<RenterRepository>)logger;
        }

        public RenterRepository()
        {
        }

        public Renter? get(int id)
        {
            Renter? renter = null;

            Console.WriteLine("Retriving one renter");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string[] columns = { "id", "dni", "email", "first_name", "last_name", "phone" };
                    string sql = @"SELECT " + string.Join(", ", columns) + " FROM renters WHERE id=@id;";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {

                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@id", id);
                        if (_logger != null)
                            _logger.LogInformation("hola");
                        Console.WriteLine("Executing query: \n\n\t" + command.CommandText.Replace("@id", id.ToString()) + "\n");

                        connection.Open();

                        var reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            renter = new Renter
                            {
                                Id = reader["id"].ToString() != null ? Int32.Parse(reader["id"].ToString()!) : null,
                                Dni = reader["dni"].ToString(),
                                Email = reader["email"].ToString(),
                                First_name = reader["first_name"].ToString(),
                                Last_name = reader["last_name"].ToString(),
                                Phone = reader["phone"].ToString()
                            };
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retriving renter " + ex);

            }

            return renter;
        }

        public List<Renter>? getAll()
        {
            List<Renter>? renters = new List<Renter> { };
            Console.WriteLine("Retriving all renters");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string columns = "id, dni, email, first_name, last_name, phone";
                    string sql = @"SELECT " + columns + " FROM renters;";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        if (_logger != null)
                            _logger.LogInformation("hola");
                        connection.Open();
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var renter = new Renter
                            {
                                Id = reader["id"].ToString() != null ? Int32.Parse(reader["id"].ToString()!) : null,
                                Dni = reader["dni"].ToString(),
                                Email = reader["email"].ToString(),
                                First_name = reader["first_name"].ToString(),
                                Last_name = reader["last_name"].ToString(),
                                Phone = reader["phone"].ToString()
                            };

                            renters.Add(renter);
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retriving all renters " + ex);
                return null;
            }

            return renters;
        }

        public int create(Renter renter)
        {
            int id = -1;

            try
            {
                Console.WriteLine("Creating renter");

                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {

                    string sql = @"INSERT INTO renters(dni, first_name, last_name, email" + (renter.Phone != string.Empty ? ", phone)" : ")") + "  VALUES (@dni, @first_name, @last_name, @email" + (renter.Phone != string.Empty ? ", @phone)" : ")") + "; SELECT LAST_INSERT_ID()";
                    Console.WriteLine(sql);
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@dni", renter.Dni);
                        command.Parameters.AddWithValue("@first_name", renter.First_name);
                        command.Parameters.AddWithValue("@last_name", renter.Last_name);
                        command.Parameters.AddWithValue("@email", renter.Email);
                        if (renter.Phone != string.Empty)
                            command.Parameters.AddWithValue("@phone", renter.Phone);
                        connection.Open();
                        id = Convert.ToInt32(command.ExecuteScalar());
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating the renter " + ex);
            }

            return id;
        }

        public int delete(int id)
        {
            int result = -1;
            Console.WriteLine("Deleting renter");
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"DELETE FROM renters WHERE id=@id";
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
                Console.WriteLine("Error executing delete renter " + ex);
            }

            return result;
        }


        public int update(Renter renter)
        {
            int result = -1;
            Console.WriteLine("Updating renter");
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"UPDATE renters SET dni=@dni, first_name=@first_name, last_name=@last_name, email=@email" + (renter.Phone != string.Empty ? ", phone=@phone" : "") + " WHERE id=@id; SELECT LAST_INSERT_ID()";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@dni", renter.Dni);
                        command.Parameters.AddWithValue("@first_name", renter.First_name);
                        command.Parameters.AddWithValue("@last_name", renter.Last_name);
                        command.Parameters.AddWithValue("@email", renter.Email);
                        command.Parameters.AddWithValue("@phone", renter.Phone);
                        command.Parameters.AddWithValue("@id", renter.Id);
                        connection.Open();
                        result = Convert.ToInt32(command.ExecuteScalar());
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating renter" + ex);
            }

            return result;
        }
    }
}