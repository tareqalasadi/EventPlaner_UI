namespace EventPlaner_UI.Helper
{
    public class ManageAPI
    {
        #region Get
        public static async Task<string> GetURI(Uri u)
        {

            var response = string.Empty;
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(u);
                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsStringAsync();
                }
            }
            return response;
        }

        #endregion
        #region Post
        public static async Task<string> PostURI(Uri u, HttpContent c)
        {
            var response = string.Empty;
            HttpResponseMessage result;

            using (var client = new HttpClient())
            {
                result = await client.PostAsync(u, c);

                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsStringAsync();
                }
            }
            return response;
        }
        #endregion

    }
}
