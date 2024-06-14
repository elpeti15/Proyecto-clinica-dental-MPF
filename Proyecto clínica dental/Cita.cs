using System;
using System.Diagnostics;

abstract class Cita : IComparable<Cita>
{
    public string IdCita { get; set; }
    public Paciente Paciente { get; set; }
    public Doctor Doctor { get; set; }
    public DateTime Fecha { get; set; }
    public bool Cancelada { get; set; }

    public Cita(string idCita, Paciente paciente, Doctor doctor, DateTime fecha, bool cancelada)
    {
        IdCita = idCita;
        Paciente = paciente;
        Doctor = doctor;
        Fecha = fecha;
        Cancelada = cancelada;
    }

    public int CompareTo(Cita otra)
    {
        if (Fecha != otra.Fecha)
        {
            return Fecha.CompareTo(otra.Fecha);
        }
        else
        {
            return 0;
        }
    }

    public override string ToString()
    {
        return Paciente.NombreCompleto + " - " + Fecha.ToString("g");
    }

    public bool Contiene(string texto)
    {
        if (Paciente.NombreCompleto.ToUpper().Contains(texto.ToUpper())
            || Doctor.Nombre.ToUpper().Contains(texto.ToUpper()))
            return true;
        else
            return false;
    }
}

class CitaNormal : Cita
{
    public string DescripcionProblema { get; set; }
    public string Tratamiento { get; set; }

    public CitaNormal(string idCita, Paciente paciente, Doctor doctor,
        DateTime fecha, bool cancelada, string descripcionProblema, string tratamiento)
        : base(idCita, paciente, doctor, fecha, cancelada)
    {
        DescripcionProblema = descripcionProblema;
        Tratamiento = tratamiento;
    }

    public override string ToString()
    {
        return base.ToString() + " - " + DescripcionProblema
            + " (" + Tratamiento + ")";
    }
}

class CitaPrimera : Cita
{
    public string ValoracionBucal { get; set; }

    public CitaPrimera(string idCita, Paciente paciente,
        Doctor doctor, DateTime fecha, bool cancelada, string valoracionBucal)
        : base(idCita, paciente, doctor, fecha, cancelada)
    {
        ValoracionBucal = valoracionBucal;
    }

    public override string ToString()
    {
        return base.ToString() + " (" + ValoracionBucal + ")";
    }
}