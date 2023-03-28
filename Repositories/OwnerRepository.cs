using MySql.Data.MySqlClient;
using testNetMVC.Models;
using testNetMVC.Helpers;

namespace testNetMVC.Repositories
{
    public class OwnerRepository
    {
        private readonly string _connecString = "Server=localhost;User=root;Password=password;Database=dotnettest1;SslMode=none";

        private readonly ILogger<OwnerRepository>? _logger;
        public OwnerRepository(ILogger logger)
        {
            _logger = (ILogger<OwnerRepository>)logger;
        }

        public OwnerRepository()
        {
        }

        public Owner? get(int id)
        {
            Owner? owner = null;

            Console.WriteLine("---Retriving one owner");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string[] columns = { "id", "dni", "email", "first_name", "last_name", "phone" };
                    string sql = @"SELECT " + string.Join(", ", columns) + " FROM owners WHERE id=@id;";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {

                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@id", id);
                        if (_logger != null)
                            _logger.LogInformation("hola");
                        Console.WriteLine("---Executing query: \n\n\t" + command.CommandText.Replace("@id", id.ToString()) + "\n");

                        connection.Open();

                        var reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            owner = new Owner
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
                Console.WriteLine("---Error retriving owner " + ex);

            }

            return owner;
        }

        public List<Owner>? getAll()
        {
            List<Owner>? owners = new List<Owner> { };
            Console.WriteLine("---Retriving all owners");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string columns = "id, dni, email, first_name, last_name, phone";
                    string sql = @"SELECT " + columns + " FROM owners;";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        if (_logger != null)
                            _logger.LogInformation("hola");
                        connection.Open();
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var owner = new Owner
                            {
                                Id = reader["id"].ToString() != null ? Int32.Parse(reader["id"].ToString()!) : null,
                                Dni = reader["dni"].ToString(),
                                Email = reader["email"].ToString(),
                                First_name = reader["first_name"].ToString(),
                                Last_name = reader["last_name"].ToString(),
                                Phone = reader["phone"].ToString()
                            };

                            owners.Add(owner);
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error retriving all owners " + ex);
                return null;
            }

            return owners;
        }

        public int create(Owner owner)
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


        public int update(Owner owner)
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
    }
}