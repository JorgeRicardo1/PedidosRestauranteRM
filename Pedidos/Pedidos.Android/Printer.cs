using PedidosRestaurante.Models;
using PedidosRestaurante.ViewsModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(PedidosRestaurante.Droid.Printer))]

namespace PedidosRestaurante.Droid
{
    public class Printer : IPrinter
    {

        public void Print(string ipAddress, int port, ObservableCollection<ClienteModel> clientes)
        {
            string idPrint = AsignarIDPrint(clientes).GetAwaiter().GetResult().ToString();
            Socket pSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //haciendo conecion con la ip y el puerto
            pSocket.Connect(ipAddress, port);
            List<byte> outputList = new List<byte>();
            outputList.AddRange(System.Text.Encoding.UTF8.GetBytes("------------------OrdersApp---------------------"));
            outputList.Add(0x0A);

            string hora = string.Format("{0,-15}{1,9}{2,5}", "COMANDA", "HORA", DateTime.Now.ToString("h:mm"));

            outputList.AddRange(System.Text.Encoding.UTF8.GetBytes(hora));
            outputList.Add(0x0A);
            outputList.AddRange(System.Text.Encoding.UTF8.GetBytes("------------------------------------------------"));
            outputList.Add(0x0A);
            Console.WriteLine(hora);
            string mesero = string.Format("{0,-8}{1,5}{2,10}{3,5}", idPrint, "", "MESA", clientes[0].Mesa);
            outputList.AddRange(System.Text.Encoding.UTF8.GetBytes(mesero));
            outputList.Add(0x0A);
            Console.WriteLine(mesero);
            outputList.AddRange(System.Text.Encoding.UTF8.GetBytes("================================================"));
            outputList.Add(0x0A);
            string multiple = string.Format("{0,-3}{1,3}{2,8}", "", "Cant ", "DESCRIPCION");
            Console.WriteLine(multiple);
            outputList.AddRange(System.Text.Encoding.UTF8.GetBytes(multiple));
            outputList.Add(0x0A);
            outputList.AddRange(System.Text.Encoding.UTF8.GetBytes("................................................"));
            outputList.Add(0x0A);
            foreach (ClienteModel txt in clientes)
            {
                string multiple3 = string.Format("{0,-3:N1}{1,-6:N0}{2,5}", "#" + txt.Nit, txt.Cantidad, txt.Plato);
                outputList.AddRange(System.Text.Encoding.UTF8.GetBytes(multiple3));
                outputList.Add(0x0A);
                Console.WriteLine(multiple3);
                foreach (var item in txt.ArticulosXCliente)
                {
                    if (item.Cmp_cant > 0)
                    {
                        string comb = string.Format("{0,-3:N1}{1,-6:N1}{2,5}", "", "" + item.Cmp_cant, "" + item.Cmp_nomb);
                        outputList.AddRange(System.Text.Encoding.UTF8.GetBytes(comb));
                        outputList.Add(0x0A);
                        Console.WriteLine(comb);
                    }
                    else
                    {
                        string comb1 = string.Format("{0,-5:N1}{1,-4:N1}{2,4}", "", "", "..." + item.Cmp_nomb);
                        outputList.AddRange(System.Text.Encoding.UTF8.GetBytes(comb1));
                        outputList.Add(0x0A);
                        Console.WriteLine(comb1);
                    }

                }
                if (!string.IsNullOrEmpty(txt.notas))
                {
                    string notas = string.Format("{0,-8:N1}{1,5}", "Nota", txt.notas);
                    outputList.AddRange(System.Text.Encoding.UTF8.GetBytes(notas));
                    outputList.Add(0x0A);
                    Console.WriteLine(notas);
                }
                outputList.AddRange(System.Text.Encoding.UTF8.GetBytes("------------------------------------------------"));
                outputList.Add(0x0A);
            }
            outputList.AddRange(System.Text.Encoding.UTF8.GetBytes("..............www.rmsoft.com.co................."));
            outputList.Add(0x0A);
            outputList.Add(0x0A);
            outputList.Add(0x0A);
            outputList.Add(0x0A);
            outputList.Add(0x0A);
            outputList.Add(0x0A);

            //comando para cortar el papel
            outputList.AddRange(System.Text.Encoding.UTF8.GetBytes(("\x1b" + "\x69")));
            ////enviando el comando a la impresora
            pSocket.Send(outputList.ToArray());
            outputList.Clear();
            //cierra la conexion
            pSocket.Close();

        }

        //------CAMBIO REALIZADO 11/05/2023 JORGE---------
        public async Task<int> AsignarIDPrint(ObservableCollection<ClienteModel> clientes)
        {
            FacturacionViewModel fvm = new FacturacionViewModel();
            await fvm.ElminarLastIdPrint();
            await fvm.AddIdPrint(clientes[0].Mesa);
            
            var idPrint = await fvm.GetIdPrint();
            var idmesa = await fvm.GetIdMesa(clientes[0].Mesa);
            await fvm.UpdatePrintMesa(idmesa, idPrint);
            return idPrint;
            //------------------------------------------------
        }
    }
}