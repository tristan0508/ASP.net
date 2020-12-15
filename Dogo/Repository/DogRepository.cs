using Dogo.Models;
using Dogo.Repositories;
using Dogo.Repositories.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;



namespace DogGo.Repositories
{
        public class DogRepository : IDogRepository
        {
            private readonly IConfiguration _config;

            public DogRepository(IConfiguration config)
            {
                _config = config;
            }

            public SqlConnection Connection
            {
                get
                {
                    return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                }
            }

            public List<Dog> GetAllDogs()
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        // Get the dog and the related owner
                        cmd.CommandText = @"
                        SELECT d.Id, d.[Name], d.Breed, d.Notes, d.ImageUrl, d.OwnerId,
	                           o.Name AS OwnerName, o.Address AS OwnerAddress, o.Phone AS OwnerPhone, o.NeighborhoodId AS OwnerNeighborhoodId
	                    FROM Dog d
	                    INNER JOIN Owner o ON d.OwnerId = o.Id
                    ";
                        SqlDataReader reader = cmd.ExecuteReader();

                        List<Dog> dogs = new List<Dog>();

                        while (reader.Read())
                        {
                            Dog dog = new Dog
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Breed = reader.GetString(reader.GetOrdinal("Breed")),
                                Notes = ReaderUtils.GetNullableString(reader, "Notes"),
                                ImageUrl = ReaderUtils.GetNullableString(reader, "ImageUrl"),
                                OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                Owner = new Owner
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                    Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                                    Address = reader.GetString(reader.GetOrdinal("OwnerAddress")),
                                    Phone = reader.GetString(reader.GetOrdinal("OwnerPhone")),
                                    NeighborhoodId = reader.GetInt32(reader.GetOrdinal("OwnerNeighborhoodId"))
                                }
                            };

                            dogs.Add(dog);
                        }

                        reader.Close();

                        return dogs;
                    }
                }
            }

            public Dog GetDogById(int id)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                        SELECT d.Id, d.Name, d.Breed, d.Notes, d.ImageUrl, d.OwnerId, o.Name AS OwnerName
                        FROM Dog d
                        INNER JOIN Owner o ON d.OwnerId = o.Id
                        WHERE d.Id = @id
                    ";

                        cmd.Parameters.AddWithValue("@id", id);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            Dog dog = new Dog
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Breed = reader.GetString(reader.GetOrdinal("Breed")),
                                Notes = ReaderUtils.GetNullableString(reader, "Notes"),
                                ImageUrl = ReaderUtils.GetNullableString(reader, "ImageUrl"),
                                OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                Owner = new Owner
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                    Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                                }
                            };

                            reader.Close();
                            return dog;
                        }
                        else
                        {
                            reader.Close();
                            return null;
                        }
                    }
                }
            }

            public List<Dog> GetDogsByOwnerId(int ownerId)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                SELECT Id, Name, Breed, Notes, ImageUrl, OwnerId 
                FROM Dog
                WHERE OwnerId = @ownerId
            ";

                        cmd.Parameters.AddWithValue("@ownerId", ownerId);

                        SqlDataReader reader = cmd.ExecuteReader();

                        List<Dog> dogs = new List<Dog>();

                        while (reader.Read())
                        {
                            Dog dog = new Dog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Breed = reader.GetString(reader.GetOrdinal("Breed")),
                                Notes = ReaderUtils.GetNullableString(reader, "Notes"),
                                ImageUrl = ReaderUtils.GetNullableString(reader, "ImageUrl"),
                                OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId"))
                            };

                            dogs.Add(dog);
                        }
                        reader.Close();
                        return dogs;
                    }
                }
            }

            public void AddDog(Dog dog)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                    INSERT INTO Dog ([Name], Breed, Notes, ImageUrl, OwnerId)
                    OUTPUT INSERTED.ID
                    VALUES (@name, @breed, @notes, @imageUrl, @ownerId)
                    ";

                        cmd.Parameters.AddWithValue("@name", dog.Name);
                        cmd.Parameters.AddWithValue("@breed", dog.Breed);
                        cmd.Parameters.AddWithValue("@notes", ReaderUtils.GetNullableValue(dog.Notes));
                        cmd.Parameters.AddWithValue("@imageUrl", ReaderUtils.GetNullableValue(dog.ImageUrl));
                        cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);

                        int id = (int)cmd.ExecuteScalar();

                        dog.Id = id;
                    }
                }
            }

            public void UpdateDog(Dog dog)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                        UPDATE Dog
                        SET
                            [Name] = @name,
                            Breed = @breed,
                            Notes = @notes,
                            ImageUrl = @imageUrl,
                            OwnerId = @ownerId
                        WHERE Id = @id
                    ";

                        cmd.Parameters.AddWithValue("@name", dog.Name);
                        cmd.Parameters.AddWithValue("@breed", dog.Breed);
                        cmd.Parameters.AddWithValue("@notes", ReaderUtils.GetNullableValue(dog.Notes));
                        cmd.Parameters.AddWithValue("@imageUrl", ReaderUtils.GetNullableValue(dog.ImageUrl));
                        cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                        cmd.Parameters.AddWithValue("@id", dog.Id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }

            public void DeleteDog(int dogId)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                        DELETE FROM Dog
                        WHERE Id = @id
                    ";

                        cmd.Parameters.AddWithValue("@id", dogId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
