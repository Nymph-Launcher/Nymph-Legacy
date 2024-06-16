using Microsoft.EntityFrameworkCore;

namespace Nymph.App.Service;

public record Configuration(string Key, string? Value)
{
    public int Id { get; set; }
    public string Key { get; set; } = Key;
    public string? Value { get; set; } = Value;
}

public class ConfigurationContext : DbContext
{
    public DbSet<Configuration> Configurations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=configuration.db");
    }
}

public interface IConfigurationService
{
    string? Get(string key);
    void Set(string key, string? value);
}

public class ConfigurationService : IConfigurationService
{
    private readonly ConfigurationContext _context;

    public ConfigurationService()
    {
        _context = new ConfigurationContext();
        _context.Database.EnsureCreated();
    }

    public string? Get(string key)
    {
        var config = _context.Configurations.FirstOrDefault(c => c.Key == key);
        return config?.Value;
    }

    public void Set(string key, string? value)
    {
        var config = _context.Configurations.FirstOrDefault(c => c.Key == key);
        if (config == null)
        {
            _context.Configurations.Add(new Configuration(Key: key, Value: value));
        }
        else
        {
            config.Value = value;
        }

        _context.SaveChanges();
    }
}