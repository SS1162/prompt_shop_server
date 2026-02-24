using Azure;
using Entities;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTO;
using System.Threading.Tasks;
namespace Services
{
    public class gemini : Igemini
    {
        private readonly IConfiguration _config;
        private readonly ILogger<gemini> logger;

        public gemini(IConfiguration config , ILogger<gemini> logger)
        {
            this._config = config;
            this.logger = logger; 

        }
        public async Task<Resulte<string>> RunGeminiForUserProduct(string userRequest, string category)
        {
            string myApiKey = _config.GetValue<string>("GEMINI_API_KEY");
            
            StringBuilder requestBuilder = new StringBuilder();
  requestBuilder.Append("# Gemini API Processor\n\n");
      requestBuilder.Append("I am using the Gemini API now and I am going to directly convert everything that returns into JSON, and if the output is not exclusively JSON, the program will crash during conversion, so return only, only JSON.\n\n");
     requestBuilder.Append("## The Strict JSON Transformer\n\n");
    requestBuilder.Append("**Role:** High-precision JSON generation engine for automated backend systems.\n\n");
            requestBuilder.Append("**Operational Protocol:**\n");
    requestBuilder.Append("**Task:** Map the User Input to the Category and generate a single technical value.\n\n");
        requestBuilder.Append("## Output Format\n\n");
       requestBuilder.Append("Provide the result exclusively as a valid JSON object.\n\n");
        requestBuilder.Append("**Property:** The JSON must contain exactly one key named `technical_value`\n\n");
      requestBuilder.Append("**Finality:** The JSON object is the complete and final response.\n\n");
         requestBuilder.Append("## Input Data\n\n");
  requestBuilder.Append("**Category:** ");
     requestBuilder.Append(category);
  requestBuilder.Append("\n");
       requestBuilder.Append("**Input:** ");
            requestBuilder.Append(userRequest);
    requestBuilder.Append("\n\n");
            requestBuilder.Append("returns to each employee the schedule of hours they worked in the last\n\n");
        requestBuilder.Append("## Target Output Schema\n\n");
            requestBuilder.Append("```json\n");
    requestBuilder.Append("{ \"technical_value\": \"string\" }\n");
       requestBuilder.Append("```\n\n");
       requestBuilder.Append("**Generate JSON now:**");
   
            string request = requestBuilder.ToString();

            var client = new Client(apiKey: myApiKey);

    try
       {
        var response = await client.Models.GenerateContentAsync(
            model: "gemini-3-flash-preview",
        contents: request,
            config: new GenerateContentConfig()
         {
            Temperature = 0.2f,
          ResponseMimeType = "application/json"
               
            }
             );

                return Resulte<string>.Success(response.Candidates[0].Content.Parts[0].Text);
    }
        catch (Exception ex)
          {
         return Resulte<string>.Failure($"Error: {ex.Message}");
            }
}
        public async Task<Resulte<string>> RunGeminiForFillBasicSite(string userRequest)
        {
            string myApiKey = _config.GetValue<string>("GEMINI_API_KEY");
            
            StringBuilder requestBuilder = new StringBuilder();
           requestBuilder.Append("# Gemini API Processor\n\n");
       requestBuilder.Append("I am using the Gemini API now and I am going to directly convert everything that returns into JSON, and if the output is not exclusively JSON, the program will crash during conversion, so return only, only JSON.\n\n");
 requestBuilder.Append("## The Strict Architectural Transformer\n\n");
   requestBuilder.Append("**Role:** Senior Full-Stack System Architect.\n\n");
       requestBuilder.Append("**Operational Protocol:**\n");
requestBuilder.Append("**Task:** Receive the raw input provided below and expand it into a highly detailed, professional technical definition.\n\n");
  requestBuilder.Append("## Constraints & Rules\n\n");
       requestBuilder.Append("**Language Constraint:** The generated 'technical_value' MUST be written exclusively in ENGLISH.\n\n");
     requestBuilder.Append("**Expansion Rule:** You MUST transform simple terms into high-level engineering specifications including core functionality and system purpose.\n\n");
   requestBuilder.Append("### Example\n\n");
   requestBuilder.Append("If input is 'Sales', output MUST be: 'and payment, You MUST strictly build a fully functional E-commerce application. An online retail platform designed for product listing, shopping cart management, and payment processing.'\n\n");
   requestBuilder.Append("## Output Format\n\n");
    requestBuilder.Append("Provide the result exclusively as a valid JSON object.\n\n");
            requestBuilder.Append("**Property:** The JSON must contain exactly one key named 'technical_value'.\n\n");
  requestBuilder.Append("**Finality:** No conversation. No preamble. Only the raw JSON object.\n\n");
      requestBuilder.Append("## Input Data\n\n");
        requestBuilder.Append("**Input Data to Process:** ");
      requestBuilder.Append(userRequest);
   requestBuilder.Append("\n\n");
        requestBuilder.Append("## Target Output Schema\n\n");
         requestBuilder.Append("```json\n");
    requestBuilder.Append("{ \"technical_value\": \"Detailed system specification string\" }\n");
       requestBuilder.Append("```\n\n");
     requestBuilder.Append("**Generate JSON now:**");
    
            string request = requestBuilder.ToString();

      var client = new Client(apiKey: myApiKey);

try
 {
       var response = await client.Models.GenerateContentAsync(
  model: "gemini-3-flash-preview",
  contents: request,
         config: new GenerateContentConfig()
     {
  Temperature = 0.2f,
      ResponseMimeType = "application/json"
           }
        );

        return Resulte<string>.Success(response.Candidates[0].Content.Parts[0].Text);
       }
   catch (Exception ex)
    {
      return Resulte<string>.Failure($"Error: {ex.Message}");
  }
}
        public async Task<Resulte<string>> RunGeminiForFillCategory(string userRequest, string mainCategory)
        {
      string myApiKey = _config.GetValue<string>("GEMINI_API_KEY");
        
        StringBuilder requestBuilder = new StringBuilder();
     requestBuilder.Append("# Gemini API Processor\n\n");
    requestBuilder.Append("I am using the Gemini API now and I am going to directly convert everything that returns into JSON, and if the output is not exclusively JSON, the program will crash during conversion, so return only, only JSON.\n\n");
  requestBuilder.Append("## The Specialized Extension Architect\n\n");
    requestBuilder.Append("**Role:** Expert UI/UX and Feature Engineer.\n\n");
  requestBuilder.Append("**Operational Protocol:**\n");
      requestBuilder.Append("**Task:** Process the Primary Category as context only, and generate a highly detailed technical expansion for the User Extensions provided.\n\n");
requestBuilder.Append("## Constraints\n\n");
   requestBuilder.Append("**Constraint:** Do NOT describe the general category. Your technical output must focus EXCLUSIVELY on expanding the specific User Extensions into professional instructions.\n\n");
       requestBuilder.Append("**Language Constraint:** The generated 'technical_value' MUST be written in ENGLISH.\n\n");
   requestBuilder.Append("## Expansion Rules\n\n");
   requestBuilder.Append("- **Design Extensions:** Convert simple style requests into deep visual specifications (e.g., negative space, typography, color theory).\n");
     requestBuilder.Append("- **Functional Extensions:** Convert feature requests into implementation logic (e.g., API integration, dynamic routing, component architecture).\n\n");
      requestBuilder.Append("## Output Format\n\n");
     requestBuilder.Append("Provide the result exclusively as a valid JSON object.\n\n");
  requestBuilder.Append("**Property:** The JSON must contain exactly one key named 'technical_value'.\n\n");
    requestBuilder.Append("**Finality:** No preamble. No chat. Only raw JSON.\n\n");
    requestBuilder.Append("## Input Data\n\n");
 requestBuilder.Append("**Context (Category):** ");
requestBuilder.Append(mainCategory);
    requestBuilder.Append("\n\n");
   requestBuilder.Append("**Input to Expand (User Request):** ");
    requestBuilder.Append(userRequest);
  requestBuilder.Append("\n\n");
   requestBuilder.Append("## Target Output Schema\n\n");
  requestBuilder.Append("```json\n");
  requestBuilder.Append("{ \"technical_value\": \"Detailed technical expansion of the extensions ONLY\" }\n");
  requestBuilder.Append("```\n\n");
   requestBuilder.Append("**Generate JSON now:**");

         string request = requestBuilder.ToString();

      var client = new Client(apiKey: myApiKey);

   try
      {
  var response = await client.Models.GenerateContentAsync(
         model: "gemini-3-flash-preview",
   contents: request,
    config: new GenerateContentConfig()
      {
Temperature = 0.2f,
     ResponseMimeType = "application/json"
  }
      );

       return Resulte<string>.Success(response.Candidates[0].Content.Parts[0].Text);
          }
     catch (Exception ex)
             {
        return Resulte<string>.Failure($"Error: {ex.Message}");
             }
        }



    }


}