using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Homework;

namespace Homework
{
    [TestClass]
    public class HttpClientTest
    {


        private static HttpClient httpClient;

        private static readonly string BaseURL = "https://petstore.swagger.io/v2/";

        private static readonly string UsersEndpoint = "pet";

        private static string GetURL(string enpoint) => $"{BaseURL}{enpoint}";

        private static Uri GetURI(string endpoint) => new Uri(GetURL(endpoint));

        private readonly List<UserModel> cleanUpList = new List<UserModel>();

        [TestInitialize]
        public void TestInitialize()
        {
            httpClient = new HttpClient();
        }

        [TestCleanup]
        public async Task TestCleanUp()
        {
            foreach (var data in cleanUpList)
            {
                var httpResponse = await httpClient.DeleteAsync(GetURL($"{UsersEndpoint}/{data.Id}"));
            }
        }

        [TestMethod]
        public async Task PutMethod()
        {
            // Create Json Object
            List<Category> categories = new List<Category>();
            categories.Add(new Category()
            {
                Id = 1031,
                Name = "string"
            });
            #region create data

            // Create Json Object
            UserModel userData = new UserModel()
            {
                Id = 1031,
                Name = "Garfield",
                Status = "available",
                Category = new Category()
                {
                    Id = 1031,
                    Name = "string"
                },
                Tags = categories,
                PhotoUrls = new string[] { "https://petstore.swagger.io/#/pet/updatePet" },
            };

            // Serialize Content
            var request = JsonConvert.SerializeObject(userData);
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Post Request
            await httpClient.PostAsync(GetURL(UsersEndpoint), postRequest);

            #endregion

            #region get the created pet

            // Get Request
            var getResponse = await httpClient.GetAsync(GetURI($"{UsersEndpoint}/{userData.Id}"));

            // Deserialize Content
            var listUserData = JsonConvert.DeserializeObject<UserModel>(getResponse.Content.ReadAsStringAsync().Result);

            // filter created data
            var createdUserData = listUserData.Id;

            #endregion

            #region send put request to update data

            // Update value of userData
            userData = new UserModel()
            {
                Id = 0115,
                Name = "Winter",
                Status = "Sold",
                Category = new Category()
                {
                    Id = 0115,
                    Name = "string",
                },
                Tags = listUserData.Tags,
                PhotoUrls = new string[] { "https://petstore.swagger.io/#/pet/updatePetsto" },

            };

            // Serialize Content
            request = JsonConvert.SerializeObject(userData);
            postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Put Request
            var httpResponse = await httpClient.PutAsync(GetURL($"{UsersEndpoint}"), postRequest);

            // Get Status Code
            var statusCode = httpResponse.StatusCode;

            #endregion

            #region get updated data

            // Get Request
            getResponse = await httpClient.GetAsync(GetURI($"{UsersEndpoint}/{userData.Id}"));

            // Deserialize Content
            listUserData = JsonConvert.DeserializeObject<UserModel>(getResponse.Content.ReadAsStringAsync().Result);

            // filter created data
            createdUserData = listUserData.Id;
            string updatedName = listUserData.Name;
            string updatedStatus = listUserData.Status;
            var categoryid = listUserData.Category.Id;
            var categoryname = listUserData.Category.Name;
            var photouls = listUserData.PhotoUrls[0];
            var tags = listUserData.Tags[0].Id;
            var tagsname = listUserData.Tags[0].Name;

            #endregion

            #region cleanup data

            // Add data to cleanup list
            cleanUpList.Add(listUserData);

            #endregion


            #region assertion

            Assert.AreEqual(HttpStatusCode.OK, statusCode, "Status code is not equal to 200");
            Assert.AreEqual(userData.Id, createdUserData, "No Matching ID");
            Assert.AreEqual(userData.Name, updatedName, "No match found for name");
            Assert.AreEqual(userData.Status, updatedStatus, "No Matching Status found");

            #endregion



        }
    }


}