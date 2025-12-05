using EmployeeManagementModels;
using System.Collections.Specialized;
using System.Web;

namespace EmployeeManagementFrontend.Services
{
    public class EmployeeService : BaseService
    {
        public EmployeeService(HttpClient client) : base(client) { }

        public async Task<ResponseModel<IEnumerable<EmployeeModel>>> GetAll(Sorting sorting, Filter filter, Paging paging) {
            UriBuilder uriBuilder = new($"{httpClient.BaseAddress}{employeesURI}");
            NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
            
            if (!String.IsNullOrEmpty(sorting.SortField))
            {
                query[nameof(sorting.SortField)] = sorting.SortField!.ToString();
                query[nameof(sorting.SortOrder)] = sorting.SortOrder! == SortingOrder.Ascending? "0": "1";
            }

            if (!String.IsNullOrEmpty(filter.FilterField) && !String.IsNullOrEmpty(filter.FilterBody))
            {
                query[nameof(filter.FilterField)] = filter.FilterField!.ToString();
                query[nameof(filter.FilterBody)] = filter.FilterBody!.ToString();
            }

            query[nameof(paging.PageIndex)] = paging.PageIndex.ToString();
            query[nameof(paging.PageSize)] = paging.PageSize.ToString();

            return await SerializeGet<IEnumerable<EmployeeModel>>($"{employeesURI}?{query}");
        }

        public async Task<ResponseModel<EmployeeModel>> GetById(int id) => await SerializeGet<EmployeeModel>($"{employeesURI}/{id}");

        public async Task<ResponseModel<EmployeeModel>> Add(EmployeeModel employee) => await SerializePost<EmployeeModel, EmployeeModel>(employeesURI, employee);

        public async Task<ResponseModel<bool>> Delete(int id) => await SerializeDelete<bool>($"{employeesURI}/{id}");

        public async Task<ResponseModel<bool>> Update(EmployeeModel employee) => await SerializePut<bool, EmployeeModel>(employeesURI, employee);

        private static readonly string employeesURI = "api/employees";
    }
}
