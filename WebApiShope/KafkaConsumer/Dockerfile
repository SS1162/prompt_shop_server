FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["KafkaConsumer/KafkaConsumer.csproj", "KafkaConsumer/"]
RUN dotnet restore "KafkaConsumer/KafkaConsumer.csproj"
COPY . .
WORKDIR "/src/KafkaConsumer"
RUN dotnet publish "KafkaConsumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "KafkaConsumer.dll"]
