using Dapper;
using GTT.Application;
using GTT.Application.Repositories;
using GTT.Application.ViewModels;
using GTT.Domain.Entities;
using System.Data;

namespace GTT.Infrastructure.Repositories
{
    public class ChallengeRepository : IChallengeRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _tran;

        public ChallengeRepository(IDbConnectionFactory dbConnectionFactory) {
            _connection = dbConnectionFactory.CreateConnection();
            _tran = _connection.BeginTransaction();
        }

        public async Task<ChallengeVM> AddAsync(ChallengeVM challenge)
        {
            
            try{
                var insertChallengeSql = @"INSERT INTO Challenge(Calories, SplatPoints, AvgHR, MaxHR, Miles, Steps, MemberID, CreatedDate, UpdatedDate)
                                        VALUES(@Calories, @SplatPoints, @AvgHR, @MaxHR, @Miles, @Steps, @MemberID, @CreatedDate, @UpdatedDate)
                                        DECLARE @challengeID int
                                        SET @challengeID = SCOPE_IDENTITY()
                                        SELECT* FROM Challenge WHERE challengeID = @ChallengeId
                                        ";

                var param = new
                {
                    Calories = challenge.Calories,
                    SplatPoints = challenge.SplatPoints,
                    AvgHR = challenge.AvgHr,
                    MaxHR = challenge.MaxHr,
                    Miles = challenge.Miles,
                    Steps = challenge.Steps,
                    MemberID = challenge.memberID,
                    CreatedDate = challenge.CreatedDate,
                    UpdatedDate = challenge.UpdatedDate,            
                };

                var result = await _connection.QuerySingleOrDefaultAsync<ChallengeVM>(insertChallengeSql, param, _tran);
                _tran.Commit();
                return result;
            }
            catch (Exception ex)
            {
                _tran.Rollback();
                throw new Exception();
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                var deleteChallengeSql = @"DELETE FROM Challenge WHERE challengeId = @Id";
                var param = new
                {
                    Id = id
                };
                var result = await _connection.ExecuteAsync(deleteChallengeSql, param);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<IReadOnlyList<Challenge>> GetAllAsync()
        {
            try
            {
                var getAllChallengeSql = @"SELECT * FROM Challenge";
                var result = await _connection.QueryAsync<Challenge>(getAllChallengeSql, _tran);

                return result.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IReadOnlyList<ChallengeVM>> GetAllPagingAsync(int pageindex, int pagesize)
        {
            try
            {
                var getAllPagingChallengeSql = @"DECLARE @PageNumber AS INT, @RowspPage AS INT
                                    SET @PageNumber = @pageIndex
                                    SET @RowspPage = @pageSize
                                    SELECT *
                                    FROM Challenge
                                    ORDER BY challengeID
                                    OFFSET ((@PageNumber - 1) * @RowspPage) ROWS
                                    FETCH NEXT @RowspPage ROWS ONLY;";

                var param = new
                {
                    pageIndex = pageindex,
                    pageSize = pagesize
                };

                var result = await _connection.QueryAsync<ChallengeVM>(getAllPagingChallengeSql, param, _tran);

                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<Challenge> GetByIdAsync(int id)
        {
            try
            {
                var getByIDChallengeSql = @"SELECT * FROM Challenge WHERE challengeId = @Id";

                var result = await _connection.QuerySingleOrDefaultAsync<Challenge>(getByIDChallengeSql, new { Id = id }, _tran);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> UpdateAsync(Challenge entity)
        {
            try
            {
                var updateChallengeSql = @"UPDATE Challenge SET something equal something";

                var param = new
                {
                    
                };

                var result = await _connection.ExecuteAsync(updateChallengeSql, param, _tran);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
