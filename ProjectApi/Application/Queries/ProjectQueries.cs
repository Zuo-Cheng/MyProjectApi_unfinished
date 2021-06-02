using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Dapper;
//using MySql.Data.MySqlClient;使用MySqlClient需要添加MySql.Data的nuget包

namespace ProjectApi.Application.Queries
{
    public class ProjectQueries : IProjectQueries
    {
        private readonly string _connStr;

        public ProjectQueries(string connStr)
        {
            _connStr = connStr;
        }

        public async Task<dynamic> GetProjectsByUserId(int userId)
        {
            using(var conn  = new SqlConnection(_connStr))
            {
                conn.Open();
                var sql = @"SELECT 
                            Projects.Id,
                            Projects.Avatar,
                            Projects.Company,
                            Projects.FinStage,
                            Projects.Introduction,
                            Projects.ShowSecurityInfo,
                            Projects.CreatedTime
                            FROM Projects WHERE Projects.UserId =@userId";
                var result = await  conn.QueryAsync<dynamic>(sql,new { userId});

                return result;
            }
        }

        public async Task<dynamic> GetProjectDetail(int projectId)
        {
            using(var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                string sql = @"SELECT 
                            Projects.Company,
                            Projects.CityName,
                            Projects.ProvinceName,
                            Projects.AreaName,
                            Projects.FinStage,
                            Projects.FinMoney,
                            Projects.Valuation,
                            Projects.FinPercentage,
                            Projects.Introduction,
                            Projects.UserId,
                            Projects.Income,
                            Projects.Revenue,
                            Projects.Avatar,
                            Projects.BrokerageOptions,
                            ProjectVisibleRules.Tags,
                            ProjectVisibleRules.Visible
                            FROM Projects INNER JOIN ProjectVisibleRules ON
                            Projects.Id = ProjectVisibleRules.ProjectId
                            WHERE Projects.Id = @projectId";
               var result = await conn.QueryAsync(sql,new { projectId});
                return result;
            }
        }
    }
}
