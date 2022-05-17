using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using SoulsText.Utils;
using SoulsText.Models;
using Microsoft.Extensions.Logging;
using System;

namespace SoulsText.Repositories
{
    public class MessageRepository : BaseRepository, IMessageRepository
    {
        private readonly ILogger<MessageRepository> _logger;
        public MessageRepository(IConfiguration configuration, ILogger<MessageRepository> logger) : base(configuration) {
            _logger = logger;
        }

        public List<Message> GetAll()
        {
            _logger.LogInformation("Getting All Messages");
            try
            {
                using (var conn = Connection)
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = MessageQuery;

                        var messages = new List<Message>();

                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            var messageId = DbUtils.GetInt(reader, "MessageId");

                            var existingMessage = messages.FirstOrDefault(m => m.Id == messageId);
                            if (existingMessage == null)
                            {
                                existingMessage = MessageFromReader(reader);
                                existingMessage.UserProfile = ProfileFromReader(reader);
                                existingMessage.Votes = new List<Vote>();

                                messages.Add(existingMessage);
                            }

                            if (DbUtils.IsNotDbNull(reader, "VoteId"))
                            {
                                existingMessage.Votes.Add(VoteFromReader(reader));
                            }

                        }

                        reader.Close();
                        //set vote counts
                        messages.ForEach(m => SetVoteCount(m));

                        return messages;
                    }
                }
            } catch (Exception ex)
            {
                _logger.LogWarning(ex, "Something went wrong when getting all messages.");
                throw;
            }
        }

        public Message GetById(int id)
        {
            _logger.LogInformation($"Getting Message - ID: {id} - By Id");
            try
            {
                using (var conn = Connection)
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = MessageQuery + " WHERE m.Id = @id";

                        DbUtils.AddParameter(cmd, "@id", id);

                        Message message = null;
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            if (message == null)
                            {
                                message = MessageFromReader(reader);
                                message.UserProfile = ProfileFromReader(reader);
                                message.Votes = new List<Vote>();
                            }

                            if (DbUtils.IsNotDbNull(reader, "VoteId"))
                            {
                                message.Votes.Add(VoteFromReader(reader));
                            }
                        }

                        reader.Close();

                        //make sure we set the vote count
                        SetVoteCount(message);

                        return message;
                    }
                }
            } catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Something went wrong when getting Message - ID: {id}");
                throw;
            }
        }

        public void Add(Message message)
        {
            _logger.LogInformation($"User - ID: {message.UserProfileId} - Adding Message To Database");
            try
            {
                using (var conn = Connection)
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            INSERT INTO Message (Content, X, Y, Z, UserProfileId)
                            OUTPUT INSERTED.Id
                            VALUES (@content, @x, @y, @z, @userProfileId)";

                        DbUtils.AddParameter(cmd, "@content", message.Content);
                        DbUtils.AddParameter(cmd, "@x", message.X);
                        DbUtils.AddParameter(cmd, "@y", message.Y);
                        DbUtils.AddParameter(cmd, "@z", message.Z);
                        DbUtils.AddParameter(cmd, "@userProfileId", message.UserProfileId);

                        message.Id = (int)cmd.ExecuteScalar();
                    }
                }
            } catch (Exception ex)
            {
                _logger.LogWarning(ex, "Something went wrong when attempting to add message");
                throw;
            }
        }

        public void Update(Message message)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Message
                               SET Content = @content,
                                   X = @x,
                                   Y = @y, 
                                   Z = @z,
                                   UserProfileId = @userProfileId
                             WHERE Id = @id";

                    DbUtils.AddParameter(cmd, "@id", message.Id);
                    DbUtils.AddParameter(cmd, "@content", message.Content);
                    DbUtils.AddParameter(cmd, "@x", message.X);
                    DbUtils.AddParameter(cmd, "@y", message.Y);
                    DbUtils.AddParameter(cmd, "@z", message.Z);
                    DbUtils.AddParameter(cmd, "@userProfileId", message.UserProfileId);

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
                    cmd.CommandText = "DELETE FROM Message WHERE Id = @id";

                    DbUtils.AddParameter(cmd, "@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private string MessageQuery
        {
            get
            {
                return @"SELECT m.Id AS MessageId, m.Content, m.X, m.Y, m.Z, m.UserProfileId AS MessageUserId,
                                up.UserName,
                                v.Id AS VoteId, v.Upvote, v.UserProfileId AS VoteUserId
                           FROM Message m
                                LEFT JOIN UserProfile up ON m.UserProfileId = up.Id
                                LEFT JOIN Vote v ON v.MessageId = m.Id";
            }
        }

        /// <summary>
        /// Get a Message object from a data reader object.
        /// </summary>
        /// <param name="reader">A SqlDataReader that has not exhausted it's result set</param>
        /// <returns>A Message object.</returns>
        private Message MessageFromReader(SqlDataReader reader)
        {
            return new Message()
            {
                Id = DbUtils.GetInt(reader, "MessageId"),
                Content = DbUtils.GetString(reader, "Content"),
                X = DbUtils.GetFloat(reader, "X"),
                Y = DbUtils.GetFloat(reader, "Y"),
                Z = DbUtils.GetFloat(reader, "Z"),
                UserProfileId = DbUtils.GetInt(reader, "MessageUserId"),
            };
        }

        /// <summary>
        /// Get a UserProfile object from a data reader object.
        /// This method is specifically for use in MessageRepository.
        /// </summary>
        /// <param name="reader">A SqlDataReader that has not exhausted it's result set</param>
        /// <returns>A UserProfile object.</returns>
        private UserProfile ProfileFromReader(SqlDataReader reader)
        {
            return new UserProfile()
            {
                Id = DbUtils.GetInt(reader, "MessageUserId"),
                UserName = DbUtils.GetString(reader, "UserName")
            };
        }

        /// <summary>
        /// Get a Vote object from a data reader object. 
        /// This method is specifially for use in MessageRepository.
        /// </summary>
        /// <param name="reader">A SqlDataReader that has not exhausted it's result set</param>
        /// <returns>A Vote object.</returns>
        private Vote VoteFromReader(SqlDataReader reader)
        {
            return new Vote()
            {
                Id = DbUtils.GetInt(reader, "VoteId"),
                Upvote = DbUtils.GetBool(reader, "Upvote"),
                UserProfileId = DbUtils.GetNullableInt(reader, "VoteUserId"),
                MessageId = DbUtils.GetInt(reader, "MessageId")
            };
        }

        /// <summary>
        /// Set the VoteCount property for a message when it is retrieved 
        /// This method is specifially for use in MessageRepository.
        /// </summary>
        /// <param name="message">A Message object that has not had its VoteCount property set</param>
        private void SetVoteCount(Message message)
        {
            message.Votes.ForEach(v =>
            {
                if (v.Upvote)
                {
                    message.VoteCount++;
                } else
                {
                    message.VoteCount--;
                }
            });
        }
    }
}
