using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RonSijm.GoogleDriveMonitor.DAL.Entities;

// ReSharper disable PropertyCanBeMadeInitOnly.Global - Justification: Used by Entity Framework
[Table("GoogleUser")]
public class TokenResponseEntity
{
    [Key]
    public int Id { get; set; }

    public string AccessToken { get; set; }

    public string TokenType { get; set; }

    public long? ExpiresInSeconds { get; set; }
    public string RefreshToken { get; set; }
    public string Scope { get; set; }

    public string IdToken { get; set; }

    public DateTime IssuedUtc { get; set; }
}