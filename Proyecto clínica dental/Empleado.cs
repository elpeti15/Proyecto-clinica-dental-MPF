using System;

abstract class Empleado
{
    public string Login { get; set; }
    public string Password { get; set; }

    public Empleado(string login, string password)
    {
        Login = login;
        Password = password;
    }

    public override string ToString()
    {
        return Login + " - " + Password;
    }
}

class Auxiliar : Empleado
{
    public Auxiliar(string login, string password)
        : base(login, password)
    {
        Login = login;
        Password = password;
    }

    public override string ToString()
    {
        return base.ToString();
    }
}

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

class Especialista : Doctor
{
    public string Especialidad { get; set; }

    public Especialista(string login, string password, string nombre, string especialidad)
        : base(login, password, nombre)
    {
        Especialidad = especialidad;
    }

    public override string ToString()
    {
        return base.ToString() + " - (" + Especialidad + ")";
    }
}