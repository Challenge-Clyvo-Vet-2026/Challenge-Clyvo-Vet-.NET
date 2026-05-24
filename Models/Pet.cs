namespace Challenge_Clyvo_Vet_DotNet.Models;

public class Pet
{
    public int IdPet { get; set; }
    public int IdResponsavel { get; set; }
    public string NomePet { get; set; } = string.Empty;
    public string EspeciePet { get; set; } = string.Empty;
    public string RacaPet { get; set; } = string.Empty;
    public DateTime DataNascimentoPet { get; set; }
    public string? StatusCastrado { get; set; }
}