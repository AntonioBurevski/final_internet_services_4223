using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using midTerm.Data;
using midTerm.Data.Entities;
using System;
using System.Collections.Generic;

namespace midTerm.Service.Test.Internal
{
    public class SqlLiteContext
        : IDisposable
    {
        private const string InMemoryConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;
        protected readonly MidTermDbContext DbContext;

        protected DbContextOptions<MidTermDbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<MidTermDbContext>()
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseSqlite(_connection)
                .Options;
        }
        protected SqlLiteContext(bool withData = false)
        {
            _connection = new SqliteConnection(InMemoryConnectionString);
            DbContext = new MidTermDbContext(CreateOptions());
            _connection.Open();
            DbContext.Database.EnsureCreated();
            if (withData)
                SeedData(DbContext);
        }

        private void SeedData(MidTermDbContext context)
        {
            var question = new List<Question>
            {
                new Question
                {
                    Id = 1,
                    Text = "Question 1?",
                    Description = "Description 1"
                },
                new Question
                {
                    Id = 2,
                    Text = "Question 2?",
                    Description = "Description 2"
                }
            };
            var option = new List<Option>
            {
                new Option
                {
                    Id = 1,
                    Text = "Option 1",
                    Order = 1,
                    QuestionId = 1
                },
                new Option
                {
                    Id = 2,
                    Text = "Option 2",
                    Order = 2,
                    QuestionId = 1
                },
                new Option
                {
                    Id = 3,
                    Text = "Option 3",
                    Order = 3,
                    QuestionId = 2
                },
                new Option
                {
                    Id = 4,
                    Text = "Option 4",
                    Order = 4,
                    QuestionId = 2
                }
            };

            context.AddRange(question);
            context.AddRange(option);
            context.SaveChanges();

        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
