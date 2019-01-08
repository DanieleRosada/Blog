using Dapper;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Data.SqlClient;
using TSAC.Rosada.Blog.Data.Models;

namespace TSAC.Rosada.Blog.Data
{
    public class DataAccess : IDataAccess
    {
        readonly string _connectionString;

        public DataAccess(IConfiguration configuration)
        {
            this._connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Post> GetPosts()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // generare query da management studio
                var query = @"
                    SELECT *
                    FROM [dbo].[Posts]";

                return connection.Query<Post>(query);
            }
        }

        public Post GetPost(int postId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                        SELECT *
                        FROM [dbo].[Posts]
                        WHERE Id = @id";

                return connection.QueryFirstOrDefault<Post>(query, new { id = postId });
            }
        }

        public IEnumerable<Post> GetOwnPost(string userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                        SELECT *
                        FROM [dbo].[Posts]
                        WHERE UserInsert = @id";

                return connection.Query<Post>(query, new { id = userId });
            }
        }

        public int GetPostsCount()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                        SELECT Count(*) FROM [dbo].[Posts]";

                return connection.ExecuteScalar<int>(query);
            }
        }

        public void InsertPost(Post post)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    INSERT INTO [dbo].[Posts]
                               ([Title]
                               ,[Content]
                               ,[UserInsert]
                               ,[PublishedDate]
                               ,[Image])
                         VALUES
                               (@Title,
                                @Content,
                                @UserInsert,
                                @PublishedDate,
                                @Image)";
                connection.Execute(query, post);
            }
        }

        public void UpdatePost(Post post)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    UPDATE [dbo].[Posts] SET
                                [Title] = @Title
                                ,[Content] = @Content
                                ,[DataUpdate] = @DataUpdate
                                ,[UserUpdate] = @UserUpdate
                                ,[PublishedDate] = @PublishedDate
                                ,[Image] = @Image 
                    WHERE Id = @Id";
                connection.Execute(query, post);
            }
        }


        public void DeletePost(int postId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    DELETE FROM [dbo].[Posts]
                    WHERE Id = @id";

                connection.Execute(query, new { id = postId });
            }
        }

        public IEnumerable<Comment> GetComments(int idPhoto) {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    SELECT 
                    Id
                    ,PostId
                    ,Author
                    ,Email
                    ,Comment as CommentText 
                    ,InsertDate
                    FROM [dbo].[Comments]
                    WHERE PostId=@id";

                return connection.Query<Comment>(query, new { id = idPhoto });
            }
        }

        public void InsertComment(Comment comment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    INSERT INTO [dbo].[Comments]
                        ([PostId]
                       ,[Author]
                       ,[Email]
                       ,[Comment])
                 VALUES
                       (@PostId
                        ,@Author
                        ,@Email
                        ,@CommentText)";

                connection.Execute(query, comment);
            }
        }
    }

}
