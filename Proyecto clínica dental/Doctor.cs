using System;

class Doctor : Empleado
{
    public string Nombre { get; set; }

    public Doctor(string login, string password, string nombre)
        : base(login, password)
        
    {
        Nombre = nombre;
        Login = login;
        Password = password;
    }

    public override string ToString()
    {
        return Nombre;
    }
}

