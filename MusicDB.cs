using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ADO.Net5
{
    internal class MusicDB
    {
        private string connectionString;

        public MusicDB(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private readonly List<int> predefinedTrackIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };

        public void CreatePlaylist(string playlistName, string category, List<int> trackIds)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    string queryPlaylist = "INSERT INTO Playlists (Name, Category) VALUES (@Name, @Category); SELECT SCOPE_IDENTITY();";
                    SqlCommand cmdPlaylist = new SqlCommand(queryPlaylist, connection, transaction);
                    cmdPlaylist.Parameters.AddWithValue("@Name", playlistName);
                    cmdPlaylist.Parameters.AddWithValue("@Category", category);

                    int playlistId = Convert.ToInt32(cmdPlaylist.ExecuteScalar());

                    foreach (int trackId in trackIds)
                    {
                        if (predefinedTrackIds.Contains(trackId))
                        {
                            string queryTrack = "INSERT INTO PlaylistTracks (PlaylistId, TrackId) VALUES (@PlaylistId, @TrackId)";
                            SqlCommand cmdTrack = new SqlCommand(queryTrack, connection, transaction);
                            cmdTrack.Parameters.AddWithValue("@PlaylistId", playlistId);
                            cmdTrack.Parameters.AddWithValue("@TrackId", trackId);
                            cmdTrack.ExecuteNonQuery();
                        }
                        else
                        {
                            throw new Exception($"Track with Id {trackId} does not exist.");
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error creating playlist: " + ex.Message);
                }
            }
        }
    }
}
