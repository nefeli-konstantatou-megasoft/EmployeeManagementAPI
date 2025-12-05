using EmployeeManagementModels;
using System.Net.Http.Json;
using System.Text.Json;

namespace EmployeeManagementFrontend.Services
{
    public class DepartmentService : BaseService
    {
        public DepartmentService(HttpClient client) : base(client) { }

        public async Task<ResponseModel<IEnumerable<DepartmentModel>>> GetAll() => await SerializeGet<IEnumerable<DepartmentModel>>(departmentsURI);

        public async Task<ResponseModel<DepartmentModel>> GetById(int id) => await SerializeGet<DepartmentModel>($"{departmentsURI}/{id}");

        public async Task<ResponseModel<DepartmentModel>> Add(DepartmentModel employee) => await SerializePost<DepartmentModel, DepartmentModel>(departmentsURI, employee);

        public async Task<ResponseModel<bool>> Delete(int id) => await SerializeDelete<bool>($"{departmentsURI}/{id}");

        public async Task<ResponseModel<bool>> Update(DepartmentModel employee) => await SerializePut<bool, DepartmentModel>(departmentsURI, employee);

        private static readonly string departmentsURI = "api/departments";
    }
}
