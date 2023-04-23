using MySql.Data.MySqlClient;
using testNetMVC.Models;
using System;
using System.Globalization;

namespace testNetMVC.Repositories
{
    public class ContractRepository
    {
        private readonly string _connecString = "Server=localhost;User=root;Password=password;Database=dotnettest1;SslMode=none";

        private readonly ILogger<ContractRepository>? _logger;
        CultureInfo provider = CultureInfo.InvariantCulture;
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
                    string sql = @"SELECT contracts.id id, since, until, fee, renter_id,
                    property_id, address, rooms,
                    t.label,
                    o.first_name o_first_name, o.last_name o_last_name, o.dni o_dni,
                    renter_id, r.dni r_dni, r.first_name r_first_name, r.last_name r_last_name
                    FROM contracts
                    JOIN properties p ON p.id = contracts.property_id
                    JOIN owners o ON p.owner_id = o.id
                    JOIN types t ON t.id = p.type_id
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
                                    Id = reader["id"] != DBNull.Value ? Int32.Parse(reader["id"].ToString()!) : null,
                                    Address = reader["address"].ToString(),
                                    Rooms = Convert.ToInt32(reader["rooms"]),
                                    PropType = new PropType
                                    {
                                        Label = reader["label"].ToString()
                                    },
                                    Owner = new Owner
                                    {
                                        First_name = reader["o_first_name"].ToString(),
                                        Last_name = reader["o_first_name"].ToString(),
                                        Dni = reader["o_dni"].ToString(),
                                    }
                                },
                                From = Convert.ToDateTime(reader["since"]),
                                Until = Convert.ToDateTime(reader["until"]),
                                Fee = Convert.ToDecimal(reader["fee"]),
                                Renter_id = Int32.Parse(reader["renter_id"].ToString()!),
                                Renter = new Renter
                                {
                                    First_name = reader["r_first_name"].ToString(),
                                    Last_name = reader["r_last_name"].ToString(),
                                    Dni = reader["r_dni"].ToString(),
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
            List<Contract>? contracts = new List<Contract> { };
            Console.WriteLine("---Retriving all contracts");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"SELECT contracts.id id, since, until, fee, renter_id,
                    property_id, address, rooms,
                    t.label,
                    o.first_name o_first_name, o.last_name o_last_name, o.dni o_dni,
                    renter_id, r.dni r_dni, r.first_name r_first_name, r.last_name r_last_name
                    FROM contracts
                    JOIN properties p ON p.id = contracts.property_id
                    JOIN owners o ON p.owner_id = o.id
                    JOIN types t ON t.id = p.type_id
                    JOIN renters r ON r.id = contracts.renter_id;";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        Console.WriteLine("---Executing query: \n\n\t" + command.CommandText);
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
                                    Id = reader["id"] != DBNull.Value ? Int32.Parse(reader["id"].ToString()!) : null,
                                    Address = reader["address"].ToString(),
                                    Rooms = Convert.ToInt32(reader["rooms"]),
                                    PropType = new PropType
                                    {
                                        Label = reader["label"].ToString()
                                    },
                                    Owner = new Owner
                                    {
                                        First_name = reader["o_first_name"].ToString(),
                                        Last_name = reader["o_first_name"].ToString(),
                                        Dni = reader["o_dni"].ToString(),
                                    }
                                },
                                From = Convert.ToDateTime(reader["since"]),
                                Until = Convert.ToDateTime(reader["until"]),
                                Fee = Convert.ToDecimal(reader["fee"]),
                                Renter_id = Int32.Parse(reader["renter_id"].ToString()!),
                                Renter = new Renter
                                {
                                    First_name = reader["r_first_name"].ToString(),
                                    Last_name = reader["r_last_name"].ToString(),
                                    Dni = reader["r_dni"].ToString(),
                                },
                            };

                            contracts.Add(contract);
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error retriving all contracts " + ex);
                return null;
            }

            return contracts;
        }

        public int create(Contract contract)
        {
            int id = -1;

            try
            {
                Console.WriteLine("---Creating contract");

                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string fromDate = Convert.ToDateTime(contract.From.ToString()).ToString("yyyy-MM-dd H:mm:ss"),
                    untilDate = Convert.ToDateTime(contract.Until.ToString()).ToString("yyyy-MM-dd H:mm:ss");
                    string sql = @"INSERT INTO contracts(since, until, renter_id, property_id, fee) 
                    VALUES (@since, @until, @renter_id, @property_id, @fee);
                    SELECT LAST_INSERT_ID()";
                    Console.WriteLine(sql);
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@renter_id", contract.Renter_id);
                        command.Parameters.AddWithValue("@property_id", contract.Property_id);
                        command.Parameters.AddWithValue("@since", fromDate);
                        command.Parameters.AddWithValue("@until", untilDate);
                        command.Parameters.AddWithValue("@fee", contract.Fee);
                        connection.Open();
                        id = Convert.ToInt32(command.ExecuteScalar());
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error creating contract " + ex);
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

        public List<Payment> getAllPayments()
        {
            var payments = new List<Payment>();
            Console.WriteLine("---Retriving all payments");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"SELECT payments.id id, amount, date, number,
                    contract_id, 
                    p.address, p.rooms,
                    t.label,
                    o.first_name o_first_name, o.last_name o_last_name, o.dni o_dni,
                    renter_id, r.dni r_dni, r.first_name r_first_name, r.last_name r_last_name
                    FROM payments
                    JOIN contracts c ON c.id = payments.contract_id
                    JOIN properties p ON p.id = c.property_id
                    JOIN owners o ON p.owner_id = o.id
                    JOIN types t ON t.id = p.type_id
                    JOIN renters r ON r.id = c.renter_id;";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        Console.WriteLine("---Executing query: \n\n\t" + command.CommandText);
                        connection.Open();
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var payment = new Payment
                            {
                                Id = Int32.Parse(reader["id"].ToString()!),
                                Amount = Convert.ToDecimal(reader["amount"]),
                                Date = Convert.ToDateTime(reader["date"]),
                                Number = Convert.ToInt32(reader["number"]),
                                Contract = new Contract
                                {
                                    Id = Int32.Parse(reader["contract_id"].ToString()!),
                                    Property = new Property
                                    {
                                        Address = reader["address"].ToString(),
                                        Rooms = Convert.ToInt32(reader["rooms"]),
                                        PropType = new PropType
                                        {
                                            Label = reader["label"].ToString()
                                        },
                                        Owner = new Owner
                                        {
                                            First_name = reader["o_first_name"].ToString(),
                                            Last_name = reader["o_first_name"].ToString(),
                                            Dni = reader["o_dni"].ToString(),
                                        }
                                    },
                                    Renter = new Renter
                                    {
                                        First_name = reader["r_first_name"].ToString(),
                                        Last_name = reader["r_last_name"].ToString(),
                                        Dni = reader["r_dni"].ToString(),
                                    },
                                }
                            };

                            payments.Add(payment);
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error retriving all payments " + ex);
            }
            return payments;
        }

        public Payment? getPayment(int id)
        {
            Payment? payment = null;
            Console.WriteLine("---Retriving one payment");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"SELECT payments.id id, amount, date, number,
                    contract_id, 
                    p.address, p.rooms,
                    t.label,
                    o.first_name o_first_name, o.last_name o_last_name, o.dni o_dni,
                    renter_id, r.dni r_dni, r.first_name r_first_name, r.last_name r_last_name
                    FROM payments
                    JOIN contracts c ON c.id = payments.contract_id
                    JOIN properties p ON p.id = c.property_id
                    JOIN owners o ON p.owner_id = o.id
                    JOIN types t ON t.id = p.type_id
                    JOIN renters r ON r.id = c.renter_id
                    WHERE payments.id=@id;";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@id", id);
                        Console.WriteLine("---Executing query: \n\n\t" + command.CommandText);
                        connection.Open();
                        var reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            payment = new Payment
                            {
                                Id = Int32.Parse(reader["id"].ToString()!),
                                Amount = Convert.ToDecimal(reader["amount"]),
                                Date = Convert.ToDateTime(reader["date"]),
                                Number = Convert.ToInt32(reader["number"]),
                                Contract = new Contract
                                {
                                    Id = Int32.Parse(reader["contract_id"].ToString()!),
                                    Property = new Property
                                    {
                                        Address = reader["address"].ToString(),
                                        Rooms = Convert.ToInt32(reader["rooms"]),
                                        PropType = new PropType
                                        {
                                            Label = reader["label"].ToString()
                                        },
                                        Owner = new Owner
                                        {
                                            First_name = reader["o_first_name"].ToString(),
                                            Last_name = reader["o_first_name"].ToString(),
                                            Dni = reader["o_dni"].ToString(),
                                        }
                                    },
                                    Renter = new Renter
                                    {
                                        First_name = reader["r_first_name"].ToString(),
                                        Last_name = reader["r_last_name"].ToString(),
                                        Dni = reader["r_dni"].ToString(),
                                    },
                                }
                            };
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error retriving one payment " + ex);
            }
            return payment;
        }
        public List<Payment> getAllPaymentsByContract(int id)
        {
            var payments = new List<Payment>();
            Console.WriteLine("---Retriving all payments");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"SELECT payments.id id, amount, date, number,
                    contract_id, 
                    p.address, p.rooms,
                    t.label,
                    o.first_name o_first_name, o.last_name o_last_name, o.dni o_dni,
                    renter_id, r.dni r_dni, r.first_name r_first_name, r.last_name r_last_name
                    FROM payments
                    JOIN contracts c ON c.id = payments.contract_id
                    JOIN properties p ON p.id = c.property_id
                    JOIN owners o ON p.owner_id = o.id
                    JOIN types t ON t.id = p.type_id
                    JOIN renters r ON r.id = c.renter_id
                    WHERE contracts.id=@id;";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@id", id);
                        Console.WriteLine("---Executing query: \n\n\t" + command.CommandText);
                        connection.Open();
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var payment = new Payment
                            {
                                Id = Int32.Parse(reader["id"].ToString()!),
                                Amount = Convert.ToDecimal(reader["amount"]),
                                Date = Convert.ToDateTime(reader["date"]),
                                Number = Convert.ToInt32(reader["number"]),
                                Contract = new Contract
                                {
                                    Id = Int32.Parse(reader["contract_id"].ToString()!),
                                    Property = new Property
                                    {
                                        Address = reader["address"].ToString(),
                                        Rooms = Convert.ToInt32(reader["rooms"]),
                                        PropType = new PropType
                                        {
                                            Label = reader["label"].ToString()
                                        },
                                        Owner = new Owner
                                        {
                                            First_name = reader["o_first_name"].ToString(),
                                            Last_name = reader["o_first_name"].ToString(),
                                            Dni = reader["o_dni"].ToString(),
                                        }
                                    },
                                    Renter = new Renter
                                    {
                                        First_name = reader["r_first_name"].ToString(),
                                        Last_name = reader["r_last_name"].ToString(),
                                        Dni = reader["r_dni"].ToString(),
                                    },
                                }
                            };

                            payments.Add(payment);
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error retriving all payments " + ex);
            }
            return payments;
        }

        public int createPayment(Payment payment)
        {
            int id = -1;
            int number;
            string date = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss");

            try
            {
                Console.WriteLine("---Creating payment");

                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"
                    SELECT MAX(number) 
                    FROM payments
                    WHERE payments.contract_id=@contract_id";
                    Console.WriteLine(sql);
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@contract_id", payment.Contract_id);
                        connection.Open();
                        number = command.ExecuteScalar() != DBNull.Value ? Convert.ToInt32(command.ExecuteScalar()) + 1 : 1;
                        connection.Close();
                    }
                }

                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"INSERT INTO payments(number, date, amount, contract_id) 
                    VALUES (@number, @date, @amount, @contract_id);
                    SELECT LAST_INSERT_ID()";
                    Console.WriteLine(sql);
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.Parameters.AddWithValue("@number", number);
                        command.Parameters.AddWithValue("@date", date);
                        command.Parameters.AddWithValue("@amount", payment.Amount);
                        command.Parameters.AddWithValue("@contract_id", payment.Contract_id);
                        connection.Open();
                        id = Convert.ToInt32(command.ExecuteScalar());
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("---Error creating payment " + ex);
            }

            return id;
        }

        public int deletePayment(int id)
        {
            int result = -1;
            Console.WriteLine("---Deleting payment");
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connecString))
                {
                    string sql = @"DELETE FROM payments WHERE id=@id";
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
                Console.WriteLine("---Error executing delete payment " + ex);
            }

            return result;
        }

    }
}