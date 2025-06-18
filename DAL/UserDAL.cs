using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MLMModel;

namespace DAL
{
    public class UserDAL
    {
        private readonly string _connectionString = @"Data Source=SWAPNIL\SQLEXPRESS;Initial Catalog=MLMDb;Integrated Security=True;";

        public User RegisterUser(RegisterDto dto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                       
                        if (dto.SponsorId.HasValue)
                        {
                            string sponsorCheckQuery = "SELECT COUNT(1) FROM Users WHERE Id = @SponsorId";
                            using (var cmd = new SqlCommand(sponsorCheckQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@SponsorId", dto.SponsorId.Value);
                                int sponsorExists = Convert.ToInt32(cmd.ExecuteScalar());
                                if (sponsorExists == 0)
                                    throw new Exception("Invalid Sponsor ID.");
                            }
                        }

                        // 2. Validate email uniqueness
                        string emailCheckQuery = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
                        using (var cmd = new SqlCommand(emailCheckQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Email", dto.Email);
                            int emailExists = Convert.ToInt32(cmd.ExecuteScalar());
                            if (emailExists > 0)
                                throw new Exception("Email already exists.");
                        }

                       
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                      
                        using (var cmd = new SqlCommand("RegisterUser", connection, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@FullName", dto.FullName);
                            cmd.Parameters.AddWithValue("@Email", dto.Email);
                            cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                            cmd.Parameters.AddWithValue("@MobileNumber", dto.MobileNumber);
                            cmd.Parameters.AddWithValue("@SponsorId", dto.SponsorId.HasValue ? (object)dto.SponsorId.Value : DBNull.Value);

                            SqlParameter outParam = new SqlParameter("@NewUserId", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            };
                            cmd.Parameters.Add(outParam);

                            cmd.ExecuteNonQuery();

                            int newUserId = (int)outParam.Value;
                            if (newUserId == 0)
                                throw new Exception("User registration failed.");

                            transaction.Commit();

                            return new User
                            {
                                Id = newUserId,
                                FullName = dto.FullName,
                                Email = dto.Email,
                                PasswordHash = hashedPassword,
                                MobileNumber = dto.MobileNumber,
                                SponsorId = dto.SponsorId,
                                CreatedAt = DateTime.Now
                            };
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public ReferralDto GetReferralTree(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new SqlCommand("GetReferralTree", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new Exception("User not found.");

                        var referralDto = new ReferralDto
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("Id")),
                            FullName = reader.GetString(reader.GetOrdinal("FullName")),
                            Referrals = new List<ReferralDto>()
                        };

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                referralDto.Referrals.Add(new ReferralDto
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("Id")),
                                    FullName = reader.GetString(reader.GetOrdinal("FullName"))
                                });
                            }
                        }

                        return referralDto;
                    }
                }
            }
        }
    }
}
