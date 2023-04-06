using MySql.Data.MySqlClient;
using testNetMVC.Models;
using testNetMVC.Helpers;

namespace testNetMVC.Repositories
{
    public class UserRepository
    {
        private readonly string _connecString = "Server=localhost;User=root;Password=password;Database=dotnettest1;SslMode=none";

        private readonly ILogger<UserRepository>? _logger;
        public UserRepository(ILogger logger)
        {
            _logger = (ILogger<UserRepository>)logger;
        }

        public UserRepository()
        {
        }

        public User? getByEmail(string email)
        {
            User? user = null;

            Console.WriteLine("---Retriving one user by email");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = $"SELECT users.id id, email, password, avatar, role_id, label FROM users JOIN roles ON roles.id=role_id WHERE email='{email}';";
                    // string sql = @"SELECT email, password, avatar, role_id, label FROM users JOIN roles ON roles.id=role_id WHERE email='@email';";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {

                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@email", email);
                        Console.WriteLine("---Executing query: \n\n\t" + command.CommandText.Replace("@email", email) + "\n");
                        connection.Open();

                        var reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            user = new User
                            {
                                Id = reader["id"].ToString() != null ? Int32.Parse(reader["id"].ToString()!) : null,
                                // Avatar = reader["avatar"].ToString() ?? null,
                                Email = reader["email"].ToString(),
                                Password = reader["password"].ToString(),
                                RoleId = Convert.ToInt32(reader["role_id"]),
                                Role = new Role
                                {
                                    Id = Convert.ToInt32(reader["role_id"]),
                                    Label = reader["label"].ToString(),
                                },
                            };
                        }
                        else
                        {
                            Console.WriteLine("user not found");
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error retriving user by email  " + ex);

            }

            return user;
        }
        public User? get(int id)
        {
            User? user = null;

            Console.WriteLine("---Retriving one user");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"
                    SELECT email, password, avatar, role_id, label
                    FROM users
                    JOIN roles ON roles.id = role_id
                    WHERE id=@id;";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {

                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@id", id);
                        Console.WriteLine("---Executing query: \n\n\t" + command.CommandText.Replace("@id", id.ToString()) + "\n");

                        connection.Open();

                        var reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            user = new User
                            {
                                Id = reader["id"].ToString() != null ? Int32.Parse(reader["id"].ToString()!) : null,
                                Avatar = reader["avatar"].ToString(),
                                Email = reader["email"].ToString(),
                                Password = reader["password"].ToString(),
                                RoleId = Convert.ToInt32(reader["role_id"]),
                                Role = new Role
                                {
                                    Id = Convert.ToInt32(reader["role_id"]),
                                    Label = reader["label"].ToString(),
                                },
                            };
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error retriving user " + ex);

            }

            return user;
        }

        public List<User>? getAll()
        {
            List<User>? users = new List<User> { };
            Console.WriteLine("---Retriving all users");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"
                    SELECT email, password, avatar, role_id, label
                    FROM users
                    JOIN roles ON roles.id = role_id;";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        connection.Open();
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var user = new User
                            {
                                Id = reader["id"].ToString() != null ? Int32.Parse(reader["id"].ToString()!) : null,
                                Avatar = reader["avatar"].ToString(),
                                Email = reader["email"].ToString(),
                                Password = reader["password"].ToString(),
                                RoleId = Convert.ToInt32(reader["role_id"]),
                                Role = new Role
                                {
                                    Id = Convert.ToInt32(reader["role_id"]),
                                    Label = reader["label"].ToString(),
                                },
                            };

                            users.Add(user);
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error retriving all users " + ex);
                return null;
            }

            return users;
        }

        public int create(User user)
        {
            int id = -1;

            try
            {
                Console.WriteLine("Creating user");

                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {

                    string sql = @"
                    INSERT INTO users(email, password, avatar, role_id)
                    VALUES (@email, @password, @avatar, @role_id);
                    SELECT LAST_INSERT_ID()";
                    Console.WriteLine(sql);
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@email", user.Email);
                        command.Parameters.AddWithValue("@password", user.Password);
                        command.Parameters.AddWithValue("@avatar", user.Avatar);
                        command.Parameters.AddWithValue("@role_id", user.RoleId);
                        connection.Open();
                        id = Convert.ToInt32(command.ExecuteScalar());
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error creating the user " + ex);
            }

            return id;
        }

        public int delete(int id)
        {
            int result = -1;
            Console.WriteLine("---Deleting user");
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"DELETE FROM users WHERE id=@id";
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
                Console.WriteLine("---Error executing delete user " + ex);
            }

            return result;
        }


        public int update(User user)
        {
            int result = -1;
            Console.WriteLine("---Updating user");
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"
                    UPDATE users
                    SET email=@email, password=@password, avatar=@avatar, role_id=@role_id
                    WHERE id=@id;";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@email", user.Email);
                        command.Parameters.AddWithValue("@password", user.Password);
                        command.Parameters.AddWithValue("@avatar", user.Avatar);
                        command.Parameters.AddWithValue("@role_id", user.RoleId);
                        connection.Open();
                        result = Convert.ToInt32(command.ExecuteNonQuery());
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating user" + ex);
            }

            return result;
        }
    }
}