using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using SoulsText.Utils;
using SoulsText.Models;
using Microsoft.Extensions.Logging;
using System;

namespace SoulsText.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        private readonly ILogger _logger;
        public UserProfileRepository(IConfiguration configuration, ILogger logger) : base(configuration) { 
            _logger = logger;
        }

        public List<UserProfile> GetAll()
        {
            _logger.LogInformation("Getting All Users");
            try
            {
                using (var conn = Connection)
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            SELECT Id, [UserName]
                              FROM UserProfile";

                        List<UserProfile> profiles = new List<UserProfile>();

                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            profiles.Add(ProfileFromReader(reader));
                        }
                        reader.Close();

                        return profiles;
                    }
                }
            } catch (Exception ex)
            {
                _logger.LogWarning(ex, "Something went wrong when attempting to get all users");
                throw;
            }
        }

        public UserProfile GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT Id, [UserName]
                              FROM UserProfile
                             WHERE Id = @id";
                    DbUtils.AddParameter(cmd, "@id", id);

                    UserProfile profile = null;
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        profile = ProfileFromReader(reader);
                    }

                    reader.Close();

                    return profile;
                }
            }
        }

        public void Add(UserProfile profile)
        {
            _logger.LogInformation("Adding New User");
            try
            {
                using (var conn = Connection)
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            INSERT INTO UserProfile ([UserName])
                            OUTPUT INSERTED.Id
                            VALUES (@userName)";
                        DbUtils.AddParameter(cmd, "@userName", profile.UserName);

                        profile.Id = (int)cmd.ExecuteScalar();
                    }
                }
            } catch (Exception ex)
            {
                _logger.LogWarning(ex, "Something went wrong when attempting to add new user");
                throw;
            }
        }

        public void Update(UserProfile profile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE UserProfile
                               SET UserName = @userName
                             WHERE Id = @id";

                    DbUtils.AddParameter(cmd, "@id", profile.Id);
                    DbUtils.AddParameter(cmd, "@userName", profile.UserName);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM UserProfile WHERE Id = @id";

                    DbUtils.AddParameter(cmd, "@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Get a UserProfile object from a data reader object.
        /// </summary>
        /// <param name="reader">A SqlDataReader that has not exhausted it's result set</param>
        /// <returns>A UserProfile object.</returns>
        private UserProfile ProfileFromReader(SqlDataReader reader)
        {
            return new UserProfile()
            {
                Id = DbUtils.GetInt(reader, "Id"),
                UserName = DbUtils.GetString(reader, "UserName")
            };
        }
    }
}
