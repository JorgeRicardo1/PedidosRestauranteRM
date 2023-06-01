using SQLite;
using System;

namespace PedidosRestaurante.Models
{
    public class EmpresasModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Id_mac { get; set; }
        public string Empresa { get; set; }
        public string Nro_mac { get; set; }
        public string Maquina { get; set; }
        public string Factivar { get; set; }
        public string Facceso { get; set; }
        public string Activar { get; set; }
        public DateTime Fsuspende { get; set; }
        public string Eq_notas { get; set; }
        public string Ip_origen { get; set; }
        public string Serieprg { get; set; }
        public string Mensaje { get; set; }
        public string Modulos { get; set; }
        public string Terminal { get; set; }
        public int Grupo { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public string Ipserver { get; set; }
        public string ServerPasword { get; set; }

        //---CAMBIO REALIZADO JORGE 30/05/2023
        public string Error { get; set; }
        //-----------------------------------

    }
}
