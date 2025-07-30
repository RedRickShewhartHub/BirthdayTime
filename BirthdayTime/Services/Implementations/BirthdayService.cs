using BirthdayTime.Data;
using BirthdayTime.Models.Domain;
using BirthdayTime.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BirthdayTime.Services.Implementations;

public class BirthdayService : IBirthdayService
{
    private readonly AppDbContext _context;

    public BirthdayService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BirthdayEntry>> GetAllAsync()
    {
        return await _context.Birthdays.OrderBy(b => b.DateOfBirth.Month).ThenBy(b => b.DateOfBirth.Day).ToListAsync();
    }

    public async Task<BirthdayEntry?> GetByIdAsync(Guid id)
    {
        return await _context.Birthdays.FindAsync(id);
    }

    public async Task AddAsync(BirthdayEntry entry)
    {
        entry.Id = Guid.NewGuid();
        _context.Birthdays.Add(entry);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BirthdayEntry entry)
    {
        _context.Birthdays.Update(entry);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Birthdays.FindAsync(id);
        if (entity != null)
        {
            _context.Birthdays.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<BirthdayEntry>> GetTodayAsync()
    {
        var today = DateTime.Today;
        return await _context.Birthdays
            .Where(b => b.DateOfBirth.Month == today.Month && b.DateOfBirth.Day == today.Day)
            .ToListAsync();
    }

    public async Task<IEnumerable<BirthdayEntry>> GetUpcomingAsync(int daysAhead = 7)
    {
        var today = DateTime.Today;
        var endDate = today.AddDays(daysAhead);

        return await _context.Birthdays
            .Where(b =>
                // День рождения между сегодня и endDate (по месяцу и дню)
                IsBirthdayInRange(b.DateOfBirth, today, endDate))
            .OrderBy(b => b.DateOfBirth.Month).ThenBy(b => b.DateOfBirth.Day)
            .ToListAsync();
    }

    private bool IsBirthdayInRange(DateTime birthday, DateTime start, DateTime end)
    {
        var year = start.Year;
        var birthdayThisYear = new DateTime(year, birthday.Month, birthday.Day);

        if (birthdayThisYear < start)
            birthdayThisYear = birthdayThisYear.AddYears(1);

        return birthdayThisYear >= start && birthdayThisYear <= end;
    }
}
