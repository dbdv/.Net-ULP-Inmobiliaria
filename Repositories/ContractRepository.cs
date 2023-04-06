using MySql.Data.MySqlClient;
using testNetMVC.Models;

namespace testNetMVC.Repositories
{
    public class ContractRepository
    {
        private readonly string _connecString = "Server=localhost;User=root;Password=password;Database=dotnettest1;SslMode=none";

        private readonly ILogger<ContractRepository>? _logger;
        public ContractRepository(ILogger logger)
        {
            _logger = (ILogger<ContractRepository>)logger;
        }

        public ContractRepository()
        {
        }

        public Contract? get(int id)
        {
            Contract? contract = null;

            Console.WriteLine("---Retriving one contract");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"SELECT contract.id id, from, until, fee, renter_id,
                    property_id,
                    renter_id, dni, email, first_name, last_name, phone
                    FROM contracts
                    JOIN properties p ON p.id = contracts.property_id
                    JOIN renters r ON r.id = contracts.renter_id
                    WHERE contracts.id=@id;";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {

                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@id", id);
                        Console.WriteLine("---Executing query: \n\n\t" + command.CommandText.Replace("@id", id.ToString()) + "\n");

                        connection.Open();

                        var reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            contract = new Contract
                            {
                                Id = Int32.Parse(reader["id"].ToString()!),
                                Property_id = Int32.Parse(reader["property_id"].ToString()!),
                                Property = new Property
                                {
                                    Id = reader["id"].ToString() != null ? Int32.Parse(reader["id"].ToString()!) : null,
                                    Address = reader["address"].ToString(),
                                    // Rooms = Convert.ToInt32(reader["rooms"]),
                                    Price = Convert.ToDouble(reader["price"]),
                                    // Latitude = Convert.ToDouble(reader["latitude"]),
                                    // Longitude = Convert.ToDouble(reader["longitude"]),
                                    Owner_id = Convert.ToInt32(reader["owner_id"].ToString()),
                                },
                                From = Convert.ToDateTime(reader["from"]),
                                Until = Convert.ToDateTime(reader["until"]),
                                Fee = Convert.ToDecimal(reader["fee"]),
                                Renter_id = Int32.Parse(reader["renter_id"].ToString()!),
                                Renter = new Renter
                                {
                                    First_name = reader["first_name"].ToString(),
                                    Last_name = reader["last"].ToString(),
                                    Dni = reader["dni"].ToString(),
                                },
                            };
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error retriving contract " + ex);

            }

            return contract;
        }

        public List<Contract>? getAll()
        {
            List<Contract>? properties = new List<Contract> { };
            Console.WriteLine("---Retriving all properties var por aca");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"SELECT contract.id id, from, until, fee, renter_id,
                    property_id,
                    renter_id, dni, email, first_name, last_name, phone
                    FROM contracts
                    JOIN properties p ON p.id = contracts.property_id
                    JOIN renters r ON r.id = contracts.renter_id;";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        if (_logger != null)
                            _logger.LogInformation("hola");
                        connection.Open();
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var contract = new Contract
                            {
                                Id = Int32.Parse(reader["id"].ToString()!),
                                Property_id = Int32.Parse(reader["property_id"].ToString()!),
                                Property = new Property
                                {
                                    Id = reader["id"].ToString() != null ? Int32.Parse(reader["id"].ToString()!) : null,
                                    Address = reader["address"].ToString(),
                                    // Rooms = Convert.ToInt32(reader["rooms"]),
                                    Price = Convert.ToDouble(reader["price"]),
                                    // Latitude = Convert.ToDouble(reader["latitude"]),
                                    // Longitude = Convert.ToDouble(reader["longitude"]),
                                    Owner_id = Convert.ToInt32(reader["owner_id"].ToString()),
                                },
                                From = Convert.ToDateTime(reader["from"]),
                                Until = Convert.ToDateTime(reader["until"]),
                                Fee = Convert.ToDecimal(reader["fee"]),
                                Renter_id = Int32.Parse(reader["renter_id"].ToString()!),
                                Renter = new Renter
                                {
                                    First_name = reader["first_name"].ToString(),
                                    Last_name = reader["last"].ToString(),
                                    Dni = reader["dni"].ToString(),
                                },
                            };

                            properties.Add(contract);
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error retriving all properties " + ex);
                return null;
            }

            return properties;
        }

        public int create(Contract contract)
        {
            int id = -1;

            try
            {
                Console.WriteLine("---Creating contract");

                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {

                    string sql = @"INSERT INTO contracts(from, until, renter_id, property_id, fee) 
                    VALUES (@from, @until, @renter_id, @property_id, @fee);
                    SELECT LAST_INSERT_ID()";
                    Console.WriteLine(sql);
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@renter_id", contract.Renter_id);
                        command.Parameters.AddWithValue("@property_id", contract.Property_id);
                        command.Parameters.AddWithValue("@from", contract.From);
                        command.Parameters.AddWithValue("@until", contract.Until);
                        command.Parameters.AddWithValue("@fee", contract.Fee);
                        connection.Open();
                        id = Convert.ToInt32(command.ExecuteScalar());
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error creating the renter " + ex);
            }

            return id;
        }

        public int delete(int id)
        {
            int result = -1;
            Console.WriteLine("---Deleting contract");
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"DELETE FROM contracts WHERE id=@id";
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
                Console.WriteLine("---Error executing delete contract " + ex);
            }

            return result;
        }

        /*

        public int update(Renter renter)
        {
            int result = -1;
            Console.WriteLine("---Updating renter");
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"UPDATE properties SET dni=@dni, first_name=@first_name, last_name=@last_name, email=@email" + (renter.Phone != string.Empty ? ", phone=@phone" : "") + " WHERE id=@id; SELECT LAST_INSERT_ID()";
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
                Console.WriteLine("---Error updating renter" + ex);
            }

            return result;
        }
        */
    }
}