
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



using GestorAsignaturas.DAL;
using GestorAsignaturas.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GestorAsignaturas.Controllers
{
    public class AsignaturaController : Controller
    {
        // Contexto de datos
        private readonly GestorData bd = new GestorData();

        // -------------------------
        // Helper: Validar y calcular Horas
        // -------------------------
        private void ValidarYCalcularHoras(Asignatura a)
        {
            // Área por defecto si viene vacía
            if (string.IsNullOrWhiteSpace(a.Area))
                a.Area = "sin área";

            // Horas esperadas por créditos (regla del enunciado)
            var horasEsperadas = (a.Creditos == 0) ? 2 : a.Creditos * 3;

            // Reglas de componentes
            if (a.CD <= 0)
                ModelState.AddModelError(nameof(a.CD), "CD debe ser al menos 1 (no puede ser 0).");

            if (a.AA <= 0)
                ModelState.AddModelError(nameof(a.AA), "AA debe ser al menos 1 (no puede ser 0).");

            if (a.CP < 0)
                ModelState.AddModelError(nameof(a.CP), "CP no puede ser negativo.");

            var suma = a.CD + a.CP + a.AA;

            // Suma vs horas esperadas
            if (suma != horasEsperadas)
                ModelState.AddModelError("", $"La suma CD({a.CD}) + CP({a.CP}) + AA({a.AA}) debe ser {horasEsperadas} para {a.Creditos} crédito(s).");

            // Caso especial créditos=0 → CD=2, CP=0, AA=0
            if (a.Creditos == 0 && !(a.CD == 2 && a.CP == 0 && a.AA == 0))
                ModelState.AddModelError("", "Con 0 créditos: Horas=2 distribuidas como CD=2, CP=0, AA=0.");

            // Asignar Horas calculadas
            a.Horas = suma;
        }

        // -------------------------
        // GET: Asignatura
        // -------------------------
        public ActionResult Index()
        {
            return View(bd.Asignaturas.ToList());
        }

        // -------------------------
        // GET: Asignatura/Detalles/5
        // -------------------------
        public ActionResult Detalles(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var asignatura = bd.Asignaturas.Find(id);
            if (asignatura == null)
                return HttpNotFound();

            return View(asignatura);
        }

        // -------------------------
        // GET: Asignatura/Crear
        // -------------------------
        public ActionResult Crear()
        {
            return View();
        }

        // -------------------------
        // POST: Asignatura/Crear
        // -------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "ID,Nombre,Codigo,Creditos,CD,CP,AA,Area")] Asignatura asignatura)
        {
            // Calcula/valida antes de guardar
            ValidarYCalcularHoras(asignatura);

            if (ModelState.IsValid)
            {
                bd.Asignaturas.Add(asignatura);
                bd.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(asignatura);
        }

        // -------------------------
        // GET: Asignatura/Editar/5
        // -------------------------
        public ActionResult Editar(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var asignatura = bd.Asignaturas.Find(id);
            if (asignatura == null)
                return HttpNotFound();

            return View(asignatura);
        }

        // -------------------------
        // POST: Asignatura/Editar/5
        // -------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "ID,Nombre,Codigo,Creditos,CD,CP,AA,Area")] Asignatura asignatura)
        {
            // Calcula/valida antes de actualizar
            ValidarYCalcularHoras(asignatura);

            if (ModelState.IsValid)
            {
                bd.Entry(asignatura).State = EntityState.Modified;
                bd.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(asignatura);
        }

        // -------------------------
        // GET: Asignatura/Borrar/5
        // -------------------------
        public ActionResult Borrar(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var asignatura = bd.Asignaturas.Find(id);
            if (asignatura == null)
                return HttpNotFound();

            return View(asignatura);
        }

        // -------------------------
        // POST: Asignatura/Borrar/5
        // -------------------------
        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarBorrar(int id)
        {
            var asignatura = bd.Asignaturas.Find(id);
            if (asignatura == null)
                return HttpNotFound();

            bd.Asignaturas.Remove(asignatura);
            bd.SaveChanges();
            return RedirectToAction("Index");
        }

        // -------------------------
        // Limpieza
        // -------------------------
        protected override void Dispose(bool disposing)
        {
            if (disposing) bd.Dispose();
            base.Dispose(disposing);
        }
    }
}
