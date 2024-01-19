#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace IConnet.Presale.Domain.Aggregates.Presales;

public class ApprovalOpportunity
{
    public ApprovalOpportunity()
    {
        ApprovalOpportunityId = Guid.NewGuid();
        Applicant = new Applicant();
        Agent = new Agent();
        Regional = new Regional();
    }

    public Guid ApprovalOpportunityId { get; set; }
    public string ApplicationId { get; set; }
    public DateTime ApplicationDate { get; set; }
    public string ApplicationStatus { get; set; }
    public string Service { get; set; }
    public string ApplicationSource { get; set; }
    public string ApplicationType { get; set; }
    public string Splitter { get; set; }

    public Applicant Applicant { get; set; }
    public Agent Agent { get; set; }
    public Regional Regional { get; set; }

    [NotMapped] public TimeSpan OpportunityLifetime => DateTime.Now - ApplicationDate;

    public DateTimeOffset ImportDateTime { get; set; }
    public string ImportClaimName { get; set; }
}

public class Agent
{
    public string FullName { get; set; }
    public string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }
    public string Partner { get; set; }
}

public class Applicant
{
    public string FullName { get; set; }
    public string PlnId { get; set; }
    public string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }
    public string Nik { get; set; }
    public string Npwp { get; set; }
    public string Information { get; set; }
    public string Address { get; set; }
}

public class Regional
{
    public Regional()
    {
        Coordinate = new Coordinate();
    }

    public string Bagian { get; set; }
    public string RepresentativeOffice { get; set; }
    public string Provinsi { get; set; }
    public string Kabupaten { get; set; }
    public string Kecamatan { get; set; }
    public string Kelurahan { get; set; }
    public Coordinate Coordinate { get; set; }
}

public class Coordinate
{
    public string Latitude { get; set; }
    public string Longitude { get; set; }
}