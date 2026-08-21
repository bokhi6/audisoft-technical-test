namespace AudiSoft.Domain.Entities;

public class Nota
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int IdEstudiante { get; set; }
    public int IdProfesor { get; set; }
    public decimal Valor { get; set; }

    public Estudiante? Estudiante { get; set; }
    public Profesor? Profesor { get; set; }
}
