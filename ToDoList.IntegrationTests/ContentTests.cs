using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ToDoList_netaspmvc;
using Xunit;

namespace ToDoList.IntegrationTests
{
    public class ContentTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public ContentTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;

        }

        [Theory]
        [InlineData("Test1List")]
        [InlineData("Test2List")]
        [InlineData("Test3List")]
        [InlineData("3 tasks")]
        [InlineData("Log out")]
        [InlineData("0 tasks")]
        [InlineData("Perfect description Test1List")]
        [InlineData("No description.")]
        [InlineData("Add new List")]
        [InlineData("Your To Do Lists")]
        [InlineData("Copy")]
        [InlineData("Delete")]
        [InlineData("Edit")]
        [InlineData("View")]
        public async Task TestUserLists(string listTitle)
        {
            var client = factory.CreateClient();

            var values = new Dictionary<string, string>
            {
                { "Username", "admin" },
                { "Password", "admin" }
            };

            var content = new FormUrlEncodedContent(values);

            await client.PostAsync("https://localhost:44352/user/login?Username=admin&Password=admin", content);

            var response2 = await client.GetAsync("/Home/Index");
            var pageContent = await response2.Content.ReadAsStringAsync();
            var contentString = pageContent.ToString();

            Assert.Contains(listTitle, contentString);
        }

        [Theory]
        [InlineData("AdminList1Task1Test")]
        [InlineData("AdminList1Task2Test")]
        [InlineData("AdminList1Task3Test")]
        [InlineData("Add new record to a list")]
        [InlineData("Hide completed")]
        [InlineData("Show due today")]
        [InlineData("Back")]
        [InlineData("Create reminder")]
        [InlineData("Delete")]
        [InlineData("Edit")]
        [InlineData("Full View")]
        [InlineData("Ongoing")]
        [InlineData("Not started")]
        [InlineData("Completed")]
        [InlineData("Due: 2022-12-22")]
        [InlineData("3 tasks")]
        [InlineData("Test1List")]
        [InlineData("Log out")]
        public async Task TestUserListRecords(string recordTitle)
        {
            var client = factory.CreateClient();

            var values = new Dictionary<string, string>
            {
                { "Username", "admin" },
                { "Password", "admin" }
            };

            var content = new FormUrlEncodedContent(values);

            await client.PostAsync("https://localhost:44352/user/login?Username=admin&Password=admin", content);

            var response2 = await client.GetAsync("/List/Index/120");
            var pageContent = await response2.Content.ReadAsStringAsync();
            var contentString = pageContent.ToString();

            Assert.Contains(recordTitle, contentString);
        }

        [Theory]
        [InlineData("AdminList1Task1Test")]
        [InlineData("Ongoing")]
        [InlineData("Test1List")]
        [InlineData("Task1Description")]
        [InlineData("Log out")]
        [InlineData("Task details:")]
        [InlineData("Due: 2022-12-22")]
        [InlineData("Back to List")]
        public async Task TestUserListRecordsFullView(string testContent)
        {
            var client = factory.CreateClient();

            var values = new Dictionary<string, string>
            {
                { "Username", "admin" },
                { "Password", "admin" }
            };

            var content = new FormUrlEncodedContent(values);

            await client.PostAsync("https://localhost:44352/user/login?Username=admin&Password=admin", content);

            var response2 = await client.GetAsync("/List/ViewFull/98");
            var pageContent = await response2.Content.ReadAsStringAsync();
            var contentString = pageContent.ToString();

            Assert.Contains(testContent, contentString);
        }

        [Theory]
        [InlineData("Copy")]
        [InlineData("a List")]
        [InlineData("Name")]
        [InlineData("Description")]
        [InlineData("Back to Home")]
        [InlineData("Log out")]
        public async Task TestUserListCopy(string testContent)
        {
            var client = factory.CreateClient();

            var values = new Dictionary<string, string>
            {
                { "Username", "admin" },
                { "Password", "admin" }
            };

            var content = new FormUrlEncodedContent(values);

            await client.PostAsync("https://localhost:44352/user/login?Username=admin&Password=admin", content);

            var response2 = await client.GetAsync("/Home/Copy/120");
            var pageContent = await response2.Content.ReadAsStringAsync();
            var contentString = pageContent.ToString();

            Assert.Contains(testContent, contentString);
        }

        [Theory]
        [InlineData("Edit")]
        [InlineData("a List")]
        [InlineData("Name")]
        [InlineData("Test1List")]
        [InlineData("Description")]
        [InlineData("Perfect description Test1List")]
        [InlineData("Save")]
        [InlineData("Back to Home")]
        [InlineData("Log out")]

        public async Task TestUserListEdit(string testContent)
        {
            var client = factory.CreateClient();

            var values = new Dictionary<string, string>
            {
                { "Username", "admin" },
                { "Password", "admin" }
            };

            var content = new FormUrlEncodedContent(values);

            await client.PostAsync("https://localhost:44352/user/login?Username=admin&Password=admin", content);

            var response2 = await client.GetAsync("/Home/Edit/120");
            var pageContent = await response2.Content.ReadAsStringAsync();
            var contentString = pageContent.ToString();

            Assert.Contains(testContent, contentString);
        }

        [Theory]
        [InlineData("Create")]
        [InlineData("a List")]
        [InlineData("Name")]
        [InlineData("Description")]
        [InlineData("Back to Home")]
        [InlineData("Log out")]

        public async Task TestUserListCreate(string testContent)
        {
            var client = factory.CreateClient();

            var values = new Dictionary<string, string>
            {
                { "Username", "admin" },
                { "Password", "admin" }
            };

            var content = new FormUrlEncodedContent(values);

            await client.PostAsync("https://localhost:44352/user/login?Username=admin&Password=admin", content);

            var response2 = await client.GetAsync("/Home/Create");
            var pageContent = await response2.Content.ReadAsStringAsync();
            var contentString = pageContent.ToString();

            Assert.Contains(testContent, contentString);
        }

        [Theory]
        [InlineData("Insert")]
        [InlineData("a Task")]
        [InlineData("Title")]
        [InlineData("Description")]
        [InlineData("Due Date")]
        [InlineData("Status")]
        [InlineData("Notes")]
        [InlineData("Create")]
        [InlineData("Back to List")]
        [InlineData("Log out")]
        public async Task TestUserListRecordCreate(string testContent)
        {
            var client = factory.CreateClient();

            var values = new Dictionary<string, string>
            {
                { "Username", "admin" },
                { "Password", "admin" }
            };

            var content = new FormUrlEncodedContent(values);

            await client.PostAsync("https://localhost:44352/user/login?Username=admin&Password=admin", content);

            var response2 = await client.GetAsync("/List/Create/120");
            var pageContent = await response2.Content.ReadAsStringAsync();
            var contentString = pageContent.ToString();

            Assert.Contains(testContent, contentString);
        }

        [Theory]
        [InlineData("Create a Reminder")]
        [InlineData("Date to remind")]
        [InlineData("Note")]
        [InlineData("Create")]
        [InlineData("Back to List")]
        [InlineData("Log out")]
        public async Task TestUserListRecordCreateReminder(string testContent)
        {
            var client = factory.CreateClient();

            var values = new Dictionary<string, string>
            {
                { "Username", "admin" },
                { "Password", "admin" }
            };

            var content = new FormUrlEncodedContent(values);

            await client.PostAsync("https://localhost:44352/user/login?Username=admin&Password=admin", content);

            var response2 = await client.GetAsync("/List/CreateNotification?recordId=98");
            var pageContent = await response2.Content.ReadAsStringAsync();
            var contentString = pageContent.ToString();

            Assert.Contains(testContent, contentString);
        }

        [Theory]
        [InlineData("Edit")]
        [InlineData("a Task")]
        [InlineData("Title")]
        [InlineData("AdminList1Task1Test")]
        [InlineData("Description")]
        [InlineData("Task1Description")]
        [InlineData("Due Date")]
        [InlineData("2022-12-22")]
        [InlineData("Status")]
        [InlineData("Ongoing")]
        [InlineData("Notes")]
        [InlineData("Save")]
        [InlineData("Back to List")]
        [InlineData("Log out")]
        public async Task TestUserListRecordEdit(string testContent)
        {
            var client = factory.CreateClient();

            var values = new Dictionary<string, string>
            {
                { "Username", "admin" },
                { "Password", "admin" }
            };

            var content = new FormUrlEncodedContent(values);

            await client.PostAsync("https://localhost:44352/user/login?Username=admin&Password=admin", content);

            var response2 = await client.GetAsync("/List/EditRecord/98");
            var pageContent = await response2.Content.ReadAsStringAsync();
            var contentString = pageContent.ToString();

            Assert.Contains(testContent, contentString);
        }
    }
}
