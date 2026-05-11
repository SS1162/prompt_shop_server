# ─────────────────────────────────────────────────────────────────────────────
# Stage 1 – restore
# ─────────────────────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS restore
WORKDIR /src

# פקודות לעקיפת בדיקת SSL עבור עבודה עם סינון אינטרנט
ENV DOTNET_NET_HTTP_USESOCKETSHTTPHANDLER=0
ENV CURL_CA_BUNDLE=""

COPY WebApiShope.sln ./
COPY DTO/DTO.csproj                   DTO/
COPY Entities/Entities.csproj         Entities/
COPY Repositories/Repositories.csproj Repositories/
COPY Services/Services.csproj         Services/
COPY Tests/Tests.csproj               Tests/
COPY WebApiShope/WebApiShope.csproj   WebApiShope/

# הרצת restore עם התעלמות זמנית מבעיות אבטחה בחיבור
RUN dotnet restore WebApiShope/WebApiShope.csproj --disable-parallel

# ─────────────────────────────────────────────────────────────────────────────
# Stage 2 – build
# ─────────────────────────────────────────────────────────────────────────────
FROM restore AS build
COPY . .
RUN dotnet build WebApiShope/WebApiShope.csproj \
      --configuration Release \
      --no-restore

# ─────────────────────────────────────────────────────────────────────────────
# Stage 3 – publish
# ─────────────────────────────────────────────────────────────────────────────
FROM build AS publish
RUN dotnet publish WebApiShope/WebApiShope.csproj \
      --configuration Release \
      --no-build \
      --output /app/publish \
      /p:UseAppHost=false

# ─────────────────────────────────────────────────────────────────────────────
# Stage 4 – final runtime image
# ─────────────────────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

# יצירת משתמש מערכת ללא הרשאות root לצורכי אבטחה
RUN addgroup --system appgroup \
 && adduser  --system --ingroup appgroup --no-create-home appuser

WORKDIR /app

# העתקת התוצרים המוכנים משלב ה-publish
COPY --from=publish /app/publish .

# יצירת תיקיות לתמונות והגדרת הרשאות למשתמש החדש
RUN mkdir -p wwwroot/images wwwroot/reviews \
 && chown -R appuser:appgroup /app

USER appuser

# הגדרת פורט העבודה של Kestrel
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "WebApiShope.dll"]