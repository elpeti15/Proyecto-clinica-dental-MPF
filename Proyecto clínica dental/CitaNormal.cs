using System;

class CitaNormal : Cita
{
    public string DescripcionProblema { get; set; }
    public string Tratamiento { get; set; }

    public CitaNormal(string idCita, Paciente paciente, Doctor doctor,
        DateTime fecha, string descripcionProblema, string tratamiento)
        : base(idCita, paciente, doctor, fecha)
    {
        DescripcionProblema = descripcionProblema;
        Tratamiento = tratamiento;
    }

    public override string ToString()
    {
        return base.ToString() + " -Descripción: " + DescripcionProblema
            + " -Tratamiento: " + Tratamiento;
    }
}

