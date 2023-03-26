using MySql.Data.MySqlClient;
using testNetMVC.Models;
using testNetMVC.Helpers;

namespace testNetMVC.Repositories
{
    public class OwnerRepository
    {
        private string connecString = "Server=localhost;User=root;Password=password;Database=dotnettest1;SslMode=none";

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

            Helper.executeSafe("get one Owner", () =>
            {
                // string connecString = "Server=localhost;User=root;Password=password;Database=dotnettest1;SslMode=none";
                using (MySqlConnection connection = new MySqlConnection(connecString))
                {
                    string[] columns = { "id", "dni", "email", "first_name", "last_name", "phone" };
                    string sql = @"SELECT " + string.Join(", ", columns) + " FROM owners WHERE id=@id;";
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
            });

            return owner;
        }

        public List<Owner> getAll()
        {
            List<Owner> owners = new List<Owner> { };

            Helper.executeSafe("get all Owners", () =>
            {
                using (MySqlConnection connection = new MySqlConnection(connecString))
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
            });

            return owners;
        }

        public int create(Owner owner)
        {
            int id = -1;

            Helper.executeSafe("create Owner", () =>
            {
                using (MySqlConnection connection = new MySqlConnection(connecString))
                {

                    // List<string> defaultColumns = new List<string> { "dni", "email", "first_name", "last_name", "phone" };
                    /* string[] columns = {};
                    for (int i = 0; i < columns.Count(); i++)
                    {
                        if(owner.GetType().GetProperty(columns[i]) != null){
                            columns.Append(columns[i]);
                        }
                    } */
                    // List<string> values = (List<string>)defaultColumns.Select(col => "@" + col);
                    // string sql = @"INSERT INTO Owners(" + string.Join(", ", defaultColumns) + ") VALUES(" + string.Join(", ", values) + "); SELECT LAST_INSERT_ID()";
                    string sql = @"INSERT INTO Owners(dni, first_name, last_name, phone, email) VALUES (@dni, @first_name, @last_name, @phone, @email); SELECT LAST_INSERT_ID()";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@dni", owner.Dni);
                        command.Parameters.AddWithValue("@first_name", owner.First_name);
                        command.Parameters.AddWithValue("@last_name", owner.Last_name);
                        command.Parameters.AddWithValue("@phone", owner.Phone);
                        command.Parameters.AddWithValue("@email", owner.Email);
                        /* for (int i = 0; i < values.Count(); i++)
                        {
                            command.Parameters.AddWithValue(values[i], owner.GetType().GetProperty(Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(defaultColumns[i])).GetValue(owner, null));
                        } */
                        connection.Open();
                        id = Convert.ToInt32(command.ExecuteScalar());
                        connection.Close();
                    }
                }
            });

            return id;
        }

        public int delete(int id)
        {
            int result = -1;

            Helper.executeSafe("delete Owner", () =>
            {
                using (MySqlConnection connection = new MySqlConnection(connecString))
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
            });

            return result;
        }


        public int update(Owner owner)
        {
            int result = -1;

            Helper.executeSafe("update Owner", () =>
            {
                using (MySqlConnection connection = new MySqlConnection(connecString))
                {
                    string[] columns = { "id", "dni", "email", "first_name", "last_name", "phone" };
                    string values = string.Join(", ", columns.Select(col => col + "=" + "@" + col));
                    string sql = @"UPDATE Owners SET " + values + " WHERE id=@id; SELECT LAST_INSERT_ID()";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@id", owner.Id);
                        connection.Open();
                        result = (int)command.ExecuteScalar();
                        connection.Close();
                    }
                }
            });

            return result;
        }
    }
}