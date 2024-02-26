FROM node:20-alpine AS client-builder
ENV TZ="America/Sao_Paulo"
WORKDIR /app
COPY /src/Notes.Spa/package.json ./
COPY /src/Notes.Spa/package-lock.json ./
COPY /src/Notes.Spa/tsconfig.json ./
RUN npm install
COPY /src/Notes.Spa/ .
RUN npm run build-prod

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS server-builder
WORKDIR /server
COPY ["/src/Notes.Api/Notes.Api.csproj", "/server/src/Notes.Api/Notes.Api.csproj"]
COPY ["/src/Notes.Application/Notes.Application.csproj", "/server/src/Notes.Application/Notes.Application.csproj"]
COPY ["/src/Notes.Domain/Notes.Domain.csproj", "/server/src/Notes.Domain/Notes.Domain.csproj"]
COPY ["/src/Notes.Infraestructure/Notes.Infraestructure.csproj", "/server/src/Notes.Infraestructure/Notes.Infraestructure.csproj"]
COPY ["/src/Notes.IntegrationTests/Notes.IntegrationTests.csproj", "/server/src/Notes.IntegrationTests/Notes.IntegrationTests.csproj"]
RUN dotnet restore /server/src/Notes.Api/Notes.Api.csproj

COPY . .

RUN dotnet build
RUN dotnet publish -c Release ./src/Notes.Api/Notes.Api.csproj -o /publish/

FROM mcr.microsoft.com/dotnet/aspnet:8.0

RUN DEBIAN_FRONTEND=noninteractive TZ=America/Sao_Paulo apt-get update && apt-get install -y tzdata
ENV TZ=America/Sao_Paulo
RUN cat /usr/share/zoneinfo/$TZ > /etc/localtime \
&& cat /usr/share/zoneinfo/$TZ > /etc/timezone

RUN sed -i 's/TLSV1.2/TLSv1.1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=0/g' /etc/ssl/openssl.cnf
EXPOSE 8080 
ENV ASPNETCORE_URLS http://+:8080
WORKDIR /app
RUN mkdir wwwroot
COPY --from=server-builder /publish .
COPY --from=client-builder /app/dist/browser ./wwwroot
ENTRYPOINT ["dotnet", "Notes.Api.dll"]