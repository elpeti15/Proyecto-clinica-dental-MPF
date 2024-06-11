using System;
using System.Diagnostics;

class Cita : IComparable<Cita>
{
    public string IdCita { get; set; }
    public Paciente Paciente { get; set; }
    public Doctor Doctor { get; set; }
    public DateTime Fecha { get; set; }

    public Cita(string idCita, Paciente paciente, Doctor doctor, DateTime fecha)
    {
        IdCita = idCita;
        Paciente = paciente;
        Doctor = doctor;
        Fecha = fecha;
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
        return IdCita + " - " + Paciente.NombreCompleto 
            + " - " + Doctor.Nombre + " - " + Fecha.ToString("g");
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

