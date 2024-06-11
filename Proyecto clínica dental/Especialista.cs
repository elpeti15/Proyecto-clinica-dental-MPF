using System;

class Especialista : Doctor
{
    public string Especialidad { get; set; }

    public Especialista (string login,  string password, string nombre, string especialidad)
        : base (login, password, nombre)
    {
        Especialidad = especialidad;
    }

    public override string ToString()
    {
        return base.ToString() + " - (" + Especialidad + ")";
    }
}

