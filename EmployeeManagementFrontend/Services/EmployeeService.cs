using EmployeeManagementModels;
using System.Net.Http.Json;
using System.Text.Json;

namespace EmployeeManagementFrontend.Services
{
    public class EmployeeService : BaseService
    {
        public EmployeeService(HttpClient client) : base(client) { }

        public async Task<ResponseModel<IEnumerable<EmployeeModel>>> GetAll() => await SerializeGet<IEnumerable<EmployeeModel>>(employeesURI);

        public async Task<ResponseModel<EmployeeModel>> GetById(int id) => await SerializeGet<EmployeeModel>($"{employeesURI}/{id}");

        public async Task<ResponseModel<EmployeeModel>> Add(EmployeeModel employee) => await SerializePost<EmployeeModel, EmployeeModel>(employeesURI, employee);

        public async Task<ResponseModel<bool>> Delete(int id) => await SerializeDelete<bool>($"{employeesURI}/{id}");

        public async Task<ResponseModel<bool>> Update(EmployeeModel employee) => await SerializePut<bool, EmployeeModel>(employeesURI, employee);

        private static readonly string employeesURI = "api/employees";
    }
}
