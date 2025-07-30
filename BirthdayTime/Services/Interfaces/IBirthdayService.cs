using BirthdayTime.Models.Domain;

namespace BirthdayTime.Services.Interfaces;

public interface IBirthdayService
{
    Task<IEnumerable<BirthdayEntry>> GetAllAsync();
    Task<BirthdayEntry?> GetByIdAsync(Guid id);
    Task AddAsync(BirthdayEntry entry);
    Task UpdateAsync(BirthdayEntry entry);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<BirthdayEntry>> GetTodayAsync();
    Task<IEnumerable<BirthdayEntry>> GetUpcomingAsync(int daysAhead = 7);
}
