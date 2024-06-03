using System;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace WebApplication5
{
    public class EmployeeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<Employee> list = new List<Employee>();
      


        //private readonly string _password;
        public EmployeeMiddleware(RequestDelegate next/*, string password*/)
        {

            //if (string.IsNullOrEmpty(password))
            //{
            //    _password = password;
            //}
            _next = next;

            
            list.Add(new Employee("qwerty1", 30001, 1));
            list.Add(new Employee("qwerty2", 30002, 2));
            list.Add(new Employee("qwerty3", 30003, 3));
            list.Add(new Employee("qwerty4", 30004, 4));

        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path;
            var response = context.Response;
            var request = context.Request;

            if (path == "/api/employees" && request.Method == "GET")
            {
                await GetAllPeople(response);
            }
            
            else if (path == "/api/users" && request.Method == "POST")
            {
                await Create(response, request);
            }
            /*else if (path == "/api/users" && request.Method == "PUT")
            {
                await Update(response, request);
            }*/
            else if (path == "/api/users" && request.Method == "DELETE")
            {
                string? id = path.Value?.Split("/")[3];
                await Delete(int.Parse(id), response);
            }
            else
            {
                
            }

            if (path == "/api/employees" /*&& context.Request.Method == "POST"*/)
            {
                if (context.Request.Method == "POST")
                {
                    var quer = context.Request.Query["token"];
                    if (quer == "employees")
                    {
                        response.StatusCode = 200;
                        await _next.Invoke(context);
                    }
                    else
                    {
                        response.StatusCode = 404;
                    }
                }
            }
        }

        async Task Create(HttpResponse response, HttpRequest request)
        {
            try
            {                
                var emp = await request.ReadFromJsonAsync<Employee>();
                if (emp != null)
                {
                    list.Add(emp);
                    await response.WriteAsJsonAsync(emp);
                }
                else
                {
                    throw new Exception("Некорректные данные");
                }
            }
            catch (Exception)
            {
                response.StatusCode = 400;
                await response.WriteAsJsonAsync(new { message = "Некорректные данные" });
            }
        }


        async Task Update(HttpResponse response, HttpRequest request, int id)
        {
            try
            {
                var temp = await request.ReadFromJsonAsync<Employee>();
                if (temp != null)
                {
                    var emp = list.FirstOrDefault(e => e.Id == temp.Id);
                    if (emp != null)
                    {
                        emp.Name = temp.Name;
                        emp.Salary = temp.Salary;
                    }

                }
            }
            catch 
            {
                response.StatusCode = 400;
            }
        }

        async Task GetAllPeople(HttpResponse response)
        {
            await response.WriteAsJsonAsync(list);
        }

        async Task Delete(int id, HttpResponse response)
        {
            var emp = list.FirstOrDefault(emp => emp.Id == id);
            if (emp != null)
            {
                list.Remove(emp);
                await response.WriteAsJsonAsync(emp);
            }
            else
            {
                response.StatusCode = 404;
                await response.WriteAsJsonAsync(new { message = "Пользователь не найден" });
            }
        }


    }
}

