using Entities;
using Entities;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using Repositories;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Services
{
    public class CreatePrompt : ICreatePrompt
    {
        private readonly IOrdersReposetory _ordersReposetory;
        private readonly IBasicSitesReposetory _basicSitesReposetory;
        private readonly IGeminiPromptsReposetory _geminiPromptReposetory;
        private readonly ISiteTypesRepository _siteTypesRepository;
        private readonly IPlatformsReposetory _platformsReposetory;
        public CreatePrompt(IOrdersReposetory ordersReposetory, IBasicSitesReposetory basicSitesReposetory
            , IGeminiPromptsReposetory geminiPromptsReposetory, ISiteTypesRepository siteTypesRepository,
            PlatformsReposetory platformsReposetory)
        {
            _ordersReposetory = ordersReposetory;
            _basicSitesReposetory = basicSitesReposetory;
            _geminiPromptReposetory = geminiPromptsReposetory;
            _siteTypesRepository = siteTypesRepository;
            _platformsReposetory = platformsReposetory;
        }
        public async Task<string> Prompt(long orderId)
        {
            IEnumerable<OrdersItem> list = await _ordersReposetory.BringsAllPromptsReposetory(orderId);
            Order order = await _ordersReposetory.GetOrderByIdReposetory(orderId);
            BasicSite basicSite = await _basicSitesReposetory.GetByIDBasicSiteReposetory(order.BasicId);

            StringBuilder promptBuilder = new StringBuilder();

            promptBuilder.Append("# SYSTEM INSTRUCTION: ALL-IN-ONE FULL STACK ENGINE\n\n");
            promptBuilder.Append("You are a highly advanced full-stack web developer acting as a Strict Compiler. Your mission is to generate a complete, production-ready web application in one single response.\n\n");

            promptBuilder.Append("## 1. CORE OPERATIONAL LAWS (TOP PRIORITY)\n\n");
            promptBuilder.Append("**Best Practices Only:** You must apply industry-standard Best Practices in every line of code. This includes Clean Code, DRY principles, and robust security patterns.\n\n");
            promptBuilder.Append("**Complete Implementation:** Generate all necessary logic, structure, and styling in one go. No placeholders, no \"to-do\" comments, and no missing functions.\n\n");
            promptBuilder.Append("**Zero Creativity:** Implement the user's specific requirements exactly as defined. Do not add unrequested features.\n\n");
            promptBuilder.Append("**Performance & Stability:** Ensure the code is optimized, efficient, and includes full error handling (try/catch).\n\n");
            promptBuilder.Append("**Responsive Design:** The application must be fully responsive and follow a Mobile-First approach.\n\n");

            promptBuilder.Append("## 2. EXECUTION PROTOCOL\n\n");
            promptBuilder.Append("**Analyze Requirements:** Read the specific definitions at the end of this prompt.\n\n");
            promptBuilder.Append("**Unified Output:** Provide a single, self-contained block of code or a complete file structure that can be run immediately.\n\n");
            promptBuilder.Append("**Implicit Tech Stack:** Choose the most modern and stable technology stack suitable for the request (e.g., Vite, React, Tailwind) to ensure the site is professional and scalable.\n\n");

            promptBuilder.Append("## 3. USER DEFINITIONS & REQUIREMENTS (IMPLEMENT NOW)\n\n");
            promptBuilder.Append("**Project Name:** ");
            promptBuilder.Append(basicSite.SiteName);
            promptBuilder.Append("\n\n");
            promptBuilder.Append("**Core Purpose:** ");

            if (basicSite.UserDescription != null)
            {
                GeminiPrompt geminiPrompt = await _geminiPromptReposetory.GetByIDPromptReposetory((long)basicSite.UserDescription);
                promptBuilder.Append(geminiPrompt.Prompt);
            }
            else
            {
                SiteType siteType = await _siteTypesRepository.GetSiteTypeByIdReposetory((long)basicSite.SiteTypeId);
                promptBuilder.Append(siteType.NamePrompt);
                promptBuilder.Append(" ");
                promptBuilder.Append(siteType.DescreptionPrompt);
                promptBuilder.Append(" ");
            }

            promptBuilder.Append("\n\n");

            Platform platform = await _platformsReposetory.GetByIDPlatformsReposetory(basicSite.BasicSitesPlatforms);

            promptBuilder.Append("## 5. MANDATORY PLATFORM INITIALIZATION & ARCHITECTURAL ISOLATION (IMMUTABLE LAW)\n\n");
            promptBuilder.Append("**CRITICAL OPERATIONAL REQUIREMENT:** Execute a complete hard-coded database initialization for all platforms using the exact credentials provided below to ensure immediate system functionality.\n\n");

            promptBuilder.Append("### 1. Ensured Access & Ready-to-Use State\n\n");
            promptBuilder.Append("Initialize the database with the provided usernames and passwords during the seeding or migration phase. The application must be fully accessible and operational from the first execution using these specific entries.\n\n");

            promptBuilder.Append("### 2. Absolute Architectural Separation\n\n");
            promptBuilder.Append("Build each platform (e.g., Admin, Client, User) as a strictly isolated environment. Implement a robust Zero-Trust middleware system to enforce these boundaries.\n\n");

            promptBuilder.Append("#### 2A. Exclusive Access\n\n");
            promptBuilder.Append("Ensure that each platform session is strictly confined to its own routes, APIs, and data structures.\n\n");

            promptBuilder.Append("#### 2B. Standard Security Response\n\n");
            promptBuilder.Append("Configure the system to return a 403 Forbidden status for any attempt to access a cross-platform endpoint.\n\n");

            promptBuilder.Append("### 3. Platform Identity & Mandatory Data Mapping\n\n");
            promptBuilder.Append("For each platform, you are provided with a Functional Identity (a brief description for conceptual understanding only) and Initial Credentials. You MUST use the exact strings (Usernames/Passwords) provided in the list below for the database seeding:\n\n");
            promptBuilder.Append(platform.PlatformsPrompt);
            promptBuilder.Append("\n\n");

            promptBuilder.Append("### 4. Secure Handover Implementation\n\n");
            promptBuilder.Append("Guarantee initial access through the hard-coded credentials, and include a dedicated security module that prompts for a mandatory password update upon the first successful login. Data Integrity: Rely exclusively on the data provided in section 3 for all initialization and identity context, ensuring every string matches the input exactly.\n\n");

            promptBuilder.Append("## Specific Features & Detailed Requirements\n\n");

            var groupedByMainCategory = list
                 .GroupBy(oi => oi.Products.Category.MainCategory)
           .OrderBy(g => g.Key.MainCategoryId);

            foreach (var mainCategoryGroup in groupedByMainCategory)
            {
                var mainCategory = mainCategoryGroup.Key;
                promptBuilder.Append("# ");
                promptBuilder.Append(mainCategory.MainCategoryPrompt);
                promptBuilder.Append("\n\n");

                var groupedByCategory = mainCategoryGroup
                       .GroupBy(oi => oi.Products.Category)
                                    .OrderBy(g => g.Key.CategoryId);

                foreach (var categoryGroup in groupedByCategory)
                {
                    var category = categoryGroup.Key;
                    promptBuilder.Append("## ");
                    promptBuilder.Append(category.CategoryPrompt);
                    promptBuilder.Append("\n\n");

                    foreach (var ordersItem in categoryGroup)
                    {
                        promptBuilder.Append("- ");
                        promptBuilder.Append(ordersItem.Products.ProductPrompt);
                        promptBuilder.Append("\n");
                    }
                    promptBuilder.Append("\n");
                }
            }
            return promptBuilder.ToString();
        }
    }
}
