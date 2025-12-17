
// ************************************************************************** 
// Práctica 09 – ASP.NET MVC con Entity Framework
// Diana Rodriguez
// Fecha de realización: 10/12/2025
// Fecha de entrega: 17/12/2025
// Resultados:
//   * Se implementó una aplicación web ASP.NET MVC que gestiona asignaturas
//     mediante operaciones CRUD (Crear, Leer, Editar, Eliminar).
//   * Se creó el modelo Asignatura con validaciones usando anotaciones
//     (Required, StringLength, Range) y se agregó lógica para componentes
//     CD, CP y AA, calculando las horas totales según créditos.
//   * Se configuró la capa DAL con la clase GestorData derivada de DbContext,
//     enlazada a la cadena de conexión en Web.config.
//   * Se habilitaron migraciones y se creó la base de datos en LocalDB
//     mediante comandos Enable-Migrations, Add-Migration y Update-Database.
//   * Se desarrollaron vistas Razor (Index, Crear, Editar, Detalles, Borrar)
//     con helpers HTML y validaciones cliente/servidor.
//   * Se implementó el layout con barra de navegación y estilos básicos.
// Conclusiones:
//   * ASP.NET MVC junto con Entity Framework facilita la creación de
//     aplicaciones web estructuradas y escalables.
//   * El uso de migraciones permite mantener la base de datos sincronizada
//     con los cambios en el modelo de forma controlada.
//   * Razor simplifica la integración entre lógica y presentación,
//     permitiendo generar vistas dinámicas con validaciones.
//   * La separación en capas (Modelo, Controlador, Vistas, DAL) mejora la
//     mantenibilidad y organización del código.
// Recomendaciones:
//   * Validar correctamente los datos en el modelo y en las vistas para
//     evitar errores de entrada.
//   * Implementar manejo de excepciones en el controlador para casos como
//     IDs inexistentes o errores de conexión.
//   * Considerar el uso de autenticación y autorización para proteger las
//     operaciones CRUD.
//   * Como mejora futura, integrar persistencia en base de datos externa
//     (SQL Server completo) y agregar paginación y filtros en la vista Index.
//   * Incluir pruebas unitarias para los//   * Incluir pruebas unitarias para los métodos del controlador y la lógica
//     del modelo.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestorAsignaturas.Models;

using System.Data.Entity;

namespace GestorAsignaturas.DAL
{




    public class GestorData : DbContext
    {
        public GestorData() : base("name=GestorData") { }
        public DbSet<Asignatura> Asignaturas { get; set; }

    }
}