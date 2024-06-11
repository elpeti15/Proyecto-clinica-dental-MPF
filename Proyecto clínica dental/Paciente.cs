using System;
using System.Threading;

class Paciente : IComparable<Paciente>
{
    public string NombreCompleto { get; set; }
    public string Telefono { get; set; }
    public string Direccion { get; set; }
    public bool Alergia { get; set; }

    public Paciente(string nombreCompleto, string telefono, string direccion, bool alergia)
    {
        NombreCompleto = nombreCompleto;
        Telefono = telefono;
        Direccion = direccion;
        Alergia = alergia;
    }

    public int CompareTo(Paciente otro)
    {
        if (NombreCompleto != otro.NombreCompleto)
        {
            return NombreCompleto.ToLower().CompareTo(otro.NombreCompleto.ToLower());
        }
        else if (Direccion != otro.Direccion)
        {
            return Direccion.ToLower().CompareTo(otro.Direccion.ToLower());
        }
        else
        {
            return Telefono.CompareTo(otro.Telefono);
        }
    }

    public bool Contiene(string texto)
    {
        if (NombreCompleto.ToLower().Contains(texto.ToLower())
            || Telefono.Contains(texto))
        {
            return true;
        }
        else
        {
            return false;
        }    
    }

    public override string ToString()
    {
        string alergia2;
        if (Alergia)
            alergia2 = "Con alergia";
        else
            alergia2 = "Sin alergia";

        return NombreCompleto + " - " + Telefono + " - " + Direccion + " (" +  alergia2 + ")";
    }
}

