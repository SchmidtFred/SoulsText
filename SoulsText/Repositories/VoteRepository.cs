using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using SoulsText.Utils;
using SoulsText.Models;
using Microsoft.Extensions.Logging;
using System;

namespace SoulsText.Repositories
{
    public class VoteRepository : BaseRepository, IVoteRepository
    {
        private readonly ILogger<VoteRepository> _logger;
        public VoteRepository(IConfiguration configuration, ILogger<VoteRepository> logger) : base(configuration) {
            _logger = logger;
        }

        public List<Vote> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, Upvote, UserProfileId, MessageId
                          FROM Vote";

                    var votes = new List<Vote>();

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        votes.Add(VoteFromReader(reader));
                    }

                    reader.Close();

                    return votes;
                }
            }
        }

        public Vote GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, Upvote, UserProfileId, MessageId
                          FROM Vote
                         WHERE Id = @id";

                    DbUtils.AddParameter(cmd, "@id", id);

                    Vote vote = null;

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        vote = VoteFromReader(reader);
                    }

                    reader.Close();

                    return vote;
                }
            }
        }

        public void Add(Vote vote)
        {
            _logger.LogInformation("Adding Vote");
            try
            {
                using (var conn = Connection)
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                        INSERT INTO Vote (Upvote, UserProfileId, MessageId)
                        OUTPUT INSERTED.Id
                        VALUES (@upvote, @userProfileId, @messageId)";

                        DbUtils.AddParameter(cmd, "@upvote", vote.Upvote);
                        DbUtils.AddParameter(cmd, "@userProfileId", vote.UserProfileId);
                        DbUtils.AddParameter(cmd, "@messageId", vote.MessageId);

                        vote.Id = (int)cmd.ExecuteScalar();
                    }
                }
            } catch (Exception ex)
            {
                _logger.LogWarning(ex, "Something went wrong when adding vote");
            }
        }

        public void Update(Vote vote)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Vote
                           SET Upvote = @upvote,
                               UserProfileId = @userProfileId,
                               MessageId = @messageId
                         WHERE Id = @id";

                    DbUtils.AddParameter(cmd, "@id", vote.Id);
                    DbUtils.AddParameter(cmd, "@upvote", vote.Upvote);
                    DbUtils.AddParameter(cmd, "@userProfileId", vote.UserProfileId);
                    DbUtils.AddParameter(cmd, "@messageId", vote.MessageId);

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
                    cmd.CommandText = "DELETE FROM Vote WHERE Id = @id";

                    DbUtils.AddParameter(cmd, "@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Get a Vote object from a data reader object.
        /// </summary>
        /// <param name="reader">A SqlDataReader that has not exhausted it's result set</param>
        /// <returns>A Vote object.</returns>
        private Vote VoteFromReader(SqlDataReader reader)
        {
            return new Vote()
            {
                Id = DbUtils.GetInt(reader, "Id"),
                Upvote = DbUtils.GetBool(reader, "Upvote"),
                UserProfileId = DbUtils.GetNullableInt(reader, "UserProfileId"),
                MessageId = DbUtils.GetInt(reader, "MessageId")
            };
        }
    }
}
