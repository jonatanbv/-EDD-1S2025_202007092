using System;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;



unsafe class NodoUsuario
{
    
    //atributos
    public int ID;
    public string Nombres;
    public string Apellidos;
    public string Correo;
    public string Contra;
    public NodoUsuario* Siguiente;

    public NodoUsuario(int id, string nombres, string apellidos, string correo, string contra)
    {
        ID = id;
        Nombres = nombres;
        Apellidos = apellidos;
        Correo = correo;
        Contra = contra;
        Siguiente = null;
    }
}

unsafe class ListaUsuario
{
    private NodoUsuario* cabeza = null;

    public void Agregar(int id,  string nombres, string apellidos, string correo, string contra)
    {
        NodoUsuario* nuevo = (NodoUsuario*)System.Runtime.InteropServices.Marshal.AllocHGlobal(sizeof(NodoUsuario));
        *nuevo = new NodoUsuario(id, nombres, apellidos, correo, contra);

        if (cabeza == null) cabeza = nuevo;
        else
        {
            NodoUsuario* actual = cabeza;
            while (actual->Siguiente != null) actual = actual->Siguiente;
            actual->Siguiente = nuevo;
        }
    }

    public void Mostrar()
    {
        NodoUsuario* actual = cabeza;
        while (actual != null)
        {
            Console.WriteLine($"ID: {actual->ID}, {actual->Nombres}, {actual->Apellidos}, {actual->Correo}");
            actual = actual->Siguiente;
        }
    }
    public void Editar(int id, string nombres, string apellidos, string correo)
    {
        NodoUsuario* actual = cabeza;
        while (actual != null)
        {
            if (actual->ID == id)
            {
                actual->Nombres = nombres;
                actual->Apellidos = apellidos;
                actual->Correo = correo;
                return;
            }
            actual = actual->Siguiente;
        }
    }
}

unsafe struct Vehiculos
{
    public int ID;
    public int ID_Usuario;
    public string Marca;
    public string Modelo;
    public string Placa;
}

unsafe class ListaVehiculos
{
    private class Nodo
    {
        public Vehiculos vehiculos;
        public Nodo* Siguiente;
        public Nodo* Anterior;
    }

    private Nodo* cabeza = null;
    private Nodo* cola = null;

    // Método para agregar vehículos
    public void Agregar(int id, int id_usuario, string marca, string modelo, string placa)
    {
        // Verificar si la memoria fue correctamente reservada antes de usarla
        Nodo* nuevo = (Nodo*)System.Runtime.InteropServices.Marshal.AllocHGlobal(sizeof(Nodo));

        if (nuevo == null)  // Verificación adicional para asegurarse de que la memoria se ha asignado correctamente
        {
            Console.WriteLine("No se pudo asignar memoria para el nuevo nodo.");
            return;
        }

        nuevo->vehiculos = new Vehiculos
        {
            ID = id,
            ID_Usuario = id_usuario, // Fix: Asegúrate de que el ID_Usuario es correcto.
            Marca = marca,
            Modelo = modelo,
            Placa = placa
        };
        nuevo->Siguiente = null;
        nuevo->Anterior = cola;

        if (cola != null)
            cola->Siguiente = nuevo;

        cola = nuevo;

        if (cabeza == null)
            cabeza = nuevo;

        // Verifica si los punteros están correctamente asignados.
        Console.WriteLine($"Vehículo con ID {id} agregado.");
    }

    // Método para mostrar los vehículos
    public void Mostrar()
    {
        if (cabeza == null)
        {
            Console.WriteLine("La lista de vehículos está vacía.");
            return;  // Salir si la lista está vacía
        }

        Nodo* actual = cabeza;
        while (actual != null)
        {
            Console.WriteLine($"ID: {actual->vehiculos.ID}, ID Usuario: {actual->vehiculos.ID_Usuario}, Marca: {actual->vehiculos.Marca}, Modelo: {actual->vehiculos.Modelo}, Placa: {actual->vehiculos.Placa}");
            actual = actual->Siguiente; // Avanzar al siguiente nodo
        }
    }

    // Método para eliminar un vehículo por ID
    public void Eliminar(int id)
    {
        if (cabeza == null)
        {
            Console.WriteLine("La lista de vehículos está vacía.");
            return;
        }

        Nodo* actual = cabeza;
        while (actual != null && actual->vehiculos.ID != id)
        {
            actual = actual->Siguiente;
        }

        if (actual == null)
        {
            Console.WriteLine("Vehículo no encontrado.");
            return;
        }

        if (actual->Anterior != null)
            actual->Anterior->Siguiente = actual->Siguiente;  // Enlazar el anterior con el siguiente nodo

        if (actual->Siguiente != null)
            actual->Siguiente->Anterior = actual->Anterior;  // Enlazar el siguiente con el anterior nodo

        if (actual == cabeza)
            cabeza = actual->Siguiente;  // Si es el primer nodo, actualizar cabeza

        if (actual == cola)
            cola = actual->Anterior;  // Si es el último nodo, actualizar cola

        // Liberar memoria
        System.Runtime.InteropServices.Marshal.FreeHGlobal((IntPtr)actual);
        Console.WriteLine($"Vehículo con ID {id} eliminado.");
    }

    // Método para editar un vehículo por ID
    public void Editar(int id, string nuevaMarca, string nuevoModelo, string nuevaPlaca)
    {
        Nodo* actual = cabeza;
        while (actual != null && actual->vehiculos.ID != id)
        {
            actual = actual->Siguiente;
        }

        if (actual == null)
        {
            Console.WriteLine("Vehículo no encontrado.");
            return;
        }

        // Actualizar los detalles del vehículo
        actual->vehiculos.Marca = nuevaMarca;
        actual->vehiculos.Modelo = nuevoModelo;
        actual->vehiculos.Placa = nuevaPlaca;

        Console.WriteLine($"Vehículo con ID {id} actualizado.");
    }
}




unsafe struct Repuesto
{
    public int ID;
    public string NombreRepuesto;
    public string Detalle;
    public decimal Costo;
}

unsafe class ListaRepuestos
{
    private class Nodo
    {
        public Repuesto repuestos;
        public Nodo* siguiente; // apunta al siguiente nodo
    }

    private Nodo* cabeza = null;

    public void Agregar(int id, string nombreRepuesto, string detalles, decimal costo)
    {
        Nodo* nuevo = (Nodo*)System.Runtime.InteropServices.Marshal.AllocHGlobal(sizeof(Nodo)); // reserva memoria


        nuevo->repuestos = new Repuesto
        {
            ID = id,
            NombreRepuesto=nombreRepuesto,
            Detalle = detalles,
            Costo=costo
        };
        nuevo->siguiente = cabeza;

        if (cabeza == null)
        {
            cabeza = nuevo; // nuevo es cabeza si esta vacia
            nuevo->siguiente = cabeza; // apunta al ultimo 
        }
        else
        {
            Nodo* actual = cabeza;
            while(actual-> siguiente != cabeza)
            {
                actual = actual->siguiente;
            }
            actual->siguiente = nuevo; // el ultimo apunta al nuevo
            nuevo->siguiente = cabeza; // el nuevo nodo apunta a la cabneza
        }
    }

    public void Mostrar()
    {
        if (cabeza == null)
        {
            Console.WriteLine("la lista de repuestos esta vacia");
            return;
        }
        Nodo* actual = cabeza;
        do
        {
            Console.WriteLine($"ID: {actual->repuestos.ID}, Repuesto: {actual->repuestos.NombreRepuesto}, Detalles: {actual->repuestos.Detalle}, Costo: {actual->repuestos.Costo}");
            actual = actual->siguiente;  // Avanzar al siguiente nodo
        } while (actual != cabeza);  // Volver a la cabeza para detenerse cuando se completa el ciclo
    }
}

unsafe class NodoServicios
{
    private static int contadorID = 1;
    public int ID { get; private set; }
    public int ID_Repuesto;
    public int ID_Vehiculo;
    public string Detalles;
    public float Costo;

    public NodoServicios(int id_repuesto, int id_vehiculo, string detalles, float costo)
    {
        ID = contadorID++;
        ID_Repuesto = id_repuesto;
        ID_Vehiculo = id_vehiculo;
        Detalles = detalles;
        Costo = costo;
    }
}


public unsafe struct NodoUsuario<T> where T : unmanaged
{
    public T ID;
    public string Nombre;
    public string Apellidos;
    public string Correo;
    public string Contra;
    public NodoUsuario<T>* Siguiente;
}


public unsafe class ListaUsuario<T> where T : unmanaged
{
    private NodoUsuario<T>* cabeza;

    public ListaUsuario()
    {
        cabeza = null;
    }

    public void Insertar(T id, string nombre, string apellido, string correo, string contra)
    {
        NodoUsuario<T>* nuevo = (NodoUsuario<T>*)Marshal.AllocHGlobal(sizeof(NodoUsuario<T>));
        nuevo->ID = id;
        nuevo->Nombre = nombre;
        nuevo->Apellidos = apellido;
        nuevo->Correo = correo;
        nuevo->Contra = contra;
        nuevo->Siguiente = null;

        if (cabeza == null)
        {
            cabeza = nuevo;
        }
        else
        {
            NodoUsuario<T>* temp = cabeza;
            while (temp->Siguiente != null)
            {
                temp = temp->Siguiente;
            }
            temp->Siguiente = nuevo;
        }
    }

    public void Eliminar(T id)
    {
        if (cabeza == null) return;

        if (cabeza->ID.Equals(id))
        {
            NodoUsuario<T>* temp = cabeza;
            cabeza = cabeza->Siguiente;
            Marshal.FreeHGlobal((IntPtr)temp);
            return;
        }

        NodoUsuario<T>* actual = cabeza;
        while (actual->Siguiente != null && !actual->Siguiente->ID.Equals(id))
        {
            actual = actual->Siguiente;
        }

        if (actual->Siguiente != null)
        {
            NodoUsuario<T>* temp = actual->Siguiente;
            actual->Siguiente = actual->Siguiente->Siguiente;
            Marshal.FreeHGlobal((IntPtr)temp);
        }
    }

    public void Imprimir()
    {
        NodoUsuario<T>* temp = cabeza;
        while (temp != null)
        {
            Console.WriteLine($"ID: {temp->ID}, Nombre: {temp->Nombre}, Apellidos: {temp->Apellidos}, Correo: {temp->Correo}");
            temp = temp->Siguiente;
        }
    }


}




public unsafe struct NodoVehiculo<T> where T : unmanaged
{
    public T ID;
    public int ID_Usuario;
    public string Marca;
    public string Modelo;
    public string Placa;

    public NodoVehiculo<T>* Siguiente;
    public NodoVehiculo<T>* Anterior;

}

public unsafe class ListaVehiculo
{
    private NodoVehiculo<int>* head;
    private NodoVehiculo<int>* tail;

    public ListaVehiculo()
    {
        head = null;
        tail = null;
    }
    public void Insertar(int id, int id_usuario, string marca, string modelo, string placa)
    {
        NodoVehiculo<int>* nuevo = (NodoVehiculo<int>*)Marshal.AllocHGlobal(sizeof(NodoVehiculo<int>));
        nuevo->ID = id;
        nuevo->ID_Usuario = id_usuario;
        nuevo->Marca = marca;
        nuevo->Modelo = modelo;
        nuevo->Placa = placa;
        nuevo->Siguiente = null;
        nuevo->Anterior = null;

        if (head == null)
        {
            head = tail = nuevo;
        }
        else
        {
            tail->Siguiente = nuevo;
            nuevo->Anterior = tail;
            tail = nuevo;
        }
    }


    public void Mostrar()
    {
        NodoVehiculo<int>* actual = head;
        while (actual != null)
        {
            Console.WriteLine($"ID: {actual->ID}, ID Usuario: {actual->ID_Usuario}, Modelo: {actual->Modelo}");
            actual = actual->Siguiente;
        }
    }

}







public unsafe struct NodoRepuestos<T> where T : unmanaged
{
    public T ID;
    public string Repuestos;
    public string Detalles;
    public decimal Costo;
    public NodoRepuestos<T>* Siguiente;
}

public unsafe class ListaRepuestos<T> where T: unmanaged
{
    private NodoRepuestos<int>* head;

    public ListaRepuestos()
    {
        head = null;
    }

    public void Insertar(int id, string repuestos, string detalles, decimal costo)
    {
        NodoRepuestos<int>* nuevo = (NodoRepuestos<int>*)Marshal.AllocHGlobal(sizeof(NodoRepuestos<T>));

        // asignar valores 
        nuevo->ID = id;
        nuevo->Repuestos = repuestos;
        nuevo->Detalles = detalles;
        nuevo->Costo = costo;

        if (head == null)
        {
            head = nuevo;
            head->Siguiente = head;
        }
        else
        {
            NodoRepuestos<int>* temp = head;
            while (temp->Siguiente != head)
            {
                temp = temp->Siguiente;
            }
            temp->Siguiente = nuevo;
            nuevo->Siguiente = head;
        }
    }
    public void Mostrar()
    {
        if (head == null)
        {
            Console.WriteLine("linea vacia");
            return;
        }
        NodoRepuestos<int>* temp = head;
        do
        {
            Console.WriteLine($"ID: {temp->ID}, Repuestos: {temp->Repuestos}, Detalle: {temp->Detalles}");
            temp = temp->Siguiente;
        } while (temp != head);
    }
}


public unsafe struct NodoServicios<T> where T : unmanaged
{
    public T ID;
    public int ID_Repuesto;
    public int ID_Vehiculo;
    public string Detalles;
    public decimal Costo;
    public NodoServicios<T>* Siguiente;
}

public unsafe class ColaServicios<T> where T: unmanaged
{
    private NodoServicios<T>* primero;
    private NodoServicios<T>* ultimo;

    public ColaServicios()
    {
        primero = null;
        ultimo = null;
    }
    public void encolar(T id, int id_repuestos, int id_vehiculo, string detalles, decimal costo)
    {
        NodoServicios<T>* nuevo = (NodoServicios<T>*)Marshal.AllocHGlobal(sizeof(NodoServicios<T>));
        nuevo->ID = id;
        nuevo->ID_Repuesto = id_repuestos;
        nuevo->ID_Vehiculo = id_vehiculo;
        nuevo->Detalles = detalles;
        nuevo->Costo = costo;
        nuevo->Siguiente = null;
        if (ultimo == null)
        {
            primero = nuevo;
            ultimo = nuevo;
        }
        else
        {
            ultimo->Siguiente = nuevo;
            ultimo = nuevo;
        }
    }

    public void Mostrar()
    {
        NodoServicios<T>* temp = primero;
        while(temp != null)
        {
            Console.WriteLine($"ID: {temp->ID}, ID Repuesto: {temp->ID_Repuesto}, ID Vehiculo: {temp->ID_Vehiculo}, Detalle: {temp->Detalles}, Costo: {temp->Costo}");
            temp = temp->Siguiente;
        }
    }
}


public unsafe struct NodoFactura<T> where T : unmanaged
{
    public T ID;
    public int ID_Orden;
    public Decimal Total;
    public NodoFactura<T>* Siguiente;
}

public unsafe class PilaFacturas<T> where T: unmanaged
{
    private NodoFactura<T>* top;
    public PilaFacturas()
    {
        top = null;
    }

    public void push(T id, int id_orden, decimal total)
    {
        NodoFactura<T>* nuevo = (NodoFactura<T>*)Marshal.AllocHGlobal(sizeof(NodoFactura<T>));
        nuevo->ID = id;
        nuevo->ID_Orden = id_orden;
        nuevo->Total = total;
        nuevo->Siguiente = top;
        top = nuevo;
    }

    public void Print()
    {
        NodoFactura<T>* temp = top;
        while (temp != null)
        {
            Console.WriteLine($"ID: {temp->ID}, ID Orden: {temp->ID_Orden}, Total: {temp->Total}");
            temp = temp->Siguiente;
        }
        Console.WriteLine("Ya no hay mas facturas");
    }
}








class Persona
{
    public int ID { get; set; }
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public string Correo { get; set; }
    public string Contra { get; set; }
     
}

class VehiculoList {
    public int ID {get;set;}
    public int ID_Usuario {get;set;}
    public string Marca {get;set;}
    public string Modelo {get;set;}
    public string Placa {get;set;}
}


class RepuestosList {
    public int ID {get;set;}
    public string Repuestos {get;set;}
    public string Detalles {get;set;}
    public decimal Costo {get;set;}
}









unsafe class Program
{

    

    static void Main()
    {
        ListaUsuario<int> listaUsuariosAgregados = new ListaUsuario<int>();
        ListaVehiculos listaVehiculosAgregados = new ListaVehiculos();
        ListaRepuestos<int> listaRepuestosAgregados = new ListaRepuestos<int>();


        Console.Write("Enter username: ");
        string username = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = Console.ReadLine();


        if(Login(username, password))
        {
            bool bandera = true;

            while (bandera)
            {
                Console.WriteLine("Welcome :D");

                Console.WriteLine("Opciones\n1.Carga Masiva.\n2.Ingreso Manual\n3.Gestion de usuarios\n4.Generar Servicio\n5.Generar Factura\n6. Salir");
                Console.Write("Ingresa la opcion: ");
                int opcion = Convert.ToInt32(Console.ReadLine());

                switch (opcion)
                {
                    case 1:
                        Console.WriteLine("============ Realiza una carga Masiva====================");
                        Console.WriteLine("Opciones\n1.Usuarios.\n2.Vehiculos\n3.Repuestos\n4.Regresar");
                        Console.Write("Ingresa la opcion: ");
                        int opcionCM = Convert.ToInt32(Console.ReadLine());
                        switch (opcionCM)
                        {
                            case 1:

                            Console.Write("Ingresa el nombre de tu archivo: ");
                            string archi = Console.ReadLine();

                                string filePath = "/home/ubuntu/Desktop/proyecto/MiProyectoEdd/"+archi ;

                                try
                                {
                                    string jsonText = File.ReadAllText(filePath);

                                    // Deserializar el JSON en una lista de objetos Persona
                                    List<Persona> usuarios = JsonConvert.DeserializeObject<List<Persona>>(jsonText);

                                    // Acceder a las personas de diferentes formas
                                    Console.WriteLine("Accediendo a las personas con foreach:");
                                    foreach (var usuario in usuarios)
                                    {

                                        listaUsuariosAgregados.Insertar(usuario.ID, usuario.Nombres, usuario.Apellidos, usuario.Correo, usuario.Contra);
                                    }

                                    

                                    // Usando LINQ para acceder a personas mayores de 30 años
                                    
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error al deserializar: {ex.Message}");
                                }

                                break;
                            case 2:

                            Console.Write("Ingresa el nombre de tu archivo: ");
                            string arch = Console.ReadLine();


                                string filePathVehiculos = "/home/ubuntu/Desktop/proyecto/MiProyectoEdd/"+arch;

                                try
                                {
                                    string jsonText = File.ReadAllText(filePathVehiculos);

                                    // Deserializar el JSON en una lista de objetos Persona
                                    List<VehiculoList> uvehiculos = JsonConvert.DeserializeObject<List<VehiculoList>>(jsonText);

                                    // Acceder a las personas de diferentes formas
                                    //Console.WriteLine("Accediendo a las personas con foreach:");
                                    foreach (var usuario in uvehiculos)
                                    {
                                        listaVehiculosAgregados.Agregar(usuario.ID, usuario.ID_Usuario, usuario.Marca, usuario.Modelo, usuario.Placa);
                                    }

                                    

                                    // Usando LINQ para acceder a personas mayores de 30 años
                                    
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error al deserializar: {ex.Message}");
                                }

                                break;
                            case 3:
                                Console.Write("Ingresa el nombre de tu archivo: ");
                                string arc = Console.ReadLine();


                                string filePathRepuestos= "/home/ubuntu/Desktop/proyecto/MiProyectoEdd/"+arc;

                                try
                                {
                                    string jsonText = File.ReadAllText(filePathRepuestos);

                                    // Deserializar el JSON en una lista de objetos Persona
                                    List<RepuestosList> repuestosA = JsonConvert.DeserializeObject<List<RepuestosList>>(jsonText);

                                    // Acceder a las personas de diferentes formas
                                    //Console.WriteLine("Accediendo a las personas con foreach:");
                                    foreach (var usuario in repuestosA)
                                    {
                                        listaRepuestosAgregados.Insertar(usuario.ID, usuario.Repuestos, usuario.Detalles, usuario.Costo);
                                    }

                                    

                                    // Usando LINQ para acceder a personas mayores de 30 años
                                    
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error al deserializar: {ex.Message}");
                                }
                                break;
                            case 4:
                                Console.WriteLine("Regresando...");
                                break;
                            default:
                                Console.WriteLine("Opcion no valida");
                                break;
                        }
                        break;
                    case 2:
                        Console.WriteLine("============ Realiza una carga manual====================");
                        Console.WriteLine("Opciones\n1.Usuarios.\n2.Vehiculos\n3.Repuestos\n4.Servicios\n5.Regresar");
                        Console.Write("Ingresa la opcion: ");
                        int opcionCMa = Convert.ToInt32(Console.ReadLine());
                        switch (opcionCMa)
                        {
                            case 1:
                                Console.Write("Ingresa el ID: ");
                                int ID_agg = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Ingresa el Nombre: ");
                                string nombre_agg = Console.ReadLine();
                                Console.Write("Ingresa el Apellido: ");
                                string apellido_agg = Console.ReadLine();
                                Console.Write("Ingresa el Correo: ");
                                string correo_agg = Console.ReadLine();
                                Console.Write("Ingresa el Contrasenia: ");
                                string contra_agg = Console.ReadLine();

                                listaUsuariosAgregados.Insertar(ID_agg, nombre_agg, apellido_agg, correo_agg,contra_agg);

                                Console.WriteLine("Agregado con exito!");
                                break;
                            case 2:

                                Console.Write("Ingresa el ID: ");
                                int id_agg = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Ingresa el ID del usuario: ");
                                int id_us = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Ingresa el Marca: ");
                                string marcaagg = Console.ReadLine();
                                Console.Write("Ingresa el Modelo: ");
                                string modeloagg = Console.ReadLine();
                                Console.Write("Ingresa el Placa: ");
                                string placagg = Console.ReadLine();
                                
                                listaVehiculosAgregados.Agregar(id_agg, id_us, marcaagg, modeloagg, placagg);

                                Console.WriteLine("Ingreso Vehiculos con exito!!!!");
                                break;
                            case 3:
                                Console.Write("Ingresa el ID: ");
                                int idrepuesto = Convert.ToInt32(Console.ReadLine());
                                
                                Console.Write("Ingresa el Repuesto: ");
                                string repuestoagg = Console.ReadLine();
                                Console.Write("Ingresa el detalle: ");
                                string detalleagg = Console.ReadLine();
                                Console.Write("Ingresa el Costo: ");
                                decimal costoagg = decimal.Parse(Console.ReadLine());

                                listaRepuestosAgregados.Insertar(idrepuesto, repuestoagg, detalleagg, costoagg);


                                Console.WriteLine("Ingreso Repuestows con exito!!!!");
                                break;
                            case 4:
                                Console.WriteLine("Ingreso manual de servicios con exito!");
                                break;
                            case 5:
                                Console.WriteLine("Regresando...");
                                break;
                            default:
                                Console.WriteLine("Opcion no valida");
                                break;
                        }
                        break;
                    case 3:
                        Console.WriteLine("=======Gestion de usuarios=======");
                        Console.WriteLine("Opciones\n1.editar.\n2.Eliminar\n3.ver\n4.Regresar");

                        Console.Write("Ingresa la opcion: ");
                        int opcionus = Convert.ToInt32(Console.ReadLine());

                        switch(opcionus){
                            case 1:
                                Console.Write("Ingresa el ID del usuario a editar: ");
                                int id_us_edit = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Ingresa el nuevo nombre: ");
                                string nombreedit = Console.ReadLine();
                                break;
                            case 2:
                            case 3:
                            case 4:
                            default:
                                Console.Write("Opcion no valida");
                                break;
                        }


                        break;
                    case 4:
                        Console.WriteLine("Generar Servicios");
                        break;
                    case 5:
                        Console.WriteLine("Generar Factura");
                        break;
                    case 6:
                        Console.WriteLine("Hasta pronto");
                        bandera = false;
                        break;
                    default:
                        Console.WriteLine("Opcion no valida");
                        break;
                }
            }
        }
        else
        {
            Console.WriteLine("I sorry, password incorrect :C");
        }


        
    }

    public static  bool Login(string usser, string passw)
    {
        string userMain = "root@gmail.com";
        string passMain = "root123";

        if (userMain==usser && passw== passMain){
            return true;
        }
        else
        {
            return false;
        }
    }

}
