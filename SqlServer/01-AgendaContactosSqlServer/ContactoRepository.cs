using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AgendaContactosSqlServer
{
    internal class ContactoRepository
    {
        private readonly string _connectionString;

        public ContactoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CrearTablaSiNoExiste()
        {
            const string sql = @"
IF OBJECT_ID('Contactos', 'U') IS NULL
BEGIN
    CREATE TABLE Contactos
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Telefono NVARCHAR(30) NOT NULL,
        Email NVARCHAR(100) NOT NULL
    );
END";

            EjecutarComando(sql);
        }

        public void Agregar(Contacto contacto)
        {
            const string sql = "INSERT INTO Contactos (Nombre, Telefono, Email) VALUES (@Nombre, @Telefono, @Email)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Nombre", contacto.Nombre);
                command.Parameters.AddWithValue("@Telefono", contacto.Telefono);
                command.Parameters.AddWithValue("@Email", contacto.Email);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Contacto> ObtenerTodos()
        {
            const string sql = "SELECT Id, Nombre, Telefono, Email FROM Contactos ORDER BY Nombre";
            List<Contacto> contactos = new List<Contacto>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        contactos.Add(new Contacto
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            Email = reader["Email"].ToString()
                        });
                    }
                }
            }

            return contactos;
        }

        public void Eliminar(int id)
        {
            const string sql = "DELETE FROM Contactos WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private void EjecutarComando(string sql)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
