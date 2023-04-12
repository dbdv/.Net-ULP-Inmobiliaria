using MySql.Data.MySqlClient;
using testNetMVC.Models;

namespace testNetMVC.Repositories
{
    public class PropertyRepository
    {
        private readonly string _connecString = "Server=localhost;User=root;Password=password;Database=dotnettest1;SslMode=none";

        private readonly ILogger<PropertyRepository>? _logger;
        public PropertyRepository(ILogger logger)
        {
            _logger = (ILogger<PropertyRepository>)logger;
        }

        public PropertyRepository()
        {
        }

        public Property? get(int id)
        {
            Property? property = null;

            Console.WriteLine("---Retriving one property");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"SELECT props.id id, address, rooms, latitude, longitude, price,
                    purpose_id, description,
                    type_id, label,
                    owner_id, dni, email, first_name, last_name, phone
                    FROM properties props
                    JOIN owners ON owners.id = props.owner_id
                    JOIN purposes ON purposes.id = props.purpose_id
                    JOIN types ON types.id = props.type_id
                    WHERE props.id=@id;";
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
                            property = new Property
                            {
                                Id = reader["id"].ToString() != null ? Int32.Parse(reader["id"].ToString()!) : null,
                                Address = reader["address"].ToString(),
                                Rooms = Convert.ToInt32(reader["rooms"]),
                                Price = Convert.ToDouble(reader["price"]),
                                Latitude = reader["latitude"] != DBNull.Value ? Convert.ToDouble(reader["latitude"]) : null,
                                Longitude = reader["longitude"] != DBNull.Value ? Convert.ToDouble(reader["longitude"]) : null,
                                Owner_id = Convert.ToInt32(reader["owner_id"].ToString()),
                                Owner = new Owner
                                {
                                    Dni = reader["dni"].ToString(),
                                    Email = reader["email"].ToString(),
                                    First_name = reader["first_name"].ToString(),
                                    Last_name = reader["last_name"].ToString(),
                                    Id = Convert.ToInt32(reader["owner_id"].ToString()),
                                    // Phone = reader["phone"].ToString(),
                                },
                                Purpose_id = Convert.ToInt32(reader["purpose_id"].ToString()),
                                Purpose = new Purpose
                                {
                                    Id = Convert.ToInt32(reader["purpose_id"].ToString()),
                                    Description = reader["description"].ToString()
                                },
                                Type_id = Convert.ToInt32(reader["type_id"].ToString()),
                                PropType = new PropType
                                {
                                    Id = Convert.ToInt32(reader["type_id"].ToString()),
                                    Label = reader["label"].ToString(),
                                }
                            };
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error retriving property " + ex);

            }

            return property;
        }

        public List<Property>? getAll()
        {
            List<Property>? properties = new List<Property> { };
            Console.WriteLine("---Retriving all properties");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"SELECT props.id id, address, rooms, latitude, longitude, price,
                    purpose_id, description,
                    type_id, label,
                    owner_id, dni, email, first_name, last_name, phone
                    FROM properties props
                    JOIN owners ON owners.id = props.owner_id
                    JOIN purposes ON purposes.id = props.purpose_id
                    JOIN types ON types.id = props.type_id;";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        if (_logger != null)
                            _logger.LogInformation("hola");
                        connection.Open();
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var property = new Property
                            {
                                Id = reader["id"].ToString() != null ? Int32.Parse(reader["id"].ToString()!) : null,
                                Address = reader["address"].ToString(),
                                Rooms = Convert.ToInt32(reader["rooms"]),
                                Price = Convert.ToDouble(reader["price"]),
                                Latitude = reader["latitude"] != DBNull.Value ? Convert.ToDouble(reader["latitude"]) : null,
                                Longitude = reader["longitude"] != DBNull.Value ? Convert.ToDouble(reader["longitude"]) : null,
                                Owner_id = Convert.ToInt32(reader["owner_id"].ToString()),
                                Owner = new Owner
                                {
                                    Dni = reader["dni"].ToString(),
                                    Email = reader["email"].ToString(),
                                    First_name = reader["first_name"].ToString(),
                                    Last_name = reader["last_name"].ToString(),
                                    Id = Convert.ToInt32(reader["owner_id"].ToString()),
                                    // Phone = reader["phone"].ToString(),
                                },
                                Purpose_id = Convert.ToInt32(reader["purpose_id"].ToString()),
                                Purpose = new Purpose
                                {
                                    Id = Convert.ToInt32(reader["purpose_id"].ToString()),
                                    Description = reader["description"].ToString()
                                },
                                Type_id = Convert.ToInt32(reader["type_id"].ToString()),
                                PropType = new PropType
                                {
                                    Id = Convert.ToInt32(reader["type_id"].ToString()),
                                    Label = reader["label"].ToString(),
                                }
                            };

                            properties.Add(property);
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

        public int create(Property property)
        {
            int id = -1;

            try
            {
                Console.WriteLine("---Creating property");

                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {

                    string sql = @"INSERT INTO properties(purpose_id, type_id, rooms, latitude, longitude, price, owner_id, address) 
                    VALUES (@purpose_id, @type_id, @rooms, @latitude, @longitude, @price, @owner_id, @address);
                    SELECT LAST_INSERT_ID()";
                    Console.WriteLine(sql);
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@purpose_id", property.Purpose_id);
                        command.Parameters.AddWithValue("@type_id", property.Type_id);
                        command.Parameters.AddWithValue("@rooms", property.Rooms);
                        command.Parameters.AddWithValue("@price", property.Price);
                        command.Parameters.AddWithValue("@latitude", property.Latitude != null ? property.Latitude : DBNull.Value);
                        command.Parameters.AddWithValue("@longitude", property.Longitude != null ? property.Longitude : DBNull.Value);
                        command.Parameters.AddWithValue("@owner_id", property.Owner_id);
                        command.Parameters.AddWithValue("@address", property.Address);
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
            Console.WriteLine("---Deleting property");
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"DELETE FROM properties WHERE id=@id";
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
                Console.WriteLine("---Error executing delete property " + ex);
            }

            return result;
        }

        public List<PropType>? getPropTypes()
        {
            List<PropType>? types = new List<PropType> { };
            Console.WriteLine("---Retriving all types");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"SELECT id, label FROM types;";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        connection.Open();
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var type = new PropType
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

        public List<Purpose>? getPurposes()
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

        public List<UnavailableDates> getUnavailableDates(int id)
        {
            List<UnavailableDates> dates = new List<UnavailableDates>();

            Console.WriteLine($"---Retriving unavailable dates for prop with id {id}");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"
                    SELECT since, until
                    FROM contracts
                    JOIN properties ON properties.id = contracts.property_id
                    WHERE properties.id=@id;";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {

                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@id", id);
                        Console.WriteLine("---Executing query: \n\n\t" + command.CommandText.Replace("@id", id.ToString()) + "\n");

                        connection.Open();

                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            var date = new UnavailableDates
                            {
                                From = Convert.ToDateTime(reader["since"]),
                                Until = Convert.ToDateTime(reader["until"]),
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

            return dates;
        }

        public class UnavailableDates
        {
            public DateTime From { get; set; }
            public DateTime Until { get; set; }
        }
    }
}