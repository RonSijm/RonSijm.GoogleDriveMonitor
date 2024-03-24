using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Text.Json;

using Google.Apis.Drive.v3.Data;

using RonSijm.GoogleDriveMonitor.DAL.Entities;
using RonSijm.GoogleDriveMonitor.DAL.Extensions;

namespace RonSijm.GoogleDriveMonitor.DAL;

// ReSharper disable UnusedAutoPropertyAccessor.Global - Justification: Used by Entity Framework

public class LocalDataContext : DbContext
{
    public DbSet<ProcessEntity> Process { get; set; }
    public DbSet<TokenResponseEntity> TokenResponses { get; set; }
    public DbSet<File> File { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Permission> Permission { get; set; }

    public DbSet<User> User { get; set; }

    private readonly string _dbPath;
    private readonly string _pathLocation;

    public LocalDataContext(GoogleAccountSettings googleAccountSettings)
    {
        _pathLocation = googleAccountSettings.StorageLocation;

        var accountToUse = googleAccountSettings.AccountToUse + ".sqlite";
        _dbPath = System.IO.Path.Join(googleAccountSettings.StorageLocation, accountToUse);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!System.IO.Directory.Exists(_pathLocation))
        {
            System.IO.Directory.CreateDirectory(_pathLocation);
        }

        options.UseSqlite($"Data Source={_dbPath};foreign keys=true");
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure cascade delete behavior for each relationship
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Cascade;
        }

        var dictionaryValueComparer = new ValueComparer<IDictionary<string, string>>((c1, c2) => c1.CompareTo(c2), c => c.GetHashCode(), c => c.ToDictionary(entry => entry.Key, entry => entry.Value));
        AddJsonProperty<File, IDictionary<string, string>>(p => p.AppProperties, modelBuilder, dictionaryValueComparer);
        AddJsonProperty<File, IDictionary<string, string>>(p => p.ExportLinks, modelBuilder, dictionaryValueComparer);
        AddJsonProperty<File, IDictionary<string, string>>(p => p.Properties, modelBuilder, dictionaryValueComparer);

        modelBuilder.Entity<File>()
            .HasMany(e => e.Owners)
            .WithMany(e => e.Files)
            .UsingEntity("FileOwners");

        modelBuilder.Entity<File>().HasOne(e => e.LastModifyingUser);
        modelBuilder.Entity<File>().HasOne(e => e.SharingUser);
        modelBuilder.Entity<File>().HasOne(e => e.TrashingUser);
    }

    public void EnsureCreated()
    {
        if (!System.IO.File.Exists(_dbPath))
        {
            Database.EnsureCreated();
        }
    }

    private static void AddJsonProperty<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, ModelBuilder modelBuilder, ValueComparer valueComparer) where TEntity : class
    {
        modelBuilder.Entity<TEntity>().Property(propertyExpression).HasConversion(v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null), v => JsonSerializer.Deserialize<TProperty>(v, (JsonSerializerOptions)null)).Metadata.SetValueComparer(valueComparer);
    }
}