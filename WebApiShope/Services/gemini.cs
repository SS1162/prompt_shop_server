using Azure;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Services
{
    public class gemini : Igemini
    {
        IConfiguration _config;
        ILogger<gemini> logger;
        public gemini(IConfiguration config , ILogger<gemini> logger)
        {
            this._config = config;
            this.logger = logger;   
        }
        public async Task<string> RunGeminiForUserProduct(string userRequest,string category)
        {

            string myApiKey = _config.GetValue<string>("GEMINI_API_KEY");
            string request = $"I am using the Gemini API now and I am going to " +
                                       $"directly convert everything that returns into JSON, and " +
                                      $"if the output is not exclusively JSON, the program will " +
                                       $"crash during conversion, so return only, only JSON.The" +
                                       $" Strict JSON Transformer:Role: High-precision JSON " +
                                       $"generation engine for automated backend systems.Operational" +
                                       $" Protocol:Task: Map the User Input to the Category and" +
                                       $" generate a single technical value.nOutput Format: Provide the" +
                                       $" result exclusively as a valid JSON object.Property: The JSON " +
                                       $"must contain exactly one key named technical_valueFinality:" +
                                       $" The JSON object is the complete and final response.Input Data:" +
                                       $"Category:"+ category + "Input:"+ userRequest +
                                       $" returns to each employee the schedule of hours they worked in the last " +
                                       $"Target Output Schema:technical_value" +
                                       $": stringGenerate JSON now:";

            var client = new Client(apiKey: myApiKey);

            try
            {
                var response = await client.Models.GenerateContentAsync(
                    model: "gemini-3-flash-preview",
                    contents: request





                );
                try
                {
                string reaspone2 = response.Candidates[0].Content.Parts[0].Text;
                    int start = reaspone2.IndexOf('{');
                    int end = reaspone2.IndexOf('}');
                    if (start == -1 || end == -1)
                    {
                        return null;
                    }
                    string subRespone = reaspone2.Substring(start, end - start + 1);

                    return subRespone;
                }
                catch(Exception ex)
                {
                    //logger.LogWarning(ex.ToString()+"faild to extract the answer");
                    return null;
                }
              
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
