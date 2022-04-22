using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using SoulsText.Utils;
using SoulsText.Models;

namespace SoulsText.Repositories
{
    public class UserProfileRepository : BaseRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

        public List<UserProfile> GetAll()
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
