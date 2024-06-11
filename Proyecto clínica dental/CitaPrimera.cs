using System;

class CitaPrimera : Cita
{
    public string ValoracionBucal { get; set; }

    public CitaPrimera(string idCita, Paciente paciente, 
        Doctor doctor, DateTime fecha, string valoracionBucal)
        : base(idCita, paciente, doctor, fecha)
    {
        ValoracionBucal = valoracionBucal;
    }

    public override string ToString()
    {
        return base.ToString() + " - (" + ValoracionBucal + ")";
    }
}