using DataAccessLayer;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPIWithNetFramworkTemplate.Controllers
{
    /// <summary>
    /// Employee Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class EmployeeController : ApiController
    {
        /// <summary>
        /// The employee database entities
        /// </summary>
        private EmployeeDBEntities employeeDBEntities;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeController"/> class.
        /// </summary>
        public EmployeeController()
        {
            employeeDBEntities = new EmployeeDBEntities();
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Employee> Get()
        {
            return employeeDBEntities.Employees.ToList();
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public HttpResponseMessage Get(int id)
        {
            var entity = employeeDBEntities.Employees.FirstOrDefault(x => x.Id == id);
            if (entity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Employee Id {0} not found", id.ToString()));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
        }
        /// <summary>
        /// Posts the specified employee.
        /// </summary>
        /// <param name="employee">The employee.</param>
        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            employeeDBEntities.Employees.Add(employee);
            employeeDBEntities.SaveChanges();
            var message = Request.CreateResponse(HttpStatusCode.Created, employee);
            message.Headers.Location = new System.Uri(Request.RequestUri + employee.Id.ToString());
            return message;
        }

        /// <summary>
        /// Patches the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="jsonPatchDocument">The json patch document.</param>
        [HttpPatch]
        public void Patch(int id, [FromBody] JsonPatchDocument<Employee> jsonPatchDocument)
        {
            var entity = employeeDBEntities.Employees.FirstOrDefault(x => x.Id == id);

            if (entity != null)
            {
                jsonPatchDocument.ApplyTo(entity);

                employeeDBEntities.SaveChanges();
            }
        }
    }
}
