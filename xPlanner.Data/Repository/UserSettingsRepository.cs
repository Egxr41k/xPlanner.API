using xPlanner.Domain.Entities;

namespace xPlanner.Data.Repository;

internal class UserSettingsRepository : IRepository<UserSettings>
{
    public Task<UserSettings> Add(UserSettings entity)
    {
        throw new NotImplementedException();
    }

    public Task<UserSettings> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserSettings>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<UserSettings> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<UserSettings> Update(UserSettings entity)
    {
        throw new NotImplementedException();
    }
}
