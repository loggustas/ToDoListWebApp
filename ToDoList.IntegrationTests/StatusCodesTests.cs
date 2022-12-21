using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToDoList_DomainModel.ViewModels;
using ToDoList_netaspmvc;
using Xunit;
using ToDoList_DomainModel.Models;
using Flurl.Http;

namespace ToDoList.IntegrationTests
{
    public class StatusCodesTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;
        
        public StatusCodesTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }
        [Theory]
        [InlineData("/")]
        [InlineData("/User/Login")]
        [InlineData("/User/Register")]
        public async Task TestUserPages(string URL)
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync(URL);
            int code = (int)response.StatusCode;
            
            Assert.Equal(200, code);
        }

        [Theory]
        [InlineData("/Home/Index")]
        [InlineData("/Home/Create")]
        [InlineData("/Home/Copy")]
        [InlineData("/Home/Edit")]
        [InlineData("/Home/DeleteList")]
        [InlineData("/List/EditRecord/98")]
        [InlineData("/List/Create/120")]
        [InlineData("/List/ViewFull/98")]
        [InlineData("/List/Index/120")]
        [InlineData("/List/CreateNotification?recordId=98")]
        public async Task TestOtherPages404WithoutLogin(string URL)
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync(URL);
            int code = (int)response.StatusCode;

            Assert.Equal(404, code);
        }

        [Theory]
        [InlineData("/Home/Copy/120")]
        [InlineData("/Home/Create")]
        [InlineData("/Home/Edit/120")]
        [InlineData("/Home/Index")]
        [InlineData("/List/EditRecord/98")]
        [InlineData("/List/Create/120")]
        [InlineData("/List/ViewFull/98")]
        [InlineData("/List/Index/120")]
        [InlineData("/List/CreateNotification?recordId=98")]
        public async Task TestOtherPagesWithLogin(string URL)
        {
            var client = factory.CreateClient();

            var values = new Dictionary<string, string>
            {
                { "Username", "admin" },
                { "Password", "admin" }
            };

            var content = new FormUrlEncodedContent(values);

            var response1 = await client.PostAsync("https://localhost:44352/user/login?Username=admin&Password=admin", content);
            int code1 = (int)response1.StatusCode;
            Assert.Equal(200, code1);


            var response2 = await client.GetAsync(URL);
            int code2 = (int)response2.StatusCode;

            Assert.Equal(200, code2);

        }
    }
}
